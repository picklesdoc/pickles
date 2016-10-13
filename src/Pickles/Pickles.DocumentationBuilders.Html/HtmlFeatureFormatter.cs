//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlFeatureFormatter.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles.DocumentationBuilders.HTML
{
    public class HtmlFeatureFormatter : IHtmlFeatureFormatter
    {
        private readonly HtmlDescriptionFormatter htmlDescriptionFormatter;
        private readonly HtmlImageResultFormatter htmlImageResultFormatter;
        private readonly HtmlScenarioFormatter htmlScenarioFormatter;
        private readonly HtmlScenarioOutlineFormatter htmlScenarioOutlineFormatter;
        private readonly XNamespace xmlns;

        public HtmlFeatureFormatter(
            HtmlScenarioFormatter htmlScenarioFormatter,
            HtmlDescriptionFormatter htmlDescriptionFormatter,
            HtmlScenarioOutlineFormatter htmlScenarioOutlineFormatter,
            HtmlImageResultFormatter htmlImageResultFormatter)
        {
            this.htmlScenarioFormatter = htmlScenarioFormatter;
            this.htmlScenarioOutlineFormatter = htmlScenarioOutlineFormatter;
            this.htmlDescriptionFormatter = htmlDescriptionFormatter;
            this.htmlImageResultFormatter = htmlImageResultFormatter;
            this.xmlns = HtmlNamespace.Xhtml;
        }

        #region IHtmlFeatureFormatter Members

        public XElement Format(Feature feature)
        {
            var div = new XElement(
                this.xmlns + "div",
                new XAttribute("id", "feature"),
                this.htmlImageResultFormatter.Format(feature),
                new XElement(this.xmlns + "h1", feature.Name));

            var tags = RetrieveTags(feature);
            if (tags.Length > 0)
            {
                var paragraph = new XElement(this.xmlns + "p", HtmlScenarioFormatter.CreateTagElements(tags.OrderBy(t => t).ToArray(), this.xmlns));
                paragraph.Add(new XAttribute("class", "tags"));
                div.Add(paragraph);
            }

            div.Add(this.htmlDescriptionFormatter.Format(feature.Description));

            var scenarios = new XElement(this.xmlns + "ul", new XAttribute("id", "scenarios"));
            int id = 0;

            if (feature.Background != null)
            {
                scenarios.Add(this.htmlScenarioFormatter.Format(feature.Background, id++));
            }

            foreach (IFeatureElement featureElement in feature.FeatureElements)
            {
                var scenario = featureElement as Scenario;
                if (scenario != null)
                {
                    scenarios.Add(this.htmlScenarioFormatter.Format(scenario, id++));
                }

                var scenarioOutline = featureElement as ScenarioOutline;
                if (scenarioOutline != null)
                {
                    scenarios.Add(this.htmlScenarioOutlineFormatter.Format(scenarioOutline, id++));
                }
            }

            div.Add(scenarios);

            return div;
        }

        #endregion

        private static string[] RetrieveTags(Feature feature)
        {
            if (feature == null)
            {
                return new string[0];
            }

            return feature.Tags.ToArray();
        }
    }
}
