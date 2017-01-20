//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenParsingCucumberJsonFromJSResultsFile.cs" company="PicklesDoc">
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
using PicklesDoc.Pickles.TestFrameworks.CucumberJson;

using Feature = PicklesDoc.Pickles.ObjectModel.Feature;

namespace PicklesDoc.Pickles.TestFrameworks.UnitTests.CucumberJson
{
    [TestFixture]
    public class WhenParsingCucumberJsonFromJSResultsFileWithMissingElements : StandardTestSuite<CucumberJsonResults>
    {
        public WhenParsingCucumberJsonFromJSResultsFileWithMissingElements()
            : base("CucumberJson." + "results-example-cucumberjs-missing-elements-array.json")
        {
        }

        [Test]
        public void TestResultsFileIsMessingElementsForAFeature_ShouldReturnInconclusive()
        {
            var results = ParseResultsFile();
            var feature = new Feature { Name = "Feature That Has No Scenarios In The Test Run" };
            var scenarioWithoutElements = new Scenario { Name = "Ignored scenario", Feature = feature };

            var result = results.GetScenarioResult(scenarioWithoutElements);

            Check.That(result).IsEqualTo(TestResult.Inconclusive);
        }
    }
}
