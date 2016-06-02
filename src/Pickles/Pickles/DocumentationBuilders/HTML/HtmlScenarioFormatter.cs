//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlScenarioFormatter.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles.DocumentationBuilders.HTML
{
    public class HtmlScenarioFormatter
    {
        private readonly HtmlDescriptionFormatter htmlDescriptionFormatter;
        private readonly HtmlImageResultFormatter htmlImageResultFormatter;
        private readonly HtmlStepFormatter htmlStepFormatter;
        private readonly XNamespace xmlns;

        public HtmlScenarioFormatter(
            HtmlStepFormatter htmlStepFormatter,
            HtmlDescriptionFormatter htmlDescriptionFormatter,
            HtmlImageResultFormatter htmlImageResultFormatter)
        {
            this.htmlStepFormatter = htmlStepFormatter;
            this.htmlDescriptionFormatter = htmlDescriptionFormatter;
            this.htmlImageResultFormatter = htmlImageResultFormatter;
            this.xmlns = HtmlNamespace.Xhtml;
        }

        public XElement Format(Scenario scenario, int id)
        {
            var header = new XElement(
                this.xmlns + "div",
                new XAttribute("class", "scenario-heading"),
                string.IsNullOrEmpty(scenario.Slug) ? null : new XAttribute("id", scenario.Slug),
                new XElement(this.xmlns + "h2", scenario.Name));

            var tags = RetrieveTags(scenario);
            if (tags.Length > 0)
            {
                var paragraph = new XElement(this.xmlns + "p", CreateTagElements(tags.OrderBy(t => t).ToArray(), this.xmlns));
                paragraph.Add(new XAttribute("class", "tags"));
                header.Add(paragraph);
            }

            header.Add(this.htmlDescriptionFormatter.Format(scenario.Description));

            return new XElement(
                this.xmlns + "li",
                new XAttribute("class", "scenario"),
                this.htmlImageResultFormatter.Format(scenario),
                header,
                new XElement(
                    this.xmlns + "div",
                    new XAttribute("class", "steps"),
                    new XElement(
                        this.xmlns + "ul",
                        scenario.Steps.Select(step => this.htmlStepFormatter.Format(step)))), 
                this.FormatLinkButton(scenario));
        }

        private XElement FormatLinkButton(Scenario scenarioOutline)
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

        internal static XNode[] CreateTagElements(string[] tags, XNamespace xNamespace)
        {
            List<XNode> result = new List<XNode>();

            result.Add(new XText("Tags: "));
            result.Add(new XElement(xNamespace + "span", tags.First()));

            foreach (var tag in tags.Skip(1))
            {
                result.Add(new XText(", "));
                result.Add(new XElement(xNamespace + "span", tag));
            }

            return result.ToArray();
        }

        private static string[] RetrieveTags(Scenario scenario)
        {
            if (scenario == null)
            {
                return new string[0];
            }

            if (scenario.Feature == null)
            {
                return scenario.Tags.ToArray();
            }

            return scenario.Feature.Tags.Concat(scenario.Tags).ToArray();
        }
    }
}
