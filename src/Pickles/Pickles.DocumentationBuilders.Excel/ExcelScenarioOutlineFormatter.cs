//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ExcelScenarioOutlineFormatter.cs" company="PicklesDoc">
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

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Excel
{
    public class ExcelScenarioOutlineFormatter
    {
        private readonly ExcelStepFormatter excelStepFormatter;
        private readonly ExcelTableFormatter excelTableFormatter;
        private readonly IConfiguration configuration;
        private readonly ITestResults testResults;

        public ExcelScenarioOutlineFormatter(
            ExcelStepFormatter excelStepFormatter,
            ExcelTableFormatter excelTableFormatter,
            IConfiguration configuration,
            ITestResults testResults)
        {
            this.excelStepFormatter = excelStepFormatter;
            this.excelTableFormatter = excelTableFormatter;
            this.configuration = configuration;
            this.testResults = testResults;
        }

        public void Format(IXLWorksheet worksheet, ScenarioOutline scenarioOutline, ref int row)
        {
            int originalRow = row;
            worksheet.Cell(row++, "B").Value = scenarioOutline.Name;
            worksheet.Cell(row++, "C").Value = scenarioOutline.Description;

            var results = this.testResults.GetScenarioOutlineResult(scenarioOutline);
            if (this.configuration.HasTestResults && (results != TestResult.Inconclusive))
            {
                worksheet.Cell(originalRow, "B").Style.Fill.SetBackgroundColor(results == TestResult.Passed
                    ? XLColor.AppleGreen
                    : XLColor.CandyAppleRed);
            }

            foreach (Step step in scenarioOutline.Steps)
            {
                this.excelStepFormatter.Format(worksheet, step, ref row);
            }

            row++;

            foreach (var example in scenarioOutline.Examples)
            {
                worksheet.Cell(row++, "B").Value = "Examples";
                worksheet.Cell(row, "C").Value = example.Description;
                this.excelTableFormatter.Format(worksheet, example.TableArgument, ref row);
            }
        }
    }
}
