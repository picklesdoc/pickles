using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PicklesDoc.Pickles.Parser;
using  CucumberObjects = PicklesDoc.Pickles.Parser.JsonResult;

namespace PicklesDoc.Pickles.TestFrameworks
{
    public class CucumberJsonResults : ITestResults
    {
        private readonly Configuration configuration;
        private List<CucumberObjects.Feature> resultsDocument;

        public CucumberJsonResults(Configuration configuration)
        {
            this.configuration = configuration;
            if (configuration.HasTestResults)
            {
                this.resultsDocument = this.ReadResultsFile();
            }
        }

        private List<CucumberObjects.Feature> ReadResultsFile()
        {
            List<CucumberObjects.Feature> result;
            using (Stream stream = this.configuration.TestResultsFile.OpenRead())
            {
                using (var reader = new StreamReader(stream))
                {
                    result = JsonConvert.DeserializeObject<List<CucumberObjects.Feature>>(reader.ReadToEnd());
                }
            }
            return result;
        }

        #region ITestResults Members

        public TestResult GetFeatureResult(Feature feature)
        {
            CucumberObjects.Feature cucumberFeature = this.GetFeatureElement(feature);
            return this.GetResultFromFeature(cucumberFeature);

        }

        public TestResult GetScenarioOutlineResult(ScenarioOutline scenarioOutline)
        {
            //Not applicable
            return new TestResult();
        }

        public TestResult GetScenarioResult(Scenario scenario)
        {
            CucumberObjects.Element cucumberScenario  = null;
            CucumberObjects.Feature cucumberFeature = this.GetFeatureElement(scenario.Feature);
            if(cucumberFeature != null)
             cucumberScenario = cucumberFeature.elements.FirstOrDefault(x => x.name == scenario.Name);
            return this.GetResultFromScenario(cucumberScenario);

        }

        #endregion

        private CucumberObjects.Feature GetFeatureElement(Feature feature)
        {
            return this.resultsDocument.FirstOrDefault(x => x.name == feature.Name);
        }

        private TestResult GetResultFromScenario(CucumberObjects.Element cucumberScenario)
        {
            if (cucumberScenario == null) return new TestResult { WasExecuted = false, WasSuccessful = false };
            bool wasExecuted = true;
            bool wasSuccessful = CheckScenarioStatus(cucumberScenario);


            return new TestResult { WasExecuted = wasExecuted, WasSuccessful = wasSuccessful };
        }

        private static bool CheckScenarioStatus(CucumberObjects.Element cucumberScenario)
        {
            return cucumberScenario.steps.All(x => x.result.status == "passed");
        }

        private TestResult GetResultFromFeature(CucumberObjects.Feature cucumberFeature)
        {
            if (cucumberFeature == null) return new TestResult { WasExecuted = false, WasSuccessful = false };
            bool wasExecuted = true;
            bool wasSuccessful = cucumberFeature.elements.All(CheckScenarioStatus);

            return new TestResult { WasExecuted = wasExecuted, WasSuccessful = wasSuccessful };
        }

    }
}
