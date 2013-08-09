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
using DocumentFormat.OpenXml.Wordprocessing;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.Extensions;
using PicklesDoc.Pickles.Parser;
using PicklesDoc.Pickles.TestFrameworks;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word
{
    public class WordFeatureFormatter
    {
        private readonly Configuration configuration;
        private readonly ITestResults nunitResults;
        private readonly WordScenarioFormatter wordScenarioFormatter;
        private readonly WordScenarioOutlineFormatter wordScenarioOutlineFormatter;
        private readonly WordStyleApplicator wordStyleApplicator;
        private readonly WordDescriptionFormatter wordDescriptionFormatter;
        private readonly WordBackgroundFormatter wordBackgroundFormatter;

        public WordFeatureFormatter(WordScenarioFormatter wordScenarioFormatter,
                                    WordScenarioOutlineFormatter wordScenarioOutlineFormatter,
                                    WordStyleApplicator wordStyleApplicator, 
                                    WordDescriptionFormatter wordDescriptionFormatter,
                                    WordBackgroundFormatter wordBackgroundFormatter,
                                    Configuration configuration,
                                    ITestResults nunitResults)
        {
            this.wordScenarioFormatter = wordScenarioFormatter;
            this.wordScenarioOutlineFormatter = wordScenarioOutlineFormatter;
            this.wordStyleApplicator = wordStyleApplicator;
            this.wordDescriptionFormatter = wordDescriptionFormatter;
            this.wordBackgroundFormatter = wordBackgroundFormatter;
            this.configuration = configuration;
            this.nunitResults = nunitResults;
        }

        public void Format(Body body, FeatureNode featureNode)
        {
            Feature feature = featureNode.Feature;

            body.InsertPageBreak();

            if (this.configuration.HasTestResults)
            {
                TestResult testResult = this.nunitResults.GetFeatureResult(feature);
                if (testResult.WasExecuted && testResult.WasSuccessful)
                {
                    body.GenerateParagraph("Passed", "Passed");
                }
                else if (testResult.WasExecuted && !testResult.WasSuccessful)
                {
                    body.GenerateParagraph("Failed", "Failed");
                }
            }

            body.GenerateParagraph(feature.Name, "Heading1");
            this.wordDescriptionFormatter.Format(body, feature.Description);
            this.wordBackgroundFormatter.Format(body, feature.Background);

            foreach (IFeatureElement featureElement in feature.FeatureElements)
            {
                var scenario = featureElement as Scenario;
                if (scenario != null)
                {
                    this.wordScenarioFormatter.Format(body, scenario);
                }

                var scenarioOutline = featureElement as ScenarioOutline;
                if (scenarioOutline != null)
                {
                    this.wordScenarioOutlineFormatter.Format(body, scenarioOutline);
                }
            }
        }
    }
}