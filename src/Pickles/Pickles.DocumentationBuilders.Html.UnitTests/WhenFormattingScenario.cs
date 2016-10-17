//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenFormattingScenario.cs" company="PicklesDoc">
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

using Autofac;

using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.Test;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html.UnitTests
{
    [TestFixture]
    public class WhenFormattingScenario : BaseFixture
    {
        [Test]
        public void ThenCanRenderTags()
        {
            var configuration = Container.Resolve<Configuration>();

            var scenario = new Scenario
            {
                Name = "A Scenario",
                Description = @"This scenario has tags",
                Tags = { "tag1", "tag2" }
            };

            var htmlFeatureFormatter = Container.Resolve<HtmlScenarioFormatter>();
            XElement featureElement = htmlFeatureFormatter.Format(scenario, 1);
            XElement header = featureElement.Elements().FirstOrDefault(element => element.Name.LocalName == "div");

            Check.That(header).IsNotNull();
            Check.That(header).IsNamed("div");
            Check.That(header).IsInNamespace("http://www.w3.org/1999/xhtml");
            Check.That(header).HasAttribute("class", "scenario-heading");
            Check.That(header.Elements().Count()).IsEqualTo(3);

            Check.That(header.Elements().ElementAt(0)).IsNamed("h2");
            Check.That(header.Elements().ElementAt(1)).IsNamed("p");
            Check.That(header.Elements().ElementAt(2)).IsNamed("div");

            var tagsParagraph = header.Elements().ElementAt(1);

            Check.That(tagsParagraph.ToString()).IsEqualTo(
                @"<p class=""tags"" xmlns=""http://www.w3.org/1999/xhtml"">Tags: <span>tag1</span>, <span>tag2</span></p>");
        }

        [Test]
        public void NoTags()
        {
            var configuration = Container.Resolve<Configuration>();

            var scenario = new Scenario
            {
                Name = "A Scenario",
                Description = @"This scenario has no tags",
                Tags = { }
            };

            var htmlFeatureFormatter = Container.Resolve<HtmlScenarioFormatter>();
            XElement featureElement = htmlFeatureFormatter.Format(scenario, 1);
            XElement header = featureElement.Elements().FirstOrDefault(element => element.Name.LocalName == "div");

            Check.That(header).IsNotNull();
            Check.That(header).IsNamed("div");
            Check.That(header).IsInNamespace("http://www.w3.org/1999/xhtml");
            Check.That(header).HasAttribute("class", "scenario-heading");
            Check.That(header.Elements().Count()).IsEqualTo(2);

            Check.That(header.Elements().ElementAt(0)).IsNamed("h2");
            Check.That(header.Elements().ElementAt(1)).IsNamed("div");
        }

        [Test]
        public void FeatureTagsAreAddedToScenarioTags()
        {
            var feature = new Feature
            {
                Name = "A Scenario with Tags",
                FeatureElements =
                {
                    new Scenario
                    {
                        Name = "A Scenario",
                        Description = @"This scenario has tags",
                        Tags = { "scenarioTag1", "scenarioTag2" }
                    }
                },
                Tags = { "featureTag1", "featureTag2" }
            };

            feature.FeatureElements[0].Feature = feature;

            var htmlFeatureFormatter = Container.Resolve<HtmlFeatureFormatter>();
            XElement featureElement = htmlFeatureFormatter.Format(feature);

            var header = featureElement.Descendants().First(n => n.Attributes().Any(a => a.Name == "class" && a.Value == "scenario-heading"));

            Check.That(header.Elements().Count()).IsEqualTo(3);

            Check.That(header.Elements().ElementAt(0)).IsNamed("h2");
            Check.That(header.Elements().ElementAt(1)).IsNamed("p");
            Check.That(header.Elements().ElementAt(2)).IsNamed("div");

            var tagsParagraph = header.Elements().ElementAt(1);

            Check.That(tagsParagraph.ToString()).IsEqualTo(@"<p class=""tags"" xmlns=""http://www.w3.org/1999/xhtml"">Tags: <span>featureTag1</span>, <span>featureTag2</span>, <span>scenarioTag1</span>, <span>scenarioTag2</span></p>");
        }

        [Test]
        public void TagsAreRenderedAlphabetically()
        {
            var feature = new Feature
            {
                Name = "A Scenario with Tags",
                FeatureElements =
                {
                    new Scenario
                    {
                        Name = "A Scenario",
                        Description = @"This scenario has tags",
                        Tags = { "a", "c" }
                    }
                },
                Tags = { "d", "b" }
            };

            feature.FeatureElements[0].Feature = feature;

            var htmlFeatureFormatter = Container.Resolve<HtmlFeatureFormatter>();
            XElement featureElement = htmlFeatureFormatter.Format(feature);

            var header = featureElement.Descendants().First(n => n.Attributes().Any(a => a.Name == "class" && a.Value == "scenario-heading"));

            Check.That(header.Elements().Count()).IsEqualTo(3);

            Check.That(header.Elements().ElementAt(0)).IsNamed("h2");
            Check.That(header.Elements().ElementAt(1)).IsNamed("p");
            Check.That(header.Elements().ElementAt(2)).IsNamed("div");

            var tagsParagraph = header.Elements().ElementAt(1);

            Check.That(tagsParagraph.ToString()).IsEqualTo(@"<p class=""tags"" xmlns=""http://www.w3.org/1999/xhtml"">Tags: <span>a</span>, <span>b</span>, <span>c</span>, <span>d</span></p>");
        }
    }
}
