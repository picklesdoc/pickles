//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="JSONDocumentationBuilder.cs" company="PicklesDoc">
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
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.JSON
{
    using System.Linq;
    using System.Text.RegularExpressions;

    public class JsonDocumentationBuilder : IDocumentationBuilder
    {
        public const string JsonFileName = @"pickledFeatures.json";
        private static readonly Logger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        private readonly IConfiguration configuration;
        private readonly ITestResults testResults;

        private readonly IFileSystem fileSystem;

        public JsonDocumentationBuilder(IConfiguration configuration, ITestResults testResults, IFileSystem fileSystem)
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

        public void Build(Tree features)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Info("Writing JSON to {0}", this.configuration.OutputFolder.FullName);
            }

            var featuresToFormat = new List<JsonFeatureWithMetaInfo>();

            foreach (var node in features)
            {
                var featureTreeNode =
                    node as FeatureNode;
                if (featureTreeNode != null)
                {
                    if (this.configuration.HasTestResults)
                    {
                        featuresToFormat.Add(
                            new JsonFeatureWithMetaInfo(
                                featureTreeNode,
                                this.testResults.GetFeatureResult(
                                    featureTreeNode.Feature)));
                    }
                    else
                    {
                        featuresToFormat.Add(
                            new JsonFeatureWithMetaInfo(
                                featureTreeNode));
                    }
                }
            }

            this.CreateFile(this.OutputFilePath, this.GenerateJson(featuresToFormat));
        }

        #endregion

        private string GenerateJson(List<JsonFeatureWithMetaInfo> features)
        {
            var data = new
            {
                Features = features,
                Summary = this.GenerateSummary(features),
                Configuration = new
                {
                    SutName = this.configuration.SystemUnderTestName,
                    SutVersion = this.configuration.SystemUnderTestVersion,
                    GeneratedOn = DateTime.Now.ToString("d MMMM yyyy HH:mm:ss")
                }
            };

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented, settings);
        }

        private void CreateFile(string outputFolderName, string jsonToWrite)
        {
            using (var writer = this.fileSystem.File.CreateText(outputFolderName))
            {
                writer.Write(jsonToWrite);
                writer.Close();
            }
        }

        private dynamic GenerateSummary(List<JsonFeatureWithMetaInfo> features)
        {
            // master lists
            // TODO: support filtering out of certain tags
            var filteredFeatures = features.Where(x => true).ToList();
            var scenarios = features.SelectMany(x => x.Feature.FeatureElements).ToList();
            var filteredScenarios = scenarios;

            // calculate tag summary - total scenarios (combining features and scenarios with tags)
            var tagSummary = features
                .SelectMany(x => x.Feature.Tags)
                .Union(scenarios.SelectMany(x => x.Tags))
                .Distinct()
                .Select(tag =>
                    {
                        var scenariosWithTag = features
                            .Where(f => f.Feature.Tags.Contains(tag)).SelectMany(f => f.Feature.FeatureElements)
                            .Union(scenarios.Where(s => s.Tags.Contains(tag)))
                            .Distinct()
                            .ToList();

                        return new
                            {
                                Tag = tag,
                                Total = scenariosWithTag.Count,
                                Passing = scenariosWithTag.LongCount(x => x.Result.WasExecuted && x.Result.WasSuccessful),
                                Failing = scenariosWithTag.LongCount(x => x.Result.WasExecuted && !x.Result.WasSuccessful),
                                Inconclusive = scenariosWithTag.LongCount(x => !x.Result.WasExecuted)
                            };
                    });

            // calculate top-level folder summary - total scenarios (excluding filtered scenarios)
            var topLevelFolderName = new Regex(@"^(.*?)\\\\?.*$", RegexOptions.Compiled);

            var topLevelFolderSummary = filteredFeatures
                .Select(x => topLevelFolderName.Replace(x.RelativeFolder, "$1"))
                .Distinct()
                .Select(folder =>
                    {
                        var scenariosInFolder = filteredFeatures
                            .Where(f => f.RelativeFolder.StartsWith(folder))
                            .SelectMany(f => f.Feature.FeatureElements)
                            .Where(s => filteredScenarios.Contains(s))
                            .ToList();

                        return new
                            {
                                Folder = folder,
                                Total = scenariosInFolder.Count,
                                Passing = scenariosInFolder.LongCount(x => x.Result.WasExecuted && x.Result.WasSuccessful),
                                Failing = scenariosInFolder.LongCount(x => x.Result.WasExecuted && !x.Result.WasSuccessful),
                                Inconclusive = scenariosInFolder.LongCount(x => !x.Result.WasExecuted)
                            };
                    });

            var notTestedScenarios = features
                .Where(f => f.Feature.Tags.Contains("@NotTested")).SelectMany(f => f.Feature.FeatureElements)
                .Union(scenarios.Where(s => s.Tags.Contains("@NotTested")))
                .Distinct()
                .ToList();

            // calculate top-level folder summary - @NotTested scenarios only
            var topLevelNotTestedFolderSummary = features
                .Select(x => topLevelFolderName.Replace(x.RelativeFolder, "$1"))
                .Distinct()
                .Select(folder =>
                    {
                        var notTestedScenariosInFolder = filteredFeatures
                            .Where(f => f.RelativeFolder.StartsWith(folder))
                            .SelectMany(f => f.Feature.FeatureElements)
                            .Where(s => notTestedScenarios.Contains(s))
                            .ToList();

                        return new
                            {
                                Folder = folder,
                                Total = notTestedScenariosInFolder.Count,
                                Passing = notTestedScenariosInFolder.LongCount(x => x.Result.WasExecuted && x.Result.WasSuccessful),
                                Failing = notTestedScenariosInFolder.LongCount(x => x.Result.WasExecuted && !x.Result.WasSuccessful),
                                Inconclusive = notTestedScenariosInFolder.LongCount(x => !x.Result.WasExecuted)
                            };
                    });

            return new
                {
                    Tags = tagSummary,
                    Folders = topLevelFolderSummary,
                    NotTestedFolders = topLevelNotTestedFolderSummary,
                    Scenarios = new
                        {
                            Total = filteredScenarios.Count,
                            Passing = filteredScenarios.LongCount(x => x.Result.WasExecuted && x.Result.WasSuccessful),
                            Failing = filteredScenarios.LongCount(x => x.Result.WasExecuted && !x.Result.WasSuccessful),
                            Inconclusive = filteredScenarios.LongCount(x => !x.Result.WasExecuted)
                        },
                    Features = new
                        {
                            Total = filteredFeatures.Count,
                            Passing = filteredFeatures.LongCount(x => x.Result.WasExecuted && x.Result.WasSuccessful),
                            Failing = filteredFeatures.LongCount(x => x.Result.WasExecuted && !x.Result.WasSuccessful),
                            Inconclusive = filteredFeatures.LongCount(x => !x.Result.WasExecuted)
                        }
                };
        }
    }
}
