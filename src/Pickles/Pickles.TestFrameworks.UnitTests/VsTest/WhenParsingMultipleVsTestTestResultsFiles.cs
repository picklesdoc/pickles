//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenParsingMultipleVsTestTestResultsFiles.cs" company="PicklesDoc">
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

using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.TestFrameworks.VsTest;

namespace PicklesDoc.Pickles.TestFrameworks.UnitTests.VsTest
{
    [TestFixture]
    public class WhenParsingMultipleVsTestTestResultsFiles : WhenParsingTestResultFiles<VsTestResults>
    {
        public WhenParsingMultipleVsTestTestResultsFiles()
            : base("VsTest." + "results-example-vstest - Run 1 (failing).trx;" + "VsTest." + "results-example-vstest - Run 2 (passing).trx")
        {
        }

        [Test]
        public void ThenCanReadFailedFeatureResultSuccessfully()
        {
            var results = ParseResultsFile();

            TestResult result = results.GetFeatureResult(new Feature { Name = "Failing" });

            Check.That(result).IsEqualTo(TestResult.Failed);
        }

        [Test]
        public void ThenCanReadPassedScenarioResultSuccessfully()
        {
            var results = ParseResultsFile();

            var scenario = new Scenario
            {
                Name = "Failing Feature Passing Scenario",
                Feature = new Feature { Name = "Failing" }
            };

            var result = results.GetScenarioResult(scenario);

            Check.That(result).IsEqualTo(TestResult.Passed);
        }

        [Test]
        public void ThenCanReadFailedScenarioResultSuccessfully()
        {
            var results = ParseResultsFile();

            var scenario = new Scenario
            {
                Name = "Failing Feature Failing Scenario",
                Feature = new Feature { Name = "Failing" }
            };

            var result = results.GetScenarioResult(scenario);

            Check.That(result).IsEqualTo(TestResult.Failed);
        }
    }
}
