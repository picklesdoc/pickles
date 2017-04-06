//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenParsingMultipleTestResultsTests.cs" company="PicklesDoc">
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

using NFluent;

using NSubstitute;

using NUnit.Framework;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.TestFrameworks.UnitTests.SpecRun
{
    [TestFixture]
    public class WhenParsingMultipleTestResultsTests
    {
        [Test]
        public void GetFeatureResult_OnePassingOneInconclusive_ReturnsPassed()
        {
            var feature = new Feature();

            var testResults1 = SetupStubForGetFeatureResult(feature, TestResult.Passed);
            var testResults2 = SetupStubForGetFeatureResult(feature, TestResult.Inconclusive);

            ITestResults multipleTestResults = CreateMultipleTestResults(testResults1, testResults2);

            var result = multipleTestResults.GetFeatureResult(feature);

            Check.That(result).IsEqualTo(TestResult.Passed);
        }

        private static MultipleTestRunsBase CreateMultipleTestResults(SingleTestRunBase testResults1, SingleTestRunBase testResults2)
        {
            return new TestableMultipleTestResults(new[] { testResults1, testResults2 });
        }

        private class TestableMultipleTestResults : MultipleTestRunsBase
        {
            public TestableMultipleTestResults(IEnumerable<SingleTestRunBase> testResults)
                : base(testResults)
            {
            }
        }

        private static SingleTestRunBase SetupStubForGetFeatureResult(Feature feature, TestResult resultOfGetFeatureResult)
        {
            var testResults1 = Substitute.For<SingleTestRunBase>();
            testResults1.GetFeatureResult(feature).Returns(resultOfGetFeatureResult);
            return testResults1;
        }

        [Test]
        public void GetFeatureResult_OnePassingOneFailed_ReturnsPassed()
        {
            var feature = new Feature();

            var testResults1 = SetupStubForGetFeatureResult(feature, TestResult.Passed);
            var testResults2 = SetupStubForGetFeatureResult(feature, TestResult.Failed);

            ITestResults multipleTestResults = CreateMultipleTestResults(testResults1, testResults2);

            var result = multipleTestResults.GetFeatureResult(feature);

            Check.That(result).IsEqualTo(TestResult.Failed);
        }

        [Test]
        public void GetFeatureResult_TwoInconclusive_ReturnsInconclusive()
        {
            var feature = new Feature();

            var testResults1 = SetupStubForGetFeatureResult(feature, TestResult.Inconclusive);
            var testResults2 = SetupStubForGetFeatureResult(feature, TestResult.Inconclusive);

            ITestResults multipleTestResults = CreateMultipleTestResults(testResults1, testResults2);

            var result = multipleTestResults.GetFeatureResult(feature);

            Check.That(result).IsEqualTo(TestResult.Inconclusive);
        }

        [Test]
        public void GetScenarioOutlineResult_OnePassingOneInconclusive_ReturnsPassed()
        {
            var scenarioOutline = new ScenarioOutline();

            var testResults1 = SetupStubForGetScenarioOutlineResult(TestResult.Passed);
            var testResults2 = SetupStubForGetScenarioOutlineResult(TestResult.Inconclusive);

            ITestResults multipleTestResults = CreateMultipleTestResults(testResults1, testResults2);

            var result = multipleTestResults.GetScenarioOutlineResult(scenarioOutline);

            Check.That(result).IsEqualTo(TestResult.Passed);
        }

        private static SingleTestRunBase SetupStubForGetScenarioOutlineResult(TestResult resultOfGetFeatureResult)
        {
            var testResults1 = Substitute.For<SingleTestRunBase>();
            testResults1.GetScenarioOutlineResult(Arg.Any<ScenarioOutline>()).Returns(resultOfGetFeatureResult);
            return testResults1;
        }

        [Test]
        public void GetScenarioOutlineResult_OnePassingOneFailing_ReturnsFailed()
        {
            var scenarioOutline = new ScenarioOutline();

            var testResults1 = SetupStubForGetScenarioOutlineResult(TestResult.Passed);
            var testResults2 = SetupStubForGetScenarioOutlineResult(TestResult.Failed);

            ITestResults multipleTestResults = CreateMultipleTestResults(testResults1, testResults2);

            var result = multipleTestResults.GetScenarioOutlineResult(scenarioOutline);

            Check.That(result).IsEqualTo(TestResult.Failed);
        }

        [Test]
        public void GetScenarioOutlineResult_TwoInconclusive_ReturnsInconclusive()
        {
            var scenarioOutline = new ScenarioOutline();

            var testResults1 = SetupStubForGetScenarioOutlineResult(TestResult.Inconclusive);
            var testResults2 = SetupStubForGetScenarioOutlineResult(TestResult.Inconclusive);

            ITestResults multipleTestResults = CreateMultipleTestResults(testResults1, testResults2);

            var result = multipleTestResults.GetScenarioOutlineResult(scenarioOutline);

            Check.That(result).IsEqualTo(TestResult.Inconclusive);
        }

        [Test]
        public void GetScenarioResult_OnePassingOneInconclusive_ReturnsPassed()
        {
            var scenario = new Scenario();

            var testResults1 = SetupStubForGetScenarioResult(TestResult.Passed);
            var testResults2 = SetupStubForGetScenarioResult(TestResult.Inconclusive);

            ITestResults multipleTestResults = CreateMultipleTestResults(testResults1, testResults2);

            var result = multipleTestResults.GetScenarioResult(scenario);

            Check.That(result).IsEqualTo(TestResult.Passed);
        }

        private static SingleTestRunBase SetupStubForGetScenarioResult(TestResult resultOfGetFeatureResult)
        {
            var testResults1 = Substitute.For<SingleTestRunBase>();
            testResults1.GetScenarioResult(Arg.Any<Scenario>()).Returns(resultOfGetFeatureResult);
            return testResults1;
        }

        [Test]
        public void GetScenarioResult_OnePassingOneFailing_ReturnsFailed()
        {
            var scenario = new Scenario();

            var testResults1 = SetupStubForGetScenarioResult(TestResult.Passed);
            var testResults2 = SetupStubForGetScenarioResult(TestResult.Failed);

            ITestResults multipleTestResults = CreateMultipleTestResults(testResults1, testResults2);

            var result = multipleTestResults.GetScenarioResult(scenario);

            Check.That(result).IsEqualTo(TestResult.Failed);
        }

        [Test]
        public void GetScenarioResult_TwoInconclusive_ReturnsInconclusive()
        {
            var scenario = new Scenario();

            var testResults1 = SetupStubForGetScenarioResult(TestResult.Inconclusive);
            var testResults2 = SetupStubForGetScenarioResult(TestResult.Inconclusive);

            ITestResults multipleTestResults = CreateMultipleTestResults(testResults1, testResults2);

            var result = multipleTestResults.GetScenarioResult(scenario);

            Check.That(result).IsEqualTo(TestResult.Inconclusive);
        }
    }
}
