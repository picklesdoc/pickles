//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MockStylist.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.UnitTests
{
    class MockStylist : Stylist
    {
        private const string oneParamDefaultFormat = ">>{0}<<";
        private const string twoParamDefaultFormat = oneParamDefaultFormat + ">>{1}<<";
        public MockStylist()
        {
            FeatureHeadingFormat = oneParamDefaultFormat;
            TagFormat = oneParamDefaultFormat;
            BackgroundHeadingFormat = oneParamDefaultFormat;
            ScenarioHeadingFormat = oneParamDefaultFormat;
            ScenarioOutlineHeadingFormat = oneParamDefaultFormat;
            StepFormat = twoParamDefaultFormat;
        }

        internal override string AsTitle(string title)
        {
            return "Mocked Title Style: " + title;
        }

        public string FeatureHeadingFormat { get; set; }

        internal override string AsFeatureHeading(string featureName)
        {
            return string.Format(FeatureHeadingFormat, featureName);
        }

        public string BackgroundHeadingFormat { get; set; }

        internal override string AsBackgroundHeading(string scenarioName)
        {
            return string.Format(BackgroundHeadingFormat, scenarioName).Trim();
        }

        public string ScenarioHeadingFormat { get; set; }

        internal override string AsScenarioHeading(string scenarioName)
        {
            return string.Format(ScenarioHeadingFormat, scenarioName);
        }

        internal override string AsScenarioHeading(string scenarioName, TestResult result)
        {
            return string.Format(ScenarioHeadingFormat, string.Concat("result ",scenarioName));
        }

        public string ScenarioOutlineHeadingFormat { get; set; }

        internal override string AsScenarioOutlineHeading(string scenarioName)
        {
            return string.Format(ScenarioOutlineHeadingFormat, scenarioName);
        }

        public string ExampleHeadingFormat { get; set; }

        internal override string AsExampleHeading(string exampleName)
        {
            return string.Format(ExampleHeadingFormat, exampleName);
        }

        public string TagFormat { get; set; }

        internal override string AsTag(string tagName)
        {
            return string.Format(TagFormat, tagName);
        }

        public string StepFormat { get; set; }

        internal override string AsStep(string keyword, string step)
        {
            step = EscapeMarkdownSensitiveCharacters(step);

            return string.Format(StepFormat, keyword.Trim(), step.Trim());
        }
    }
}
