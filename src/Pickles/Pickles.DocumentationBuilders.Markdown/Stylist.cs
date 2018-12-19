//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Stylist.cs" company="PicklesDoc">
//  Copyright 2018 Darren Comeau
//  Copyright 2018-present PicklesDoc team and community contributors
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

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown
{
    class Stylist
    {
        public string TableResultHeading { get { return "Result"; } }

        internal virtual string AsTitle(string title)
        {
            var titleTemplate = "# {0}";

            var styledTitle = string.Format(titleTemplate, title);

            return styledTitle;
        }

        internal virtual string AsFeatureHeading(string featureName)
        {
            var featureHeadingTemplate = "### {0}";

            var styledFeatureHeading = string.Format(featureHeadingTemplate, featureName);

            return styledFeatureHeading;
        }

        internal virtual string AsBackgroundHeading(string scenarioName)
        {
            var backgroundHeadingTemplate = "#### Background: {0}";

            var styledBackgroundHeading = string.Format(backgroundHeadingTemplate, scenarioName).Trim();

            return styledBackgroundHeading;
        }

        internal virtual string AsScenarioHeading(string scenarioName)
        {
            var scenarioHeadingTemplate = "#### Scenario: {0}";

            var styledScenarioHeading = string.Format(scenarioHeadingTemplate, scenarioName);

            return styledScenarioHeading;
        }

        internal virtual string AsResult(TestResult testresult)
        {
            var resultTemplate = "![{0}]({1})";

            string styledResult = "";

            switch (testresult)
            {
                case TestResult.Passed:
                    styledResult = string.Format(resultTemplate,"Passed","pass.png");
                    break;

                case TestResult.Failed:
                    styledResult = string.Format(resultTemplate, "Failed", "fail.png");
                    break;

                case TestResult.Inconclusive:
                    styledResult = string.Format(resultTemplate, "Inconclusive", "inconclusive.png");
                    break;

                case TestResult.NotProvided:
                    break;

                default:
                    throw new System.Exception("Unhandled TestResult value");
            }

            return styledResult;
        }

        internal virtual string AsScenarioHeading(string scenarioName, TestResult result)
        {
            var scenarioHeadingTemplate = "#### Scenario: {0} {1}";

            var styledScenarioHeading = string.Format(scenarioHeadingTemplate, AsResult(result), scenarioName);

            return styledScenarioHeading;
        }

        internal virtual string AsScenarioOutlineHeading(string scenarioOutlineName)
        {
            var scenarioOutlineHeadingTemplate = "#### Scenario Outline: {0}";

            var styledScenarioOutlineHeading = string.Format(scenarioOutlineHeadingTemplate, scenarioOutlineName);

            return styledScenarioOutlineHeading;
        }

        internal virtual string AsExampleHeading(string exampleName)
        {
            var exampleHeadingTemplate = "> Examples: {0}";

            var styledExampleHeading = string.Format(exampleHeadingTemplate, exampleName)
                .TrimEnd();

            return styledExampleHeading;
        }

        internal virtual string AsTag(string tag)
        {
            var tagTemplate = "*`{0}`*";

            var styledFeatureHeading = string.Format(tagTemplate, tag);

            return styledFeatureHeading;
        }

        internal virtual string AsStep(string keyword, string step)
        {
            var stepTemplate = "**{0}** {1}";

            var styledStep = string.Format(stepTemplate, keyword.Trim(), step.Trim());

            return AsStepLine(styledStep);
        }

        internal virtual string AsStepTable(string tableLine)
        {
            var line = string.Empty;
            line = string.Concat(line, string.Format(tableLine, " | ", "---"))
                .Trim();
            return AsStepLine(line);
        }

        internal virtual string AsStepLine(string stepLine)
        {
            var stepLineTemplate = "> {0}";

            var styledStepLine = string.Format(stepLineTemplate, EscapeMarkdownSensitiveCharacters(stepLine))
                .TrimEnd();

            return styledStepLine;
        }

        protected string EscapeMarkdownSensitiveCharacters(string rawText)
        {
            var escapedText = rawText
                .Replace("<", @"\<")
                .Replace(">", @"\>");

            return escapedText;
        }
    }
}
