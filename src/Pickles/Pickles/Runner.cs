//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Runner.cs" company="PicklesDoc">
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
using System.Linq;
using System.Reflection;
using Autofac;
using NLog;
using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles
{
    public class Runner
    {
        private static readonly Logger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        public void Run(IContainer container)
        {
            var configuration = container.Resolve<IConfiguration>();
            if (!configuration.OutputFolder.Exists)
            {
                configuration.OutputFolder.Create();
            }

            var featureCrawler = container.Resolve<DirectoryTreeCrawler>();
            var parsingReport = new ParsingReport();

            Tree features = featureCrawler.Crawl(configuration.FeatureFolder, parsingReport);

            if (parsingReport.Any())
            {
                var error = $"Some files ({parsingReport.Count}) were not parsed correctly.";
                Log.Error(error);
            }

            if (features == null)
            {
                Log.Warn("No features found at {0}", configuration.FeatureFolder);
                return;
            }

            ApplyTestResultsToFeatures(container, configuration, features);

            var documentationBuilder = container.Resolve<IDocumentationBuilder>();
            try
            {
                documentationBuilder.Build(features);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong during generation: {0}", ex);
                throw;
            }

            if (parsingReport.Any())
            {
                throw new ApplicationException("Some files were not parsed correctly.");
            }
        }

        private static void ApplyTestResultsToFeatures(IContainer container, IConfiguration configuration, Tree features)
        {
            var testResults = container.Resolve<ITestResults>();

            foreach (var featureTreeNode in features.OfType<FeatureNode>())
            {
                if (featureTreeNode == null)
                {
                    return;
                }

                if (configuration.HasTestResults)
                {
                    SetResultsAtFeatureLevel(featureTreeNode, testResults);
                    SetResultsForIndividualScenariosUnderFeature(featureTreeNode, testResults);
                }
                else
                {
                    featureTreeNode.Feature.Result = TestResult.Inconclusive;
                }
            }
        }

        private static void SetResultsForIndividualScenariosUnderFeature(FeatureNode featureTreeNode, ITestResults testResults)
        {
            foreach (var featureElement in featureTreeNode.Feature.FeatureElements)
            {
                var scenario = featureElement as Scenario;

                if (scenario != null)
                {
                    featureElement.Result = testResults.GetScenarioResult(scenario);
                    continue;
                }

                var scenarioOutline = featureElement as ScenarioOutline;

                if (scenarioOutline != null)
                {
                    foreach (var example in scenarioOutline.Examples.SelectMany(e => e.TableArgument.DataRows).Cast<TableRowWithTestResult>())
                    {
                        if(example!=null)
                            example.Result = testResults.GetExampleResult(scenarioOutline, example.Cells.ToArray());
                    }

                    scenarioOutline.Result =
                        scenarioOutline.Examples.SelectMany(e => e.TableArgument.DataRows)
                        .Cast<TableRowWithTestResult>()
                            .Select(row => row.Result)
                            .Merge();
                }
            }
        }

        private static void SetResultsAtFeatureLevel(FeatureNode featureTreeNode, ITestResults testResults)
        {
            featureTreeNode.Feature.Result = testResults.GetFeatureResult(featureTreeNode.Feature);
        }
    }
}
