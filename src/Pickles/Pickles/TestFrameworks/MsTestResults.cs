#region License

/*
    Copyright [2011] [Jeffrey Cameron]

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using PicklesDoc.Pickles.Parser;

namespace PicklesDoc.Pickles.TestFrameworks
{
    public class MsTestResults : ITestResults
    {
        private static readonly XNamespace ns = @"http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
        private readonly Configuration configuration;
        private readonly XDocument resultsDocument;

        public MsTestResults(Configuration configuration)
        {
            this.configuration = configuration;
            if (configuration.HasTestResults)
            {
                this.resultsDocument = this.ReadResultsFile();
            }
        }

        private XDocument ReadResultsFile()
        {
            XDocument document;
            using (var stream = this.configuration.TestResultsFile.OpenRead())
            {
                var xmlReader = XmlReader.Create(stream);
                document = XDocument.Load(xmlReader);
                stream.Close();
            }
            return document;
        }

        private Guid GetScenarioExecutionId(Scenario queriedScenario)
        {
            var idString =
                (from scenario in AllScenariosInResultFile()
                 let properties = PropertiesOf(scenario)
                 where properties != null
                 where FeatureNamePropertyExistsWith(queriedScenario.Feature.Name, among: properties)
                 where NameOf(scenario) == queriedScenario.Name
                 select ScenarioExecutionIdStringOf(scenario)).FirstOrDefault();

            return !string.IsNullOrEmpty(idString) ? new Guid(idString) : Guid.Empty;
        }

        private TestResult GetExecutionResult(Guid scenarioExecutionId)
        {
            var resultText =
                (from scenarioResult in AllScenarioExecutionResultsInResultFile()
                 let executionId = ResultExecutionIdOf(scenarioResult)
                 where scenarioExecutionId == executionId
                 let outcome = ResultOutcomeOf(scenarioResult)
                 select outcome).FirstOrDefault() ?? string.Empty;

            switch (resultText.ToLowerInvariant())
            {
                case "passed":
                    return TestResult.Passed();
                case "failed":
                    return TestResult.Failed();
                default:
                    return TestResult.Inconclusive();
            }
        }

        private static string ResultOutcomeOf(XElement scenarioResult)
        {
            return scenarioResult.Attribute("outcome").Value;
        }

        private static Guid ResultExecutionIdOf(XElement unitTestResult)
        {
            return new Guid(unitTestResult.Attribute("executionId").Value);
        }

        #region ITestResults Members

        #endregion

        #region Linq Helpers

        public TestResult GetFeatureResult(Feature feature)
        {
            var featureExecutionIds =
                from scenario in AllScenariosInResultFile()
                let properties = PropertiesOf(scenario)
                where properties != null
                where FeatureNamePropertyExistsWith(feature.Name, among: properties)
                select ScenarioExecutionIdOf(scenario);

            TestResult result = featureExecutionIds.Select(this.GetExecutionResult).Merge();

            return result;
        }

        public TestResult GetScenarioOutlineResult(ScenarioOutline scenarioOutline)
        {
            var queriedFeatureName = scenarioOutline.Feature.Name;
            var queriedScenarioOutlineName = scenarioOutline.Name;

            var allScenariosForAFeature =
                from scenario in AllScenariosInResultFile()
                let scenarioProperties = PropertiesOf(scenario)
                where scenarioProperties != null
                where FeatureNamePropertyExistsWith(queriedFeatureName, among: scenarioProperties)
                select scenario;

            var scenarioOutlineExecutionIds = from scenario in allScenariosForAFeature
                                              where NameOf(scenario) == queriedScenarioOutlineName
                                              select ScenarioExecutionIdOf(scenario);

            TestResult result = scenarioOutlineExecutionIds.Select(this.GetExecutionResult).Merge();

            return result;
        }


        public TestResult GetScenarioResult(Scenario scenario)
        {
            Guid scenarioExecutionId = this.GetScenarioExecutionId(scenario);
            return this.GetExecutionResult(scenarioExecutionId);
        }


        private static Guid ScenarioExecutionIdOf(XElement scenario)
        {
            return new Guid(ScenarioExecutionIdStringOf(scenario));
        }

        private static string ScenarioExecutionIdStringOf(XElement scenario)
        {
            return scenario.Element(ns + "Execution").Attribute("id").Value;
        }

        private static string NameOf(XElement scenario)
        {
            return scenario.Element(ns + "Description").Value;
        }

        private static XElement PropertiesOf(XElement scenariosReportes)
        {
            return scenariosReportes.Element(ns + "Properties");
        }

        private static bool FeatureNamePropertyExistsWith(string featureName, XElement among)
        {
            var properties = among;
            return (from property in properties.Elements(ns + "Property")
                    let key = property.Element(ns + "Key")
                    let value = property.Element(ns + "Value")
                    where key.Value == "FeatureTitle" && value.Value == featureName
                    select property).Any();
        }

        private IEnumerable<XElement> AllScenariosInResultFile()
        {
            return this.resultsDocument.Root.Descendants(ns + "UnitTest");
        }

        private IEnumerable<XElement> AllScenarioExecutionResultsInResultFile()
        {
            return this.resultsDocument.Root.Descendants(ns + "UnitTestResult");
        }

        #endregion
    }
}
