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

                lines.Add(TableHeader(step.TableArgument.HeaderRow));

                lines.Add(TableRows(step.TableArgument.DataRows));
            }

            return lines;
        }

        private Lines TableHeader(TableRow headerRow)
        {
            var lines = new Lines
            {
                style.AsStepTable(TableLine(headerRow)),

                style.AsStepTable(TableSeperatorLine(headerRow))
            };

            return lines;
        }

        private Lines TableRows(List<TableRow> dataRows)
        {
            var lines = new Lines();

            foreach (var row in dataRows)
            {
                lines.Add(TableRow(row));
            }

            return lines;
        }

        private Lines TableRow(TableRow row)
        {
            var lines = new Lines
            {
                style.AsStepTable(TableLine(row))
            };

            return lines;
        }

        private string TableLine(TableRow row)
        {
            var line = string.Empty;
            foreach (var column in row.Cells)
            {
                line = string.Concat(line, "{0}", column);
            }
            line = string.Concat(line, "{0}");

            return line;
        }

        private string TableSeperatorLine(TableRow row)
        {
            var line = string.Empty;
            var cellIndex = 0;

            while (cellIndex < row.Cells.Count)
            {
                line = string.Concat(line, "{0}", "{1}");
                cellIndex++;
            }

            line = string.Concat(line, "{0}");

            return line;
        }
    }
}
