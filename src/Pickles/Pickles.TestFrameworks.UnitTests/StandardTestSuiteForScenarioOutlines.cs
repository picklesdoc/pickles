//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StandardTestSuiteForScenarioOutlines.cs" company="PicklesDoc">
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


using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.TestFrameworks.UnitTests
{
    public class StandardTestSuiteForScenarioOutlines<TResults> : WhenParsingTestResultFiles<TResults>
        where TResults : ITestResults
    {
        private readonly TestResult valueForInconclusive;

        protected StandardTestSuiteForScenarioOutlines(string resultsFileName, bool treatInconclusiveAsFailed = false)
            : base(resultsFileName)
        {
            this.valueForInconclusive = treatInconclusiveAsFailed ? TestResult.Failed : TestResult.Inconclusive;
        }

        public void ThenCanReadIndividualResultsFromScenarioOutline_AllPass_ShouldBeTestResultPassed()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Scenario Outlines" };

            var scenarioOutline = new ScenarioOutline { Name = "This is a scenario outline where all scenarios pass", Feature = feature };

            TestResult exampleResultOutline = results.GetScenarioOutlineResult(scenarioOutline);
            Check.That(exampleResultOutline).IsEqualTo(TestResult.Passed);

            TestResult exampleResult1 = results.GetExampleResult(scenarioOutline, new[] { "pass_1" });
            Check.That(exampleResult1).IsEqualTo(TestResult.Passed);

            TestResult exampleResult2 = results.GetExampleResult(scenarioOutline, new[] { "pass_2" });
            Check.That(exampleResult2).IsEqualTo(TestResult.Passed);

            TestResult exampleResult3 = results.GetExampleResult(scenarioOutline, new[] { "pass_3" });
            Check.That(exampleResult3).IsEqualTo(TestResult.Passed);
        }

        public void ThenCanReadIndividualResultsFromScenarioOutline_OneInconclusive_ShouldBeTestResultInconclusive()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Scenario Outlines" };

            var scenarioOutline = new ScenarioOutline { Name = "This is a scenario outline where one scenario is inconclusive", Feature = feature };

            TestResult exampleResultOutline = results.GetScenarioOutlineResult(scenarioOutline);
            Check.That(exampleResultOutline).IsEqualTo(this.valueForInconclusive);

            TestResult exampleResult1 = results.GetExampleResult(scenarioOutline, new[] { "pass_1" });
            Check.That(exampleResult1).IsEqualTo(TestResult.Passed);

            TestResult exampleResult2 = results.GetExampleResult(scenarioOutline, new[] { "pass_2" });
            Check.That(exampleResult2).IsEqualTo(TestResult.Passed);

            TestResult exampleResult3 = results.GetExampleResult(scenarioOutline, new[] { "inconclusive_1" });
            Check.That(exampleResult3).IsEqualTo(this.valueForInconclusive);
        }

        public void ThenCanReadIndividualResultsFromScenarioOutline_OneFailed_ShouldBeTestResultFailed()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Scenario Outlines" };

            var scenarioOutline = new ScenarioOutline { Name = "This is a scenario outline where one scenario fails", Feature = feature };

            TestResult exampleResultOutline = results.GetScenarioOutlineResult(scenarioOutline);
            Check.That(exampleResultOutline).IsEqualTo(TestResult.Failed);

            TestResult exampleResult1 = results.GetExampleResult(scenarioOutline, new[] { "pass_1" });
            Check.That(exampleResult1).IsEqualTo(TestResult.Passed);

            TestResult exampleResult2 = results.GetExampleResult(scenarioOutline, new[] { "pass_2" });
            Check.That(exampleResult2).IsEqualTo(TestResult.Passed);

            TestResult exampleResult3 = results.GetExampleResult(scenarioOutline, new[] { "fail_1" });
            Check.That(exampleResult3).IsEqualTo(TestResult.Failed);
        }

        public void ThenCanReadIndividualResultsFromScenarioOutline_MultipleExampleSections_ShouldBeTestResultFailed()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Scenario Outlines" };

            var scenarioOutline = new ScenarioOutline { Name = "And we can go totally bonkers with multiple example sections.", Feature = feature };

            TestResult exampleResultOutline = results.GetScenarioOutlineResult(scenarioOutline);
            Check.That(exampleResultOutline).IsEqualTo(TestResult.Failed);

            TestResult exampleResult1 = results.GetExampleResult(scenarioOutline, new[] { "pass_1" });
            Check.That(exampleResult1).IsEqualTo(TestResult.Passed);

            TestResult exampleResult2 = results.GetExampleResult(scenarioOutline, new[] { "pass_2" });
            Check.That(exampleResult2).IsEqualTo(TestResult.Passed);

            TestResult exampleResult3 = results.GetExampleResult(scenarioOutline, new[] { "inconclusive_1" });
            Check.That(exampleResult3).IsEqualTo(this.valueForInconclusive);

            TestResult exampleResult4 = results.GetExampleResult(scenarioOutline, new[] { "inconclusive_2" });
            Check.That(exampleResult4).IsEqualTo(this.valueForInconclusive);

            TestResult exampleResult5 = results.GetExampleResult(scenarioOutline, new[] { "fail_1" });
            Check.That(exampleResult5).IsEqualTo(TestResult.Failed);

            TestResult exampleResult6 = results.GetExampleResult(scenarioOutline, new[] { "fail_2" });
            Check.That(exampleResult6).IsEqualTo(TestResult.Failed);
        }

        public void ThenCanReadExamplesWithRegexValuesFromScenarioOutline_ShouldBeTestResultPassed()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Scenarios With Special Characters" };

            var scenarioOutline = new ScenarioOutline { Name = "This scenario contains examples with Regex-special characters", Feature = feature };

            TestResult exampleResultOutline = results.GetScenarioOutlineResult(scenarioOutline);
            Check.That(exampleResultOutline).IsEqualTo(TestResult.Passed);

            TestResult exampleResult1 = results.GetExampleResult(scenarioOutline, new[] { "**" });
            Check.That(exampleResult1).IsEqualTo(TestResult.Passed);

            TestResult exampleResult2 = results.GetExampleResult(scenarioOutline, new[] { "++" });
            Check.That(exampleResult2).IsEqualTo(TestResult.Passed);

            TestResult exampleResult3 = results.GetExampleResult(scenarioOutline, new[] { ".*" });
            Check.That(exampleResult3).IsEqualTo(TestResult.Passed);

            TestResult exampleResult4 = results.GetExampleResult(scenarioOutline, new[] { "[]" });
            Check.That(exampleResult4).IsEqualTo(TestResult.Passed);

            TestResult exampleResult5 = results.GetExampleResult(scenarioOutline, new[] { "{}" });
            Check.That(exampleResult5).IsEqualTo(TestResult.Passed);

            TestResult exampleResult6 = results.GetExampleResult(scenarioOutline, new[] { "()" });
            Check.That(exampleResult6).IsEqualTo(TestResult.Passed);

            TestResult exampleResult7 = results.GetExampleResult(scenarioOutline, new[] { @"^.*(?<foo>BAR)\s[^0-9]{3,4}A+$" });
            Check.That(exampleResult7).IsEqualTo(TestResult.Passed);
        }
    }
}
