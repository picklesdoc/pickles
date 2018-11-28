//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StepBlock.cs" company="PicklesDoc">
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
using System.Collections.Generic;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.Blocks
{
    class StepBlock : Block
    {
        readonly Step step;

        public StepBlock(Step step, Stylist style) : base(style)
        {
            this.step = step;
            this.lines = RenderedBlock();
        }

        private Lines RenderedBlock()
        {
            var lines = new Lines
            {
                Heading(),

                AvailableTable()
            };

            return lines;
        }

        private Lines Heading()
        {
            var lines = new Lines();

            var stepLine = style.AsStep(step.NativeKeyword, step.Name);
            lines.Add(stepLine);

            return lines;
        }

        private Lines AvailableTable()
        {
            var lines = new Lines();

            if (step.TableArgument != null)
            {
                lines.Add(style.AsStepLine(string.Empty));

                var tableBlock = new TableBlock(step.TableArgument, style);

                lines.Add(tableBlock.Lines);
            }

            return lines;
        }
    }
}
