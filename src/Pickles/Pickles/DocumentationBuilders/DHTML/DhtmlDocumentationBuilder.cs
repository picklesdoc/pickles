//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DhtmlDocumentationBuilder.cs" company="PicklesDoc">
//  Copyright 2011 Jeffrey Cameron
//  Copyright 2012-present PicklesDoc team and community contributors
//
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using NLog;
using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders.JSON;
using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.TestFrameworks;

namespace PicklesDoc.Pickles.DocumentationBuilders.DHTML
{
    public class DhtmlDocumentationBuilder : IDocumentationBuilder
    {
        private static readonly Logger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        private readonly IConfiguration configuration;
        private readonly ITestResults testResults;

        private readonly IFileSystem fileSystem;

        public DhtmlDocumentationBuilder(IConfiguration configuration, ITestResults testResults, IFileSystem fileSystem)
        {
            this.configuration = configuration;
            this.testResults = testResults;
            this.fileSystem = fileSystem;
        }

        public void Build(Tree features)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Info("Writing DHTML files to {0}", this.configuration.OutputFolder.FullName);
            }

            Log.Info("Pull Down Feature Images ");
            this.PullDownFeatureImages(features);

            Log.Info("Write Resources");
            this.WriteResources();

            Log.Info("Utilize JsonBuilder To Dump Json Feature File Next To Dthml Resources");
            this.UtilizeJsonBuilderToDumpJsonFeatureFileNextToDthmlResources(features);

            Log.Info("Tweak Json file");
            this.TweakJsonFile();
        }

        private void PullDownFeatureImages(IEnumerable<INode> features)
        {
            foreach (var image in features.Where(p => p.GetType() == typeof(ImageNode)).Select(p => (ImageNode)p))
            {
                var source = image.OriginalLocation.FullName;
                var dest = this.fileSystem.Path.Combine(this.configuration.OutputFolder.FullName, image.OriginalLocation.Name);
                this.fileSystem.File.Copy(source, dest, true);
            }
        }

        private void UtilizeJsonBuilderToDumpJsonFeatureFileNextToDthmlResources(Tree features)
        {
            var jsonBuilder = new JsonDocumentationBuilder(this.configuration, this.testResults, this.fileSystem);
            jsonBuilder.Build(features);
        }

        private void WriteResources()
        {
            var dhtmlResourceWriter = new DhtmlResourceWriter(this.fileSystem, this.configuration);
            dhtmlResourceWriter.WriteTo(this.configuration.OutputFolder.FullName);
        }

        private void TweakJsonFile()
        {
            var jsonBuilder = new JsonDocumentationBuilder(this.configuration, this.testResults, this.fileSystem);
            var jsonFilePath = jsonBuilder.OutputFilePath;

            var tweaker = new JsonTweaker(this.fileSystem);
            tweaker.AddJsonPWrapperTo(jsonFilePath);
            tweaker.RenameFileTo(jsonFilePath, jsonFilePath.Replace(".json", ".js"));
        }
    }
}
