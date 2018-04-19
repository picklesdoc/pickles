//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StandardTestSuite.cs" company="PicklesDoc">
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

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.TestFrameworks.UnitTests
{
    public abstract class StandardTestSuite<TResults> : WhenParsingTestResultFiles<TResults>
        where TResults : ITestResults
    {
        protected StandardTestSuite(string resultsFileName)
            : base(resultsFileName)
        {
        }

        public void ThenCanReadFeatureResultSuccessfully()
        {
            // Write out the embedded test results file
            var results = ParseResultsFile();

            var feature = this.AdditionFeature();
            TestResult result = results.GetFeatureResult(feature);

            Check.That(result).IsEqualTo(TestResult.Failed);
        }

        public void ThenCanReadBackgroundResultSuccessfully()
        {
            var background = new Scenario { Name = "Background", Feature = this.AdditionFeature() };
            var feature = this.AdditionFeature();
            feature.AddBackground(background);
            var results = ParseResultsFile();

            TestResult result = results.GetScenarioResult(background);

            Check.That(result).IsEqualTo(TestResult.Inconclusive);
        }

        public void ThenCanReadInconclusiveFeatureResultSuccessfully()
        {
            var results = ParseResultsFile();

            TestResult result = results.GetFeatureResult(this.InconclusiveFeature());

            Check.That(result).IsEqualTo(TestResult.Inconclusive);
        }


        public void ThenCanReadFailedFeatureResultSuccessfully()
        {
            var results = ParseResultsFile();

            TestResult result = results.GetFeatureResult(this.FailingFeature());

            Check.That(result).IsEqualTo(TestResult.Failed);
        }

        public void ThenCanReadPassedFeatureResultSuccessfully()
        {
            var results = ParseResultsFile();

            TestResult result = results.GetFeatureResult(this.PassingFeature());

            Check.That(result).IsEqualTo(TestResult.Passed);
        }

        public void ThenCanReadScenarioOutlineResultSuccessfully()
        {
            var results = ParseResultsFile();
            var scenarioOutline = new ScenarioOutline { Name = "Adding several numbers", Feature = this.AdditionFeature() };

            TestResult result = results.GetScenarioOutlineResult(scenarioOutline);

            Check.That(result).IsEqualTo(TestResult.Passed);

            TestResult exampleResult1 = results.GetExampleResult(scenarioOutline, new[] { "40", "50", "90", "180" });
            Check.That(exampleResult1).IsEqualTo(TestResult.Passed);

            TestResult exampleResult2 = results.GetExampleResult(scenarioOutline, new[] { "60", "70", "130", "260" });
            Check.That(exampleResult2).IsEqualTo(TestResult.Passed);
        }

        public void ThenCanReadSuccessfulScenarioResultSuccessfully()
        {
            var results = ParseResultsFile();
            var passedScenario = new Scenario { Name = "Add two numbers", Feature = this.AdditionFeature() };

            TestResult result = results.GetScenarioResult(passedScenario);

            Check.That(result).IsEqualTo(TestResult.Passed);
        }

        public void ThenCanReadFailedScenarioResultSuccessfully()
        {
            var results = ParseResultsFile();
            var scenario = new Scenario { Name = "Fail to add two numbers", Feature = this.AdditionFeature() };
            TestResult result = results.GetScenarioResult(scenario);

            Check.That(result).IsEqualTo(TestResult.Failed);
        }

        public void ThenCanReadIgnoredScenarioResultSuccessfully()
        {
            var results = ParseResultsFile();
            var ignoredScenario = new Scenario { Name = "Ignored adding two numbers", Feature = this.AdditionFeature() };

            var result = results.GetScenarioResult(ignoredScenario);

            Check.That(result).IsEqualTo(TestResult.Inconclusive);
        }

        public void ThenCanReadInconclusiveScenarioResultSuccessfully()
        {
            var results = ParseResultsFile();

            var inconclusiveScenario = new Scenario
            {
                Name = "Not automated adding two numbers",
                Feature = this.AdditionFeature()
            };

            var result = results.GetScenarioResult(inconclusiveScenario);

            Check.That(result).IsEqualTo(TestResult.Inconclusive);
        }

        public void ThenCanReadNotFoundScenarioCorrectly()
        {
            var results = ParseResultsFile();
            var notFoundScenario = new Scenario
            {
                Name = "Not in the file at all!",
                Feature = this.AdditionFeature()
            };

            var result = results.GetScenarioResult(notFoundScenario);

            Check.That(result).IsEqualTo(TestResult.Inconclusive);
        }

        public void ThenCanReadNotFoundFeatureCorrectly()
        {
            var results = ParseResultsFile();
            var result = results.GetFeatureResult(new Feature { Name = "NotInTheFile" });

            Check.That(result).IsEqualTo(TestResult.Inconclusive);
        }

        public void ThenCanReadResultsWithBackslashes()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Scenario Outlines" };

            var scenarioOutline = new ScenarioOutline { Name = "Deal correctly with backslashes in the examples", Feature = feature };

            TestResult exampleResultOutline = results.GetScenarioOutlineResult(scenarioOutline);
            Check.That(exampleResultOutline).IsEqualTo(TestResult.Passed);

            TestResult exampleResult1 = results.GetExampleResult(scenarioOutline, new[] { @"c:\Temp\" });
            Check.That(exampleResult1).IsEqualTo(TestResult.Passed);
        }

        public void ThenCanReadResultsWithParenthesis()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Scenario Outlines" };

            var scenarioOutline = new ScenarioOutline { Name = "Deal correctly with parenthesis in the examples", Feature = feature };

            TestResult exampleResultOutline = results.GetScenarioOutlineResult(scenarioOutline);
            Check.That(exampleResultOutline).IsEqualTo(TestResult.Passed);

            TestResult exampleResult1 = results.GetExampleResult(scenarioOutline, new[] { @"This is a description (and more)" });
            Check.That(exampleResult1).IsEqualTo(TestResult.Passed);
        }

        public void ThenCanReadResultOfScenarioWithFailingBackground()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Failing Background" };

            var scenario = new Scenario { Name = "Add two numbers", Feature = feature };

            var actualResult = results.GetScenarioResult(scenario);

            Check.That(actualResult).IsEqualTo(TestResult.Failed);
        }

        public void ThenCanReadResultOfScenarioOutlineWithFailingBackground()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Failing Background" };

            var scenarioOutline = new ScenarioOutline { Name = "Adding several numbers", Feature = feature };

            var actualResult = results.GetScenarioOutlineResult(scenarioOutline);

            Check.That(actualResult).IsEqualTo(TestResult.Failed);
        }

        public void ThenCanReadResultOfScenarioOutlineExampleWithFailingBackground()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Failing Background" };

            var scenarioOutline = new ScenarioOutline { Name = "Adding several numbers", Feature = feature };

            var actualResult = results.GetExampleResult(scenarioOutline, new string[] { "60", "70", "130", "260" });

            Check.That(actualResult).IsEqualTo(TestResult.Failed);
        }

        public void ThenCanReadResultOfFeatureWithFailingBackground()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Failing Background" };

            var actualResult = results.GetFeatureResult(feature);

            Check.That(actualResult).IsEqualTo(TestResult.Failed);
        }

        public void ThenCanReadResultOfScenarioWithSpecialCharacters()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Scenarios With Special Characters" };

            var scenario = new Scenario{ Name = "This is a scenario with parentheses, hyphen and comma (10-20, 30-40)", Feature = feature };

            var actualResult = results.GetScenarioResult(scenario);
            Check.That(actualResult).IsEqualTo(TestResult.Passed);
        }

        public void ThenCanReadResultOfScenarioOutlineWithSpecialCharacters()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Scenarios With Special Characters" };

            var scenarioOutline = new ScenarioOutline { Name = "This is a scenario outline with parentheses, hyphen and comma (10-20, 30-40)", Feature = feature };

            var actualResult = results.GetExampleResult(scenarioOutline, new string[] { "pass_1" });

            Check.That(actualResult).IsEqualTo(TestResult.Passed);
        }

        public void ThenCanReadResultOfScenarioOutlineWithUmlauts()
        {
          var results = ParseResultsFile();

          var feature = new Feature { Name = "Scenarios With Special Characters" };

          var scenarioOutline = new ScenarioOutline { Name = "This is a scenario outline with german umlauts äöüß ÄÖÜ", Feature = feature };

          var actualResult = results.GetExampleResult(scenarioOutline, new string[] { "pass_1" });

          Check.That(actualResult).IsEqualTo(TestResult.Passed);
        }

        public void ThenCanReadResultOfScenarioWithDanishCharacters()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Scenarios With Special Characters" };

            var scenario = new Scenario { Name = "This is a scenario with danish characters æøå ÆØÅ", Feature = feature };

            var actualResult = results.GetScenarioResult(scenario);

            Check.That(actualResult).IsEqualTo(TestResult.Passed);
        }

        public void ThenCanReadResultOfScenarioWithSpanishCharacters()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Scenarios With Special Characters" };

            var scenario = new Scenario { Name = "This is a scenario with spanish characters ñáéíóú", Feature = feature };

            var actualResult = results.GetScenarioResult(scenario);

            Check.That(actualResult).IsEqualTo(TestResult.Passed);
        }

        public void ThenCanReadResultOfScenarioOutlineWithAmpersand()
        {
          var results = ParseResultsFile();

          var feature = new Feature { Name = "Scenarios With Special Characters" };

          var scenarioOutline = new ScenarioOutline { Name = "This is a scenario outline with ampersand &", Feature = feature };

          var actualResult = results.GetExampleResult(scenarioOutline, new string[] { "pass_1" });

          Check.That(actualResult).IsEqualTo(TestResult.Passed);
        }

        private Feature AdditionFeature()
        {
            return new Feature { Name = "Addition" };
        }

        private Feature InconclusiveFeature()
        {
            return new Feature { Name = "Inconclusive" };
        }

        private Feature FailingFeature()
        {
            return new Feature { Name = "Failing" };
        }

        private Feature PassingFeature()
        {
            return new Feature { Name = "Passing" };
        }
    }
}