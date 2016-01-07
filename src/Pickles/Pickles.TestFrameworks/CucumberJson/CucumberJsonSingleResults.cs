//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CucumberJsonSingleResults.cs" company="PicklesDoc">
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
using System.IO;
using System.IO.Abstractions;
using System.Linq;

using Newtonsoft.Json;

using PicklesDoc.Pickles.ObjectModel;

using Feature = PicklesDoc.Pickles.Parser.JsonResult.Feature;

namespace PicklesDoc.Pickles.TestFrameworks.CucumberJson
{
    public class CucumberJsonSingleResults : ITestResults
    {
        private readonly List<Feature> resultsDocument;

        public CucumberJsonSingleResults(FileInfoBase configuration)
        {
            this.resultsDocument = this.ReadResultsFile(configuration);
        }

        public bool SupportsExampleResults
        {
            get { return false; }
        }

        public TestResult GetExampleResult(ScenarioOutline scenario, string[] exampleValues)
        {
            throw new NotSupportedException();
        }

        public TestResult GetFeatureResult(ObjectModel.Feature feature)
        {
            var cucumberFeature = this.GetFeatureElement(feature);
            return this.GetResultFromFeature(cucumberFeature);
        }

        public TestResult GetScenarioOutlineResult(ScenarioOutline scenarioOutline)
        {
            // Not applicable
            return new TestResult();
        }

        public TestResult GetScenarioResult(Scenario scenario)
        {
            Parser.JsonResult.Element cucumberScenario = null;
            var cucumberFeature = this.GetFeatureElement(scenario.Feature);
            if (cucumberFeature != null)
            {
                cucumberScenario = cucumberFeature.elements.FirstOrDefault(x => x.name == scenario.Name);
            }

            return this.GetResultFromScenario(cucumberScenario);
        }

        private List<Feature> ReadResultsFile(FileInfoBase testResultsFile)
        {
            List<Feature> result;
            using (var stream = testResultsFile.OpenRead())
            {
                using (var reader = new StreamReader(stream))
                {
                    result = JsonConvert.DeserializeObject<List<Feature>>(reader.ReadToEnd());
                }
            }

            return result;
        }

        private Feature GetFeatureElement(ObjectModel.Feature feature)
        {
            return this.resultsDocument.FirstOrDefault(x => x.name == feature.Name);
        }

        private TestResult GetResultFromScenario(Parser.JsonResult.Element cucumberScenario)
        {
            if (cucumberScenario == null)
            {
                return TestResult.Inconclusive;
            }

            bool wasSuccessful = CheckScenarioStatus(cucumberScenario);

            return wasSuccessful ? TestResult.Passed : TestResult.Failed;
        }

        private static bool CheckScenarioStatus(Parser.JsonResult.Element cucumberScenario)
        {
            return cucumberScenario.steps.All(x => x.result.status == "passed");
        }

        private TestResult GetResultFromFeature(Feature cucumberFeature)
        {
            if (cucumberFeature == null || cucumberFeature.elements == null)
            {
                return TestResult.Inconclusive;
            }

            cucumberFeature.elements.RemoveAll(e => e.type == "background");

            bool wasSuccessful = cucumberFeature.elements.All(CheckScenarioStatus);

            return wasSuccessful ? TestResult.Passed : TestResult.Failed;
        }
    }
}
