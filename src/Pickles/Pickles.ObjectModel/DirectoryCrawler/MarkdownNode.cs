//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MarkdownNode.cs" company="PicklesDoc">
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
using System.Linq;
using System.Xml.Linq;
using PicklesDoc.Pickles.Extensions;

namespace PicklesDoc.Pickles.DirectoryCrawler
{
    public class MarkdownNode : INode
    {
        public MarkdownNode(FileSystemInfoBase location, string relativePathFromRoot, XElement markdownContent)
        {
            this.OriginalLocation = location;
            this.OriginalLocationUrl = location.ToUri();
            this.RelativePathFromRoot = relativePathFromRoot;
            this.MarkdownContent = markdownContent;
        }

        public XElement MarkdownContent { get; }

        public NodeType NodeType
        {
            get { return NodeType.Content; }
        }

        public string Name
        {
            get
            {
                XElement headerElement =
                    this.MarkdownContent.Descendants().FirstOrDefault(element => element.Name.LocalName == "h1");
                return headerElement != null
                    ? headerElement.Value
                    : this.OriginalLocation.Name.Replace(this.OriginalLocation.Extension, string.Empty).ExpandWikiWord();
            }
        }

        public FileSystemInfoBase OriginalLocation { get; }

        public Uri OriginalLocationUrl { get; }

        public string RelativePathFromRoot { get; }

        public string GetRelativeUriTo(Uri other, string newExtension)
        {
            return other.GetUriForTargetRelativeToMe(this.OriginalLocation, newExtension);
        }

        public string GetRelativeUriTo(Uri other)
        {
            return this.GetRelativeUriTo(other, ".html");
        }
    }
}
