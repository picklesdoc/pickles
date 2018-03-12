//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TableToJsonTableMapperTests.cs" company="PicklesDoc">
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
using System.Collections.Generic;
using System.Linq;
using NFluent;
using NUnit.Framework;

using PicklesDoc.Pickles.DocumentationBuilders.Json;
using PicklesDoc.Pickles.DocumentationBuilders.Json.Mapper;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test.ObjectModel.Json
{
    [TestFixture]
    public class TableToJsonTableMapperTests
    {
        [Test]
        public void Map_Null_ReturnsNull()
        {
            var mapper = CreateMapper();

            JsonTable actual = mapper.Map(null);

            Check.That(actual).IsNull();
        }

        private static TableToJsonTableMapper CreateMapper()
        {
            return new TableToJsonTableMapper();
        }

        [Test]
        public void Map_TableWithHeaderRow_ReturnsJsonTableWithoutHeaderRow()
        {
            var table = new Table {  HeaderRow = null };

            var mapper = CreateMapper();

            var actual = mapper.Map(table);

            Check.That(actual.HeaderRow).IsNull();
        }

        [Test]
        public void Map_TableWithHeaderRow_ReturnsJsonTableWithHeaderRow()
        {
            var table = new Table { HeaderRow = new TableRow("column 1", "column 2") };

            var mapper = CreateMapper();

            var actual = mapper.Map(table);

            Check.That(actual.HeaderRow).ContainsExactly("column 1", "column 2");
        }

        [Test]
        public void Map_TableWithDataRows_ReturnsJsonTableWithDataRows()
        {
            var table = new Table
            {
                DataRows = new List<TableRow>
                {
                    new TableRow("cell 1-1", "cell 1-2"),
                    new TableRow("cell 2-1", "cell 2-2")
                }
            };

            var testResult = new JsonTestResult
            {
                WasExecuted = false,
                WasSuccessful = false
            };

            var mapper = CreateMapper();
            var actual = mapper.Map(table);

            Check.That(actual.DataRows.Count).IsEqualTo(2);

            Check.That(actual.DataRows[0].Count).IsEqualTo(2);
            Check.That(actual.DataRows[0].OfType<string>()).ContainsExactly("cell 1-1", "cell 1-2");

            Check.That(actual.DataRows[1].Count).IsEqualTo(2);
            Check.That(actual.DataRows[1].OfType<string>()).ContainsExactly("cell 2-1", "cell 2-2");
        }

        [Test]
        public void Map_ExampleTableWithDataRows_ReturnsJsonTableWithDataRows()
        {
            var table = new ExampleTable
            {
                DataRows = new List<TableRow>
                {
                    new TableRowWithTestResult("cell 1-1", "cell 1-2"),
                    new TableRowWithTestResult("cell 2-1", "cell 2-2")
                }
            };

            var testResult = new JsonTestResult
            {
                WasExecuted = false,
                WasSuccessful = false
            };

            var mapper = CreateMapper();
            var actual = mapper.MapWithTestResults(table);

            Check.That(actual.DataRows.Count).IsEqualTo(2);

            Check.That(actual.DataRows[0].Count).IsEqualTo(3);
            Check.That(actual.DataRows[0].OfType<string>()).ContainsExactly("cell 1-1", "cell 1-2");
            Check.That((actual.DataRows[0] as JsonTableRowWithTestResult).Result.WasSuccessful).Equals(false);
            Check.That((actual.DataRows[0] as JsonTableRowWithTestResult).Result.WasExecuted).Equals(false);

            Check.That(actual.DataRows[1].Count).IsEqualTo(3);
            Check.That(actual.DataRows[1].OfType<string>()).ContainsExactly("cell 2-1", "cell 2-2");
            Check.That((actual.DataRows[1] as JsonTableRowWithTestResult).Result.WasSuccessful).Equals(false);
            Check.That((actual.DataRows[1] as JsonTableRowWithTestResult).Result.WasExecuted).Equals(false);

        }

        [Test]
        public void Map_TableWithNullDataRow_ReturnsJsonTableWithEmptyDataRows()
        {
            var table = new Table { DataRows = null };

            var mapper = CreateMapper();

            var actual = mapper.Map(table);

            Check.That(actual.DataRows).IsNotNull();
        }
    }
}