//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenAddingATableToAWorksheet.cs" company="PicklesDoc">
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

using Autofac;

using ClosedXML.Excel;

using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.Test;

namespace PicklesDoc.Pickles.DocumentationBuilders.Excel.UnitTests
{
    [TestFixture]
    public class WhenAddingATableToAWorksheet : BaseFixture
    {
        [Test]
        public void ThenTableAddedSuccessfully()
        {
            var excelTableFormatter = Container.Resolve<ExcelTableFormatter>();
            var table = new Table();
            table.HeaderRow = new TableRow("Var1", "Var2", "Var3", "Var4");
            table.DataRows =
                new List<TableRow>(new[] { new TableRow("1", "2", "3", "4"), new TableRow("5", "6", "7", "8") });

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("SHEET1");
                int row = 6;
                excelTableFormatter.Format(worksheet, table, ref row);

                Check.That(worksheet.Cell("D6").Value).IsEqualTo("Var1");
                Check.That(worksheet.Cell("E6").Value).IsEqualTo("Var2");
                Check.That(worksheet.Cell("F6").Value).IsEqualTo("Var3");
                Check.That(worksheet.Cell("G6").Value).IsEqualTo("Var4");
                Check.That(worksheet.Cell("D7").Value).IsEqualTo(1.0);
                Check.That(worksheet.Cell("E7").Value).IsEqualTo(2.0);
                Check.That(worksheet.Cell("F7").Value).IsEqualTo(3.0);
                Check.That(worksheet.Cell("G7").Value).IsEqualTo(4.0);
                Check.That(worksheet.Cell("D8").Value).IsEqualTo(5.0);
                Check.That(worksheet.Cell("E8").Value).IsEqualTo(6.0);
                Check.That(worksheet.Cell("F8").Value).IsEqualTo(7.0);
                Check.That(worksheet.Cell("G8").Value).IsEqualTo(8.0);
                Check.That(row).IsEqualTo(9);
            }
        }
    }
}
