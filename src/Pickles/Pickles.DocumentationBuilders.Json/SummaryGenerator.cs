//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="SummaryGenerator.cs" company="PicklesDoc">
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace PicklesDoc.Pickles.DocumentationBuilders.Json
{
    public class SummaryGenerator
    {
        public static SummaryResult GenerateSummary(List<JsonFeatureWithMetaInfo> features)
        {
            // master lists
            var scenarios = features.SelectMany(x => x.Feature.FeatureElements).ToList();

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

                    return new TagWithTotals
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

            var featuresByFolder = features
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

                    return new FolderWithTotals
                    {
                        Folder = folder,
                        Total = scenariosInFolder.Count,
                        Passing = scenariosInFolder.LongCount(x => x.Result.WasExecuted && x.Result.WasSuccessful),
                        Failing = scenariosInFolder.LongCount(x => x.Result.WasExecuted && !x.Result.WasSuccessful),
                        Inconclusive = scenariosInFolder.LongCount(x => !x.Result.WasExecuted)
                    };
                });

            var notTestedScenarioByFolder = RetrieveScenariosWithACertainTagByFolder(features, scenarios, topLevelFolderName, tags => tags.Any(t => t.StartsWith("@NotTested", StringComparison.OrdinalIgnoreCase)));
            var manualScenariosByFolder = RetrieveScenariosWithACertainTagByFolder(features, scenarios, topLevelFolderName, tags => tags.Contains("@manual", StringComparer.OrdinalIgnoreCase));
            var automatedScenariosByFolder = RetrieveScenariosWithACertainTagByFolder(
                features,
                scenarios,
                topLevelFolderName,
                tags => tags.Contains("@automated", StringComparer.OrdinalIgnoreCase) ||
                        (!tags.Contains("@manual", StringComparer.OrdinalIgnoreCase)
                         &&
                         !tags.Any(t => t.StartsWith("@NotTested", StringComparison.OrdinalIgnoreCase))));

            // calculate top-level folder summary - @NotTested scenarios only
            var topLevelNotTestedFolderSummary = TopLevelFolderSummaryByTag(featuresByFolder, notTestedScenarioByFolder);

            var foldersWithTestKinds = CreateFoldersWithTestKinds(featuresByFolder, automatedScenariosByFolder, manualScenariosByFolder, notTestedScenarioByFolder);
            return new SummaryResult
            {
                Tags = tagSummary,
                Folders = topLevelFolderSummary,
                NotTestedFolders = topLevelNotTestedFolderSummary,
                FoldersWithTestKinds = foldersWithTestKinds,
                Scenarios = new Totals
                {
                    Total = scenarios.Count,
                    Passing = scenarios.LongCount(x => x.Result.WasExecuted && x.Result.WasSuccessful),
                    Failing = scenarios.LongCount(x => x.Result.WasExecuted && !x.Result.WasSuccessful),
                    Inconclusive = scenarios.LongCount(x => !x.Result.WasExecuted)
                },
                Features = new Totals
                {
                    Total = features.Count,
                    Passing = features.LongCount(x => x.Result.WasExecuted && x.Result.WasSuccessful),
                    Failing = features.LongCount(x => x.Result.WasExecuted && !x.Result.WasSuccessful),
                    Inconclusive = features.LongCount(x => !x.Result.WasExecuted)
                }
            };
        }

        private static IEnumerable<FolderWithTotals> TopLevelFolderSummaryByTag(ILookup<string, IJsonFeatureElement> featuresByFolder, ILookup<string, IJsonFeatureElement> interestingScenarios)
        {
            return featuresByFolder
                .Select(x => x.Key)
                .Select(folder =>
                {
                    var scenariosInFolder = interestingScenarios[folder].ToList();

                    return new FolderWithTotals
                    {
                        Folder = folder,
                        Total = scenariosInFolder.Count,
                        Passing = scenariosInFolder.LongCount(x => x.Result.WasExecuted && x.Result.WasSuccessful),
                        Failing = scenariosInFolder.LongCount(x => x.Result.WasExecuted && !x.Result.WasSuccessful),
                        Inconclusive = scenariosInFolder.LongCount(x => !x.Result.WasExecuted)
                    };
                });
        }
        private static IEnumerable<FolderWithTestKinds> CreateFoldersWithTestKinds(
            ILookup<string, IJsonFeatureElement> featuresByFolder,
            ILookup<string, IJsonFeatureElement> automatedScenarios,
            ILookup<string, IJsonFeatureElement> manualScenarios,
            ILookup<string, IJsonFeatureElement> nonTestedScenarios)
        {
            return featuresByFolder
                .Select(x => x.Key)
                .Select(folder =>
                    new FolderWithTestKinds
                    {
                        Folder = folder,
                        Automated = automatedScenarios[folder].LongCount(),
                        Manual = manualScenarios[folder].LongCount(),
                        NotTested = nonTestedScenarios[folder].LongCount()
                    });
        }

        private static ILookup<string, IJsonFeatureElement> RetrieveScenarioByFolder(List<JsonFeatureWithMetaInfo> filteredFeatures, Regex topLevelFolderName, HashSet<IJsonFeatureElement> interestingScenarios)
        {
            return filteredFeatures
                .SelectMany(f => f.Feature.FeatureElements.Select(
                    e => new
                    {
                        Folder = topLevelFolderName.Replace(f.RelativeFolder, "$1"),
                        Element = e
                    }))
                .Where(x => interestingScenarios.Contains(x.Element))
                .ToLookup(x => x.Folder, x => x.Element);
        }

        private static HashSet<IJsonFeatureElement> RetrieveScenariosWithACertainTag(List<IJsonFeatureElement> scenarios, Func<IEnumerable<string>, bool> tagSelector)
        {
            var featureElements = scenarios.Where(s => tagSelector(s.AllTags()));

            return new HashSet<IJsonFeatureElement>(featureElements);
        }

        private static ILookup<string, IJsonFeatureElement> RetrieveScenariosWithACertainTagByFolder(
            List<JsonFeatureWithMetaInfo> features,
            List<IJsonFeatureElement> scenarios,
            Regex topLevelFolderName,
            Func<IEnumerable<string>, bool> tagSelector)
        {
            var featuresWithACertainTag = RetrieveScenariosWithACertainTag(scenarios, tagSelector);

            var result = RetrieveScenarioByFolder(features, topLevelFolderName, featuresWithACertainTag);

            return result;
        }
    }

    public class SummaryResult
    {
        public IEnumerable<TagWithTotals> Tags { get; set; }
        public IEnumerable<FolderWithTotals> Folders { get; set; }
        public IEnumerable<FolderWithTotals> NotTestedFolders { get; set; }
        public IEnumerable<FolderWithTotals> ManualFolders { get; set; }
        public IEnumerable<FolderWithTotals> AutomatedFolders { get; set; }
        public Totals Scenarios { get; set; }
        public Totals Features { get; set; }
        public IEnumerable<FolderWithTestKinds> FoldersWithTestKinds { get; set; }
    }

    public class FolderWithTotals
    {
        public string Folder { get; set; }
        public int Total { get; set; }
        public long Passing { get; set; }
        public long Failing { get; set; }
        public long Inconclusive { get; set; }
    }

    public class FolderWithTestKinds
    {
        public string Folder { get; set; }
        public long Total => Automated + Manual + NotTested;
        public long Automated { get; set; }
        public long Manual { get; set; }
        public long NotTested { get; set; }
    }

    public class TagWithTotals
    {
        public string Tag { get; set; }
        public int Total { get; set; }
        public long Passing { get; set; }
        public long Failing { get; set; }
        public long Inconclusive { get; set; }
    }

    public class Totals
    {
        public int Total { get; set; }
        public long Passing { get; set; }
        public long Failing { get; set; }
        public long Inconclusive { get; set; }
    }
}