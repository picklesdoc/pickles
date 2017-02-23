//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WordScenarioFormatter.cs" company="PicklesDoc">
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
using DocumentFormat.OpenXml.Wordprocessing;

using PicklesDoc.Pickles.DocumentationBuilders.Word.Extensions;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word
{
    public class WordScenarioFormatter
    {
        private readonly IConfiguration configuration;
        private readonly ITestResults testResults;
        private readonly WordStepFormatter wordStepFormatter;

        public WordScenarioFormatter(WordStepFormatter wordStepFormatter, IConfiguration configuration, ITestResults testResults)
        {
            this.wordStepFormatter = wordStepFormatter;
            this.configuration = configuration;
            this.testResults = testResults;
        }

        public void Format(Body body, Scenario scenario)
        {
            if (this.configuration.HasTestResults)
            {
                TestResult testResult = this.testResults.GetScenarioResult(scenario);
                if (testResult == TestResult.Passed)
                {
                    body.GenerateParagraph("Passed", "Passed");
                }
                else if (testResult == TestResult.Failed)
                {
                    body.GenerateParagraph("Failed", "Failed");
                }
            }

            body.GenerateParagraph(scenario.Name, "Heading2");
            if (scenario.Tags.Count != 0)
            {
                var paragraph = new Paragraph(new ParagraphProperties(new ParagraphStyleId { Val = "Normal" }));
                var tagrunProp = new RunProperties(new Italic(), new Color { ThemeColor = ThemeColorValues.Text2 }) { Bold = new Bold() { Val = false } };
                paragraph.Append(new Run(tagrunProp, new Text("(Tags: " + string.Join(", ", scenario.Tags) + ")")));
                body.Append(paragraph);
            }
            if (!string.IsNullOrEmpty(scenario.Description))
            {
                body.GenerateParagraph(scenario.Description, "Normal");
            }

            foreach (Step step in scenario.Steps)
            {
                this.wordStepFormatter.Format(body, step);
            }
        }
    }
}
