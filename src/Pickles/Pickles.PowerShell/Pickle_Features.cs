//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Pickle_Features.cs" company="PicklesDoc">
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

#if __MonoCS__
#else
using System;
using System.IO.Abstractions;
using System.Management.Automation;
using System.Reflection;
using Autofac;
using PicklesDoc.Pickles.Extensions;
using System.Linq;

namespace PicklesDoc.Pickles.PowerShell
{
    [Cmdlet("Pickle", "Features")]
    public class Pickle_Features : PSCmdlet
    {
        [Parameter(HelpMessage = CommandLinArgumentHelpTexts.HelpFeatureDir, Mandatory = true)]
        public string FeatureDirectory { get; set; }

        [Parameter(HelpMessage = CommandLinArgumentHelpTexts.HelpOutputDir, Mandatory = true)]
        public string OutputDirectory { get; set; }

        [Parameter(HelpMessage = CommandLinArgumentHelpTexts.HelpLanguageFeatureFiles, Mandatory = false)]
        public string Language { get; set; }

        [Parameter(HelpMessage = CommandLinArgumentHelpTexts.HelpTestResultsFormat, Mandatory = false)]
        public string TestResultsFormat { get; set; }

        [Parameter(HelpMessage = CommandLinArgumentHelpTexts.HelpTestResultsFile, Mandatory = false)]
        public string TestResultsFile { get; set; }

        [Parameter(HelpMessage = CommandLinArgumentHelpTexts.HelpSutName, Mandatory = false)]
        public string SystemUnderTestName { get; set; }

        [Parameter(HelpMessage = CommandLinArgumentHelpTexts.HelpSutVersion, Mandatory = false)]
        public string SystemUnderTestVersion { get; set; }

        [Parameter(HelpMessage = CommandLinArgumentHelpTexts.HelpDocumentationFormat, Mandatory = false)]
        public string DocumentationFormat { get; set; }

        [Parameter(HelpMessage = CommandLinArgumentHelpTexts.HelpIncludeExperimentalFeatures, Mandatory = false)]
        public SwitchParameter IncludeExperimentalFeatures { get; set; }

        [Parameter(HelpMessage = CommandLinArgumentHelpTexts.HelpEnableComments, Mandatory = false)]
        public string EnableComments { get; set; }

        [Parameter(HelpMessage = CommandLinArgumentHelpTexts.HelpExcludeTags, Mandatory = false)]
        public string ExcludeTags { get; set; }

        [Parameter(HelpMessage = CommandLinArgumentHelpTexts.HelpHideTags, Mandatory = false)]
        public string HideTags { get; set; }
        

        protected override void ProcessRecord()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(Runner).Assembly);
            builder.Register<FileSystem>(_ => new FileSystem()).As<IFileSystem>().SingleInstance();
            builder.RegisterModule<PicklesModule>();
            var container = builder.Build();

            var configuration = container.Resolve<IConfiguration>();

            this.ParseParameters(configuration, container.Resolve<IFileSystem>(), this.SessionState.Path.CurrentFileSystemLocation);

            this.WriteObject($"Pickles v.{Assembly.GetExecutingAssembly().GetName().Version}{Environment.NewLine}");
            new ConfigurationReporter().ReportOn(configuration, this.WriteObject);

            var runner = container.Resolve<Runner>();
            runner.Run(container);

            this.WriteObject("Pickles completed successfully");
        }

        private void ParseParameters(IConfiguration configuration, IFileSystem fileSystem, PathInfo currentFileSystemLocation)
        {
            configuration.FeatureFolder = this.DetermineFeatureFolder(fileSystem, currentFileSystemLocation, this.FeatureDirectory);
            configuration.OutputFolder = this.DetermineFeatureFolder(fileSystem, currentFileSystemLocation, this.OutputDirectory);

            if (!string.IsNullOrEmpty(this.TestResultsFormat))
            {
                configuration.TestResultsFormat =
                    (TestResultsFormat)Enum.Parse(typeof(TestResultsFormat), this.TestResultsFormat, true);
            }

            if (!string.IsNullOrEmpty(this.TestResultsFile))
            {
                configuration.AddTestResultFiles(
                    PathExtensions.GetAllFilesFromPathAndFileNameWithOptionalSemicolonsAndWildCards(this.TestResultsFile, fileSystem));
            }

            configuration.SystemUnderTestName = this.SystemUnderTestName;
            configuration.SystemUnderTestVersion = this.SystemUnderTestVersion;
            if (!string.IsNullOrEmpty(this.DocumentationFormat))
            {
                configuration.DocumentationFormat = (DocumentationFormat)Enum.Parse(typeof(DocumentationFormat), this.DocumentationFormat, true);
            }

            if (!string.IsNullOrEmpty(this.Language))
            {
                configuration.Language = this.Language;
            }

            if (this.IncludeExperimentalFeatures.IsPresent)
            {
                configuration.EnableExperimentalFeatures();
            }

            if (this.IncludeExperimentalFeatures.IsPresent)
            {
                configuration.EnableExperimentalFeatures();
            }

            if (!string.IsNullOrEmpty(this.ExcludeTags))
            {
                configuration.ExcludeTags = this.ExcludeTags;
            }

            if (!string.IsNullOrEmpty(this.HideTags))
            {
                configuration.HideTags = this.HideTags;
            }

            bool shouldEnableComments;

            if (bool.TryParse(this.EnableComments, out shouldEnableComments))
            {
                if (!shouldEnableComments)
                {
                    configuration.DisableComments();
                }
            }
        }

        private DirectoryInfoBase DetermineFeatureFolder(IFileSystem fileSystem, PathInfo currentFileSystemLocation, string directory)
        {
            DirectoryInfoBase result;

            if (fileSystem.Path.IsPathRooted(directory))
            {
                result = fileSystem.DirectoryInfo.FromDirectoryName(directory);
            }
            else
            {
                result = fileSystem.DirectoryInfo.FromDirectoryName(
                    fileSystem.Path.Combine(currentFileSystemLocation.Path, directory));
            }

            return result;
        }
    }
}
#endif
