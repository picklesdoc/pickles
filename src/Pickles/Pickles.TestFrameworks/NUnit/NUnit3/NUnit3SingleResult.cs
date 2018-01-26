//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="NUnit3SingleResult.cs" company="PicklesDoc">
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

using System.Linq;
using System.Xml.Linq;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.TestFrameworks.NUnit.NUnit3
{
    public class NUnit3SingleResult : NUnitSingleResultsBase
    {
        private readonly ILookup<string, XElement> featureElements;

        public NUnit3SingleResult(XDocument resultsDocument)
            : base(
                resultsDocument,
                new[]
                    {
                        new TestResultAndName(TestResult.Inconclusive, "Skipped"),
                        new TestResultAndName(TestResult.Inconclusive, "Inconclusive"),
                        new TestResultAndName(TestResult.Failed, "Failed"),
                        new TestResultAndName(TestResult.Passed, "Passed"),
                    })
        {
            this.featureElements = resultsDocument
                .Descendants("test-suite")
                .Select(x => new
                {
                    DescriptionAttribute = x.Elements("properties").Elements("property")
                        .FirstOrDefault(p => IsDescriptionAttribute(p)),
                    Element = x
                })
                .Where(x => x.DescriptionAttribute != null)
                .ToLookup(
                    x => x.DescriptionAttribute.Attribute("value").Value,
                    x => x.Element);
        }

        protected override XElement GetScenarioElement(Scenario scenario)
        {
            XElement featureElement = this.GetFeatureElement(scenario.Feature);
            XElement scenarioElement = null;
            if (featureElement != null)
            {
                scenarioElement =
                    featureElement.Descendants("test-case")
                        .FirstOrDefault(
                            ts =>
                            ts.Elements("properties")
                                .Elements("property")
                                .Any(
                                    p =>
                                    IsDescriptionAttribute(p) && p.Attribute("value").Value == scenario.Name));
            }

            return scenarioElement;
        }

        protected override XElement GetScenarioOutlineElement(ScenarioOutline scenarioOutline)
        {
            XElement featureElement = this.GetFeatureElement(scenarioOutline.Feature);
            XElement scenarioOutlineElement = null;
            if (featureElement != null)
            {
                scenarioOutlineElement =
                    this.GetFeatureElement(scenarioOutline.Feature)
                        .Descendants("test-suite")
                        .FirstOrDefault(
                            ts =>
                            ts.Elements("properties")
                                .Elements("property")
                                .Any(
                                    p =>
                                    IsDescriptionAttribute(p)
                                    && p.Attribute("value").Value == scenarioOutline.Name));
            }

            return scenarioOutlineElement;
        }

        protected override XElement GetFeatureElement(Feature feature)
        {
            return this.featureElements[feature.Name].FirstOrDefault();
        }

        protected override XElement GetExamplesElement(ScenarioOutline scenarioOutline, string[] values)
        {
            XElement featureElement = this.GetFeatureElement(scenarioOutline.Feature);
            XElement examplesElement = null;
            if (featureElement != null)
            {
                var parameterizedTestElement =
                    featureElement.Descendants("test-suite")
                        .FirstOrDefault(
                            ts =>
                            ts.Elements("properties")
                                .Elements("property")
                                .Any(
                                    p =>
                                    IsDescriptionAttribute(p)
                                    && p.Attribute("value").Value == scenarioOutline.Name));

                if (parameterizedTestElement != null)
                {
                    examplesElement =
                        parameterizedTestElement.Descendants("test-case")
                            .FirstOrDefault(x => this.ScenarioOutlineExampleMatcher.IsMatch(scenarioOutline, values, x));
                }
            }

            return examplesElement;
        }

        private static bool IsDescriptionAttribute(XElement p)
        {
            return (
                p.Attribute("name").Value == "Description" ||
                p.Attribute("name").Value == "_DESCRIPTION"
                );
        }
    }
}
