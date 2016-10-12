//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ExcelDocumentationBuilder.cs" company="PicklesDoc">
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
using System.IO.Abstractions;
using System.Reflection;
using ClosedXML.Excel;
using NLog;
using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;

namespace PicklesDoc.Pickles.DocumentationBuilders.Excel
{
    public class ExcelDocumentationBuilder : IDocumentationBuilder
    {
        private static readonly Logger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        private readonly IConfiguration configuration;
        private readonly ExcelFeatureFormatter excelFeatureFormatter;
        private readonly ExcelSheetNameGenerator excelSheetNameGenerator;
        private readonly ExcelTableOfContentsFormatter excelTableOfContentsFormatter;
        private readonly IFileSystem fileSystem;

        public ExcelDocumentationBuilder(
            IConfiguration configuration,
            ExcelFeatureFormatter excelFeatureFormatter,
            ExcelSheetNameGenerator excelSheetNameGenerator,
            ExcelTableOfContentsFormatter excelTableOfContentsFormatter,
            IFileSystem fileSystem)
        {
            this.configuration = configuration;
            this.excelFeatureFormatter = excelFeatureFormatter;
            this.excelSheetNameGenerator = excelSheetNameGenerator;
            this.excelTableOfContentsFormatter = excelTableOfContentsFormatter;
            this.fileSystem = fileSystem;
        }

        public void Build(Tree features)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Info("Writing Excel workbook to {0}", this.configuration.OutputFolder.FullName);
            }

            string spreadsheetPath = this.fileSystem.Path.Combine(this.configuration.OutputFolder.FullName, "features.xlsx");
            using (var workbook = new XLWorkbook())
            {
                foreach (var node in features)
                {
                    var featureDirectoryTreeNode =
                        node as FeatureNode;
                    if (featureDirectoryTreeNode != null)
                    {
                        IXLWorksheet worksheet =
                            workbook.AddWorksheet(
                                this.excelSheetNameGenerator.GenerateSheetName(
                                    workbook,
                                    featureDirectoryTreeNode
                                        .Feature));
                        this.excelFeatureFormatter.Format(
                            worksheet,
                            featureDirectoryTreeNode.Feature);
                    }
                }

                this.excelTableOfContentsFormatter.Format(workbook, features);

                workbook.SaveAs(spreadsheetPath);
            }
        }
    }
}
