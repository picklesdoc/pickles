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
            FileSystem.AddFile(@"c:\test.feature", new MockFileData("Invalid feature file"));

            var featureNodeFactory = this.CreateFeatureNodeFactory();

            var report = new ParsingReport();

            featureNodeFactory.Create(null, FileSystem.FileInfo.FromFileName(@"c:\test.feature"), report);

            Check.That(report.First()).Contains(@"c:\test.feature");
        }

        private FeatureNodeFactory CreateFeatureNodeFactory()
        {
            return new FeatureNodeFactory(
                new RelevantFileDetector(),
                new FileSystemBasedFeatureParser(
                    new FeatureParser(Configuration),
                    FileSystem),
                new HtmlMarkdownFormatter(
                    new MarkdownProvider()),
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

            featureNodeFactory.Create(null, new BogusFileSystemInfoBase { fullName = "Totally Bad Name"}, report);

            Check.That(report.First()).Contains(@"Totally Bad Name");
        }

        private class BogusFileSystemInfoBase : System.IO.Abstractions.FileSystemInfoBase
        {
            internal string fullName;

            public override void Delete()
            {
                throw new NotImplementedException();
            }

            public override void Refresh()
            {
                throw new NotImplementedException();
            }

            public override FileAttributes Attributes { get; set; }
            public override DateTime CreationTime { get; set; }
            public override DateTime CreationTimeUtc { get; set; }
            public override bool Exists { get; }
            public override string Extension { get; }

            public override string FullName
            {
                get { return this.fullName; }
            }

            public override DateTime LastAccessTime { get; set; }
            public override DateTime LastAccessTimeUtc { get; set; }
            public override DateTime LastWriteTime { get; set; }
            public override DateTime LastWriteTimeUtc { get; set; }
            public override string Name { get; }
        }
    }
}