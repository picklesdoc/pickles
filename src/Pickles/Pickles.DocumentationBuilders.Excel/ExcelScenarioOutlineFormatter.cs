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
        private readonly ILanguageServicesRegistry languageServicesRegistry;

        public ExcelScenarioOutlineFormatter(
            ExcelStepFormatter excelStepFormatter,
            ExcelTableFormatter excelTableFormatter,
            IConfiguration configuration,
            ITestResults testResults,
            ILanguageServicesRegistry languageServicesRegistry)
        {
            this.excelStepFormatter = excelStepFormatter;
            this.excelTableFormatter = excelTableFormatter;
            this.configuration = configuration;
            this.testResults = testResults;
            this.languageServicesRegistry = languageServicesRegistry;
        }

        public void Format(IXLWorksheet worksheet, ScenarioOutline scenarioOutline, ref int row)
        {
            int originalRow = row;
            worksheet.Cell(row, "B").Style.Font.SetBold();
            worksheet.Cell(row++, "B").Value = scenarioOutline.Name;

            if (scenarioOutline.Tags != null && scenarioOutline.Tags.Count != 0)
            {
              worksheet.Cell(row, "B").Value = "Tags:";
              worksheet.Cell(row, "C").Value = String.Join(", ", scenarioOutline.Tags);
              worksheet.Cell(row, "B").Style.Font.Italic = true;
              worksheet.Cell(row, "B").Style.Font.FontColor = XLColor.DavysGrey;
              worksheet.Cell(row, "C").Style.Font.Italic = true;
              worksheet.Cell(row, "C").Style.Font.FontColor = XLColor.DavysGrey;
              row++;
            }

            if (! string.IsNullOrWhiteSpace(scenarioOutline.Description))
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

            var languageServices = this.languageServicesRegistry.GetLanguageServicesForLanguage(scenarioOutline.Feature?.Language);

            foreach (var example in scenarioOutline.Examples)
            {
                worksheet.Cell(row++, "B").Value = languageServices.ExamplesKeywords[0];

                if (example.Tags != null && example.Tags.Count != 0)
                {
                  worksheet.Cell(row, "C").Value = "Tags:";
                  worksheet.Cell(row, "D").Value = String.Join(", ", example.Tags);
                  worksheet.Cell(row, "C").Style.Font.Italic = true;
                  worksheet.Cell(row, "C").Style.Font.FontColor = XLColor.DavysGrey;
                  worksheet.Cell(row, "D").Style.Font.Italic = true;
                  worksheet.Cell(row, "D").Style.Font.FontColor = XLColor.DavysGrey;
                  row++;
                }

                if (! string.IsNullOrWhiteSpace(example.Description))
                    worksheet.Cell(row++, "C").Value = example.Description;

                this.excelTableFormatter.Format(worksheet, example.TableArgument, ref row);
            }
        }
    }
}
