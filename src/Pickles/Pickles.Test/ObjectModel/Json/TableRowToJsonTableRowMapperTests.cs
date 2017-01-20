//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TableRowToJsonTableRowMapperTests.cs" company="PicklesDoc">
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

using PicklesDoc.Pickles.DocumentationBuilders.Json;
using PicklesDoc.Pickles.DocumentationBuilders.Json.Mapper;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test.ObjectModel.Json
{
    [TestFixture]
    public class TableRowToJsonTableRowMapperTests
    {
        [Test]
        public void Map_NullTableRow_ReturnsNull()
        {
            var mapper = CreateMapper();

            JsonTableRow actual = mapper.Map((TableRow) null);

            Check.That(actual).IsNull();
        }

        private static TableRowToJsonTableRowMapper CreateMapper()
        {
            return new TableRowToJsonTableRowMapper();
        }

        [Test]
        public void Map_TableRowWithResult_ReturnsObjectWithResult()
        {
            var tableRow = new TableRow { Result = TestResult.Passed };

            var mapper = CreateMapper();

            var actual = mapper.Map(tableRow);

            Check.That(actual.Result.WasExecuted).IsTrue();
            Check.That(actual.Result.WasSuccessful).IsTrue();
        }

        [Test]
        public void Map_TableRowWithRows_ReturnsObjectWithStrings()
        {
            var tableRow = new TableRow("first string", "second string", "third string");

            var mapper = CreateMapper();

            var actual = mapper.Map(tableRow);

            Check.That(actual).ContainsExactly("first string", "second string", "third string");
        }
        [Test]
        public void Map_TableRowWithCells_ConvertsToJsonTableRow()
        {
            var tableRow = new TableRow { Cells = { "cell 1", "cell 2" } };

            var mapper = CreateMapper();

            var jsonTableRow = mapper.Map(tableRow);

            Check.That(jsonTableRow).ContainsExactly("cell 1", "cell 2");
        }

        [Test]
        public void Map_TableRowWithTestResult_ConvertsToJsonTableRowWithTestResult()
        {
            var tableRow = new TableRow { Result = TestResult.Passed };

            var mapper = CreateMapper();

            var jsonTableRow = mapper.Map(tableRow);

            Check.That(jsonTableRow.Result.WasExecuted).IsEqualTo(true);
            Check.That(jsonTableRow.Result.WasSuccessful).IsEqualTo(true);
        }
    }
}