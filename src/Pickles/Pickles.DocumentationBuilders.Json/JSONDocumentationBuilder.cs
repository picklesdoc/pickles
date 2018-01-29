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
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using NLog;

using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Json
{
    public class JsonDocumentationBuilder : IDocumentationBuilder
    {
        public const string JsonFileName = @"pickledFeatures.json";
        private static readonly Logger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        private readonly IConfiguration configuration;
        private readonly ITestResults testResults;

        private readonly IFileSystem fileSystem;
        private readonly ILanguageServicesRegistry languageServicesRegistry;

        public JsonDocumentationBuilder(IConfiguration configuration, ITestResults testResults, IFileSystem fileSystem, ILanguageServicesRegistry languageServicesRegistry)
        {
            this.configuration = configuration;
            this.testResults = testResults;
            this.fileSystem = fileSystem;
            this.languageServicesRegistry = languageServicesRegistry;
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
                                this.languageServicesRegistry,
                                this.testResults.GetFeatureResult(
                                    featureTreeNode.Feature)));
                    }
                    else
                    {
                        featuresToFormat.Add(
                            new JsonFeatureWithMetaInfo(
                                featureTreeNode,
                                this.languageServicesRegistry));
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

            var scenariosByTag = features
                .SelectMany(
                    x => x.Feature.FeatureElements.SelectMany(
                        e => e.Tags.Select(t => new { Tag = t, FeatureElement = e })));

            var featuresByTag = features
                .SelectMany(
                    f => f.Feature.Tags.SelectMany(
                        t => f.Feature.FeatureElements.Select(
                            e => new { Tag = t, FeatureElement = e })));

            var tagLookup = featuresByTag
                .Union(scenariosByTag)
                .Distinct()
                .ToLookup(
                    x => x.Tag,
                    x => x.FeatureElement);

            // calculate tag summary - total scenarios (combining features and scenarios with tags)
            var tagSummary = tagLookup
                .Select(g => g.Key)
                .Select(tag =>
                    {
                        var scenariosWithTag = tagLookup[tag].ToList();

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

            var featuresByFolder = filteredFeatures
                .SelectMany(f => f.Feature.FeatureElements.Select(
                    e => new
                    {
                        Folder = topLevelFolderName.Replace(f.RelativeFolder, "$1"),
                        Element = e
                    }))
                .ToLookup(x => x.Folder, x => x.Element);

            var topLevelFolderSummary = featuresByFolder
                .Select(x => x.Key)
                .Select(folder =>
                    {
                        var scenariosInFolder = featuresByFolder[folder].ToList();

                        return new
                            {
                                Folder = folder,
                                Total = scenariosInFolder.Count,
                                Passing = scenariosInFolder.LongCount(x => x.Result.WasExecuted && x.Result.WasSuccessful),
                                Failing = scenariosInFolder.LongCount(x => x.Result.WasExecuted && !x.Result.WasSuccessful),
                                Inconclusive = scenariosInFolder.LongCount(x => !x.Result.WasExecuted)
                            };
                    });

            var notTestedScenarios = new HashSet<IJsonFeatureElement>(
                    features
                        .Where(f => f.Feature.Tags.Contains("@NotTested"))
                        .SelectMany(f => f.Feature.FeatureElements)
                        .Union(scenarios.Where(s => s.Tags.Contains("@NotTested")))
                        .Distinct());

            var notTestedScenarioByFolder = filteredFeatures
                .SelectMany(f => f.Feature.FeatureElements.Select(
                    e => new
                    {
                        Folder = topLevelFolderName.Replace(f.RelativeFolder, "$1"),
                        Element = e
                    }))
                .Where(x => notTestedScenarios.Contains(x.Element))
                .ToLookup(x => x.Folder, x => x.Element);

            // calculate top-level folder summary - @NotTested scenarios only
            var topLevelNotTestedFolderSummary = featuresByFolder
                .Select(x => x.Key)
                .Select(folder =>
                    {
                        var notTestedScenariosInFolder = notTestedScenarioByFolder[folder].ToList();

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
