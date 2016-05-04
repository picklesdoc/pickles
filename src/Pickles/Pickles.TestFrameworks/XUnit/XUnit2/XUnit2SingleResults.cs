//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="XUnit2SingleResults.cs" company="PicklesDoc">
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
using System.Linq;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.TestFrameworks.XUnit.XUnit2
{
    public class XUnit2SingleResults : SingleTestRunBase
    {
        private readonly assemblies resultsDocument;

        public XUnit2SingleResults(assemblies resultsDocument)
        {
            this.resultsDocument = resultsDocument;
        }

        public override TestResult GetFeatureResult(Feature feature)
        {
            var featureElement = this.GetFeatureElement(feature);

            if (featureElement == null)
            {
                return TestResult.Inconclusive;
            }

            int passedCount = featureElement.passed;
            int failedCount = featureElement.failed;
            int skippedCount = featureElement.skipped;

            return this.GetAggregateResult(passedCount, failedCount, skippedCount);
        }

        public override TestResult GetScenarioOutlineResult(ScenarioOutline scenarioOutline)
        {
            IEnumerable<TestResult> exampleElements = this.GetScenarioOutlineElements(scenarioOutline).Select(this.GetResultFromElement);

            return this.DetermineAggregateResult(exampleElements);
        }

        public override TestResult GetScenarioResult(Scenario scenario)
        {
            var scenarioElement = this.GetScenarioElement(scenario);
            return scenarioElement != null
                ? this.GetResultFromElement(scenarioElement)
                : TestResult.Inconclusive;
        }

        public override TestResult GetExampleResult(ScenarioOutline scenarioOutline, string[] exampleValues)
        {
            IEnumerable<assembliesAssemblyCollectionTest> exampleElements = this.GetScenarioOutlineElements(scenarioOutline);

            foreach (var exampleElement in exampleElements)
            {
                if (this.ScenarioOutlineExampleMatcher.IsMatch(scenarioOutline, exampleValues, exampleElement))
                {
                    return this.GetResultFromElement(exampleElement);
                }
            }

            return TestResult.Inconclusive;
        }

        private static bool HasDescriptionTrait(assembliesAssemblyCollectionTest test, string description)
        {
            return HasTraitWithValue(test, "Description", description);
        }

        private static bool HasFeatureTitleTrait(assembliesAssemblyCollectionTest test, string featureTitle)
        {
            return HasTraitWithValue(test, "FeatureTitle", featureTitle);
        }

        private static bool HasTraitWithValue(assembliesAssemblyCollectionTest test, string trait, string value)
        {
            return test.traits != null && test.traits.Any(t => t.name == trait && t.value == value);
        }

        private assembliesAssemblyCollection GetFeatureElement(Feature feature)
        {
            var query = from collection in this.resultsDocument.assembly.collection
                        from test in collection.test
                        where HasFeatureTitleTrait(test, feature.Name)
                        select collection;

            return query.FirstOrDefault();
        }

        private assembliesAssemblyCollectionTest GetScenarioElement(Scenario scenario)
        {
            var featureElement = this.GetFeatureElement(scenario.Feature);
            if (featureElement == null)
                return null;

            var query = from test in featureElement.test
                        where HasDescriptionTrait(test, scenario.Name)
                        select test;

            return query.FirstOrDefault();
        }

        private IEnumerable<assembliesAssemblyCollectionTest> GetScenarioOutlineElements(ScenarioOutline scenario)
        {
            var featureElement = this.GetFeatureElement(scenario.Feature);
            if (featureElement == null)
                return Enumerable.Empty<assembliesAssemblyCollectionTest>();
            var query = from test in featureElement.test
                        where HasDescriptionTrait(test, scenario.Name)
                        select test;

            return query;
        }

        private TestResult GetResultFromElement(assembliesAssemblyCollectionTest element)
        {
            TestResult result;

            switch (element.result.ToLowerInvariant())
            {
                case "pass":
                    result = TestResult.Passed;
                    break;
                case "fail":
                    result = TestResult.Failed;
                    break;
                case "skip":
                default:
                    result = TestResult.Inconclusive;
                    break;
            }

            return result;
        }
    }
}
