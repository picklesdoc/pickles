//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlMultilineStringFormatter.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles.DocumentationBuilders.Html
{
    public class HtmlMultilineStringFormatter
    {
        private readonly XNamespace xmlns;

        public HtmlMultilineStringFormatter()
        {
            this.xmlns = HtmlNamespace.Xhtml;
        }

        public XElement Format(string multilineText)
        {
            if (multilineText == null)
            {
                return null;
            }

            return new XElement(
                this.xmlns + "div",
                new XAttribute("class", "pre"),
                new XElement(
                    this.xmlns + "pre",
                    new XElement(
                        this.xmlns + "code",
                        new XAttribute("class", "no-highlight"),
                        new XText(multilineText))));
        }
    }
}
