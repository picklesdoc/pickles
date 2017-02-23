//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlScenarioOutlineFormatter.cs" company="PicklesDoc">
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
using System.Linq;
using System.Xml.Linq;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html
{
    public class HtmlScenarioOutlineFormatter
    {
        private readonly HtmlDescriptionFormatter htmlDescriptionFormatter;
        private readonly HtmlImageResultFormatter htmlImageResultFormatter;
        private readonly HtmlStepFormatter htmlStepFormatter;
        private readonly HtmlTableFormatter htmlTableFormatter;
        private readonly XNamespace xmlns;
        private readonly ITestResults testResults;

        public HtmlScenarioOutlineFormatter(
            HtmlStepFormatter htmlStepFormatter,
            HtmlDescriptionFormatter htmlDescriptionFormatter,
            HtmlTableFormatter htmlTableFormatter,
            HtmlImageResultFormatter htmlImageResultFormatter,
            ITestResults testResults)
        {
            this.htmlStepFormatter = htmlStepFormatter;
            this.htmlDescriptionFormatter = htmlDescriptionFormatter;
            this.htmlTableFormatter = htmlTableFormatter;
            this.htmlImageResultFormatter = htmlImageResultFormatter;
            this.testResults = testResults;
            this.xmlns = HtmlNamespace.Xhtml;
        }

        private XElement FormatHeading(ScenarioOutline scenarioOutline)
        {
            if (string.IsNullOrEmpty(scenarioOutline.Name))
            {
                return null;
            }

            var result = new XElement(
                this.xmlns + "div",
                new XAttribute("class", "scenario-heading"),
                string.IsNullOrEmpty(scenarioOutline.Slug) ? null : new XAttribute("id", scenarioOutline.Slug)
                );

            var tags = RetrieveTags(scenarioOutline);
            if (tags.Length > 0)
            {
                var paragraph = new XElement(this.xmlns + "p", HtmlScenarioFormatter.CreateTagElements(tags.OrderBy(t => t).ToArray(), this.xmlns));
                paragraph.Add(new XAttribute("class", "tags"));
                result.Add(paragraph);
            }

            result.Add(new XElement(this.xmlns + "h2", scenarioOutline.Name));

            result.Add(this.htmlDescriptionFormatter.Format(scenarioOutline.Description));

            return result;
        }

        private XElement FormatSteps(ScenarioOutline scenarioOutline)
        {
            if (scenarioOutline.Steps == null)
            {
                return null;
            }

            return new XElement(
                this.xmlns + "div",
                new XAttribute("class", "steps"),
                new XElement(
                    this.xmlns + "ul",
                    scenarioOutline.Steps.Select(
                        step => this.htmlStepFormatter.Format(step))));
        }

        private XElement FormatLinkButton(ScenarioOutline scenarioOutline)
        {
            if (string.IsNullOrEmpty(scenarioOutline.Slug))
            {
                return null;
            }

            return new XElement(
                this.xmlns + "a",
                new XAttribute("class", "scenario-link"),
                new XAttribute("href", $"javascript:showImageLink('{scenarioOutline.Slug}')"),
                new XAttribute("title", "Copy scenario link to clipboard."),
                new XElement(
                    this.xmlns + "i",
                    new XAttribute("class", "icon-link"),
                    " "));
        }

        private XElement FormatExamples(ScenarioOutline scenarioOutline)
        {
            var exampleDiv = new XElement(this.xmlns + "div");

            foreach (var example in scenarioOutline.Examples)
            {
                exampleDiv.Add(
                    new XElement(
                        this.xmlns + "div",
                        new XAttribute("class", "examples"),
                        (example.Tags == null || example.Tags.Count == 0) ? null : new XElement(this.xmlns + "p", new XAttribute("class", "tags"), HtmlScenarioFormatter.CreateTagElements(example.Tags.OrderBy(t => t).ToArray(), this.xmlns)),
                        new XElement(this.xmlns + "h3", "Examples: " + example.Name),
                        this.htmlDescriptionFormatter.Format(example.Description),
                        (example.TableArgument == null) ? null : this.htmlTableFormatter.Format(example.TableArgument, scenarioOutline)));
            }

            return exampleDiv;
        }

        public XElement Format(ScenarioOutline scenarioOutline, int id)
        {
            return new XElement(
                this.xmlns + "li",
                new XAttribute("class", "scenario"),
                this.htmlImageResultFormatter.Format(scenarioOutline),
                this.FormatHeading(scenarioOutline),
                this.FormatSteps(scenarioOutline),
                this.FormatLinkButton(scenarioOutline),
                (scenarioOutline.Examples == null || !scenarioOutline.Examples.Any())
                    ? null
                    : this.FormatExamples(scenarioOutline));
        }

        private static string[] RetrieveTags(ScenarioOutline scenarioOutline)
        {
            if (scenarioOutline == null)
            {
                return new string[0];
            }

            if (scenarioOutline.Feature == null)
            {
                return scenarioOutline.Tags.ToArray();
            }

            return scenarioOutline.Feature.Tags.Concat(scenarioOutline.Tags).ToArray();
        }
    }
}
