//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlContentFormatter.cs" company="PicklesDoc">
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

using PicklesDoc.Pickles.DirectoryCrawler;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html
{
    public class HtmlContentFormatter
    {
        private readonly IHtmlFeatureFormatter htmlFeatureFormatter;
        private readonly HtmlIndexFormatter htmlIndexFormatter;

        public HtmlContentFormatter(IHtmlFeatureFormatter htmlFeatureFormatter, HtmlIndexFormatter htmlIndexFormatter)
        {
            if (htmlFeatureFormatter == null)
            {
                throw new ArgumentNullException("htmlFeatureFormatter");
            }

            if (htmlIndexFormatter == null)
            {
                throw new ArgumentNullException("htmlIndexFormatter");
            }

            this.htmlFeatureFormatter = htmlFeatureFormatter;
            this.htmlIndexFormatter = htmlIndexFormatter;
        }

        public XElement Format(INode contentNode, IEnumerable<INode> features)
        {
            var featureItemNode = contentNode as FeatureNode;
            if (featureItemNode != null)
            {
                var formattedContent = this.htmlFeatureFormatter.Format(featureItemNode.Feature);
                return formattedContent;
            }

            var indexItemNode = contentNode as FolderNode;
            if (indexItemNode != null)
            {
                return this.htmlIndexFormatter.Format(indexItemNode, features);
            }

            var markdownItemNode = contentNode as MarkdownNode;
            if (markdownItemNode != null)
            {
                return markdownItemNode.MarkdownContent;
            }

            throw new InvalidOperationException("Cannot format a FeatureNode with a Type of " + contentNode.GetType() +
                                                " as content");
        }
    }
}
