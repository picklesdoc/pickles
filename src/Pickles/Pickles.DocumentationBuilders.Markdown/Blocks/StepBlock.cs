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
using System;
using System.Collections.Generic;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.Blocks
{
    class StepBlock
    {
        readonly Step step;
        readonly Stylist style;

        public StepBlock(Step step, Stylist style)
        {
            this.step = step;
            this.style = style;
        }

        public new string ToString()
        {
            return RemoveLastNewLine(RenderedBlock());
        }

        private string RenderedBlock()
        {
            var lines = new List<string>();

            lines = AddHeading(lines);

            lines = AddTableIfAvailable(lines);

            return LineCollectionToString(lines);
        }

        private List<string> AddHeading(List<string> lines)
        {
            var stepLine = style.AsStep(step.NativeKeyword, step.Name);
            lines.Add(stepLine);

            return lines;
        }

        private List<string> AddTableIfAvailable(List<string> lines)
        {
            if (step.TableArgument != null)
            {
                lines.Add(style.AsStepLine(string.Empty));

                lines = AddTableHeader(lines, step.TableArgument.HeaderRow);

                lines = AddTableRows(lines, step.TableArgument.DataRows);
            }

            return lines;
        }

        private List<string> AddTableHeader(List<string> lines, TableRow headerRow)
        {
            lines.Add(style.AsStepTable(TableLine(headerRow)));

            lines.Add(style.AsStepTable(TableSeperatorLine(headerRow)));

            return lines;
        }

        private List<string> AddTableRows(List<string> lines, List<TableRow> dataRows)
        {
            foreach (var row in dataRows)
            {
                lines = AddTableRow(lines, row);
            }

            return lines;
        }

        private List<string> AddTableRow(List<string> lines, TableRow row)
        {
            lines.Add(style.AsStepTable(TableLine(row)));

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

        private string LineCollectionToString(List<string> lines)
        {
            string result = string.Empty;

            foreach (var line in lines)
            {
                result = string.Concat(result, line, Environment.NewLine);
            }

            return result;
        }

        private string RemoveLastNewLine(string text)
        {
            return text.TrimEnd( Environment.NewLine.ToCharArray());
        }
    }
}
