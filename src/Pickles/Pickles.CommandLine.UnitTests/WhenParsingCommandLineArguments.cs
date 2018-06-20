//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenParsingCommandLineArguments.cs" company="PicklesDoc">
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
using System.Reflection;
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.Extensions;
using PicklesDoc.Pickles.Test;
using StringWriter = System.IO.StringWriter;
using TextWriter = System.IO.TextWriter;

namespace PicklesDoc.Pickles.CommandLine.UnitTests
{
    [TestFixture]
    public class WhenParsingCommandLineArguments : BaseFixture
    {
        private static readonly string ExpectedHelpString = string.Format(
            "  -f, --feature-directory=VALUE" + "{0}" +
            "                             directory to start scanning recursively for " + "{0}" +
            "                               features" + "{0}" +
            "  -o, --output-directory=VALUE" + "{0}" +
            "                             directory where output files will be placed" + "{0}" +
            "      --trfmt, --test-results-format=VALUE" + "{0}" +
            "                             the format of the linked test results " + "{0}" +
            "                               (nunit|nunit3|xunit|xunit2|mstest " + "{0}" +
            "                               |cucumberjson|specrun|vstest)" + "{0}" +
            "      --lr, --link-results-file=VALUE" + "{0}" +
            "                             the path to the linked test results file (can be " + "{0}" +
            "                               a semicolon-separated list of files)" + "{0}" +
            "      --sn, --system-under-test-name=VALUE" + "{0}" +
            "                             the name of the system under test" + "{0}" +
            "      --sv, --system-under-test-version=VALUE" + "{0}" +
            "                             the version of the system under test" + "{0}" +
            "  -l, --language=VALUE       the language of the feature files" + "{0}" +
            "      --df, --documentation-format=VALUE" + "{0}" +
            "                             the format of the output documentation" + "{0}" +
            "  -v, --version              " + "{0}" +
            "  -h, -?, --help",
            Environment.NewLine);

        private static readonly string ExpectedVersionString = string.Format(@"Pickles version {0}", Assembly.GetExecutingAssembly().GetName().Version);

        public WhenParsingCommandLineArguments()
            : base(Assembly.GetExecutingAssembly().Location)
        {
        }

        [Test]
        public void ThenCanParseExcelDocumentationFormatWithLongFormSuccessfully()
        {
            var args = new[] { @"-documentation-format=excel" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.DocumentationFormat).IsEqualTo(DocumentationFormat.Excel);
        }

        [Test]
        public void ThenCanParseExcelDocumentationFormatWithShortFormSuccessfully()
        {
            var args = new[] { @"-df=excel" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.DocumentationFormat).IsEqualTo(DocumentationFormat.Excel);
        }

        [Test]
        public void ThenCanParseHelpRequestWithLongFormSuccessfully()
        {
            var args = new[] { @"--help" };

            var configuration = new Configuration();
            var writer = new StringWriter();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, writer);

            var actual = RetrieveString(writer);
            var expected = ExpectedHelpString.ComparisonNormalize();
            Check.That(actual).Contains(expected);
            Check.That(shouldContinue).IsFalse();
        }

        private static string RetrieveString(StringWriter writer)
        {
            return writer.GetStringBuilder().ToString().ComparisonNormalize();
        }

        [Test]
        public void ThenCanParseHelpRequestWithQuestionMarkSuccessfully()
        {
            var args = new[] { @"-?" };

            var configuration = new Configuration();
            var writer = new StringWriter();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, writer);

            var actual = RetrieveString(writer);
            Check.That(actual).Contains(ExpectedHelpString.ComparisonNormalize());
            Check.That(shouldContinue).IsFalse();
        }

        [Test]
        public void ThenCanParseHelpRequestWithShortFormSuccessfully()
        {
            var args = new[] { @"-h" };

            var configuration = new Configuration();
            var writer = new StringWriter();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, writer);

            var actual = RetrieveString(writer);
            Check.That(actual).Contains(ExpectedHelpString.ComparisonNormalize());
            Check.That(shouldContinue).IsFalse();
        }

        [Test]
        public void ThenCanParseLongFormArgumentsSuccessfully()
        {
            var args = new[] { @"--feature-directory=c:\features", @"--output-directory=c:\features-output" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.FeatureFolder.FullName).IsEqualTo(@"c:\features");
            Check.That(configuration.OutputFolder.FullName).IsEqualTo(@"c:\features-output");
        }

        [Test]
        public void ThenCanParseResultsFileWithLongFormSuccessfully()
        {
            FileSystem.AddFile(@"c:\results.xml", "<xml />");
            var args = new[] { @"-link-results-file=c:\results.xml" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.HasTestResults).IsTrue();
            Check.That(configuration.TestResultsFile.FullName).IsEqualTo(@"c:\results.xml");
        }

        [Test]
        public void ThenCanParseResultsFileAsSemicolonSeparatedList()
        {
            FileSystem.AddFile(@"c:\results1.xml", "<xml />");
            FileSystem.AddFile(@"c:\results2.xml", "<xml />");
            var args = new[] { @"-link-results-file=c:\results1.xml;c:\results2.xml" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.HasTestResults).IsTrue();
            Check.That(configuration.TestResultsFiles
                 .Select(trf => trf.FullName))
                 .ContainsExactly(@"c:\results1.xml", @"c:\results2.xml");
        }

        [Test]
        public void ThenCanParseResultsFileAsSemicolonSeparatedListAndTestResultsFileContainsTheFirstElementOfTestResultsFiles()
        {
            FileSystem.AddFile(@"c:\results1.xml", "<xml />");
            FileSystem.AddFile(@"c:\results2.xml", "<xml />");
            var args = new[] { @"-link-results-file=c:\results1.xml;c:\results2.xml" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.HasTestResults).IsTrue();
            Check.That(configuration.TestResultsFile.FullName).IsEqualTo(@"c:\results1.xml");
            Check.That(configuration.TestResultsFiles.First().FullName).IsEqualTo(@"c:\results1.xml");
        }

        [Test]
        public void ThenCanParseResultsFileAsSemicolonSeparatedListThatStartsWithASemicolon()
        {
            FileSystem.AddFile(@"c:\results1.xml", "<xml />");
            var args = new[] { @"-link-results-file=;c:\results1.xml" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.HasTestResults).IsTrue();
            Check.That(configuration.TestResultsFiles
                .Select(trf => trf.FullName))
                .ContainsExactly(@"c:\results1.xml");
        }

        [Test]
        public void ThenCanParseResultsFileAsSemicolonSeparatedListThatEndsWithASemicolon()
        {
            FileSystem.AddFile(@"c:\results1.xml", "<xml />");
            var args = new[] { @"-link-results-file=c:\results1.xml;" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.HasTestResults).IsTrue();
            Check.That(configuration.TestResultsFiles
                .Select(trf => trf.FullName))
                .ContainsExactly(@"c:\results1.xml");
        }

        [Test]
        public void ThenCanParseMultipleResultsFilesWithWildCard()
        {
            FileSystem.AddFile(@"c:\results1.xml", "<xml />");
            FileSystem.AddFile(@"c:\results2.xml", "<xml />");
            var args = new[] { @"-link-results-file=c:\results*.xml" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.HasTestResults).IsTrue();
            Check.That(configuration.TestResultsFiles
                .Select(trf => trf.FullName))
                .ContainsExactly(@"c:\results1.xml", @"c:\results2.xml");
        }

        [Test]
        public void ThenCanParseMultipleResultsFilesWithWildCardWhereNoMatchIsExcluded()
        {
            FileSystem.AddFile(@"c:\results1.xml", "<xml />");
            FileSystem.AddFile(@"c:\results2.xml", "<xml />");
            FileSystem.AddFile(@"c:\nomatch_results3.xml", "<xml />");
            var args = new[] { @"-link-results-file=c:\results*.xml" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.HasTestResults).IsTrue();
            Check.That(configuration.TestResultsFiles
                 .Select(trf => trf.FullName))
                 .ContainsExactly(@"c:\results1.xml", @"c:\results2.xml");
        }

        [Test]
        public void ThenCanParseMultipleResultsFilesWithWildCardAndSemicolon()
        {
            FileSystem.AddFile(@"c:\results_foo1.xml", "<xml />");
            FileSystem.AddFile(@"c:\results_foo2.xml", "<xml />");
            FileSystem.AddFile(@"c:\results_bar.xml", "<xml />");
            var args = new[] { @"-link-results-file=c:\results_foo*.xml;c:\results_bar.xml" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.HasTestResults).IsTrue();
            Check.That(configuration.TestResultsFiles
                .Select(trf => trf.FullName))
                .ContainsExactly(@"c:\results_foo1.xml", @"c:\results_foo2.xml", @"c:\results_bar.xml");
        }

        [Test]
        public void ThenCanParseMultipleResultsFilesWithWildCardsAndSemicolonWhenSomeHaveNoMatch()
        {
            FileSystem.AddFile(@"c:\results_foo1.xml", "<xml />");
            FileSystem.AddFile(@"c:\results_foo2.xml", "<xml />");
            var args = new[] { @"-link-results-file=c:\results_foo*.xml;c:\results_bar*.xml" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.HasTestResults).IsTrue();
            Check.That(configuration.TestResultsFiles
                .Select(trf => trf.FullName))
                .ContainsExactly(@"c:\results_foo1.xml", @"c:\results_foo2.xml");
        }

        [Test]
        public void ThenNoExceptionIsThrownWhenResultsFileIsDir()
        {
            FileSystem.AddFile(@"c:\temp\results_foo1.xml", "<xml />");
            FileSystem.AddFile(@"c:\temp\results_foo2.xml", "<xml />");
            var args = new[] { @"-link-results-file=c:\temp\" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.HasTestResults).IsFalse();
        }

        [Test]
        public void ThenCanParseResultsFilesWithMultipleMatchesResolvingInSingleMatch()
        {
            FileSystem.AddFile(@"c:\results_foo.xml", "<xml />");
            var args = new[] { @"-link-results-file=c:\results*.xml;c:\*foo.xml" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.HasTestResults).IsTrue();
            Check.That(configuration.TestResultsFiles
                .Select(trf => trf.FullName))
                .ContainsExactly(@"c:\results_foo.xml");
        }

        [Test]
        public void ThenCanParseResultsFileWithShortFormSuccessfully()
        {
            FileSystem.AddFile(@"c:\results.xml", "<xml />");
            var args = new[] { @"-lr=c:\results.xml" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.HasTestResults).IsTrue();
            Check.That(configuration.TestResultsFile.FullName).IsEqualTo(@"c:\results.xml");
        }

        [Test]
        public void ThenCanParseShortFormArgumentsSuccessfully()
        {
            var args = new[] { @"-f=c:\features", @"-o=c:\features-output" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.FeatureFolder.FullName).IsEqualTo(@"c:\features");
            Check.That(configuration.OutputFolder.FullName).IsEqualTo(@"c:\features-output");
        }

        [Test]
        public void ThenCanParseVersionRequestLongFormSuccessfully()
        {
            var args = new[] { @"--version" };

            var configuration = new Configuration();
            var writer = new StringWriter();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, writer);

            var actual = RetrieveString(writer);
            Check.That(actual).Contains(ExpectedVersionString.ComparisonNormalize());
            Check.That(shouldContinue).IsFalse();
        }

        [Test]
        public void ThenCanParseVersionRequestShortFormSuccessfully()
        {
            var args = new[] { @"-v" };

            var configuration = new Configuration();
            var writer = new StringWriter();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, writer);

            var actual = RetrieveString(writer);
            Check.That(actual).Contains(ExpectedVersionString.ComparisonNormalize());
            Check.That(shouldContinue).IsFalse();
        }

        [Test]
        public void ThenInputAndOutputDirectoryAreSetToTheCurrentDirectory()
        {
            var args = new[] { @"-v" };

            var configuration = new Configuration();
            var writer = new StringWriter();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            commandLineArgumentParser.Parse(args, configuration, writer);

            string currentDirectory = Assembly.GetExecutingAssembly().Location;

            Check.That(configuration.FeatureFolder.FullName).IsEqualTo(currentDirectory);
            Check.That(configuration.OutputFolder.FullName).IsEqualTo(currentDirectory);
        }

        [Test]
        public void ThenCanFilterOutNonExistingTestResultFiles()
        {
            FileSystem.AddDirectory(@"c:\");
            var args = new[] { @"-link-results-file=c:\DoesNotExist.xml;" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.HasTestResults).IsFalse();
            Check.That(configuration.TestResultsFiles).IsEmpty();
        }

        [Test]
        public void ThenSetsLanguageToEnglishByDefault()
        {
            var args = new[] { "" };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(configuration.Language).IsEqualTo("en");
        }

        [Test]
        public void ThenCanParseExcludeTagsSuccessfully()
        {
            var args = new[] { @"-excludeTags=exclude-tag" };

            IConfiguration configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.ExcludeTags).IsEqualTo("exclude-tag");
        }

        private static readonly object[] TestResultsFormatCases =
        {
            new object[] { @"mstest", TestResultsFormat.MsTest },
            new object[] { @"nunit", TestResultsFormat.NUnit },
            new object[] { @"nunit3", TestResultsFormat.NUnit3 },
            new object[] { @"xunit", TestResultsFormat.XUnit },
            new object[] { @"xunit1", TestResultsFormat.XUnit1 },
            new object[] { @"xunit2", TestResultsFormat.xUnit2 },
            new object[] { @"cucumberjson", TestResultsFormat.CucumberJson },
            new object[] { @"specrun", TestResultsFormat.SpecRun },
            new object[] { @"vstest", TestResultsFormat.VsTest },
        };

        [Test, TestCaseSource(nameof(TestResultsFormatCases))]
        public void ThenCanParseResultsFormatWithLongFormSuccessfully(string argument, TestResultsFormat expectedResultsFormat)
        {
            this.ThenCanParseResultsFormatSuccessfully(@"-test-results-format=", argument, expectedResultsFormat);
        }

        [Test, TestCaseSource(nameof(TestResultsFormatCases))]
        public void ThenCanParseResultsFormatWithShortFormSuccessfully(string argument, TestResultsFormat expectedResultsFormat)
        {
            this.ThenCanParseResultsFormatSuccessfully(@"-trfmt=", argument, expectedResultsFormat);
        }

        private void ThenCanParseResultsFormatSuccessfully(string argumentName, string argument, TestResultsFormat expectedResultsFormat)
        {
            var args = new[] { argumentName + argument };

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser(FileSystem);
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Check.That(shouldContinue).IsTrue();
            Check.That(configuration.TestResultsFormat).IsEqualTo(expectedResultsFormat);
        }
    }
}
