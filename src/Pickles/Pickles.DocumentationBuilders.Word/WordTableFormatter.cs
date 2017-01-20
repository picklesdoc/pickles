//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WordTableFormatter.cs" company="PicklesDoc">
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

using DocumentFormat.OpenXml.Wordprocessing;
using Table = PicklesDoc.Pickles.ObjectModel.Table;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word
{
    public class WordTableFormatter
    {
        private static TableProperties GenerateTableProperties()
        {
            var tableProperties1 = new TableProperties();
            var tableStyle1 = new TableStyle { Val = "TableGrid" };
            var tableWidth1 = new TableWidth { Width = "0", Type = TableWidthUnitValues.Auto };
            var tableLook1 = new TableLook { Val = "04A0" };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableLook1);
            return tableProperties1;
        }

        public void Format(Body body, Table table)
        {
            body.Append(this.CreateWordTableFromPicklesTable(table));
        }

        public DocumentFormat.OpenXml.Wordprocessing.Table CreateWordTableFromPicklesTable(Table table)
        {
            var wordTable = new DocumentFormat.OpenXml.Wordprocessing.Table();
            wordTable.Append(GenerateTableProperties());
            var headerRow = new TableRow();
            foreach (string cell in table.HeaderRow.Cells)
            {
                var wordCell = new TableCell();
                wordCell.Append(new Paragraph(new Run(new Text(cell))));
                headerRow.Append(wordCell);
            }

            wordTable.Append(headerRow);

            foreach (ObjectModel.TableRow row in table.DataRows)
            {
                var wordRow = new TableRow();

                foreach (string cell in row.Cells)
                {
                    var wordCell = new TableCell();
                    wordCell.Append(new Paragraph(new Run(new Text(cell))));
                    wordRow.Append(wordCell);
                }

                wordTable.Append(wordRow);
            }

            return wordTable;
        }
    }
}
