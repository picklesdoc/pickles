//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlIndexFormatter.cs" company="PicklesDoc">
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
using System.IO.Abstractions;
using System.Linq;
using System.Xml.Linq;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.Extensions;

namespace PicklesDoc.Pickles.DocumentationBuilders.HTML
{
    public class HtmlIndexFormatter
    {
        private readonly XNamespace xmlns = HtmlNamespace.Xhtml;

        public XElement Format(INode node, IEnumerable<INode> features)
        {
            /*
       <div id="feature">
         <h1>Folder Name</h1>
          <ul class="list">
              <li><a href="[link]"><span class="title">[title]</span><span class="separator"> - </span><span class="description">[description]</span></a></li>
              <li><a href="[link]"><span class="title">[title]</span><span class="separator"> - </span><span class="description">[description]</span></a></li>
              <li><a href="[link]"><span class="title">[title]</span><span class="separator"> - </span><span class="description">[description]</span></a></li>
          </ul>
       <div>
       */
            var directoryInfo = node.OriginalLocation as DirectoryInfoBase;

            if (directoryInfo == null)
            {
                throw new ArgumentOutOfRangeException("node", "Argument node must contain a DirectoryInfo.");
            }

            string[] files = directoryInfo.GetFiles().Select(f => f.FullName).ToArray();

            INode[] featuresThatAreDirectChildrenOfFolder =
                features.Where(f => f.OriginalLocation is FileInfoBase).Where(
                    f => files.Contains(f.OriginalLocation.FullName)).ToArray();

            var div = new XElement(
                this.xmlns + "div",
                new XAttribute("id", "feature"),
                new XElement(this.xmlns + "h1", node.Name));

            MarkdownNode markdownNode =
                featuresThatAreDirectChildrenOfFolder.Where(n => n.IsIndexMarkDownNode()).OfType<MarkdownNode>().FirstOrDefault();
            if (markdownNode != null)
            {
                div.Add(
                    new XElement(
                        this.xmlns + "div",
                        new XAttribute("class", "folderDescription"),
                        markdownNode.MarkdownContent));
            }

            div.Add(this.FormatList(node, featuresThatAreDirectChildrenOfFolder.OfType<FeatureNode>().OrderBy(feature => feature.Name)));

            return div;
        }

        private XElement FormatList(INode node, IEnumerable<FeatureNode> items)
        {
            // <ul class="list">...</ul>
            var list = new XElement(this.xmlns + "ul", new XAttribute("class", "list"));

            foreach (
                XElement li in
                    items.Select(
                        item =>
                            this.FormatListItem(item.GetRelativeUriTo(node.OriginalLocationUrl), item.Feature.Name, item.Feature.Description)))
            {
                list.Add(li);
            }

            return list;
        }

        private XElement FormatListItem(string link, string title, string description)
        {
            // <li><a href="[link]"><span class="title">[title]</span><span class="separator"> - </span><span class="description">[description]</span></a></li>
            return new XElement(
                this.xmlns + "li",
                new XElement(
                    this.xmlns + "a",
                    new XAttribute("href", link),
                    new XElement(
                        this.xmlns + "span",
                        new XAttribute("class", "title"),
                        title),
                    new XElement(
                        this.xmlns + "span",
                        new XAttribute("class", "separator"),
                        " - "),
                    new XElement(
                        this.xmlns + "span",
                        new XAttribute("class", "description"),
                        description)));
        }
    }
}
