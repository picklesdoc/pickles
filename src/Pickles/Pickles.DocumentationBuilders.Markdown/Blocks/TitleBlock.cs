//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TitleBlock.cs" company="PicklesDoc">
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

using System;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.Blocks
{
    class TitleBlock : Block
    {
        public TitleBlock(Stylist style) : base(style)
        {
            this.lines = RenderedBlock();
        }

        private Lines RenderedBlock()
        {
            var lines = new Lines
                {
                    Title(),
                    string.Empty,
                    GenerationInfo()
                };

            return lines;
        }

        private string Title()
        {
            return style.AsTitle(Localization.Title);
        }

        private string GenerationInfo()
        {
            var generatedDateTime = DateTime.Now;

            return string.Format(Localization.GenerationDateTime, generatedDateTime);
        }
    }
}
