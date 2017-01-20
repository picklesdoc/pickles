//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WordBackgroundFormatter.cs" company="PicklesDoc">
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

using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using PicklesDoc.Pickles.ObjectModel;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word
{
    public class WordBackgroundFormatter
    {
        private const string DefaultBackgroundKeyword = "Background";

        private readonly ILanguageServices languageServices;
        private readonly WordTableFormatter wordTableFormatter;

        public WordBackgroundFormatter(WordTableFormatter wordTableFormatter, ILanguageServices languageServices)
        {
            this.wordTableFormatter = wordTableFormatter;
            this.languageServices = languageServices;
        }

        public void Format(Body body, Scenario background)
        {
            var headerParagraph = new Paragraph(new ParagraphProperties(new ParagraphStyleId { Val = "Heading2" }));
            var backgroundKeyword = this.GetLocalizedBackgroundKeyword();
            headerParagraph.Append(new Run(new RunProperties(new Bold()), new Text(backgroundKeyword)));

            var table = new Table();
            table.Append(GenerateTableProperties());
            var row = new TableRow();
            var cell = new TableCell();
            cell.Append(headerParagraph);

            foreach (var descriptionSentence in WordDescriptionFormatter.SplitDescription(background.Description))
            {
                cell.Append(CreateNormalParagraph(descriptionSentence));
            }

            foreach (var step in background.Steps)
            {
                cell.Append(WordStepFormatter.GenerateStepParagraph(step));

                if (step.TableArgument != null)
                {
                    cell.Append(this.wordTableFormatter.CreateWordTableFromPicklesTable(step.TableArgument));
                }
            }

            cell.Append(CreateNormalParagraph(string.Empty)); // Is there a better way to generate a new empty line?
            row.Append(cell);
            table.Append(row);

            body.Append(table);
        }

        private string GetLocalizedBackgroundKeyword()
        {
            return this.languageServices.BackgroundKeywords.FirstOrDefault() ?? DefaultBackgroundKeyword;
        }

        private static TableProperties GenerateTableProperties()
        {
            var tableProperties1 = new TableProperties();
            var tableStyle1 = new TableStyle { Val = "TableGrid" };
            var tableWidth1 = new TableWidth { Width = "4900", Type = TableWidthUnitValues.Pct };
            var tableLook1 = new TableLook { Val = "04A0" };
            var tableJustification = new TableJustification { Val = TableRowAlignmentValues.Center };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableLook1);
            tableProperties1.Append(tableJustification);
            return tableProperties1;
        }

        private static Paragraph CreateNormalParagraph(string text)
        {
            var emptyLine = new Paragraph(new ParagraphProperties(new ParagraphStyleId { Val = "Normal" }));
            emptyLine.Append(new Run(new Text(text)));
            return emptyLine;
        }
    }
}
