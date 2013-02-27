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
using System.IO;
using System.Xml.Linq;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.Extensions;
using PicklesDoc.Pickles.Parser;
using PicklesDoc.Pickles.TestFrameworks;

namespace PicklesDoc.Pickles.DocumentationBuilders.DITA
{
    public class DitaFeatureFormatter
    {
        private readonly Configuration configuration;
        private readonly DitaMapPathGenerator ditaMapPathGenerator;
        private readonly DitaScenarioFormatter ditaScenarioFormatter;
        private readonly DitaScenarioOutlineFormatter ditaScenarioOutlineFormatter;
        private readonly ITestResults nunitResults;

        public DitaFeatureFormatter(Configuration configuration, DitaScenarioFormatter ditaScenarioFormatter,
                                    DitaScenarioOutlineFormatter ditaScenarioOutlineFormatter,
                                    DitaMapPathGenerator ditaMapPathGenerator, ITestResults nunitResults)
        {
            this.configuration = configuration;
            this.ditaScenarioFormatter = ditaScenarioFormatter;
            this.ditaScenarioOutlineFormatter = ditaScenarioOutlineFormatter;
            this.ditaMapPathGenerator = ditaMapPathGenerator;
            this.nunitResults = nunitResults;
        }

        public void Format(FeatureDirectoryTreeNode featureNode)
        {
            Feature feature = featureNode.Feature;

            var topic = new XElement("topic", new XAttribute("id", feature.Name.ToDitaName()));
            topic.Add(new XElement("title", feature.Name));
            topic.Add(new XElement("shortdesc", feature.Description));

            var body = new XElement("body");
            topic.Add(body);

            if (this.configuration.HasTestResults)
            {
                TestResult testResult = this.nunitResults.GetFeatureResult(feature);
                if (testResult.WasExecuted && testResult.WasSuccessful)
                {
                    body.Add(new XElement("note", "This feature passed"));
                }
                else if (testResult.WasExecuted && !testResult.WasSuccessful)
                {
                    body.Add(new XElement("note", "This feature failed"));
                }
            }

            foreach (IFeatureElement featureElement in feature.FeatureElements)
            {
                var scenario = featureElement as Scenario;
                if (scenario != null)
                {
                    this.ditaScenarioFormatter.Format(body, scenario);
                }

                var scenarioOutline = featureElement as ScenarioOutline;
                if (scenarioOutline != null)
                {
                    this.ditaScenarioOutlineFormatter.Format(body, scenarioOutline);
                }
            }

            // HACK - This relative path stuff needs to be refactored
            string relativePath =
                new FileInfo(Path.Combine(this.configuration.OutputFolder.FullName, featureNode.RelativePathFromRoot)).
                    Directory.FullName.ToLowerInvariant();
            if (!Directory.Exists(relativePath)) Directory.CreateDirectory(relativePath);
            Uri relativeFilePath = ditaMapPathGenerator.GeneratePathToFeature(featureNode);
            string filename = Path.Combine(relativePath, Path.GetFileName(relativeFilePath.ToString()));
            var document =
                new XDocument(new XDocumentType("topic", "-//OASIS//DTD DITA Topic//EN", "topic.dtd", string.Empty),
                              topic);
            document.Save(filename);
        }
    }
}