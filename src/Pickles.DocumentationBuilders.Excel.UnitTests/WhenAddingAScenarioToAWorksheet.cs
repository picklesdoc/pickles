//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenAddingAScenarioToAWorksheet.cs" company="PicklesDoc">
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
    public class WhenAddingAScenarioToAWorksheet : BaseFixture
    {
        [Test]
        public void ThenSingleScenarioAddedSuccessfully()
        {
            var excelScenarioFormatter = Container.Resolve<ExcelScenarioFormatter>();
            var exampleTable = new ExampleTable();
            exampleTable.HeaderRow = new TableRow("Var1", "Var2", "Var3", "Var4");
            exampleTable.DataRows =
                new List<TableRow>(new[] { new TableRowWithTestResult("1", "2", "3", "4"), new TableRowWithTestResult("5", "6", "7", "8") });
            var example = new Example { Name = "Examples", Description = string.Empty, TableArgument = exampleTable };
            var examples = new List<Example>();
            examples.Add(example);
            var scenario = new Scenario
            {
                Name = "Test Feature",
                Description =
                    "In order to test this feature,\nAs a developer\nI want to test this feature",
                Examples = examples,
                Tags = { "tag1", "tag2" }
            };

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("SHEET1");
                int row = 3;
                excelScenarioFormatter.Format(worksheet, scenario, ref row);

                Check.That(worksheet.Cell("B3").GetValue<string>()).IsEqualTo(scenario.Name);
                Check.That(worksheet.Cell("C4").GetValue<string>()).IsEqualTo("tag1, tag2");
                Check.That(worksheet.Cell("C5").GetValue<string>()).IsEqualTo(scenario.Description);
                Check.That(worksheet.Cell("B7").GetValue<string>()).IsEqualTo("Examples");
                Check.That(worksheet.Cell("D8").GetValue<string>()).IsEqualTo("Var1");
                Check.That(worksheet.Cell("E8").GetValue<string>()).IsEqualTo("Var2");
                Check.That(worksheet.Cell("F8").GetValue<string>()).IsEqualTo("Var3");
                Check.That(worksheet.Cell("G8").GetValue<string>()).IsEqualTo("Var4");
                Check.That(worksheet.Cell("D9").GetValue<string>()).IsEqualTo("1");
                Check.That(worksheet.Cell("E9").GetValue<string>()).IsEqualTo("2");
                Check.That(worksheet.Cell("F9").GetValue<string>()).IsEqualTo("3");
                Check.That(worksheet.Cell("G9").GetValue<string>()).IsEqualTo("4");
                Check.That(worksheet.Cell("D10").GetValue<string>()).IsEqualTo("5");
                Check.That(worksheet.Cell("E10").GetValue<string>()).IsEqualTo("6");
                Check.That(worksheet.Cell("F10").GetValue<string>()).IsEqualTo("7");
                Check.That(worksheet.Cell("G10").GetValue<string>()).IsEqualTo("8");
                Check.That(row).IsEqualTo(11);
            }
        }

        [Test]
        public void ThenSingleScenarioWithStepsAddedSuccessfully()
        {
            var excelScenarioFormatter = Container.Resolve<ExcelScenarioFormatter>();
            var exampleTable = new ExampleTable();
            exampleTable.HeaderRow = new TableRow("Var1", "Var2", "Var3", "Var4");
            exampleTable.DataRows =
                new List<TableRow>(new[] { new TableRowWithTestResult("1", "2", "3", "4"), new TableRowWithTestResult("5", "6", "7", "8") });
            var example = new Example { Name = "Examples", Description = string.Empty, TableArgument = exampleTable };
            var examples = new List<Example>();
            examples.Add(example);
            var scenario = new Scenario
            {
                Name = "Test Feature",
                Description =
                    "In order to test this feature,\nAs a developer\nI want to test this feature",
                Examples = examples
            };
            var given = new Step { NativeKeyword = "Given", Name = "a precondition" };
            var when = new Step { NativeKeyword = "When", Name = "an event occurs" };
            var then = new Step { NativeKeyword = "Then", Name = "a postcondition" };
            scenario.Steps = new List<Step>(new[] { given, when, then });

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("SHEET1");
                int row = 3;
                excelScenarioFormatter.Format(worksheet, scenario, ref row);

                Check.That(worksheet.Cell("B3").GetValue<string>()).IsEqualTo(scenario.Name);
                Check.That(worksheet.Cell("C4").GetValue<string>()).IsEqualTo(scenario.Description);
                Check.That(worksheet.Cell("B9").GetValue<string>()).IsEqualTo("Examples");
                Check.That(worksheet.Cell("D10").GetValue<string>()).IsEqualTo("Var1");
                Check.That(worksheet.Cell("E10").GetValue<string>()).IsEqualTo("Var2");
                Check.That(worksheet.Cell("F10").GetValue<string>()).IsEqualTo("Var3");
                Check.That(worksheet.Cell("G10").GetValue<string>()).IsEqualTo("Var4");
                Check.That(worksheet.Cell("D11").GetValue<string>()).IsEqualTo("1");
                Check.That(worksheet.Cell("E11").GetValue<string>()).IsEqualTo("2");
                Check.That(worksheet.Cell("F11").GetValue<string>()).IsEqualTo("3");
                Check.That(worksheet.Cell("G11").GetValue<string>()).IsEqualTo("4");
                Check.That(worksheet.Cell("D12").GetValue<string>()).IsEqualTo("5");
                Check.That(worksheet.Cell("E12").GetValue<string>()).IsEqualTo("6");
                Check.That(worksheet.Cell("F12").GetValue<string>()).IsEqualTo("7");
                Check.That(worksheet.Cell("G12").GetValue<string>()).IsEqualTo("8");
                Check.That(row).IsEqualTo(13);
            }
        }

        [Test]
        public void ThenDutchScenarioWithScenarioContainsVoorbeelden()
        {
            var scenario = SetupScenario();

            scenario.Feature = new Feature { Language = "nl" };

            var excelScenarioFormatter = Container.Resolve<ExcelScenarioFormatter>();


            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("SHEET1");
                int row = 3;
                excelScenarioFormatter.Format(worksheet, scenario, ref row);

                Check.That(worksheet.Cell("B5").GetValue<string>()).IsEqualTo("Voorbeelden");
            }
        }

        private static Scenario SetupScenario()
        {
            var scenario = new Scenario
            {
                Name = "Test Feature",
                Examples = new List<Example>
                {
                    new Example
                    {
                        Name = "Examples",
                        Description = string.Empty,
                        TableArgument = new ExampleTable
                        {
                            HeaderRow = new TableRow(),
                            DataRows = new List<TableRow>()
                        }
                    }
                },
            };
            return scenario;
        }
    }
}
