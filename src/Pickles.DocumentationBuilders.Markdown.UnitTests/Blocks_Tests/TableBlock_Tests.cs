//  --------------------------------------------------------------------------------------------------------------------ScenarioBlock_Tests
//  <copyright file="TableBlock_Tests.cs" company="PicklesDoc">
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

using NUnit.Framework;
using PicklesDoc.Pickles.DocumentationBuilders.Markdown.Blocks;
using PicklesDoc.Pickles.ObjectModel;
using System;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.UnitTests
{
    [TestFixture]
    public class TableBlock_Tests
    {
        [Test]
        public void A_Table_Is_Formatted()
        {
            var mockStyle = new MockStylist
            {
            };

            var table = new Table();
            table.HeaderRow = new TableRow(new[] { "Col1", "Col2" });
            table.DataRows = new System.Collections.Generic.List<ObjectModel.TableRow>();
            table.DataRows.Add(new TableRow(new[] { "Col1Row1", "Col2Row1" }));
            table.DataRows.Add(new TableRow(new[] { "Col1Row2", "Col2Row2" }));

            var tableBlock = new TableBlock(table, mockStyle);
            var actualString = tableBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.That("> | Col1 | Col2 |", Is.EqualTo(actualString[0]));
            Assert.That("> | --- | --- |", Is.EqualTo(actualString[1]));
            Assert.That("> | Col1Row1 | Col2Row1 |", Is.EqualTo(actualString[2]));
            Assert.That("> | Col1Row2 | Col2Row2 |", Is.EqualTo(actualString[3]));
            Assert.That(5, Is.EqualTo(actualString.Length));
        }

        [Test]
        public void A_Table_Is_Formatted_With_Placeholders()
        {
            var mockStyle = new MockStylist
            {
            };

            var table = new Table();
            table.HeaderRow = new TableRow(new[] { "Col1", "Col2" });
            table.DataRows = new System.Collections.Generic.List<ObjectModel.TableRow>();
            table.DataRows.Add(new TableRow(new[] { "Col1Row1", "<Col2Row1>" }));
            table.DataRows.Add(new TableRow(new[] { "<Col1Row2>", "Col2Row2" }));

            var tableBlock = new TableBlock(table, mockStyle);
            var actualString = tableBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.That("> | Col1 | Col2 |", Is.EqualTo(actualString[0]));
            Assert.That("> | --- | --- |", Is.EqualTo(actualString[1]));
            Assert.That(@"> | Col1Row1 | \<Col2Row1\> |", Is.EqualTo(actualString[2]));
            Assert.That(@"> | \<Col1Row2\> | Col2Row2 |", Is.EqualTo(actualString[3]));
            Assert.That(5, Is.EqualTo(actualString.Length));
        }

        [Test]
        public void A_Table_Is_Formatted_With_Results()
        {
            var mockStyle = new MockStylist
            {
            };

            var table = new Table();
            table.HeaderRow = new TableRow(new[] { "Col1", "Col2" });
            table.DataRows = new System.Collections.Generic.List<ObjectModel.TableRow>();
            AddRowWithResult(table, new[] { "Col1Row1", "Col2Row1" }, TestResult.Passed);
            AddRowWithResult(table, new[] { "Col1Row2", "Col2Row2" }, TestResult.Failed);
            AddRowWithResult(table, new[] { "Col1Row3", "Col2Row3" }, TestResult.Inconclusive);
            AddRowWithResult(table, new[] { "Col1Row4", "Col2Row4" }, TestResult.NotProvided);

            var tableBlock = new TableBlock(table, mockStyle, true);
            var actualString = tableBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.That("> | Col1 | Col2 | Result |", Is.EqualTo(actualString[0]));
            Assert.That("> | --- | --- | --- |", Is.EqualTo(actualString[1]));
            Assert.That("> | Col1Row1 | Col2Row1 | ![Passed](pass.png) |", Is.EqualTo(actualString[2]));
            Assert.That("> | Col1Row2 | Col2Row2 | ![Failed](fail.png) |", Is.EqualTo(actualString[3]));
            Assert.That("> | Col1Row3 | Col2Row3 | ![Inconclusive](inconclusive.png) |", Is.EqualTo(actualString[4]));
            Assert.That("> | Col1Row4 | Col2Row4 |  |", Is.EqualTo(actualString[5]));
            Assert.That(7, Is.EqualTo(actualString.Length));
        }

        private void AddRowWithResult(Table table,string[] data, TestResult result)
        {
            var tableRowWithResult = new TableRowWithTestResult(data)
            {
                Result = result
            };
            table.DataRows.Add(tableRowWithResult);
        }
    }
}
