//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CLICommandGeneratorTests.cs" company="PicklesDoc">
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
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.UserInterface.CommandGeneration;
using PicklesDoc.Pickles.UserInterface.Settings;

namespace PicklesDoc.Pickles.UserInterface.UnitTests
{
    [TestFixture]
    public class WhenGeneratingPowerShellCommands : Test.BaseFixture
    {
        private const string MinimalCommandLine = @"Pickle-Features -FeatureDirectory ""C:\Specs"" ";

        private MainModel CreateMinimalModel()
        {
            return
                new MainModel
                {
                    FeatureDirectory = @"C:\Specs",
                    DocumentationFormats = new[] { DocumentationFormat.Html },
                    EnableComments = true,
                    TestResultsFormat = TestResultsFormat.NUnit
                };
        }

        private CommandGeneratorBase CreateGenerator()
        {
            return new PowerShellCommandGenerator();
        }

        [Test]
        public void ThenASingleCommandIsGeneratedForEverySelectedDocumentFormat()
        {
            var model = new MainModel
            {
                FeatureDirectory = @"C:\Specs",
                DocumentationFormats = new[] { DocumentationFormat.Cucumber, DocumentationFormat.Json },
                EnableComments = true
            };

            var sut = this.CreateGenerator();

            Check.That(sut.Generate(model, "en"))
                 .IsEqualTo(
                     @"Pickle-Features -FeatureDirectory ""C:\Specs"" -DocumentationFormat Cucumber"
                     + Environment.NewLine
                     + @"Pickle-Features -FeatureDirectory ""C:\Specs"" -DocumentationFormat Json");
        }

        [Test]
        public void ThenTheDocumentFormatIsAppendedToTheOutputDirectoryIfCreateDirectoryForEachOutputFormatIsTrue()
        {
            var model = new MainModel
            {
                FeatureDirectory = @"C:\Specs",
                OutputDirectory = @"C:\Out",
                CreateDirectoryForEachOutputFormat = true,
                DocumentationFormats = new[] { DocumentationFormat.Cucumber, DocumentationFormat.Json },
                EnableComments = true
            };

            var sut = this.CreateGenerator();

            Check.That(sut.Generate(model, "en"))
                 .IsEqualTo(
                     @"Pickle-Features -FeatureDirectory ""C:\Specs"" -OutputDirectory ""C:\Out\Cucumber"" -DocumentationFormat Cucumber"
                     + Environment.NewLine
                     + @"Pickle-Features -FeatureDirectory ""C:\Specs"" -OutputDirectory ""C:\Out\Json"" -DocumentationFormat Json");
        }

        [Test]
        public void ThenTheDocumentFormatIsAppendedToTheOutputDirectoryIfCreateDirectoryForEachOutputFormatIsTrueIfOutputDirectoryEndsWithDirectorySeparator()
        {
            var model = new MainModel
            {
                FeatureDirectory = @"C:\Specs",
                OutputDirectory = @"C:\Out\",
                CreateDirectoryForEachOutputFormat = true,
                DocumentationFormats = new[] { DocumentationFormat.Cucumber, DocumentationFormat.Json },
                EnableComments = true
            };

            var sut = this.CreateGenerator();

            Check.That(sut.Generate(model, "en"))
                 .IsEqualTo(
                     @"Pickle-Features -FeatureDirectory ""C:\Specs"" -OutputDirectory ""C:\Out\Cucumber"" -DocumentationFormat Cucumber"
                     + Environment.NewLine
                     + @"Pickle-Features -FeatureDirectory ""C:\Specs"" -OutputDirectory ""C:\Out\Json"" -DocumentationFormat Json");
        }

        [Test]
        public void ThenTheProjectNameAndVersionIsAppendedIfSet()
        {
            var model = this.CreateMinimalModel();
            model.ProjectName = "TestProject";
            model.ProjectVersion = "v1.2.4beta1";

            var sut = this.CreateGenerator();

            Check.That(sut.Generate(model, "en"))
                 .IsEqualTo(MinimalCommandLine + "-SystemUnderTestName TestProject -SystemUnderTestVersion v1.2.4beta1");
        }

        [Test]
        public void ThenTheTestResultsFormatAndTheLinkedResultsFileIsAppendedIfIncludeTestResultsIsTrue()
        {
            var model = this.CreateMinimalModel();
            model.IncludeTestResults = true;
            model.TestResultsFile = "result.xml";
            model.TestResultsFormat = TestResultsFormat.XUnit;

            var sut = this.CreateGenerator();

            Check.That(sut.Generate(model, "en"))
                 .IsEqualTo(MinimalCommandLine + @"-TestResultsFile ""result.xml"" -TestResultsFormat xunit");
        }

        [Test]
        public void ThenTheLinkedResultsFileIsAppendedOnlyIfIncludeTestResultsIsTrueAndTheTestResultsFormatIsNunit()
        {
            var model = this.CreateMinimalModel();
            model.IncludeTestResults = true;
            model.TestResultsFile = "result.xml";
            model.TestResultsFormat = TestResultsFormat.NUnit;

            var sut = this.CreateGenerator();

            Check.That(sut.Generate(model, "en"))
                 .IsEqualTo(MinimalCommandLine + @"-TestResultsFile ""result.xml""");
        }

        [Test]
        public void ThenLanguageIsAppendedIfItIsNotEnglish()
        {
            var model = this.CreateMinimalModel();

            var sut = this.CreateGenerator();

            Check.That(sut.Generate(model, "de"))
                 .IsEqualTo(MinimalCommandLine + @"-Language de");
        }

        [Test]
        public void ThenTheIncludeExperimentalFeaturesFlagIsAppendedIfIncludeExperimentalFeaturesIsTrue()
        {
            var model = this.CreateMinimalModel();
            model.IncludeExperimentalFeatures = true;

            var sut = this.CreateGenerator();

            Check.That(sut.Generate(model, "en"))
                 .IsEqualTo(MinimalCommandLine + @"-IncludeExperimentalFeatures");
        }

        [Test]
        public void ThenTheEnableCommentsFlagIsAppendedIfIncludeEnableCommentsIsFalse()
        {
            var model = this.CreateMinimalModel();
            model.EnableComments = false;

            var sut = this.CreateGenerator();

            Check.That(sut.Generate(model, "en"))
                 .IsEqualTo(MinimalCommandLine + @"-EnableComments false");
        }

        [Test]
        public void ThenTheExcludeTagsAreAppendedIfSet()
        {
            var model = this.CreateMinimalModel();
            model.ExcludeTags = "ignore,foo";

            var sut = this.CreateGenerator();

            Check.That(sut.Generate(model, "en"))
                 .IsEqualTo(MinimalCommandLine + @"-ExcludeTags ignore,foo");
        }

        [Test]
        public void ThenTheHideTagsAreAppendedIfSet()
        {
            var model = this.CreateMinimalModel();
            model.HideTags = "ignore,foo";

            var sut = this.CreateGenerator();

            Check.That(sut.Generate(model, "en"))
                 .IsEqualTo(MinimalCommandLine + @"-HideTags ignore,foo");
        }
    }
}