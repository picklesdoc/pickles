#region License

/*
    Copyright [2011] [Jeffrey Cameron]

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

#endregion

using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Reflection;
using System.Text;
using NGenerics.DataStructures.Trees;
using NGenerics.Patterns.Visitor;
using NLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.TestFrameworks;

namespace PicklesDoc.Pickles.DocumentationBuilders.JSON
{
    public class JSONDocumentationBuilder : IDocumentationBuilder
    {
        public const string JsonFileName = @"pickledFeatures.json";
        private static readonly Logger log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        private readonly Configuration configuration;
        private readonly ITestResults testResults;

        private readonly IFileSystem fileSystem;

        public JSONDocumentationBuilder(Configuration configuration, ITestResults testResults, IFileSystem fileSystem)
        {
            this.configuration = configuration;
            this.testResults = testResults;
            this.fileSystem = fileSystem;
        }

        public string OutputFilePath
        {
            get { return this.fileSystem.Path.Combine(this.configuration.OutputFolder.FullName, JsonFileName); }
        }

        #region IDocumentationBuilder Members

        public void Build(GeneralTree<INode> features)
        {
            if (log.IsInfoEnabled)
            {
              log.Info("Writing JSON to {0}", this.configuration.OutputFolder.FullName);
            }

            var featuresToFormat = new List<FeatureWithMetaInfo>();

            var actionVisitor = new ActionVisitor<INode>(node =>
                                                                          {
                                                                              var featureTreeNode =
                                                                                  node as FeatureNode;
                                                                              if (featureTreeNode != null)
                                                                              {
                                                                                  if (this.configuration.HasTestResults)
                                                                                  {
                                                                                      featuresToFormat.Add(
                                                                                          new FeatureWithMetaInfo(
                                                                                              featureTreeNode,
                                                                                              this.testResults.
                                                                                                  GetFeatureResult(
                                                                                                      featureTreeNode.
                                                                                                          Feature)));
                                                                                  }
                                                                                  else
                                                                                  {
                                                                                      featuresToFormat.Add(
                                                                                          new FeatureWithMetaInfo(
                                                                                              featureTreeNode));
                                                                                  }
                                                                              }
                                                                          });

            features.AcceptVisitor(actionVisitor);

            CreateFile(this.OutputFilePath, GenerateJSON(featuresToFormat));
        }

        #endregion

        private static string GenerateJSON(List<FeatureWithMetaInfo> features)
        {
            var settings = new JsonSerializerSettings
                               {
                                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                   NullValueHandling = NullValueHandling.Ignore,
                                   Converters = new List<JsonConverter> {new StringEnumConverter()}
                               };

            return JsonConvert.SerializeObject(features, Formatting.Indented, settings);
        }

        private static void CreateFile(string outputFolderName, string jsonToWrite)
        {
            using (var writer = new System.IO.StreamWriter(outputFolderName, false, Encoding.UTF8))
            {
                writer.Write(jsonToWrite);
                writer.Close();
            }
        }
    }
}