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
using System.IO; // this is a legitimate usage of System.IO
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using PicklesDoc.Pickles.Parser;

namespace PicklesDoc.Pickles.TestFrameworks
{
    public class XUnitResults : ITestResults
    {
        private readonly Configuration configuration;
        private readonly xUnitExampleSignatureBuilder exampleSignatureBuilder;
        private readonly XDocument resultsDocument;

        public XUnitResults(Configuration configuration, xUnitExampleSignatureBuilder exampleSignatureBuilder)
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
            XElement featureElement = this.GetFeatureElement(feature);
            int passedCount = int.Parse(featureElement.Attribute("passed").Value);
            int failedCount = int.Parse(featureElement.Attribute("failed").Value);
            int skippedCount = int.Parse(featureElement.Attribute("skipped").Value);

            return GetAggregateResult(passedCount, failedCount, skippedCount);
        }

        public TestResult GetScenarioOutlineResult(ScenarioOutline scenarioOutline)
        {
            IEnumerable<XElement> exampleElements = this.GetScenarioOutlineElements(scenarioOutline);
            int passedCount = 0;
            int failedCount = 0;
            int skippedCount = 0;

            foreach (XElement exampleElement in exampleElements)
            {
                TestResult result = this.GetResultFromElement(exampleElement);
                if (result.WasExecuted == false) skippedCount++;
                if (result.WasExecuted && result.WasSuccessful) passedCount++;
                if (result.WasExecuted && !result.WasSuccessful) failedCount++;
            }

            return GetAggregateResult(passedCount, failedCount, skippedCount);
        }

        public TestResult GetScenarioResult(Scenario scenario)
        {
            XElement scenarioElement = this.GetScenarioElement(scenario);
            return scenarioElement != null 
                ? this.GetResultFromElement(scenarioElement)
                : TestResult.NotFound();
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
            IEnumerable<XElement> featureQuery =
                from clazz in this.resultsDocument.Root.Descendants("class")
                from test in clazz.Descendants("test")
                from trait in clazz.Descendants("traits").Descendants("trait")
                where trait.Attribute("name").Value == "FeatureTitle" && trait.Attribute("value").Value == feature.Name
                select clazz;

            return featureQuery.FirstOrDefault();
        }

        private XElement GetScenarioElement(Scenario scenario)
        {
            XElement featureElement = this.GetFeatureElement(scenario.Feature);

            IEnumerable<XElement> scenarioQuery =
                from test in featureElement.Descendants("test")
                from trait in test.Descendants("traits").Descendants("trait")
                where trait.Attribute("name").Value == "Description" && trait.Attribute("value").Value == scenario.Name
                select test;

            return scenarioQuery.FirstOrDefault();
        }

        private IEnumerable<XElement> GetScenarioOutlineElements(ScenarioOutline scenario)
        {
            XElement featureElement = this.GetFeatureElement(scenario.Feature);

            IEnumerable<XElement> scenarioQuery =
                from test in featureElement.Descendants("test")
                from trait in test.Descendants("traits").Descendants("trait")
                where trait.Attribute("name").Value == "Description" && trait.Attribute("value").Value == scenario.Name
                select test;

            return scenarioQuery;
        }

        private TestResult GetResultFromElement(XElement element)
        {
            var result = new TestResult();
            XAttribute resultAttribute = element.Attribute("result");
            switch (resultAttribute.Value.ToLowerInvariant())
            {
                case "skip":
                    result.WasExecuted = false;
                    result.WasSuccessful = false;
                    break;
                case "pass":
                    result.WasExecuted = true;
                    result.WasSuccessful = true;
                    break;
                case "fail":
                    result.WasExecuted = true;
                    result.WasSuccessful = false;
                    break;
                default:
                    result.WasExecuted = false;
                    result.WasSuccessful = false;
                    break;
            }
            return result;
        }

        private static TestResult GetAggregateResult(int passedCount, int failedCount, int skippedCount)
        {
            TestResult result;
            if (passedCount > 0 && failedCount == 0)
            {
                result = TestResult.Passed();
            }
            else if (failedCount > 0)
            {
                result = TestResult.Failed();
            }
            else
            {
                result = TestResult.Inconclusive();
            }

            return result;
        }

        public TestResult GetExampleResult(ScenarioOutline scenarioOutline, string[] row)
        {
            IEnumerable<XElement> exampleElements = this.GetScenarioOutlineElements(scenarioOutline);

            var result = new TestResult();
            foreach (XElement exampleElement in exampleElements)
            {
                Regex signature = this.exampleSignatureBuilder.Build(scenarioOutline, row);
                if (signature.IsMatch(exampleElement.Attribute("name").Value.ToLowerInvariant()))
                {
                    return this.GetResultFromElement(exampleElement);
                }
            }
            return result;
        }
    }
}