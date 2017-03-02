//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WordScenarioOutlineFormatter.cs" company="PicklesDoc">
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
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

using PicklesDoc.Pickles.DocumentationBuilders.Word.Extensions;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word
{
    public class WordScenarioOutlineFormatter
    {
        private readonly IConfiguration configuration;
        private readonly ITestResults testResults;
        private readonly WordStepFormatter wordStepFormatter;
        private readonly WordTableFormatter wordTableFormatter;
        private readonly ILanguageServicesRegistry languageServicesRegistry;

        public WordScenarioOutlineFormatter(WordStepFormatter wordStepFormatter, WordTableFormatter wordTableFormatter, IConfiguration configuration, ITestResults testResults, ILanguageServicesRegistry languageServicesRegistry)
        {
            this.wordStepFormatter = wordStepFormatter;
            this.wordTableFormatter = wordTableFormatter;
            this.configuration = configuration;
            this.testResults = testResults;
            this.languageServicesRegistry = languageServicesRegistry;
        }

        public void Format(Body body, ScenarioOutline scenarioOutline)
        {
            if (this.configuration.HasTestResults)
            {
                TestResult testResult = this.testResults.GetScenarioOutlineResult(scenarioOutline);
                if (testResult == TestResult.Passed)
                {
                    body.GenerateParagraph("Passed", "Passed");
                }
                else if (testResult == TestResult.Failed)
                {
                    body.GenerateParagraph("Failed", "Failed");
                }
            }

            body.GenerateParagraph(scenarioOutline.Name, "Heading2");
            if (scenarioOutline.Tags.Count != 0)
            {
                var paragraph = new Paragraph(new ParagraphProperties(new ParagraphStyleId { Val = "Normal" }));
                var tagrunProp = new RunProperties(new Italic(), new Color { ThemeColor = ThemeColorValues.Text2 }) { Bold = new Bold() { Val = false } };
                paragraph.Append(new Run(tagrunProp, new Text("(Tags: " + string.Join(", ", scenarioOutline.Tags) + ")")));
                body.Append(paragraph);
            }
            if (!string.IsNullOrEmpty(scenarioOutline.Description))
            {
                body.GenerateParagraph(scenarioOutline.Description, "Normal");
            }

            foreach (Step step in scenarioOutline.Steps)
            {
                this.wordStepFormatter.Format(body, step);
            }

            var languageServices = this.languageServicesRegistry.GetLanguageServicesForLanguage(scenarioOutline.Feature?.Language);
            var examplesKeyword = languageServices.ExamplesKeywords[0];

            foreach (var example in scenarioOutline.Examples)
            {
                body.Append(new Paragraph(new ParagraphProperties(new ParagraphStyleId { Val = "Heading3" }), new Run(new RunProperties(), new Text(examplesKeyword + ":"))));

                if (example.Tags.Count != 0)
                {
                    var tagrunProp = new RunProperties(new Italic(), new Color { ThemeColor = ThemeColorValues.Text2 }) { Bold = new Bold() { Val = false } };
                    body.Append(new Paragraph(new ParagraphProperties(new ParagraphStyleId { Val = "Normal" }),
                                              new Run(tagrunProp, new Text("(Tags: " + string.Join(", ", example.Tags) + ")"))));
                }

                if (!string.IsNullOrWhiteSpace(example.Description))
                {
                    body.Append(new Paragraph(new ParagraphProperties(new ParagraphStyleId { Val = "Normal" }),
                                              new Run(new Text(example.Description))));
                }

                this.wordTableFormatter.Format( body, example.TableArgument );
            }
        }
    }
}
