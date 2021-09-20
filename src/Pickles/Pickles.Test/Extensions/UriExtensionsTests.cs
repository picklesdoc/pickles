//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="UriExtensionsTests.cs" company="PicklesDoc">
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
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.Extensions;

namespace PicklesDoc.Pickles.Test.Extensions
{
    [TestFixture]
    public class UriExtensionsTests : BaseFixture
    {
        [Test]
        public void ToFileUriCombined_ValidIntput_ValidOutput()
        {
            var info = FileSystem.DirectoryInfo.FromDirectoryName("temp");

            Uri uri = info.ToFileUriCombined("test.txt", FileSystem);

            Check.That(uri).IsEqualTo(new Uri("file://" + FileSystem.Path.Combine(info.FullName,"test.txt")));
        }

        [Test]
        public void ToFileUriString_WithoutTrailingSlash_ValidOutputWithTrailingSlash()
        {
            Uri uri =FileSystem.Path.Combine("temp","test.txt").ToFileUri();

            Check.That(uri).IsEqualTo(new Uri("file://" + FileSystem.Path.Combine("temp","test.txt")));
        }

        [Test]
        public void ToFolderUriString_WithTrailingSlash_ValidOutput()
        {
            Uri uri = @"c:\temp\".ToFolderUri();

            Check.That(uri.ToString()).IsEqualTo("file:///c:/temp/");
        }

        [Test]
        public void ToFolderUriString_WithoutTrailingSlash_ValidOutputWithTrailingSlash()
        {
            Uri uri = @"c:\temp".ToFolderUri();

            Check.That(uri.ToString()).IsEqualTo("file:///c:/temp/");
        }

        [Test]
        public void ToUriDirectoryInfo_WithTrailingSlash_ProducesUriWithTrailingSlash()
        {
            var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName("temp"+Path.DirectorySeparatorChar);

            Uri uri = directoryInfo.ToUri();

            Check.That(uri).IsEqualTo(new Uri("file://"+directoryInfo.FullName+"/"));
        }

        [Test]
        public void ToUriDirectoryInfo_WithoutTrailingSlash_ProducesUriWithTrailingSlash()
        {
            var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName("temp");

            Uri uri = directoryInfo.ToUri();

            Check.That(uri).IsEqualTo(new Uri("file://"+directoryInfo.FullName+"/"));
        }

        [Test]
        public void ToUriFileInfo_NormalFilename_ProducesUri()
        {
            var fileInfo = FileSystem.FileInfo.FromFileName(FileSystem.Path.Combine("temp","test.txt"));

            Uri uri = fileInfo.ToUri();

            Check.That(uri).IsEqualTo(new Uri("file://"+fileInfo.FullName));
        }

        [Test]
        public void ToUriFileSystemInfo_DirectoryWithTrailingSlash_ProducesUriWithTrailingSlash()
        {
            var fsi = FileSystem.DirectoryInfo.FromDirectoryName("temp");

            Uri uri = fsi.ToUri();

            Check.That(uri).IsEqualTo(new Uri("file://"+fsi.FullName+"/"));
        }

        [Test]
        public void ToUriFileSystemInfo_FileInfo_ProducesUri()
        {
            var fsi = FileSystem.FileInfo.FromFileName(FileSystem.Path.Combine("temp","test.txt"));

            Uri uri = fsi.ToUri();

            Check.That(uri).IsEqualTo(new Uri("file://"+fsi.FullName));
        }
    }
}
