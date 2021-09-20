//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FolderDirectoryTreeNodeTests.cs" company="PicklesDoc">
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
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.Extensions;

namespace PicklesDoc.Pickles.Test.DirectoryCrawlers
{

    [TestFixture]
    public class FolderDirectoryTreeNodeTests : BaseFixture
    {
        [Test]
        public void Constructor_ValidFileSystemInfo_SetsOriginalLocation()
        {
            var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName(@"temp");

            var node = new FolderNode(directoryInfo, "");

            Check.That(node.OriginalLocation.FullName).EndsWith(@"temp");
        }

        [Test]
        public void Constructor_ValidFileSystemInfo_SetsOriginalLocationUrl()
        {
            var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName(@"temp");

            var node = new FolderNode(directoryInfo, "");

            Check.That(node.OriginalLocationUrl.ToString()).Matches(@"file://(.*)temp");
        }

        [Test]
        public void Constructor_ValidRelativePath()
        {
            var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName("temp");

            var node = new FolderNode(directoryInfo, "../");

            Check.That(node.RelativePathFromRoot).IsEqualTo(@"../");
        }

        [Test]
        public void GetRelativeUriTo_DirectoryToChildDirectory_ReturnsRelativePath()
        {
            var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName("temp");

            var node = new FolderNode(directoryInfo, "../");
            var uri =FileSystem.Path.Combine(directoryInfo.FullName,"child").ToFolderUri();
            string relative = node.GetRelativeUriTo(uri);

            Check.That(relative).Contains("../");
        }

        [Test]
        public void GetRelativeUriTo_DirectoryToFileBelow_ReturnsCurrentDirectory()
        {
            var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName("temp");

            var node = new FolderNode(directoryInfo, "../");

            var uri = directoryInfo.GetFileUri("test2.html");
            string relative = node.GetRelativeUriTo(uri);

            Check.That(relative).IsEqualTo("./");
        }

        [Test]
        public void GetRelativeUriTo_DirectoryToFileOutside_ReturnsRelativePath()
        {
            var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName("temp");

            var node = new FolderNode(directoryInfo, "../");

            var uri =FileSystem.Path.Combine(FileSystem.DirectoryInfo.FromDirectoryName("temp2").FullName,"test2.html").ToFileUri();
            string relative = node.GetRelativeUriTo(uri);

            Check.That(relative).IsEqualTo("../temp/");
        }

        [Test]
        public void GetRelativeUriTo_DirectoryToParentDirectory_ReturnsRelativePath()
        {
            var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName(FileSystem.Path.Combine("temp","child"));

            var node = new FolderNode(directoryInfo, "../");

            var uri = FileSystem.DirectoryInfo.FromDirectoryName("temp").FullName.ToFolderUri();
            string relative = node.GetRelativeUriTo(uri);

            Check.That(relative).IsEqualTo("child/");
        }

        [Test]
        public void GetRelativeUriTo_FileToDirectory_ReturnsNodesFileName()
        {
            var fileInfo = FileSystem.FileInfo.FromFileName(FileSystem.Path.Combine("temp","test1.html"));

            var node = new FolderNode(fileInfo, "../");

            var uri = FileSystem.FileInfo.FromFileName("temp").FullName.ToFolderUri();
            string relative = node.GetRelativeUriTo(uri);

            Check.That(relative).IsEqualTo("test1.html");
        }

        [Test]
        public void GetRelativeUriTo_FileToFile_ReturnsNodesFileName()
        {
            var fileInfo = FileSystem.FileInfo.FromFileName(FileSystem.Path.Combine("temp","test1.html"));

            var node = new FolderNode(fileInfo, "../");

            var uri = FileSystem.FileInfo.FromFileName(FileSystem.Path.Combine("temp","test2.html")).FullName.ToFileUri();
            string relative = node.GetRelativeUriTo(uri);

            Check.That(relative).IsEqualTo("test1.html");
        }

        [Test]
        public void RealData()
        {
            var originalLocation =
                FileSystem.DirectoryInfo.FromDirectoryName(FileSystem.Path.Combine(
                    "tfs","Dev.CAX","src","CAX_Main","src","net","Projects","Aim.Gain.GoldenCopy.FunctionalTesting","CAX","DistributionOfRights"));

            var node = new FolderNode(originalLocation, "");

            var uri =
                FileSystem.DirectoryInfo.FromDirectoryName(FileSystem.Path.Combine("tfs", "Dev.CAX", "src", "CAX_Main", "src", "net", "Projects",
                    "Aim.Gain.GoldenCopy.FunctionalTesting", "CAX")).FullName.ToFolderUri();

            string relative = node.GetRelativeUriTo(uri);

            Check.That(relative).IsEqualTo("DistributionOfRights"+Path.DirectorySeparatorChar);
        }
    }
}
