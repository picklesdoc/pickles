// #region License
// 
// 
// /*
//     Copyright [2011] [Jeffrey Cameron]
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// */
// #endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO.Abstractions;
using System.Linq;
using System.Windows.Input;
using Autofac;
using PicklesDoc.Pickles.Parser;
using PicklesDoc.Pickles.UserInterface.Mvvm;
using PicklesDoc.Pickles.UserInterface.Settings;

namespace PicklesDoc.Pickles.UserInterface
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        private readonly MultiSelectableCollection<DocumentationFormat> documentationFormats;

        private readonly SelectableCollection<TestResultsFormat> testResultsFormats;

        private readonly RelayCommand browseForFeatureFolderCommand;

        private readonly RelayCommand browseForOutputFolderCommand;

        private readonly RelayCommand browseForTestResultsFileCommand;

        private readonly RelayCommand generateCommand;

        private readonly RelayCommand openOutputDirectory;

        private readonly IMainModelSerializer mainModelSerializer;

        private readonly IFileSystem fileSystem;

        private string picklesVersion = typeof(Feature).Assembly.GetName().Version.ToString();

        private string featureFolder;

        private string outputFolder;

        private string projectName;

        private string projectVersion;

        private string testResultsFile;

        private CultureInfo selectedLanguage;

        private bool includeTests;

        private bool isRunning;

        private bool isFeatureDirectoryValid;

        private bool isOutputDirectoryValid;

        private bool isProjectNameValid;

        private bool isProjectVersionValid;

        private bool isTestResultsFileValid;

        private bool isTestResultsFormatValid;

        private bool isLanguageValid = true;

        private readonly CultureInfo[] neutralCultures;

        private bool isDocumentationFormatValid;

        public MainWindowViewModel()
            : this(new MainModelSerializer(DataDirectoryDeriver.DeriveDataDirectory(), new FileSystem()), new FileSystem())
        {
        }

        public MainWindowViewModel(IMainModelSerializer mainModelSerializer, IFileSystem fileSystem)
        {
            this.documentationFormats = new MultiSelectableCollection<DocumentationFormat>(Enum.GetValues(typeof(DocumentationFormat)).Cast<DocumentationFormat>());
            this.documentationFormats.First().IsSelected = true;
            this.documentationFormats.SelectionChanged += this.DocumentationFormatsOnCollectionChanged;

            this.testResultsFormats = new SelectableCollection<TestResultsFormat>(Enum.GetValues(typeof(TestResultsFormat)).Cast<TestResultsFormat>());
            this.testResultsFormats.First().IsSelected = true;
            this.testResultsFormats.SelectionChanged += this.TestResultsFormatsOnCollectionChanged;

            this.browseForFeatureFolderCommand = new RelayCommand(this.DoBrowseForFeature);
            this.browseForOutputFolderCommand = new RelayCommand(this.DoBrowseForOutputFolder);
            this.browseForTestResultsFileCommand = new RelayCommand(this.DoBrowseForTestResultsFile);
            this.generateCommand = new RelayCommand(this.DoGenerate, this.CanGenerate);
            this.openOutputDirectory = new RelayCommand(this.DoOpenOutputDirectory, this.CanOpenOutputDirectory);

            this.PropertyChanged += this.MainWindowViewModel_PropertyChanged;
            this.neutralCultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            this.selectedLanguage = CultureInfo.GetCultureInfo("en");

            this.mainModelSerializer = mainModelSerializer;
            this.fileSystem = fileSystem;
        }

        private void TestResultsFormatsOnCollectionChanged(object sender, EventArgs notifyCollectionChangedEventArgs)
        {
            this.IsTestResultsFormatValid = Enum.IsDefined(typeof(TestResultsFormat), this.testResultsFormats.Selected);
        }

        private void DocumentationFormatsOnCollectionChanged(object sender, EventArgs notifyCollectionChangedEventArgs)
        {
            this.IsDocumentationFormatValid = this.documentationFormats.Selected.Any();
        }

        public string PicklesVersion
        {
            get { return this.picklesVersion; }
            set
            {
                this.picklesVersion = value;
                RaisePropertyChanged(() => this.PicklesVersion);
            }
        }

        public string FeatureFolder
        {
            get { return this.featureFolder; }
            set
            {
                this.featureFolder = value;
                RaisePropertyChanged(() => this.FeatureFolder);
            }
        }

        public string OutputFolder
        {
            get { return this.outputFolder; }
            set
            {
                this.outputFolder = value;
                RaisePropertyChanged(() => this.OutputFolder);
            }
        }

        public MultiSelectableCollection<DocumentationFormat> DocumentationFormatValues
        {
            get { return this.documentationFormats; }
        }

        public string ProjectName
        {
            get { return this.projectName; }
            set
            {
                this.projectName = value;
                RaisePropertyChanged(() => this.ProjectName);
            }
        }

        public string ProjectVersion
        {
            get { return this.projectVersion; }
            set
            {
                this.projectVersion = value;
                RaisePropertyChanged(() => this.ProjectVersion);
            }
        }

        public string TestResultsFile
        {
            get { return this.testResultsFile; }
            set
            {
                this.testResultsFile = value;
                RaisePropertyChanged(() => this.TestResultsFile);
            }
        }

        public SelectableCollection<TestResultsFormat> TestResultsFormatValues
        {
            get { return this.testResultsFormats; }
        }

        public CultureInfo SelectedLanguage
        {
            get { return this.selectedLanguage; }
            set
            {
                this.selectedLanguage = value;
                RaisePropertyChanged(() => this.SelectedLanguage);
            }
        }

        public IEnumerable<CultureInfo> LanguageValues
        {
            get { return this.neutralCultures; }
        }

        public bool IncludeTests
        {
            get { return this.includeTests; }
            set
            {
                this.includeTests = value;
                RaisePropertyChanged(() => this.IncludeTests);
            }
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
            set
            {
                this.isRunning = value;
                this.RaisePropertyChanged(() => this.IsRunning);
            }
        }

        public bool IsFeatureDirectoryValid
        {
            get
            {
                return this.isFeatureDirectoryValid;
            }

            set
            {
                this.isFeatureDirectoryValid = value;
                this.RaisePropertyChanged(() => this.IsFeatureDirectoryValid);
            }
        }

        public bool IsDocumentationFormatValid
        {
            get
            {
                return this.isDocumentationFormatValid;
            }

            set
            {
                this.isDocumentationFormatValid = value;
                this.RaisePropertyChanged(() => this.IsDocumentationFormatValid);
            }
        }

        public bool IsOutputDirectoryValid
        {
            get { return this.isOutputDirectoryValid; }
            set
            {
                this.isOutputDirectoryValid = value;
                this.RaisePropertyChanged(() => this.IsOutputDirectoryValid);
            }
        }

        public bool IsProjectNameValid
        {
            get { return this.isProjectNameValid; }
            set
            {
                this.isProjectNameValid = value;
                this.RaisePropertyChanged(() => this.IsProjectNameValid);
            }
        }

        public bool IsProjectVersionValid
        {
            get { return this.isProjectVersionValid; }
            set
            {
                this.isProjectVersionValid = value;
                this.RaisePropertyChanged(() => this.IsProjectVersionValid);
            }
        }

        public bool IsTestResultsFileValid
        {
            get { return this.isTestResultsFileValid; }
            set
            {
                this.isTestResultsFileValid = value;
                this.RaisePropertyChanged(() => this.IsTestResultsFileValid);
            }
        }

        public bool IsTestResultsFormatValid
        {
            get { return this.isTestResultsFormatValid; }
            set
            {
                this.isTestResultsFormatValid = value;
                this.RaisePropertyChanged(() => this.IsTestResultsFormatValid);
            }
        }

        public bool IsLanguageValid
        {
            get { return this.isLanguageValid; }
            set
            {
                this.isLanguageValid = value;
                this.RaisePropertyChanged(() => this.IsLanguageValid);
            }
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
                                          TestResultsFormat = this.testResultsFormats.Selected,
                                          SelectedLanguageLcid = this.selectedLanguage.LCID,
                                          DocumentationFormats = this.documentationFormats.Where(item => item.IsSelected).Select(item => item.Item).ToArray()
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

            foreach (var item in this.TestResultsFormatValues)
            {
                if (item.Item == mainModel.TestResultsFormat)
                {
                    item.IsSelected = true;
                }
                else
                {
                    item.IsSelected = false;
                }
            }

            this.SelectedLanguage = this.neutralCultures.Where(lv => lv.LCID == mainModel.SelectedLanguageLcid).FirstOrDefault();

            foreach (var item in this.documentationFormats)
            {
                item.IsSelected = mainModel.DocumentationFormats.Contains(item.Item);
            }
        }

        private void MainWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
                        if (this.fileSystem.File.Exists(this.testResultsFile))
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

            //this.SaveToSettings();
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
                builder.Register<FileSystem>(_ => new FileSystem()).As<IFileSystem>().SingleInstance();
                builder.RegisterModule<PicklesModule>();
                var container = builder.Build();

                var configuration = container.Resolve<Configuration>();
                var fileSystem = container.Resolve<IFileSystem>();

                configuration.FeatureFolder = fileSystem.DirectoryInfo.FromDirectoryName(this.featureFolder);
                configuration.OutputFolder = fileSystem.DirectoryInfo.FromDirectoryName(this.outputFolder);
                configuration.SystemUnderTestName = this.projectName;
                configuration.SystemUnderTestVersion = this.projectVersion;
                configuration.TestResultsFile = this.IncludeTests ? fileSystem.FileInfo.FromFileName(this.testResultsFile) : null;
                configuration.TestResultsFormat = this.testResultsFormats.Selected;
                configuration.Language = this.selectedLanguage != null ? this.selectedLanguage.TwoLetterISOLanguageName : CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

                configuration.DocumentationFormat = documentationFormat;
                var runner = container.Resolve<Runner>();
                runner.Run(container);
            }
        }

        private void DoBrowseForTestResultsFile()
        {
            var dlg = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            var result = dlg.ShowDialog();
            if (result == true) this.TestResultsFile = dlg.FileName;
        }

        private void DoBrowseForFeature()
        {
            var dlg = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            var result = dlg.ShowDialog();
            if (result == true) this.FeatureFolder = dlg.SelectedPath;
        }

        private void DoBrowseForOutputFolder()
        {
            var dlg = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            var result = dlg.ShowDialog();
            if (result == true) this.OutputFolder = dlg.SelectedPath;
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
