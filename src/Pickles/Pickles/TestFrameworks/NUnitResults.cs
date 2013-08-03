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
using System.IO; // this is a legitimate usage of System.IO
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using PicklesDoc.Pickles.Parser;

namespace PicklesDoc.Pickles.TestFrameworks
{
    public class NUnitResults : ITestResults
    {
        private readonly Configuration configuration;
        private readonly NUnitExampleSignatureBuilder exampleSignatureBuilder;
        private readonly XDocument resultsDocument;

        public NUnitResults(Configuration configuration, NUnitExampleSignatureBuilder exampleSignatureBuilder)
        {
            this.configuration = configuration;
            this.exampleSignatureBuilder = exampleSignatureBuilder;
            if (configuration.HasTestResults)
            {
                this.resultsDocument = this.ReadResultsFile();
            }
        }


        #region ITestResults Members

        public TestResult GetFeatureResult(Feature feature)
        {
            var featureElement = this.GetFeatureElement(feature);

            var results = featureElement.Descendants("test-case")
                .Select(GetResultFromElement);

            return results.Merge();
        }

        public TestResult GetScenarioResult(Scenario scenario)
        {
            XElement featureElement = this.GetFeatureElement(scenario.Feature);
            XElement scenarioElement = null;
            if (featureElement != null)
            {
                scenarioElement = featureElement
                    .Descendants("test-case")
                    .Where(x => x.Attribute("description") != null)
                    .FirstOrDefault(x => x.Attribute("description").Value == scenario.Name);
            }
            return this.GetResultFromElement(scenarioElement);
        }

        public TestResult GetScenarioOutlineResult(ScenarioOutline scenarioOutline)
        {
            XElement featureElement = this.GetFeatureElement(scenarioOutline.Feature);
            XElement scenarioOutlineElement = null;
            if (featureElement != null)
            {
                scenarioOutlineElement = this.GetFeatureElement(scenarioOutline.Feature)
                    .Descendants("test-suite")
                    .Where(x => x.Attribute("description") != null)
                    .FirstOrDefault(x => x.Attribute("description").Value == scenarioOutline.Name);
            }
            return this.GetResultFromElement(scenarioOutlineElement);
        }

        #endregion

        private XDocument ReadResultsFile()
        {
            XDocument document;
            using (Stream stream = this.configuration.TestResultsFile.OpenRead())
            {
                XmlReader xmlReader = XmlReader.Create(stream);
                document = XDocument.Load(xmlReader);
                stream.Close();
            }
            return document;
        }

        private XElement GetFeatureElement(Feature feature)
        {
            return this.resultsDocument
                .Descendants("test-suite")
                .Where(x => x.Attribute("description") != null)
                .FirstOrDefault(x => x.Attribute("description").Value == feature.Name);
        }

        private TestResult GetResultFromElement(XElement element)
        {
            if (element == null)
            {
                return TestResult.NotFound();
            }
            else if (IsAttributeSetToValue(element, "result", "Ignored"))
            {
                return TestResult.Inconclusive();
            }
            else if (IsAttributeSetToValue(element, "result", "Inconclusive"))
            {
                return TestResult.Inconclusive();
            }
            else if (IsAttributeSetToValue(element, "result", "Failure"))
            {
                return TestResult.Failed();
            }
            else if (IsAttributeSetToValue(element, "result", "Success"))
            {
                return TestResult.Passed();
            }
            else
            {
                bool wasExecuted = IsAttributeSetToValue(element, "executed", "true");
                bool wasSuccessful = IsAttributeSetToValue(element, "success", "true");
                return new TestResult { WasExecuted = wasExecuted, WasSuccessful = wasSuccessful };
            }
        }

        private static bool IsAttributeSetToValue(XElement element, string attributeName, string expectedValue)
        {
            return element.Attribute(attributeName) != null
                ? string.Equals(
                    element.Attribute(attributeName).Value, 
                    expectedValue, 
                    StringComparison.InvariantCultureIgnoreCase)
                : false;
        }

        public TestResult GetExampleResult(ScenarioOutline scenarioOutline, string[] row)
        {
            XElement featureElement = this.GetFeatureElement(scenarioOutline.Feature);
            XElement examplesElement = null;
            if (featureElement != null)
            {
                Regex exampleSignature = this.exampleSignatureBuilder.Build(scenarioOutline, row);
                examplesElement = featureElement
                    .Descendants("test-suite")
                    .Where(x => x.Attribute("description") != null)
                    .FirstOrDefault(x => x.Attribute("description").Value == scenarioOutline.Name)
                    .Descendants("test-case")
                    .Where(x => x.Attribute("name") != null)
                    .FirstOrDefault(x => exampleSignature.IsMatch(x.Attribute("name").Value.ToLowerInvariant()));
            }
            return this.GetResultFromElement(examplesElement);
        }
    }
}