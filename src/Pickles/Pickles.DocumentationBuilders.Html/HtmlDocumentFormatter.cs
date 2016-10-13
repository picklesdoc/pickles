//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlDocumentFormatter.cs" company="PicklesDoc">
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
using System.IO.Abstractions;
using System.Xml.Linq;
using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;

namespace PicklesDoc.Pickles.DocumentationBuilders.HTML
{
    public class HtmlDocumentFormatter
    {
        private const string DocumentReady =
            "\n" +
            "$(document).ready(function() {" + "\n" +
            "  initializeToc();" + "\n" +
            "});" + "\n";

        private readonly IConfiguration configuration;
        private readonly HtmlContentFormatter htmlContentFormatter;
        private readonly HtmlFooterFormatter htmlFooterFormatter;
        private readonly HtmlHeaderFormatter htmlHeaderFormatter;
        private readonly HtmlResourceSet htmlResources;
        private readonly IFileSystem fileSystem;
        private readonly HtmlTableOfContentsFormatter htmlTableOfContentsFormatter;

        public HtmlDocumentFormatter(
            IConfiguration configuration,
            HtmlHeaderFormatter htmlHeaderFormatter,
            HtmlTableOfContentsFormatter htmlTableOfContentsFormatter,
            HtmlContentFormatter htmlContentFormatter,
            HtmlFooterFormatter htmlFooterFormatter,
            HtmlResourceSet htmlResources,
            IFileSystem fileSystem)
        {
            this.configuration = configuration;
            this.htmlHeaderFormatter = htmlHeaderFormatter;
            this.htmlTableOfContentsFormatter = htmlTableOfContentsFormatter;
            this.htmlContentFormatter = htmlContentFormatter;
            this.htmlFooterFormatter = htmlFooterFormatter;
            this.htmlResources = htmlResources;
            this.fileSystem = fileSystem;
        }

        public XDocument Format(INode featureNode, Tree features, DirectoryInfoBase rootFolder)
        {
            XNamespace xmlns = HtmlNamespace.Xhtml;
            string featureNodeOutputPath = this.fileSystem.Path.Combine(
                this.configuration.OutputFolder.FullName,
                featureNode.RelativePathFromRoot);
            var featureNodeOutputUri = new Uri(featureNodeOutputPath);

            var container = new XElement(xmlns + "div", new XAttribute("id", "container"));
            container.Add(this.htmlHeaderFormatter.Format());
            container.Add(this.htmlTableOfContentsFormatter.Format(featureNode.OriginalLocationUrl, features, rootFolder));
            container.Add(this.htmlContentFormatter.Format(featureNode, features));
            container.Add(this.htmlFooterFormatter.Format());

            var body = new XElement(xmlns + "body");
            body.Add(container);

            var head = new XElement(xmlns + "head");
            head.Add(new XElement(xmlns + "title", featureNode.Name));

            head.Add(
                new XElement(
                    xmlns + "link",
                    new XAttribute("rel", "stylesheet"),
                    new XAttribute(
                        "href",
                        featureNodeOutputUri.MakeRelativeUri(this.htmlResources.MasterStylesheet)),
                    new XAttribute("type", "text/css")));

            head.Add(
                new XElement(
                    xmlns + "link",
                    new XAttribute("rel", "stylesheet"),
                    new XAttribute(
                        "href",
                        featureNodeOutputUri.MakeRelativeUri(this.htmlResources.PrintStylesheet)),
                    new XAttribute("type", "text/css"),
                    new XAttribute("media", "print")));

            head.Add(
                new XElement(
                    xmlns + "script",
                    new XAttribute("src", featureNodeOutputUri.MakeRelativeUri(this.htmlResources.JQueryScript)),
                    new XAttribute("type", "text/javascript"),
                    new XText(string.Empty)));

            head.Add(
                new XElement(
                    xmlns + "script",
                    new XAttribute(
                        "src",
                        featureNodeOutputUri.MakeRelativeUri(this.htmlResources.AdditionalScripts)),
                    new XAttribute("type", "text/javascript"),
                    new XText(string.Empty)));

            head.Add(
                new XElement(
                    xmlns + "script",
                    new XAttribute("type", "text/javascript"),
                    DocumentReady));

            head.Add(new XComment(" We are using Font Awesome - http://fortawesome.github.com/Font-Awesome - licensed under CC BY 3.0 "));

            var html = new XElement(
                xmlns + "html",
                new XAttribute(XNamespace.Xml + "lang", "en"),
                head,
                body);

            var document = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XDocumentType(
                    "html",
                    "-//W3C//DTD XHTML 1.0 Strict//EN",
                    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd",
                    string.Empty),
                html);

            return document;
        }
    }
}
