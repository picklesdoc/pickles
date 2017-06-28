//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenParsingProblematicScenarioOutlineResults.cs" company="PicklesDoc">
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
using PicklesDoc.Pickles.TestFrameworks.NUnit.NUnit2;

namespace PicklesDoc.Pickles.TestFrameworks.UnitTests.NUnit.NUnit2
{
    [TestFixture]
    public class WhenParsingProblematicScenarioOutlineResults : WhenParsingTestResultFiles<NUnit2Results>
    {
        public WhenParsingProblematicScenarioOutlineResults()
            : base("NUnit.NUnit2." + "results-problem-with-outline-results-nunit.xml")
        {
        }

        [Test]
        public void ThenCanReadIndividualResultsFromScenarioOutline_ContainingDollarSigns_ShouldBeTestResultSuccess()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "ExampleWebFeature" , Tags = { "@web" }};

            var scenarioOutline = new ScenarioOutline { Name = "Login", Feature = feature };

            TestResult exampleResultOutline = results.GetScenarioOutlineResult(scenarioOutline);
            Check.That(exampleResultOutline).IsEqualTo(TestResult.Passed);

            TestResult exampleResult = results.GetExampleResult(scenarioOutline, new[] { "special characters", "$$$" });
            Check.That(exampleResult).IsEqualTo(TestResult.Passed);
        }
    }
}