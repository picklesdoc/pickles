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
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using PicklesDoc.Pickles.Extensions;
using PicklesDoc.Pickles.Parser;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word
{
    public class WordStepFormatter
    {
        private readonly WordTableFormatter wordTableFormatter;

        public WordStepFormatter(WordTableFormatter wordTableFormatter)
        {
            this.wordTableFormatter = wordTableFormatter;
        }

        public void Format(Body body, Step step)
        {
            var paragraph = GenerateStepParagraph(step);
            body.Append(paragraph);

            if (!string.IsNullOrEmpty(step.DocStringArgument))
            {
                string[] lines = step.DocStringArgument.Split(new[] { Environment.NewLine },
                                                              StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    body.GenerateParagraph(line, "Quote");
                }
            }

            if (step.TableArgument != null)
            {
                this.wordTableFormatter.Format(body, step.TableArgument);
            }
        }

        public static Paragraph GenerateStepParagraph(Step step)
        {
            // HACK - We need to generate a custom paragraph here because 2 Run objects are needed to allow for the bolded keyword
            var paragraph = new Paragraph(new ParagraphProperties(new ParagraphStyleId { Val = "Normal" }));
            paragraph.Append(new Run(new RunProperties(new Bold()), new Text(step.NativeKeyword)));
            var nameText = new Text { Space = SpaceProcessingModeValues.Preserve };
            nameText.Text = " " + step.Name;
            paragraph.Append(new Run(nameText));
            return paragraph;
        }
    }
}