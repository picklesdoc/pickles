﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WordFeatureFormatter.cs" company="PicklesDoc">
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
using DocumentFormat.OpenXml.Wordprocessing;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders.Word.Extensions;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word
{
    public class WordFeatureFormatter
    {
        private readonly IConfiguration configuration;
        private readonly ITestResults testResults;
        private readonly WordScenarioFormatter wordScenarioFormatter;
        private readonly WordScenarioFormatter wordScenarioOutlineFormatter;
        private readonly WordStyleApplicator wordStyleApplicator;
        private readonly WordDescriptionFormatter wordDescriptionFormatter;
        private readonly WordBackgroundFormatter wordBackgroundFormatter;

        public WordFeatureFormatter(
            WordScenarioFormatter wordScenarioFormatter,
            WordScenarioFormatter wordScenarioOutlineFormatter,
            WordStyleApplicator wordStyleApplicator,
            WordDescriptionFormatter wordDescriptionFormatter,
            WordBackgroundFormatter wordBackgroundFormatter,
            IConfiguration configuration,
            ITestResults testResults)
        {
            this.wordScenarioFormatter = wordScenarioFormatter;
            this.wordScenarioOutlineFormatter = wordScenarioOutlineFormatter;
            this.wordStyleApplicator = wordStyleApplicator;
            this.wordDescriptionFormatter = wordDescriptionFormatter;
            this.wordBackgroundFormatter = wordBackgroundFormatter;
            this.configuration = configuration;
            this.testResults = testResults;
        }

        public void Format(Body body, FeatureNode featureNode)
        {
            Feature feature = featureNode.Feature;
            var hideTags = configuration.HideTags?.Split(';', '@', ' ')
                .Select(s => s[0] == '@' ? s : '@' + s).ToArray() ?? Array.Empty<string>();
            if (feature.Tags.Intersect(hideTags, StringComparer.InvariantCultureIgnoreCase).Any())
            {
                return;
            }

            if (this.configuration.FeaturesOnSamePage)
            {
                body.GenerateParagraph("", "Normal");
            }
            else
            {
                body.InsertPageBreak();
            }

            if (this.configuration.HasTestResults)
            {
                TestResult testResult = this.testResults.GetFeatureResult(feature);
                if (testResult == TestResult.Passed)
                {
                    body.GenerateParagraph("Passed", "Passed");
                }
                else if (testResult == TestResult.Failed)
                {
                    body.GenerateParagraph("Failed", "Failed");
                }
            }

            body.GenerateParagraph(feature.Name, "Heading1");

            if ( feature.Tags.Count != 0 &&
                 !hideTags.Contains("@Tag"))
            {
                var paragraph = new Paragraph(new ParagraphProperties(new ParagraphStyleId {Val = "Normal"}));
                var tagrunProp = new RunProperties(new Italic(), new Color {ThemeColor = ThemeColorValues.Text2}) {Bold = new Bold() {Val = false}};
                paragraph.Append(new Run(tagrunProp, new Text("(Tags: " + string.Join(", ", feature.Tags) + ")")));
                body.Append(paragraph);
            }

            this.wordDescriptionFormatter.Format(body, feature.Description);

            if ((feature.Background != null) &&
                !hideTags.Contains("@Background") &&
                !feature.Background.Tags.Intersect(hideTags, StringComparer.InvariantCultureIgnoreCase).Any())
            {
                this.wordBackgroundFormatter.Format(body, feature.Background);
            }

            foreach (IFeatureElement featureElement in feature.FeatureElements)
            {
                var scenario = featureElement as Scenario;
                if ((scenario != null) &&
                    !hideTags.Contains("@Scenario") &&
                    !scenario.Tags.Intersect(hideTags, StringComparer.InvariantCultureIgnoreCase).Any())
                {
                    this.wordScenarioFormatter.Format(body, scenario);
                }
            }
        }
    }
}
