//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenFormattingStep.cs" company="PicklesDoc">
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
using System.Xml.Linq;
using Autofac;
using NFluent;
using NUnit.Framework;

using PicklesDoc.Pickles.DocumentationBuilders.Html;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test.DocumentationBuilders.HTML
{
    [TestFixture]
    public class WhenFormattingStep : BaseFixture
    {
        private const string ExpectedGivenHtml = "Given ";
        private readonly XNamespace xmlns = XNamespace.Get("http://www.w3.org/1999/xhtml");

        [Test]
        public void Multiline_strings_are_formatted_as_list_items_with_pre_elements_formatted_as_code_internal()
        {
            var step = new Step
            {
                Keyword = Keyword.Given,
                NativeKeyword = "Given ",
                Name = "a simple step",
                TableArgument = null,
                DocStringArgument = "this is a\nmultiline table\nargument",
            };

            var formatter = Container.Resolve<HtmlStepFormatter>();
            XElement actual = formatter.Format(step);

            var expected = new XElement(
                xmlns + "li",
                new XAttribute("class", "step"),
                new XElement(xmlns + "span", new XAttribute("class", "keyword"), ExpectedGivenHtml),
                new XText("a simple step"),
                new XElement(
                    xmlns + "div",
                    new XAttribute("class", "pre"),
                    new XElement(
                        xmlns + "pre",
                        new XElement(
                            xmlns + "code",
                            new XAttribute("class", "no-highlight"),
                            new XText(
                                "this is a\nmultiline table\nargument")))));

            Check.That(expected).IsDeeplyEqualTo(actual);
        }

        [Test]
        public void Simple_steps_are_formatted_as_list_items()
        {
            var step = new Step
            {
                Keyword = Keyword.Given,
                Name = "a simple step",
                NativeKeyword = "Given ",
                TableArgument = null,
                DocStringArgument = null,
            };

            var formatter = Container.Resolve<HtmlStepFormatter>();
            XElement actual = formatter.Format(step);

            var expected = new XElement(
                xmlns + "li",
                new XAttribute("class", "step"),
                new XElement(xmlns + "span", new XAttribute("class", "keyword"), ExpectedGivenHtml),
                "a simple step");

            Check.That(expected).IsDeeplyEqualTo(actual);
        }

        [Test]
        public void Steps_get_selected_Language()
        {
            var step = new Step
            {
                Keyword = Keyword.Given,
                Name = "ett enkelt steg",
                NativeKeyword = "Givet ",
                TableArgument = null,
                DocStringArgument = null,
            };

            var configuration = this.Configuration;
            configuration.Language = "sv";

            var formatter = Container.Resolve<HtmlStepFormatter>();
            XElement actual = formatter.Format(step);

            var expected = new XElement(
                xmlns + "li",
                new XAttribute("class", "step"),
                new XElement(xmlns + "span", new XAttribute("class", "keyword"), "Givet "),
                "ett enkelt steg");

            Check.That(expected).IsDeeplyEqualTo(actual);
        }

        [Test]
        public void Tables_are_formatted_as_list_items_with_tables_internal()
        {
            var table = new Table
            {
                HeaderRow = new TableRow("Column 1", "Column 2"),
                DataRows = new List<TableRow> { new TableRow("Value 1", "Value 2") }
            };

            var step = new Step
            {
                Keyword = Keyword.Given,
                NativeKeyword = "Given ",
                Name = "a simple step",
                TableArgument = table,
                DocStringArgument = null,
            };

            var formatter = Container.Resolve<HtmlStepFormatter>();
            XElement actual = formatter.Format(step);

            var expected = new XElement(
                xmlns + "li",
                new XAttribute("class", "step"),
                new XElement(xmlns + "span", new XAttribute("class", "keyword"), ExpectedGivenHtml),
                new XText("a simple step"),
                new XElement(
                    xmlns + "div",
                    new XAttribute("class", "table_container"),
                    new XElement(
                        xmlns + "table",
                        new XAttribute("class", "datatable"),
                        new XElement(
                            xmlns + "thead",
                            new XElement(
                                xmlns + "tr",
                                new XElement(
                                    xmlns + "th",
                                    "Column 1"),
                                new XElement(
                                    xmlns + "th",
                                    "Column 2"),
                                new XElement(
                                    xmlns + "th",
                                    " "))),
                        new XElement(
                            xmlns + "tbody",
                            new XElement(
                                xmlns + "tr",
                                new XElement(
                                    xmlns + "td",
                                    "Value 1"),
                                new XElement(
                                    xmlns + "td",
                                    "Value 2"))))));

            Check.That(expected).IsDeeplyEqualTo(actual);
        }

        [Test]
        public void Comments_are_displayed_above_their_related_step()
        {
            var step = new Step
            {
                Keyword = Keyword.Given,
                NativeKeyword = "Given ",
                Name = "a simple step",
                TableArgument = null,
                DocStringArgument = null,
                Comments = new List<Comment>
                {
                    new Comment
                    {
                        Text = "    # A simple comment",
                        Type = CommentType.StepComment
                    }
                }
            };

            var formatter = Container.Resolve<HtmlStepFormatter>();
            XElement actual = formatter.Format(step);

            var expected = new XElement(
                xmlns + "li",
                new XAttribute("class", "step"),
                new XElement(xmlns + "span", new XAttribute("class", "comment"), "# A simple comment"),
                new XElement(xmlns + "span", new XAttribute("class", "keyword"), ExpectedGivenHtml),
                "a simple step");

            Check.That(expected).IsDeeplyEqualTo(actual);
        }

        [Test]
        public void Comments_after_the_last_step_are_displayed()
        {
            var step = new Step
            {
                Keyword = Keyword.Given,
                NativeKeyword = "Given ",
                Name = "a simple step",
                TableArgument = null,
                DocStringArgument = null,
                Comments = new List<Comment>
                {
                    new Comment
                    {
                        Text = "    # A simple comment",
                        Type = CommentType.StepComment
                    },
                    new Comment
                    {
                        Text = "    # A comment after the last step",
                        Type = CommentType.AfterLastStepComment
                    }
                }
            };

            var formatter = Container.Resolve<HtmlStepFormatter>();
            XElement actual = formatter.Format(step);

            var expected = new XElement(
                xmlns + "li",
                new XAttribute("class", "step"),
                new XElement(xmlns + "span", new XAttribute("class", "comment"), "# A simple comment"),
                new XElement(xmlns + "span", new XAttribute("class", "keyword"), ExpectedGivenHtml),
                "a simple step",
                new XElement(xmlns + "span", new XAttribute("class", "comment"), "# A comment after the last step"));

            Check.That(expected).IsDeeplyEqualTo(actual);
        }

        [Test]
        public void Multiline_comments_are_displayed_in_the_same_element()
        {
            var step = new Step
            {
                Keyword = Keyword.Given,
                NativeKeyword = "Given ",
                Name = "a simple step",
                TableArgument = null,
                DocStringArgument = null,
                Comments = new List<Comment>
                {
                    new Comment
                    {
                        Text = "    # A first line",
                        Type = CommentType.StepComment
                    },
                    new Comment
                    {
                        Text = "    # A second line",
                        Type = CommentType.StepComment
                    }
                }
            };

            var formatter = Container.Resolve<HtmlStepFormatter>();
            XElement actual = formatter.Format(step);

            var expected = new XElement(
                xmlns + "li",
                new XAttribute("class", "step"),
                new XElement(xmlns + "span", new XAttribute("class", "comment"),
                    "# A first line",
                    new XElement(xmlns + "br"),
                    "# A second line"
                ),
                new XElement(xmlns + "span", new XAttribute("class", "keyword"), ExpectedGivenHtml),
                "a simple step");

            Check.That(expected).IsDeeplyEqualTo(actual);
        }
    }
}

