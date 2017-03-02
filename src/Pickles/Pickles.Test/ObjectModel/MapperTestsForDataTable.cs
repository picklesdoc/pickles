//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapperTestsForDataTable.cs" company="PicklesDoc">
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

using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.ObjectModel;
using G = Gherkin.Ast;

namespace PicklesDoc.Pickles.Test.ObjectModel
{
    [TestFixture]
    public class MapperTestsForDataTable
    {
        private readonly Factory factory = new Factory();

        [Test]
        public void MapToStringTableCell_TableCellWithValue_ReturnsThatValue()
        {
            var cell = this.factory.CreateGherkinTableCell("My cell value");

            var mapper = this.factory.CreateMapper();

            string result = mapper.MapToString(cell);

            Check.That(result).IsEqualTo("My cell value");
        }

        [Test]
        public void MapToStringTableCell_NullTableCell_ReturnsNull()
        {
            var mapper = factory.CreateMapper();

            string result = mapper.MapToString((G.TableCell)null);

            Check.That(result).IsNull();
        }

        [Test]
        public void MapToTableRow_NullTableRow_ReturnsNull()
        {
            var mapper = factory.CreateMapper();

            TableRow result = mapper.MapToTableRow(null);

            Check.That(result).IsNull();
        }

        [Test]
        public void MapToTableRow_RowWithCellValues_ReturnsRowContainingThoseValues()
        {
            G.TableRow row = this.factory.CreateGherkinTableRow(
                new[]
                {
                    "first cell",
                    "second cell"
                });

            var mapper = factory.CreateMapper();

            var result = mapper.MapToTableRow(row);

            Check.That(result.Cells).ContainsExactly("first cell", "second cell");
        }

        [Test]
        public void MapToTable_NullDataTable_ReturnsNullTable()
        {
            var mapper = factory.CreateMapper();

            Table result = mapper.MapToTable((G.DataTable) null);

            Check.That(result).IsNull();
        }

        [Test]
        public void MapToTable_DataTableWithThreeRows_ReturnsTableWithHeaderRowAndTwoRows()
        {
            G.DataTable dataTable = this.factory.CreateGherkinDataTable(new[]
            {
                new[] { "Header row, first cell", "Header row, second cell" },
                new[] { "First row, first cell", "First row, second cell" },
                new[] { "Second row, first cell", "Second row, second cell" }
            });

            var mapper = factory.CreateMapper();

            var result = mapper.MapToTable(dataTable);

            Check.That(result.HeaderRow.Cells).ContainsExactly("Header row, first cell", "Header row, second cell");
            Check.That(result.DataRows).HasSize(2);
            Check.That(result.DataRows[0].Cells).ContainsExactly("First row, first cell", "First row, second cell");
            Check.That(result.DataRows[1].Cells).ContainsExactly("Second row, first cell", "Second row, second cell");
        }
    }
}
