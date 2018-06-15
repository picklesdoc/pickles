//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="BaseFixture.cs" company="PicklesDoc">
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
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Reflection;

using Autofac;

using NUnit.Framework;

namespace PicklesDoc.Pickles.Test
{
    public class BaseFixture
    {
        protected const string FileSystemPrefix = @"c:\temp\FakeFolderStructures\";
        protected const string ResourcePrefix = "PicklesDoc.Pickles.Test.FakeFolderStructures.";
        private IContainer container;
        private readonly string currentDirectory;

        public BaseFixture(string currentDirectory)
        {
            this.currentDirectory = currentDirectory;
        }

        public BaseFixture()
            : this(Assembly.GetExecutingAssembly().Location)
        {
        }

        protected IContainer Container
        {
            get
            {
                if (this.container == null)
                {
                    var builder = new ContainerBuilder();

                    var configuration = new Configuration() { ExcludeTags = "exclude-tag", HideTags = "TagsToHideFeature;TagsToHideScenario" };
                    builder.RegisterAssemblyTypes(typeof(Runner).Assembly);
                    builder.Register<MockFileSystem>(_ => CreateMockFileSystem()).As<IFileSystem>().SingleInstance();
                    builder.RegisterModule<PicklesModule>();
                    builder.RegisterInstance(configuration).As<IConfiguration>().SingleInstance();
                    this.container = builder.Build();
                }

                return this.container;
            }
        }

        private MockFileSystem CreateMockFileSystem()
        {
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.Directory.SetCurrentDirectory(this.currentDirectory);
            return mockFileSystem;
        }

        protected MockFileSystem FileSystem
        {
            get { return (MockFileSystem)this.Container.Resolve<IFileSystem>(); }
        }

        protected IConfiguration Configuration
        {
            get
            {
                return this.Container.Resolve<IConfiguration>();
            }
        }

        [TearDown]
        public void TearDown()
        {
            this.Configuration.DisableExperimentalFeatures();

            this.container?.Dispose();

            this.container = null;
        }

        protected void AddFakeFolderStructures()
        {
            this.AddFakeFolderAndFiles("AcceptanceTest", new[] { "AdvancedFeature.feature", "LevelOne.feature" });
            this.AddFakeFolderAndFiles("EmptyFolderTests", new string[0]);

            this.AddFakeFolderAndFiles("FeatureCrawlerTests", new[] { "index.md", "LevelOne.feature", "image.png", "LevelOneIgnoredFeature.feature", "LevelOneRemoveTagsToHide.feature" });
            this.AddFakeFolderAndFiles(@"FeatureCrawlerTests\SubLevelOne", new[] { "ignorethisfile.ignore", "LevelOneSublevelOne.feature", "LevelOneSublevelTwo.feature" });
            this.AddFakeFolderAndFiles(@"FeatureCrawlerTests\SubLevelOne\SubLevelTwo", new[] { "LevelOneSublevelOneSubLevelTwo.feature" });
            this.AddFakeFolderAndFiles(@"FeatureCrawlerTests\SubLevelOne\SubLevelTwo\IgnoreThisDirectory", new[] { "IgnoreThisFile.ignore" });

            this.AddFakeFolderAndFiles(@"OrderingTests", new string[0]);
            this.AddFakeFolderAndFiles(@"OrderingTests\A", new [] {"a-a.feature", "a-b.feature"});
            this.AddFakeFolderAndFiles(@"OrderingTests\B", new [] {"b-a.feature", "b-b.feature"});
        }

        protected void AddFakeFolderAndFiles(string directoryName, IEnumerable<string> fileNames)
        {
            string directoryPath = FileSystemPrefix + directoryName + @"\";
            string resourceIdentifier = ResourcePrefix + directoryName.Replace(@"\", ".") + ".";

            this.FileSystem.AddDirectory(directoryPath);

            foreach (var fileName in fileNames)
            {
                this.FileSystem.AddFile(
                    directoryPath + fileName,
                    RetrieveContentOfFileFromResources(resourceIdentifier + fileName));
            }
        }

        protected static string RetrieveContentOfFileFromResources(string resourceName)
        {
            string resultFile;

            System.IO.Stream manifestResourceStream =
                Assembly.GetCallingAssembly().GetManifestResourceStream(resourceName);

            using (var reader = new System.IO.StreamReader(manifestResourceStream))
            {
                resultFile = reader.ReadToEnd();
            }

            return resultFile;
        }
    }
}
