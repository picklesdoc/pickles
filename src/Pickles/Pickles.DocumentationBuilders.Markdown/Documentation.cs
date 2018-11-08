//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Documentation.cs" company="PicklesDoc">
//  Copyright 2018 Darren Comeau
//  Copyright 2018-present PicklesDoc team and community contributors
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

using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders.Markdown.Blocks;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown
{
    class Documentation
    {
        private readonly Stylist style = new Stylist();

        public int PageCount { get; set; }

        public string CurrentPage { get; set; }

        public Documentation(Tree featureTree)
        {
            PageCount = 1;

            CurrentPage = Document(featureTree);
        }

        private string Document(Tree featureTree)
        {
            var documentLines = new Lines
            {
                TitleBlock(),

                TreeContent(featureTree)
            };

            return documentLines.ToString();
        }

        private Lines TitleBlock()
        {
            var titleBlock = new TitleBlock(style);

            return titleBlock.Lines;
        }

        private Lines TreeContent(Tree featureTree)
        {
            var lines = new Lines();

            if (featureTree != null)
            {
                foreach (var node in featureTree)
                {
                    if (IsFeatureNode(node))
                    {
                        lines.Add(string.Empty);
                        lines.Add(Feature((FeatureNode)node));
                    }
                }
            }

            return lines;
        }

        private bool IsFeatureNode(INode node)
        {
            return node.GetType() == typeof(FeatureNode);
        }

        private Lines Feature(FeatureNode node)
        {
            var feature = node.Feature;

            var featureBlock = new FeatureBlock(feature, style);

            return featureBlock.Lines;
        }
    }
}
