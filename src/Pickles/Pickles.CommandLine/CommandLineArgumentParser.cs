//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CommandLineArgumentParser.cs" company="PicklesDoc">
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
using System.IO.Abstractions;
using System.Reflection;
using NDesk.Options;
using PicklesDoc.Pickles.Extensions;

using TextWriter = System.IO.TextWriter;

namespace PicklesDoc.Pickles.CommandLine
{
    public class CommandLineArgumentParser
    {
        private readonly IFileSystem fileSystem;
        private readonly OptionSet options;
        private string documentationFormat;
        private string featureDirectory;
        private bool helpRequested;
        private string language;
        private string outputDirectory;
        private string systemUnderTestName;
        private string systemUnderTestVersion;
        private string testResultsFile;
        private string testResultsFormat;
        private bool versionRequested;
        private bool includeExperimentalFeatures;
        private string enableCommentsValue;
        private string excludeTags;
        private string hideTags;

        public CommandLineArgumentParser(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            this.options = new OptionSet
            {
                { "f|feature-directory=", CommandLineArgumentHelpTexts.HelpFeatureDir, v => this.featureDirectory = v },
                { "o|output-directory=", CommandLineArgumentHelpTexts.HelpOutputDir, v => this.outputDirectory = v },
                { "trfmt|test-results-format=", CommandLineArgumentHelpTexts.HelpTestResultsFormat, v => this.testResultsFormat = v },
                { "lr|link-results-file=", CommandLineArgumentHelpTexts.HelpTestResultsFile, v => this.testResultsFile = v },
                { "sn|system-under-test-name=", CommandLineArgumentHelpTexts.HelpSutName, v => this.systemUnderTestName = v },
                { "sv|system-under-test-version=", CommandLineArgumentHelpTexts.HelpSutVersion, v => this.systemUnderTestVersion = v },
                { "l|language=", CommandLineArgumentHelpTexts.HelpLanguageFeatureFiles, v => this.language = v },
                { "df|documentation-format=", CommandLineArgumentHelpTexts.HelpDocumentationFormat, v => this.documentationFormat = v },
                { "v|version", v => this.versionRequested = v != null },
                { "h|?|help", v => this.helpRequested = v != null },
                { "exp|include-experimental-features", CommandLineArgumentHelpTexts.HelpIncludeExperimentalFeatures, v => this.includeExperimentalFeatures = v != null },
                { "cmt|enableComments=", CommandLineArgumentHelpTexts.HelpEnableComments, v => this.enableCommentsValue = v },
                { "et|excludeTags=", CommandLineArgumentHelpTexts.HelpExcludeTags, v => this.excludeTags = v },
                { "ht|hideTags=", CommandLineArgumentHelpTexts.HelpHideTags, v => this.hideTags = v }
            };
        }

        public bool Parse(string[] args, IConfiguration configuration, TextWriter stdout)
        {
            var currentDirectory =
                this.fileSystem.DirectoryInfo.FromDirectoryName(this.fileSystem.Directory.GetCurrentDirectory());
            configuration.FeatureFolder = currentDirectory;
            configuration.OutputFolder = currentDirectory;

            this.options.Parse(args);

            if (this.versionRequested)
            {
                this.DisplayVersion(stdout);
                return false;
            }
            else if (this.helpRequested)
            {
                this.DisplayHelp(stdout);
                return false;
            }

            if (!string.IsNullOrEmpty(this.featureDirectory))
            {
                configuration.FeatureFolder = this.fileSystem.DirectoryInfo.FromDirectoryName(this.featureDirectory);
            }

            if (!string.IsNullOrEmpty(this.outputDirectory))
            {
                configuration.OutputFolder = this.fileSystem.DirectoryInfo.FromDirectoryName(this.outputDirectory);
            }

            if (!string.IsNullOrEmpty(this.testResultsFormat))
            {
                configuration.TestResultsFormat =
                    (TestResultsFormat)Enum.Parse(typeof(TestResultsFormat), this.testResultsFormat, true);
            }

            if (!string.IsNullOrEmpty(this.testResultsFile))
            {
                configuration.AddTestResultFiles(
                    PathExtensions.GetAllFilesFromPathAndFileNameWithOptionalSemicolonsAndWildCards(this.testResultsFile, this.fileSystem));
            }

            if (!string.IsNullOrEmpty(this.systemUnderTestName))
            {
                configuration.SystemUnderTestName = this.systemUnderTestName;
            }

            if (!string.IsNullOrEmpty(this.systemUnderTestVersion))
            {
                configuration.SystemUnderTestVersion = this.systemUnderTestVersion;
            }

            if (!string.IsNullOrEmpty(this.language))
            {
                configuration.Language = this.language;
            }

            if (!string.IsNullOrEmpty(this.documentationFormat))
            {
                configuration.DocumentationFormat =
                    (DocumentationFormat)Enum.Parse(typeof(DocumentationFormat), this.documentationFormat, true);
            }

            if (this.includeExperimentalFeatures)
            {
                configuration.EnableExperimentalFeatures();
            }

            if (!string.IsNullOrEmpty(this.excludeTags))
            {
                configuration.ExcludeTags = this.excludeTags;
            }

            if (!string.IsNullOrEmpty(this.hideTags))
            {
                configuration.HideTags = this.hideTags;
            }

            bool enableComments;

            if (bool.TryParse(this.enableCommentsValue, out enableComments) && enableComments == false)
            {
                configuration.DisableComments();
            }

            return true;
        }

        private void DisplayVersion(TextWriter stdout)
        {
            stdout.WriteLine("Pickles version {0}", Assembly.GetExecutingAssembly().GetName().Version);
        }

        private void DisplayHelp(TextWriter stdout)
        {
            this.DisplayVersion(stdout);
            this.options.WriteOptionDescriptions(stdout);
        }
    }
}
