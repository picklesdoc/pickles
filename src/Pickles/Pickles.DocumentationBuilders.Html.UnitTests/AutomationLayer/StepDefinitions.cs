//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StepDefinitions.cs" company="PicklesDoc">
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
using System.Xml;

using Autofac;

using NFluent;

using PicklesDoc.Pickles.ObjectModel;

using TechTalk.SpecFlow;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html.UnitTests.AutomationLayer
{
    [Binding]
    public class StepDefinitions
    {
        public StepDefinitions(CurrentScenarioContext currentScenarioContext)
        {
            this.CurrentScenarioContext = currentScenarioContext;
        }

        private CurrentScenarioContext CurrentScenarioContext { get; }

        [Given(@"I have this feature description")]
        public void GivenIHaveThisFeatureDescription(string multilineText)
        {
            this.CurrentScenarioContext.Feature = new Feature
            {
                Name = "a feature",
                Description = multilineText
            };
        }

        [Given(@"I have this feature file")]
        public void GivenIHaveThisFeatureFile(string multilineText)
        {
            this.CurrentScenarioContext.Feature = new Feature
            {
                Name = "a feature",
                Description = multilineText
            };
        }

        [When(@"I generate the documentation")]
        public void WhenIGenerateTheDocumentation()
        {
            var configuration = this.CurrentScenarioContext.Container.Resolve<Configuration>();
            var htmlFeatureFormatter = this.CurrentScenarioContext.Container.Resolve<HtmlFeatureFormatter>();

            this.CurrentScenarioContext.Html = htmlFeatureFormatter.Format(this.CurrentScenarioContext.Feature);
        }

        [Then(@"the result should be")]
        public void ThenTheResultShouldBe(string multilineText)
        {
            var actual = this.CurrentScenarioContext.Html.ToString();
            actual = actual.Replace(" xmlns=\"http://www.w3.org/1999/xhtml\"", string.Empty);

            actual = FormatXml(actual);
            multilineText = FormatXml(multilineText);

            Check.That(actual).IsEqualTo(multilineText);
        }

        private static string FormatXml(string xmlDocument)
        {
            using (var sw = new System.IO.StringWriter())
            {
                using (XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true, NewLineHandling = NewLineHandling.Replace, NewLineChars = Environment.NewLine }))
                {
                    xw.WriteRaw(xmlDocument);
                    xw.Flush();

                    return sw.ToString();
                }
            }
        }
    }
}
