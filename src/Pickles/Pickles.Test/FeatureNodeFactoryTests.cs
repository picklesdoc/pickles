//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FeatureNodeFactoryTests.cs" company="PicklesDoc">
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
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders.Html;

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    public class FeatureNodeFactoryTests : BaseFixture
    {
        [Test]
        public void Create_InvalidFeatureFile_AddsEntryToReport()
        {
            FileSystem.AddFile(@"test.feature", new MockFileData("Invalid feature file"));

            var featureNodeFactory = this.CreateFeatureNodeFactory();

            var report = new ParsingReport();

            featureNodeFactory.Create(null, FileSystem.FileInfo.FromFileName(@"test.feature"), report);

            Check.That(report.First()).Contains(@"test.feature");
        }

        [Test]
        public void Create_MarkdownParsingError_AddsEntryToReport()
        {
            FileSystem.AddFile(@"test.md", new MockFileData("* Some Markdown text"));

            var featureNodeFactory = this.CreateFeatureNodeFactory(new MockMarkdownProvider());

            var report = new ParsingReport();

            var fileInfo = FileSystem.FileInfo.FromFileName(@"test.md");
            featureNodeFactory.Create(null, fileInfo, report);

            Check.That(report.Count).Equals(1);
            Check.That(report.First()).Equals($"Error parsing the Markdown file located at {fileInfo.FullName}. Error: Error parsing text.");
        }

        private FeatureNodeFactory CreateFeatureNodeFactory()
        {
            return this.CreateFeatureNodeFactory(new MarkdownProvider());
        }

        private FeatureNodeFactory CreateFeatureNodeFactory(IMarkdownProvider markdownProvider)
        {
            return new FeatureNodeFactory(
                new RelevantFileDetector(),
                new FileSystemBasedFeatureParser(
                    new FeatureParser(Configuration),
                    FileSystem),
                new HtmlMarkdownFormatter(
                    markdownProvider),
                FileSystem);
        }

        [Test]
        public void Create_InvalidFileType_AddsEntryToReport()
        {
            FileSystem.AddFile(@"c:\test.dll", new MockFileData("Invalid feature file"));

            var featureNodeFactory = this.CreateFeatureNodeFactory();

            var report = new ParsingReport();

            featureNodeFactory.Create(null, FileSystem.FileInfo.FromFileName(@"c:\test.dll"), report);

            Check.That(report.First()).Contains(@"c:\test.dll");
        }

        [Test]
        public void Create_BogusLocationType_AddsEntryToReport()
        {
            var featureNodeFactory = this.CreateFeatureNodeFactory();

            var report = new ParsingReport();

            featureNodeFactory.Create(null, new BogusIFileSystemInfo { fullName = "Totally Bad Name"}, report);

            Check.That(report.First()).Contains(@"Totally Bad Name");
        }

        private class BogusIFileSystemInfo : System.IO.Abstractions.IFileSystemInfo
        {
            internal string fullName;

            public void Delete()
            {
                throw new NotImplementedException();
            }

            public void Refresh()
            {
                throw new NotImplementedException();
            }

            public IFileSystem FileSystem { get; set; }

            public FileAttributes Attributes { get; set; }
            public DateTime CreationTime { get; set; }
            public DateTime CreationTimeUtc { get; set; }
            public bool Exists { get; }
            public string Extension { get; }

            public string FullName
            {
                get { return this.fullName; }
            }

            public DateTime LastAccessTime { get; set; }
            public DateTime LastAccessTimeUtc { get; set; }
            public DateTime LastWriteTime { get; set; }
            public DateTime LastWriteTimeUtc { get; set; }
            public string Name { get; }
        }

        private class MockMarkdownProvider : IMarkdownProvider
        {
            public string Transform(string text)
            {
                throw new Exception("Error parsing text.");
            }
        }
    }
}