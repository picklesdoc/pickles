//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="NUnit2SingleResults.cs" company="PicklesDoc">
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
using System.Linq;
using System.Xml.Linq;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.TestFrameworks.NUnit.NUnit2
{
    public class NUnit2SingleResults : NUnitSingleResultsBase
    {
        private readonly ILookup<string, XElement> featureElements;

        public NUnit2SingleResults(XDocument resultsDocument)
            : base(
                resultsDocument,
                new[]
                    {
                        new TestResultAndName(TestResult.Inconclusive, "Ignored"),
                        new TestResultAndName(TestResult.Inconclusive, "Inconclusive"),
                        new TestResultAndName(TestResult.Failed, "Failure"),
                        new TestResultAndName(TestResult.Passed, "Success"),
                    })
        {
            this.featureElements = resultsDocument
                .Descendants("test-suite")
                .Where(x => x.Attribute("description") != null)
                .ToLookup(x => x.Attribute("description").Value);
        }

        protected override XElement GetScenarioElement(Scenario scenario)
        {
            XElement featureElement = this.GetFeatureElement(scenario.Feature);
            XElement scenarioElement = null;
            if (featureElement != null)
            {
                scenarioElement =
                    featureElement.Descendants("test-case")
                        .Where(x => x.Attribute("description") != null)
                        .FirstOrDefault(x => x.Attribute("description").Value == scenario.Name);
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
                        .Where(x => x.Attribute("description") != null)
                        .FirstOrDefault(x => x.Attribute("description").Value == scenarioOutline.Name);
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
                        .FirstOrDefault(x => IsMatchingParameterizedTestElement(x, scenarioOutline));

                if (parameterizedTestElement != null)
                {
                    examplesElement =
                        parameterizedTestElement.Descendants("test-case")
                            .FirstOrDefault(x => this.ScenarioOutlineExampleMatcher.IsMatch(scenarioOutline, values, x));
                }
            }
            return examplesElement;
        }

        private static bool IsMatchingParameterizedTestElement(XElement element, ScenarioOutline scenarioOutline)
        {
            var description = element.Attribute("description");

            return description != null &&
                   description.Value.Equals(scenarioOutline.Name, StringComparison.OrdinalIgnoreCase) &&
                   element.Descendants("test-case").Any();
        }
    }
}
