//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MarkdownSteps.cs" company="PicklesDoc">
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

using Autofac;
using NUnit.Framework;
using PicklesDoc.Pickles.Test;
using System;
using System.IO;
using TechTalk.SpecFlow;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.AcceptanceTests.Steps
{
    [Binding]
    public sealed class MarkdownSteps : BaseFixture
    {
        [Given(@"I specify the output folder as '(.*)'")]
        public void Given_I_Specify_The_Output_File_As(string outputFolder)
        {
            Configuration.OutputFolder = this.FileSystem.DirectoryInfo.FromDirectoryName(outputFolder);
        }

        [When(@"I generate Markdown output")]
        public void When_I_Generate_Markdown_Output()
        {
            var configuration = this.Configuration;
            var markdownDocumentationBuilder = this.Container.Resolve<MarkdownDocumentationBuilder>();

            DateTime executionTime;

            if (ScenarioContext.Current.ContainsKey("DateTime.Now"))
            {
                executionTime = (DateTime)ScenarioContext.Current["DateTime.Now"];
            }
            else
            {
                executionTime = DateTime.Now;
            }

            using (var DateTimeContext = new DisposableTestDateTime(executionTime))
            {
                markdownDocumentationBuilder.Build(null);
            }
        }

        [Then(@"the Markdown output has the line")]
        public void Then_The_Markdown_Output_Has_The_Line(Table table)
        {

            var actualResult = this.FileSystem.File.ReadAllText(TargetFile(this.Configuration));

            var resultArray = actualResult.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (var check in table.Rows)
            {
                var expectedValue = check["Content"];
                var expectedRow = int.Parse(check["Line No."]);

                Assert.IsTrue(resultArray.Length >= expectedRow, "Not enough lines in output");
                Assert.AreEqual(expectedValue, resultArray[expectedRow-1]);
            }
        }

        [Then(@"the file '(.*)\\features.md' exists")]
        public void Then_The_Features_Markdown_File_Exists(string outputFolder)
        {
            var expectedFile = Path.Combine(outputFolder, "features.md");
            Assert.IsTrue(this.FileSystem.File.Exists(expectedFile));
        }

        // Duplicated logic from Builder class, should be moved to it's own class?
        private string TargetFile(IConfiguration configuration)
        {
            var fileName = "features.md";

            string defaultOutputFile = string.Empty;
            if (configuration.OutputFolder == null)
            {
                defaultOutputFile = Path.Combine(@"C:\testing", fileName);
            }
            else
            {
                defaultOutputFile = Path.Combine(configuration.OutputFolder.FullName, fileName);
            }

            return defaultOutputFile;
        }
    }
}
