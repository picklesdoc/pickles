//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlMarkdownFormatter.cs" company="PicklesDoc">
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
using System.Xml.Linq;

using PicklesDoc.Pickles.DocumentationBuilders.Html.Extensions;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html
{
    public class HtmlMarkdownFormatter
    {
        private readonly IMarkdownProvider markdown;

        private readonly XNamespace xmlns;

        public HtmlMarkdownFormatter(IMarkdownProvider markdown)
        {
            this.markdown = markdown;
            this.xmlns = HtmlNamespace.Xhtml;
        }

        public XElement Format(string text)
        {
            // HACK - we add the div around the markdown content because XElement requires a single root element from which to parse and Markdown.Transform() returns a series of elements
            XElement xElement = XElement.Parse("<div>" + this.markdown.Transform(text) + "</div>");
            xElement.SetAttributeValue("id", "markdown");

            xElement.MoveToNamespace(this.xmlns);

            return xElement;
        }
    }
}
