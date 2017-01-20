//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenFormattingFeatures.cs" company="PicklesDoc">
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
    public class WhenFormattingFeatures : BaseFixture
    {
        [Test]
        public void ThenCanFormatDescriptionAsMarkdown()
        {
            var feature = new Feature
            {
                Name = "A feature",
                Description =
                    @"In order to see the description as nice HTML
As a Pickles user
I want to see the descriptions written in markdown rendered as HTML

Introduction
============

This feature should have some markdown elements that get displayed properly

Context
-------

> I really like blockquotes to describe
> to describe text

I also enjoy using lists as well, here are the reasons

- Lists are easy to read
- Lists make my life easier

I also enjoy ordering things

1. This is the first reason
2. This is the second reason"
            };

            var htmlFeatureFormatter = Container.Resolve<HtmlFeatureFormatter>();
            XElement featureElement = htmlFeatureFormatter.Format(feature);
            XElement description = featureElement.Elements().FirstOrDefault(element => element.Name.LocalName == "div");

            Check.That(description).IsNotNull();
            Check.That(description).IsNamed("div");
            Check.That(description).IsInNamespace("http://www.w3.org/1999/xhtml");
            Check.That(description).HasAttribute("class", "description");
            Check.That(description.Elements().Count()).IsEqualTo(9);

            Check.That(description.Elements().ElementAt(0)).IsNamed("p");
            Check.That(description.Elements().ElementAt(1)).IsNamed("h1");
            Check.That(description.Elements().ElementAt(2)).IsNamed("p");
            Check.That(description.Elements().ElementAt(3)).IsNamed("h2");
            Check.That(description.Elements().ElementAt(4)).IsNamed("blockquote");
            Check.That(description.Elements().ElementAt(5)).IsNamed("p");
            Check.That(description.Elements().ElementAt(6)).IsNamed("ul");
            Check.That(description.Elements().ElementAt(7)).IsNamed("p");
            Check.That(description.Elements().ElementAt(8)).IsNamed("ol");
        }

        [Test]
        public void ThenCanFormatMarkdownTableExtensions()
        {
            var feature = new Feature
            {
                Name = "A feature",
                Description =
                    @"In order to see the description as nice HTML
As a Pickles user
I want to see the descriptions written in markdown rendered with tables

| Table Header 1 | Table Header 2 |
| -------------- | -------------- |
| Cell value 1   | Cell value 2   |
| Cell value 3   |                |
| Cell value 4   | Cell value 5   |
"
            };

            var htmlFeatureFormatter = Container.Resolve<HtmlFeatureFormatter>();
            XElement featureElement = htmlFeatureFormatter.Format(feature);
            XElement description = featureElement.Elements().FirstOrDefault(element => element.Name.LocalName == "div");

            Check.That(description).IsNotNull();
            Check.That(description).IsNamed("div");
            Check.That(description).IsInNamespace("http://www.w3.org/1999/xhtml");
            Check.That(description).HasAttribute("class", "description");

            XElement table = description.Descendants().FirstOrDefault(el => el.Name.LocalName == "table");

            Check.That(table).IsNotNull();
        }

        [Test]
        public void ThenCanFormatMarkdownTableExtensionsEvenIfTheyAreSomewhatMalstructured()
        {
            var feature = new Feature
            {
                Name = "A feature",
                Description =
                    @"In order to see the description as nice HTML
As a Pickles user
I want to see the descriptions written in markdown rendered with tables

| Table Header 1 | Table Header 2                         |
| -------------- | -------------------------------------- |
| Cell value 1   | Cell value 2                           |
| Cell value 3     Note the missing column delimiter here |
| Cell value 4   | Cell value 5                           |
"
            };

            var htmlFeatureFormatter = Container.Resolve<HtmlFeatureFormatter>();
            XElement featureElement = htmlFeatureFormatter.Format(feature);
            XElement description = featureElement.Elements().FirstOrDefault(element => element.Name.LocalName == "div");

            Check.That(description).IsNotNull();
            Check.That(description).IsNamed("div");
            Check.That(description).IsInNamespace("http://www.w3.org/1999/xhtml");
            Check.That(description).HasAttribute("class", "description");

            XElement table = description.Descendants().FirstOrDefault(el => el.Name.LocalName == "table");

            Check.That(table).IsNotNull();
        }
    }
}