#region License

/*
    Copyright [2011] [Jeffrey Cameron]

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

#endregion

using System;
using System.Reflection;
using ClosedXML.Excel;
using NLog;
using PicklesDoc.Pickles.Parser;
using PicklesDoc.Pickles.TestFrameworks;

namespace PicklesDoc.Pickles.DocumentationBuilders.Excel
{
  public class ExcelFeatureFormatter
  {
    private static readonly Logger log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

    private readonly ExcelScenarioFormatter excelScenarioFormatter;
    private readonly ExcelScenarioOutlineFormatter excelScenarioOutlineFormatter;
    private readonly Configuration configuration;
    private readonly ITestResults testResults;

    public ExcelFeatureFormatter(ExcelScenarioFormatter excelScenarioFormatter,
                                 ExcelScenarioOutlineFormatter excelScenarioOutlineFormatter,
                                 Configuration configuration,
                                 ITestResults testResults)
    {
      this.excelScenarioFormatter = excelScenarioFormatter;
      this.excelScenarioOutlineFormatter = excelScenarioOutlineFormatter;
      this.configuration = configuration;
      this.testResults = testResults;
    }

    public void Format(IXLWorksheet worksheet, Feature feature)
    {
      worksheet.Cell("A1").Style.Font.SetBold();
      worksheet.Cell("A1").Value = feature.Name;

      if (feature.Description.Length <= short.MaxValue)
      {
        worksheet.Cell("B2").Value = feature.Description;
      }
      else
      {
        var description = feature.Description.Substring(0, short.MaxValue);
        log.Warn("The description of feature {0} was truncated because of cell size limitations in Excel.", feature.Name);
        worksheet.Cell("B2").Value = description;
      }

      worksheet.Cell("B2").Style.Alignment.WrapText = false;

      var results = this.testResults.GetFeatureResult(feature);

      if (this.configuration.HasTestResults && results.WasExecuted)
      {
        worksheet.Cell("A1").Style.Fill.SetBackgroundColor(results.WasSuccessful
                                                               ? XLColor.AppleGreen
                                                               : XLColor.CandyAppleRed);
      }

      int row = 4;
      foreach (IFeatureElement featureElement in feature.FeatureElements)
      {
        var scenario = featureElement as Scenario;
        if (scenario != null)
        {
          this.excelScenarioFormatter.Format(worksheet, scenario, ref row);
        }

        var scenarioOutline = featureElement as ScenarioOutline;
        if (scenarioOutline != null)
        {
          this.excelScenarioOutlineFormatter.Format(worksheet, scenarioOutline, ref row);
        }

        row++;
      }
    }
  }
}