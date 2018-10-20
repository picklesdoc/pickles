//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CucumberDocumentationBuilder.cs" company="PicklesDoc">
//  Copyright 2017 Dmitry Grekov
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
using System.IO;
using System.IO.Abstractions;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Cucumber
{
    using Pickles;
    using DocumentationBuilders;
    using System.Linq;
    using DataStructures;

    public class CucumberDocumentationBuilder : IDocumentationBuilder
    {
        public const string CucumberFileName = @"cucumberResult.json";

        private static readonly Logger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType?.Name);
        private readonly IConfiguration configuration;
        private readonly IFileSystem fileSystem;


        public string OutputFilePath => this.fileSystem.Path.Combine(this.configuration.OutputFolder.FullName, CucumberFileName);

        public CucumberDocumentationBuilder(IConfiguration configuration, IFileSystem fileSystem)
        {
            this.configuration = configuration;
            this.fileSystem = fileSystem;
        }

        public void Build(Tree features)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Info("Writing Cucumber to {0}", this.configuration.OutputFolder.FullName);
            }

            List<Feature> featuresToFormat = new List<Feature>();

            foreach (var node in features)
            {
                var featureTreeNode = node as FeatureNode;
                if (featureTreeNode != null)
                {
                    featuresToFormat.Add(featureTreeNode.Feature);
                }
            }

            CreateFile(OutputFilePath, GenerateJson(featuresToFormat));
        }

        private string GenerateJson(List<Feature> features)
        {
            var toOutPut = features.Select(f => new
            {
                keyword = "Feature",
                name = f.Name,
                tags = f.Tags.Select(t => new { name = t }),
                line = 1,
                elements = f.FeatureElements.Select(fe =>
                    new
                    {
                        keyword = fe is Scenario ? "Scenario" : "Scenario Outline",
                        name = fe.Name,
                        line = fe.Location.Line,
                        type = fe is Scenario ? "scenario" : "scenario_outline",
                        tags = fe.Tags.Select(t => new { name = t }),
                        steps = fe.Steps.Select(s => new
                        {
                            keyword = s.Keyword,
                            name = s.Name,
                            line = s.Location.Line,
                            result = new
                            {
                                status = DetermineStatus(fe),
                                duration = 1
                            }
                        })
                    }
                      ),

            });


            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };

            return JsonConvert.SerializeObject(toOutPut, Formatting.Indented, settings);
        }

        private static string DetermineStatus(IFeatureElement fe)
        {
            var testResult = fe.Result;

            if (testResult == TestResult.NotProvided)
            {
                testResult = TestResult.Inconclusive;
            }

            return testResult.ToString().ToLowerInvariant();
        }

        private void CreateFile(string outputFolderName, string jsonToWrite)
        {
            using (StreamWriter writer = this.fileSystem.File.CreateText(outputFolderName))
            {
                writer.Write(jsonToWrite);
                writer.Close();
            }
        }
    }
}
