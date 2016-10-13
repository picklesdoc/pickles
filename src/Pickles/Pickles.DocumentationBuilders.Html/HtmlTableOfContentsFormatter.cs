//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlTableOfContentsFormatter.cs" company="PicklesDoc">
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
using PicklesDoc.Pickles.Extensions;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.HTML
{
    public class HtmlTableOfContentsFormatter
    {
        private readonly HtmlImageResultFormatter imageResultFormatter;

        private readonly IFileSystem fileSystem;

        public HtmlTableOfContentsFormatter(HtmlImageResultFormatter imageResultFormatter, IFileSystem fileSystem)
        {
            this.imageResultFormatter = imageResultFormatter;
            this.fileSystem = fileSystem;
        }

        private XElement BuildListItems(XNamespace xmlns, Uri file, Tree features)
        {
            var ul = new XElement(xmlns + "ul", new XAttribute("class", "features"));

            foreach (var childNode in features.ChildNodes)
            {
                if (childNode.Data.NodeType == NodeType.Content)
                {
                    if (childNode.Data.IsIndexMarkDownNode())
                    {
                        continue;
                    }

                    ul.Add(this.AddNodeForFile(xmlns, file, childNode));
                }
                else if (childNode.Data.NodeType == NodeType.Structure)
                {
                    ul.Add(this.AddNodeForDirectory(xmlns, file, childNode));
                }
            }

            return ul;
        }

        private XElement AddNodeForDirectory(XNamespace xmlns, Uri file, Tree childNode)
        {
            var xElement = new XElement(
                xmlns + "li",
                new XElement(
                    xmlns + "div",
                    new XAttribute("class", "directory"),
                    new XElement(
                        xmlns + "a",
                        new XAttribute("href", childNode.Data.GetRelativeUriTo(file) + "index.html"),
                        new XText(childNode.Data.Name))),
                this.BuildListItems(xmlns, file, childNode));

            return xElement;
        }

        private XElement AddNodeForHome(XNamespace xmlns, Uri file, DirectoryInfoBase rootFolder)
        {
            var rootfile = this.fileSystem.FileInfo.FromFileName(this.fileSystem.Path.Combine(rootFolder.FullName, "index.html"));

            var xElement = new XElement(xmlns + "li", new XAttribute("class", "file"), new XAttribute("id", "root"));

            string nodeText = "Home";

            bool fileIsActuallyTheRoot = this.DetermineWhetherFileIsTheRootFile(file, rootfile);
            if (fileIsActuallyTheRoot)
            {
                xElement.Add(new XElement(xmlns + "span", new XAttribute("class", "current"), nodeText));
            }
            else
            {
                xElement.Add(new XElement(
                    xmlns + "a",
                    new XAttribute("href", file.GetUriForTargetRelativeToMe(rootfile, ".html")),
                    nodeText));
            }

            return xElement;
        }

        private bool DetermineWhetherFileIsTheRootFile(Uri file, FileInfoBase rootfile)
        {
            var fileInfo = this.fileSystem.FileInfo.FromFileName(file.LocalPath);

            if (rootfile.DirectoryName != fileInfo.DirectoryName)
            {
                return false; // they're not even in the same directory
            }

            if (rootfile.FullName == file.LocalPath)
            {
                return true; // it's really the same file
            }

            if (fileInfo.Name == string.Empty)
            {
                return true; // the file is actually the directory, so we consider that the root file
            }

            if (fileInfo.Name.StartsWith("index", StringComparison.InvariantCultureIgnoreCase))
            {
                return true; // the file is an index file, so we consider that the root
            }

            return false;
        }

        private XElement AddNodeForFile(XNamespace xmlns, Uri file, Tree childNode)
        {
            var xElement = new XElement(xmlns + "li", new XAttribute("class", "file"));

            string nodeText = childNode.Data.Name;
            if (childNode.Data.OriginalLocationUrl == file)
            {
                xElement.Add(new XElement(xmlns + "span", new XAttribute("class", "current"), nodeText));
            }
            else
            {
                xElement.Add(new XElement(xmlns + "a", new XAttribute("href", childNode.Data.GetRelativeUriTo(file)), nodeText));
            }

            var featureNode = childNode.Data as FeatureNode;
            if (featureNode != null && this.imageResultFormatter != null)
            {
                Feature feature = featureNode.Feature;

                XElement formatForToC = this.imageResultFormatter.FormatForToC(feature);

                if (formatForToC != null)
                {
                    xElement.Add(formatForToC);
                }
            }

            return xElement;
        }

        private XElement BuildCollapser(XNamespace xmlns)
        {
            return new XElement(
                xmlns + "p",
                new XAttribute("class", "tocCollapser"),
                new XAttribute("title", "Collapse Table of Content"),
                new XText("«"));
        }

        public XElement Format(Uri file, Tree features, DirectoryInfoBase outputFolder)
        {
            XNamespace xmlns = HtmlNamespace.Xhtml;

            XElement ul = this.BuildListItems(xmlns, file, features);
            ul.AddFirst(this.AddNodeForHome(xmlns, file, outputFolder));

            return new XElement(
                xmlns + "div",
                new XAttribute("id", "toc"),
                this.BuildCollapser(xmlns),
                ul);
        }
    }
}
