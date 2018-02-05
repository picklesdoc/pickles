//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="NUnitSingleResultsBase.cs" company="PicklesDoc">
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

using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.TestFrameworks.NUnit
{
    public abstract class NUnitSingleResultsBase : SingleTestRunBase
    {
        private readonly TestResultAndName[] testResultAndNames;

        protected NUnitSingleResultsBase(XDocument resultsDocument, TestResultAndName[] testResultAndNames)
        {
            this.testResultAndNames = testResultAndNames;
        }

        public override TestResult GetFeatureResult(Feature feature)
        {
            var featureElement = this.GetFeatureElement(feature);

            if (featureElement == null)
            {
                return TestResult.Inconclusive;
            }

            var results = featureElement.Descendants("test-case")
                .Select(this.GetResultFromElement);

            return TestResultExtensions.Merge(results);
        }

        public override TestResult GetScenarioResult(Scenario scenario)
        {
            var scenarioElement = this.GetScenarioElement(scenario);

            return this.GetResultFromElement(scenarioElement);
        }

        public override TestResult GetScenarioOutlineResult(ScenarioOutline scenarioOutline)
        {
            var scenarioOutlineElement = this.GetScenarioOutlineElement(scenarioOutline);

            return this.DetermineScenarioOutlineResult(scenarioOutlineElement);
        }

        public override TestResult GetExampleResult(ScenarioOutline scenarioOutline, string[] exampleValues)
        {
            var examplesElement = this.GetExamplesElement(scenarioOutline, exampleValues);

            return this.GetResultFromElement(examplesElement);
        }

        protected static bool IsMatchingTestCase(XElement x, Regex exampleSignature)
        {
            var name = x.Attribute("name");
            return name != null && exampleSignature.IsMatch(name.Value.ToLowerInvariant().Replace(@"\", string.Empty));
        }

        private TestResult DetermineScenarioOutlineResult(XElement scenarioOutlineElement)
        {
            if (scenarioOutlineElement != null)
            {
                return TestResultExtensions.Merge(scenarioOutlineElement.Descendants("test-case").Select(this.GetResultFromElement));
            }

            return TestResult.Inconclusive;
        }

        private TestResult GetResultFromElement(XElement element)
        {
            if (element == null)
            {
                return TestResult.Inconclusive;
            }

            foreach (var valueAndResult in this.testResultAndNames)
            {
                if (element.IsAttributeSetToValue("result", valueAndResult.Name))
                {
                    return valueAndResult.TestResult;
                }
            }

            bool wasExecuted = element.IsAttributeSetToValue("executed", "true");

            if (!wasExecuted)
            {
                return TestResult.Inconclusive;
            }

            bool wasSuccessful = element.IsAttributeSetToValue("success", "true");

            return wasSuccessful ? TestResult.Passed : TestResult.Failed;
        }

        protected abstract XElement GetScenarioElement(Scenario scenario);

        protected abstract XElement GetScenarioOutlineElement(ScenarioOutline scenarioOutline);

        protected abstract XElement GetFeatureElement(Feature feature);

        protected abstract XElement GetExamplesElement(ScenarioOutline scenarioOutline, string[] values);
    }
}