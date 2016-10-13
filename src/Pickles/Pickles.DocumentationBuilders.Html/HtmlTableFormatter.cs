//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlTableFormatter.cs" company="PicklesDoc">
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
using System.Linq;
using System.Xml.Linq;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html
{
    public class HtmlTableFormatter
    {
        private readonly XNamespace xmlns;
        private readonly HtmlImageResultFormatter htmlImageResultFormatter;

        public HtmlTableFormatter(HtmlImageResultFormatter htmlImageResultFormatter)
        {
            this.htmlImageResultFormatter = htmlImageResultFormatter;
            this.xmlns = HtmlNamespace.Xhtml;
        }

        public XElement Format(Table table)
        {
            return this.Format(table, null);
        }

        public XElement Format(Table table, ScenarioOutline scenarioOutline)
        {
            if (table == null)
            {
                return null;
            }

            var headerCells = table.HeaderRow.Cells.ToArray();

            headerCells = headerCells.Concat(new[] { " " }).ToArray();

            return new XElement(
                this.xmlns + "div",
                new XAttribute("class", "table_container"),
                new XElement(
                    this.xmlns + "table",
                    new XAttribute("class", "datatable"),
                    new XElement(
                        this.xmlns + "thead",
                        new XElement(
                            this.xmlns + "tr",
                            headerCells.Select(
                                cell => new XElement(this.xmlns + "th", cell)))),
                    new XElement(
                        this.xmlns + "tbody",
                        table.DataRows.Select(row => this.FormatRow(row, scenarioOutline)))));
        }

        private XElement FormatRow(TableRow row, ScenarioOutline scenarioOutline)
        {
            var formattedCells = row.Cells.Select(
                cell =>
                    new XElement(
                        this.xmlns + "td",
                        cell)).ToList();

            if (scenarioOutline != null)
            {
                formattedCells.Add(
                    new XElement(this.xmlns + "td", this.htmlImageResultFormatter.Format(scenarioOutline, row.Cells.ToArray())));
            }

            var result = new XElement(this.xmlns + "tr");

            foreach (var cell in formattedCells)
            {
                result.Add(cell);
            }

            return result;
        }
    }
}
