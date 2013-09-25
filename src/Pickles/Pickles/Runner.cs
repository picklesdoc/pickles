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
using System.Reflection;
using Autofac;
using NGenerics.DataStructures.Trees;
using NGenerics.Patterns.Visitor;
using NLog;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders;
using PicklesDoc.Pickles.Parser;
using PicklesDoc.Pickles.TestFrameworks;

namespace PicklesDoc.Pickles
{
    public class Runner
    {
      private static readonly Logger log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
      
      public void Run(IContainer container)
        {
            var configuration = container.Resolve<Configuration>();
            if (!configuration.OutputFolder.Exists) configuration.OutputFolder.Create();

            var featureCrawler = container.Resolve<DirectoryTreeCrawler>();
            GeneralTree<INode> features = featureCrawler.Crawl(configuration.FeatureFolder);

            if (features == null)
            {
              log.Warn("No features found at {0}", configuration.FeatureFolder);
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
              log.Error("Something went wrong while during generation: {0}", ex);
              throw;
            }
        }

        private static void ApplyTestResultsToFeatures(IContainer container, Configuration configuration, GeneralTree<INode> features)
        {
            var testResults = container.Resolve<ITestResults>();

            var actionVisitor = new ActionVisitor<INode>(node =>
                {
                    var featureTreeNode = node as FeatureNode;
                    if (featureTreeNode == null) return;
                    if (configuration.HasTestResults)
                    {
                        SetResultsAtFeatureLevel(featureTreeNode, testResults);
                        SetResultsForIndividualScenariosUnderFeature(featureTreeNode, testResults);
                    }
                    else
                    {
                        featureTreeNode.Feature.Result = TestResult.Inconclusive;
                    }
                });

            features.AcceptVisitor(actionVisitor);
        }

        private static void SetResultsForIndividualScenariosUnderFeature(FeatureNode featureTreeNode, ITestResults testResults)
        {
            foreach (var scenario in featureTreeNode.Feature.FeatureElements)
            {
                scenario.Result = scenario.GetType().Name == "Scenario"
                                      ? testResults.GetScenarioResult(scenario as Scenario)
                                      : testResults.GetScenarioOutlineResult(scenario as ScenarioOutline);
            }
        }

        private static void SetResultsAtFeatureLevel(FeatureNode featureTreeNode, ITestResults testResults)
        {
            featureTreeNode.Feature.Result = testResults.GetFeatureResult(featureTreeNode.Feature);
        }
    }
}