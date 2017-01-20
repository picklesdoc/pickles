//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ExcelTableOfContentsFormatter.cs" company="PicklesDoc">
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
using System.Linq;
using ClosedXML.Excel;
using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;

namespace PicklesDoc.Pickles.DocumentationBuilders.Excel
{
    public class ExcelTableOfContentsFormatter
    {
        private void WriteFileCell(IXLWorksheet worksheet, ref int row, int column, IXLWorksheet featureWorksheet)
        {
            worksheet.Cell(row, column).Value = featureWorksheet.Cell("A1").Value;
            worksheet.Cell(row++, column).Hyperlink = new XLHyperlink(featureWorksheet.Name + "!A1");
        }

        private void WriteDirectoryCell(IXLWorksheet worksheet, ref int row, int column, string directoryName)
        {
            worksheet.Cell(row, column).Style.Font.Bold = true;
            worksheet.Cell(row++, column).Value = directoryName;
        }

        public void BuildTableOfContents(XLWorkbook workbook, IXLWorksheet worksheet, ref int row, int column, Tree features)
        {
            foreach (var childNode in features.ChildNodes)
            {
                var featureChildNode = childNode.Data as FeatureNode;
                if (featureChildNode != null)
                {
                    var featureWorksheet = FindFirstMatchingA1TitleUsingFeatureName(workbook, featureChildNode);
                    this.WriteFileCell(worksheet, ref row, column, featureWorksheet);
                }
                else if (childNode.Data.NodeType == NodeType.Structure)
                {
                    this.WriteDirectoryCell(worksheet, ref row, column, childNode.Data.Name);
                    this.BuildTableOfContents(workbook, worksheet, ref row, column + 1, childNode);
                }
            }
        }

        private static IXLWorksheet FindFirstMatchingA1TitleUsingFeatureName(XLWorkbook workbook, FeatureNode featureChildNode)
        {
            return workbook.Worksheets.FirstOrDefault(
                sheet => sheet.Cell("A1").Value.ToString() == featureChildNode.Feature.Name);
        }

        public void Format(XLWorkbook workbook, Tree features)
        {
            IXLWorksheet tocWorksheet = workbook.AddWorksheet("TOC", 0);

            int startRow = 1;
            this.BuildTableOfContents(workbook, tocWorksheet, ref startRow, 1, features);
        }
    }
}
