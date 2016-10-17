//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenAddingADocumentStringToAWorksheet.cs" company="PicklesDoc">
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
using ClosedXML.Excel;
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.DocumentationBuilders.Excel;

namespace PicklesDoc.Pickles.Test.DocumentationBuilders.Excel
{
    [TestFixture]
    public class WhenAddingADocumentStringToAWorksheet : BaseFixture
    {
        [Test]
        public void ThenDocumentStringAddedSuccessfully()
        {
            var excelDocumentStringFormatter = new ExcelDocumentStringFormatter();
            string documentString = @"This is an example
document string for use
in testing";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("SHEET1");
                int row = 7;
                excelDocumentStringFormatter.Format(worksheet, documentString, ref row);

                Check.That(worksheet.Cell("D7").Value).IsEqualTo("This is an example");
                Check.That(worksheet.Cell("D8").Value).IsEqualTo("document string for use");
                Check.That(worksheet.Cell("D9").Value).IsEqualTo("in testing");
                Check.That(row).IsEqualTo(10);
            }
        }
    }
}
