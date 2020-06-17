//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="VsTestSingleResults.cs" company="PicklesDoc">
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
using System.Linq;
using System.Xml.Linq;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.TestFrameworks.VsTest
{
    /// <summary>
    /// The class responsible for parsing a single VS Test result file.
    /// </summary>
    /// <remarks>
    /// The VS Test result format is a bit weird in that it stores the tests and their results in
    /// separate lists. So in order to know the result of a scenario,
    /// we first have to identify the test definition that belongs to the scenario.
    /// Then with the id of the scenario we look up an execution id,
    /// and with the execution id we can look up the result.
    /// </remarks>
    public class VsTestSingleResults : SingleTestRunBase
    {
        private readonly XDocument resultsDocument;

        private readonly ILookup<string, XElement> featureScenarios;

        private readonly IDictionary<Guid, TestResult> executionOutcomes;

        public VsTestSingleResults(XDocument resultsDocument)
        {
            this.resultsDocument = resultsDocument;

            this.featureScenarios = this.resultsDocument.AllScenarios()
                .Select(x => new
                {
                    Feature = x.Feature(),
                    Scenario = x
                })
                .Where(x => x.Feature != null)
                .ToLookup(
                    x => x.Feature,
                    x => x.Scenario,
                    StringComparer.OrdinalIgnoreCase);

            this.executionOutcomes = this.resultsDocument.AllExecutionResults()
                .ToDictionary(
                    x => x.ExecutionIdAttribute(),
                    x => x.Outcome());
        }

        public override TestResult GetFeatureResult(Feature feature)
        {
            var scenarios = this.GetScenariosForFeature(feature);

            var featureExecutionIds = scenarios.ExecutionIdElements();

            TestResult result = this.GetExecutionResult(featureExecutionIds);

            return result;
        }

        /// <summary>
        /// Retrieves all UnitTest XElements that belong to the specified feature.
        /// </summary>
        /// <param name="feature">The feature for which to retrieve the unit tests.</param>
        /// <returns>A sequence of <see cref="XElement" /> instances that are called "UnitTest"
        /// that belong to the specified feature.</returns>
        private IEnumerable<XElement> GetScenariosForFeature(Feature feature)
        {
            return this.featureScenarios[SpecFlowNameMapping.Build(feature.Name)];
        }

        private TestResult GetExecutionResult(IEnumerable<Guid> featureExecutionIds)
        {
            TestResult result = featureExecutionIds.Select(this.GetExecutionResult).Merge();
            return result;
        }

        private TestResult GetExecutionResult(Guid scenarioExecutionId)
        {
            TestResult result;
            this.executionOutcomes.TryGetValue(scenarioExecutionId, out result);

            return result;
        }

        public override TestResult GetScenarioOutlineResult(ScenarioOutline scenarioOutline)
        {
            var scenarios = this.GetScenariosForScenarioOutline(scenarioOutline);

            var executionIds = scenarios.Select(scenario => scenario.ExecutionIdElement());

            TestResult result = this.GetExecutionResult(executionIds);

            return result;
        }

        private IEnumerable<XElement> GetScenariosForScenarioOutline(ScenarioOutline scenarioOutline)
        {
            var scenarios =
                this.GetScenariosForFeature(scenarioOutline.Feature)
                    .Where(scenario => scenario.BelongsToScenarioOutline(scenarioOutline));

            return scenarios;
        }

        public override TestResult GetScenarioResult(Scenario scenario)
        {
            var scenarios = this.GetScenariosForScenario(scenario);

            Guid executionId = scenarios.Select(s => s.ExecutionIdElement()).FirstOrDefault();

            TestResult testResult = this.GetExecutionResult(executionId);

            return testResult;
        }

        private IEnumerable<XElement> GetScenariosForScenario(Scenario scenario)
        {
            var scenarios =
                this.GetScenariosForFeature(scenario.Feature);

            scenarios = scenarios.Where(s => s.BelongsToScenario(scenario));

            return scenarios;
        }

        public override TestResult GetExampleResult(ScenarioOutline scenario, string[] exampleValues)
        {
            var scenarioElements = this.GetScenariosForScenarioOutline(scenario);

            var theScenario = this.GetScenarioThatMatchesTheExampleValues(scenario, exampleValues, scenarioElements);

            Guid executionId = theScenario.ExecutionIdElement();

            TestResult testResult = this.GetExecutionResult(executionId);

            return testResult;
        }

        private XElement GetScenarioThatMatchesTheExampleValues(ScenarioOutline scenarioOutline, string[] exampleValues, IEnumerable<XElement> scenarioElements)
        {
            // filter for example values
            XElement theScenario = null;

            foreach (var element in scenarioElements)
            {
                var isMatch = this.ScenarioOutlineExampleMatcher.IsMatch(scenarioOutline, exampleValues, element);

                if (isMatch)
                {
                    theScenario = element;
                    break;
                }
            }

            return theScenario;
        }
    }
}
