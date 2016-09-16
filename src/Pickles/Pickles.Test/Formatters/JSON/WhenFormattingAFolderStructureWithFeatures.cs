//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenFormattingAFolderStructureWithFeatures.cs" company="PicklesDoc">
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

using System;
using Autofac;
using NUnit.Framework;
using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders.JSON;
using PicklesDoc.Pickles.Test.Helpers;

namespace PicklesDoc.Pickles.Test.Formatters.JSON
{
    [TestFixture]
    public class WhenFormattingAFolderStructureWithFeatures : BaseFixture
    {
        private const string OutputDirectory = FileSystemPrefix + @"JSONFeatureOutput";

        public void Setup()
        {
            this.AddFakeFolderStructures();

            Tree features = Container.Resolve<DirectoryTreeCrawler>().Crawl(FileSystemPrefix);

            var outputDirectory = FileSystem.DirectoryInfo.FromDirectoryName(OutputDirectory);
            if (!outputDirectory.Exists)
            {
                outputDirectory.Create();
            }

            var configuration = new Configuration
            {
                OutputFolder = FileSystem.DirectoryInfo.FromDirectoryName(OutputDirectory),
                DocumentationFormat = DocumentationFormat.Json
            };

            var jsonDocumentationBuilder = new JsonDocumentationBuilder(configuration, null, FileSystem);
            jsonDocumentationBuilder.Build(features);
        }

        [Test]
        public void ShouldContainTheFeatures()
        {
            this.Setup();

            string content = FileSystem.File.ReadAllText(this.FileSystem.Path.Combine(OutputDirectory, JsonDocumentationBuilder.JsonFileName));
            content.AssertJsonKeyValue("Name", "Addition");
        }
    }
}
