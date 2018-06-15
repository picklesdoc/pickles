//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MainViewModel.cs" company="PicklesDoc">
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO.Abstractions;
using System.Linq;
using System.Windows.Input;

using Autofac;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.UserInterface.Mvvm;
using PicklesDoc.Pickles.UserInterface.Settings;

namespace PicklesDoc.Pickles.UserInterface.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly MultiSelectableCollection<DocumentationFormat> documentationFormats;

        private readonly TestResultsFormat[] testResultsFormats;

        private readonly RelayCommand browseForFeatureFolderCommand;

        private readonly RelayCommand browseForOutputFolderCommand;

        private readonly RelayCommand browseForTestResultsFileCommand;

        private readonly RelayCommand generateCommand;

        private readonly RelayCommand openOutputDirectory;

        private readonly IMainModelSerializer mainModelSerializer;

        private readonly IFileSystem fileSystem;

        private readonly CultureInfo[] neutralCultures;

        private string picklesVersion = typeof(Feature).Assembly.GetName().Version.ToString();

        private string featureFolder;

        private string outputFolder;

        private string projectName;

        private string projectVersion;

        private string testResultsFile;

        private CultureInfo selectedLanguage;

        private bool includeTests;

        private string excludeTags;

        private string hideTags;

        private bool isRunning;

        private bool isFeatureDirectoryValid;

        private bool isOutputDirectoryValid;

        private bool isProjectNameValid;

        private bool isProjectVersionValid;

        private bool isTestResultsFileValid;

        private bool isTestResultsFormatValid;

        private bool isLanguageValid = true;

        private bool createDirectoryForEachOutputFormat;

        private bool isDocumentationFormatValid;

        private TestResultsFormat selectedTestResultsFormat;

        private bool includeExperimentalFeatures;

        private bool enableComments = true;

        public MainViewModel(IMainModelSerializer mainModelSerializer, IFileSystem fileSystem)
        {
            this.documentationFormats =
                new MultiSelectableCollection<DocumentationFormat>(
                    Enum.GetValues(typeof(DocumentationFormat)).Cast<DocumentationFormat>());
            this.documentationFormats.First().IsSelected = true;
            this.documentationFormats.SelectionChanged += this.DocumentationFormatsOnCollectionChanged;

            this.testResultsFormats = Enum.GetValues(typeof(TestResultsFormat)).Cast<TestResultsFormat>().ToArray();
            this.selectedTestResultsFormat = TestResultsFormat.NUnit;

            this.browseForFeatureFolderCommand = new RelayCommand(this.DoBrowseForFeature);
            this.browseForOutputFolderCommand = new RelayCommand(this.DoBrowseForOutputFolder);
            this.browseForTestResultsFileCommand = new RelayCommand(this.DoBrowseForTestResultsFile);
            this.generateCommand = new RelayCommand(this.DoGenerate, this.CanGenerate);
            this.openOutputDirectory = new RelayCommand(this.DoOpenOutputDirectory, this.CanOpenOutputDirectory);

            this.PropertyChanged += this.MainWindowViewModelPropertyChanged;
            this.neutralCultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            this.selectedLanguage = CultureInfo.GetCultureInfo("en");

            this.mainModelSerializer = mainModelSerializer;
            this.fileSystem = fileSystem;
        }

        public string PicklesVersion
        {
            get { return this.picklesVersion; }
            set { this.Set(() => this.PicklesVersion, ref this.picklesVersion, value); }
        }

        public string FeatureFolder
        {
            get { return this.featureFolder; }
            set { this.Set(() => this.FeatureFolder, ref this.featureFolder, value); }
        }

        public string OutputFolder
        {
            get { return this.outputFolder; }
            set { this.Set(() => this.OutputFolder, ref this.outputFolder, value); }
        }

        public MultiSelectableCollection<DocumentationFormat> DocumentationFormatValues
        {
            get { return this.documentationFormats; }
        }

        public string ProjectName
        {
            get { return this.projectName; }
            set { this.Set(() => this.ProjectName, ref this.projectName, value); }
        }

        public string ProjectVersion
        {
            get { return this.projectVersion; }
            set { this.Set(() => this.ProjectVersion, ref this.projectVersion, value); }
        }

        public string TestResultsFile
        {
            get { return this.testResultsFile; }

            set { this.Set(() => this.TestResultsFile, ref this.testResultsFile, value); }
        }

        public IEnumerable<TestResultsFormat> TestResultsFormatValues
        {
            get { return this.testResultsFormats; }
        }

        public TestResultsFormat SelectedTestResultsFormat
        {
            get { return this.selectedTestResultsFormat; }
            set { this.Set(() => this.SelectedTestResultsFormat, ref this.selectedTestResultsFormat, value); }
        }

        public CultureInfo SelectedLanguage
        {
            get { return this.selectedLanguage; }
            set { this.Set(() => this.SelectedLanguage, ref this.selectedLanguage, value); }
        }

        public IEnumerable<CultureInfo> LanguageValues
        {
            get { return this.neutralCultures; }
        }

        public bool IncludeTests
        {
            get { return this.includeTests; }
            set { this.Set(() => this.IncludeTests, ref this.includeTests, value); }
        }

        public string ExcludeTags
        {
            get { return this.excludeTags; }

            set { this.Set(nameof(this.ExcludeTags), ref this.excludeTags, value); }
        }

        public string HideTags
        {
            get { return this.hideTags; }

            set { this.Set(nameof(this.HideTags), ref this.hideTags, value); }
        }

        public ICommand GeneratePickles
        {
            get { return this.generateCommand; }
        }

        public ICommand BrowseForFeatureFolder
        {
            get { return this.browseForFeatureFolderCommand; }
        }

        public ICommand BrowseForOutputFolder
        {
            get { return this.browseForOutputFolderCommand; }
        }

        public ICommand BrowseForTestResultsFile
        {
            get { return this.browseForTestResultsFileCommand; }
        }

        public RelayCommand OpenOutputDirectory
        {
            get { return this.openOutputDirectory; }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.Set(() => this.IsRunning, ref this.isRunning, value); }
        }

        public bool IsFeatureDirectoryValid
        {
            get { return this.isFeatureDirectoryValid; }

            set { this.Set(() => this.IsFeatureDirectoryValid, ref this.isFeatureDirectoryValid, value); }
        }

        public bool IsDocumentationFormatValid
        {
            get { return this.isDocumentationFormatValid; }

            set { this.Set(() => this.IsDocumentationFormatValid, ref this.isDocumentationFormatValid, value); }
        }

        public bool IsOutputDirectoryValid
        {
            get { return this.isOutputDirectoryValid; }
            set { this.Set(() => this.IsOutputDirectoryValid, ref this.isOutputDirectoryValid, value); }
        }

        public bool IsProjectNameValid
        {
            get { return this.isProjectNameValid; }
            set { this.Set(() => this.IsProjectNameValid, ref this.isProjectNameValid, value); }
        }

        public bool IsProjectVersionValid
        {
            get { return this.isProjectVersionValid; }
            set { this.Set(() => this.IsProjectVersionValid, ref this.isProjectVersionValid, value); }
        }

        public bool IsTestResultsFileValid
        {
            get { return this.isTestResultsFileValid; }
            set { this.Set(() => this.IsTestResultsFileValid, ref this.isTestResultsFileValid, value); }
        }

        public bool IsTestResultsFormatValid
        {
            get { return this.isTestResultsFormatValid; }
            set { this.Set(() => this.IsTestResultsFormatValid, ref this.isTestResultsFormatValid, value); }
        }

        public bool IsLanguageValid
        {
            get { return this.isLanguageValid; }
            set { this.Set(() => this.IsLanguageValid, ref this.isLanguageValid, value); }
        }

        public bool CreateDirectoryForEachOutputFormat
        {
            get { return this.createDirectoryForEachOutputFormat; }
            set { this.Set(() => this.CreateDirectoryForEachOutputFormat, ref this.createDirectoryForEachOutputFormat, value); }
        }

        public bool IncludeExperimentalFeatures
        {
            get { return this.includeExperimentalFeatures; }
            set { this.Set(nameof(this.IncludeExperimentalFeatures), ref this.includeExperimentalFeatures, value); }
        }

        public bool EnableComments
        {
            get { return this.enableComments; }
            set { this.Set(nameof(this.enableComments), ref this.enableComments, value); }
        }

        public void SaveToSettings()
        {
            MainModel mainModel = new MainModel
            {
                FeatureDirectory = this.featureFolder,
                OutputDirectory = this.outputFolder,
                ProjectName = this.projectName,
                ProjectVersion = this.projectVersion,
                IncludeTestResults = this.includeTests,
                TestResultsFile = this.testResultsFile,
                TestResultsFormat = this.SelectedTestResultsFormat,
                SelectedLanguageLcid = this.selectedLanguage.LCID,
                DocumentationFormats =
                    this.documentationFormats.Where(item => item.IsSelected).Select(item => item.Item).ToArray(),
                CreateDirectoryForEachOutputFormat = this.createDirectoryForEachOutputFormat,
                IncludeExperimentalFeatures = this.includeExperimentalFeatures,
                EnableComments = this.enableComments,
                ExcludeTags = this.excludeTags,
                HideTags = this.HideTags,
            };

            this.mainModelSerializer.Write(mainModel);
        }

        public void LoadFromSettings()
        {
            MainModel mainModel = this.mainModelSerializer.Read();

            if (mainModel == null)
            {
                return;
            }

            this.FeatureFolder = mainModel.FeatureDirectory;
            this.OutputFolder = mainModel.OutputDirectory;
            this.ProjectName = mainModel.ProjectName;
            this.ProjectVersion = mainModel.ProjectVersion;
            this.IncludeTests = mainModel.IncludeTestResults;
            this.TestResultsFile = mainModel.TestResultsFile;

            this.SelectedTestResultsFormat =
                this.testResultsFormats.Where(tf => tf == mainModel.TestResultsFormat).FirstOrDefault();

            this.SelectedLanguage =
                this.neutralCultures.Where(lv => lv.LCID == mainModel.SelectedLanguageLcid).FirstOrDefault();

            foreach (var item in this.documentationFormats)
            {
                item.IsSelected = mainModel.DocumentationFormats.Contains(item.Item);
            }

            this.CreateDirectoryForEachOutputFormat = mainModel.CreateDirectoryForEachOutputFormat;
            this.IncludeExperimentalFeatures = mainModel.IncludeExperimentalFeatures;
            this.EnableComments = mainModel.EnableComments;
            this.ExcludeTags = mainModel.ExcludeTags;
            this.HideTags = mainModel.HideTags;
        }

        private void DocumentationFormatsOnCollectionChanged(object sender, EventArgs notifyCollectionChangedEventArgs)
        {
            this.IsDocumentationFormatValid = this.documentationFormats.Selected.Any();
        }

        private void MainWindowViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "FeatureFolder":
                {
                    if (this.fileSystem.Directory.Exists(this.featureFolder))
                    {
                        this.IsFeatureDirectoryValid = true;
                    }
                    else
                    {
                        this.IsFeatureDirectoryValid = false;
                    }

                    break;
                }

                case "OutputFolder":
                {
                    if (this.fileSystem.Directory.Exists(this.outputFolder))
                    {
                        this.IsOutputDirectoryValid = true;
                    }
                    else
                    {
                        this.IsOutputDirectoryValid = false;
                    }

                    this.openOutputDirectory.RaiseCanExecuteChanged();

                    break;
                }

                case "TestResultsFile":
                {
                    if (this.testResultsFile == null ||
                        this.testResultsFile.Split(';').All(trf => this.fileSystem.File.Exists(trf)))
                    {
                        this.IsTestResultsFileValid = true;
                    }
                    else
                    {
                        this.IsTestResultsFileValid = false;
                    }

                    break;
                }

                case "ProjectName":
                {
                    this.IsProjectNameValid = !string.IsNullOrWhiteSpace(this.projectName);
                    break;
                }

                case "ProjectVersion":
                {
                    this.IsProjectVersionValid = !string.IsNullOrWhiteSpace(this.projectVersion);
                    break;
                }

                case "SelectedTestResultsFormat":
                {
                    this.IsTestResultsFormatValid = Enum.IsDefined(typeof(TestResultsFormat), this.selectedTestResultsFormat);
                    break;
                }

                case "IsRunning":
                case "IsFeatureDirectoryValid":
                case "IsOutputDirectoryValid":
                case "IsProjectNameValid":
                case "IsProjectVersionValid":
                case "IsTestResultsFileValid":
                case "IsTestResultsFormatValid":
                case "IsLanguageValid":
                case "IncludeTests":
                case "IsDocumentationFormatValid":
                {
                    this.generateCommand.RaiseCanExecuteChanged();
                    break;
                }
            }
        }

        private bool CanGenerate()
        {
            return !this.isRunning
                   && this.isFeatureDirectoryValid
                   && this.isOutputDirectoryValid
                   && this.isProjectNameValid
                   && this.isProjectVersionValid
                   && (this.isTestResultsFileValid || !this.includeTests)
                   && (this.isTestResultsFormatValid || !this.includeTests)
                   && this.isDocumentationFormatValid
                   && this.isLanguageValid;
        }

        private void DoGenerate()
        {
            this.IsRunning = true;

            var backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += (sender, args) => this.DoWork();
            backgroundWorker.RunWorkerCompleted += (sender, args) =>
            {
                this.IsRunning = false;
            };
            backgroundWorker.RunWorkerAsync();
        }

        private void DoWork()
        {
            foreach (DocumentationFormat documentationFormat in this.documentationFormats.Selected)
            {
                var builder = new ContainerBuilder();
                builder.RegisterAssemblyTypes(typeof(Runner).Assembly);
                builder.Register<IFileSystem>(_ => this.fileSystem).SingleInstance();
                builder.RegisterModule<PicklesModule>();
                var container = builder.Build();

                var configuration = container.Resolve<IConfiguration>();

                configuration.FeatureFolder = this.fileSystem.DirectoryInfo.FromDirectoryName(this.featureFolder);

                if (this.createDirectoryForEachOutputFormat)
                {
                    configuration.OutputFolder =
                        this.fileSystem.DirectoryInfo.FromDirectoryName(
                            this.fileSystem.Path.Combine(
                                this.outputFolder,
                                documentationFormat.ToString("G")));
                }
                else
                {
                    configuration.OutputFolder = this.fileSystem.DirectoryInfo.FromDirectoryName(this.outputFolder);
                }

                configuration.SystemUnderTestName = this.projectName;
                configuration.SystemUnderTestVersion = this.projectVersion;
                configuration.AddTestResultFiles(this.IncludeTests
                    ? this.testResultsFile.Split(';').Select(trf => this.fileSystem.FileInfo.FromFileName(trf)).ToArray()
                    : null);
                configuration.TestResultsFormat = this.selectedTestResultsFormat;
                configuration.Language = this.selectedLanguage != null
                    ? this.selectedLanguage.TwoLetterISOLanguageName
                    : CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

                configuration.DocumentationFormat = documentationFormat;

                configuration.ExcludeTags = this.ExcludeTags;
                configuration.HideTags = this.HideTags;

                if (this.includeExperimentalFeatures)
                {
                    configuration.EnableExperimentalFeatures();
                }
                else
                {
                    configuration.DisableExperimentalFeatures();
                }

                if (this.enableComments)
                {
                    configuration.EnableComments();
                }
                else
                {
                    configuration.DisableComments();
                }

                var runner = container.Resolve<Runner>();
                runner.Run(container);
            }
        }

        private void DoBrowseForTestResultsFile()
        {
            var dlg = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            dlg.Multiselect = true;
            var result = dlg.ShowDialog();
            if (result == true)
            {
                this.TestResultsFile = string.Join(";", dlg.FileNames);
            }
        }

        private void DoBrowseForFeature()
        {
            var dlg = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            var result = dlg.ShowDialog();
            if (result == true)
            {
                this.FeatureFolder = dlg.SelectedPath;
            }
        }

        private void DoBrowseForOutputFolder()
        {
            var dlg = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            var result = dlg.ShowDialog();
            if (result == true)
            {
                this.OutputFolder = dlg.SelectedPath;
            }
        }

        private void DoOpenOutputDirectory()
        {
            Process.Start(this.outputFolder);
        }

        private bool CanOpenOutputDirectory()
        {
            return this.isOutputDirectoryValid;
        }
    }
}
