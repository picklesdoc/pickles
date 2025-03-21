﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PathExtensionsTests.cs" company="PicklesDoc">
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
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.Extensions;

namespace PicklesDoc.Pickles.Test.Extensions
{
    [TestFixture]
    internal class PathExtensionsTests : BaseFixture
    {

        [Test]
        public void Get_A_Relative_Path_When_Location_Is_Deeper_Than_Root()
        {
            MockFileSystem fileSystem = FileSystem;
            fileSystem.AddFile(FileSystem.Path.Combine("test","deep","blah.feature"),"Feature:"); // adding a file automatically adds all parent directories

            string actual = PathExtensions.MakeRelativePath("test",FileSystem.Path.Combine("test","deep","blah.feature"), fileSystem);

            Check.That(actual).IsEqualTo("deep" + PathExtensions.Separator + "blah.feature");
        }

        [Test]
        public void Get_A_Relative_Path_When_Location_Is_Deeper_Than_Root_Even_When_Root_Contains_End_Slash()
        {
            MockFileSystem fileSystem = FileSystem;
            fileSystem.AddFile(FileSystem.Path.Combine("test","deep","blah.feature"), "Feature:"); // adding a file automatically adds all parent directories

            string actual = PathExtensions.MakeRelativePath("test",FileSystem.Path.Combine("test","deep","blah.feature"), fileSystem);

            Check.That(actual).IsEqualTo("deep" + PathExtensions.Separator + "blah.feature");
        }
    }
}
