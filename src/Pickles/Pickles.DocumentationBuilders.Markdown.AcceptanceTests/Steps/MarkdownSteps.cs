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
using TechTalk.SpecFlow;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.AcceptanceTests.Steps
{
    [Binding]
    public sealed class MarkdownSteps : BaseFixture
    {
        [When(@"I generate Markdown output")]
        public void WhenIGenerateMarkdownOutput()
        {
            var configuration = this.Configuration;
            configuration.OutputFolder = this.FileSystem.DirectoryInfo.FromDirectoryName(@"c:\output\");
            var markdownDocumentationBuilder = this.Container.Resolve<MarkdownDocumentationBuilder>();

            markdownDocumentationBuilder.Build(null);
        }

        [Then(@"the Markdown output has the line")]
        public void ThenTheMarkdownOutputHasTheLine(Table table)
        {
            var actualResult = this.FileSystem.File.ReadAllText(@"c:\output\pickledFeatures.md");

            var resultArray = actualResult.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (var check in table.Rows)
            {
                var expectedValue = check["Content"];
                var expectedRow = int.Parse(check["Line No."]);

                Assert.IsTrue(resultArray.Length >= expectedRow, "Not enough lines in output");
                Assert.AreEqual(expectedValue, resultArray[expectedRow-1]);
            }
        }
    }
}
