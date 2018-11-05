//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FeatureBlock.cs" company="PicklesDoc">
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

using PicklesDoc.Pickles.ObjectModel;
using System;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.Blocks
{
    class FeatureBlock
    {
        readonly Feature feature;
        readonly Stylist style;

        public FeatureBlock(Feature feature, Stylist style)
        {
            this.feature = feature;
            this.style = style;
        }

        public new string ToString()
        {
            return RenderedBlock();
        }

        private string RenderedBlock()
        {
            var lines = new string[]
                {
                    FeatureHeading()
                };

            return LineArrayToString(lines);
        }

        private string FeatureHeading()
        {
            return style.AsFeatureHeading(feature.Name);
        }

        private string LineArrayToString(string[] lines)
        {
            string result = string.Empty;

            foreach (var line in lines)
            {
                result = string.Concat(result, line, Environment.NewLine);
            }

            return result;
        }
    }
}
