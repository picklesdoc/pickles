//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Documentation_Tests.cs" company="PicklesDoc">
//  Copyright 2018 Darren Comeau
//  Copyright 2018-present PicklesDoc team and community contributors
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

using NUnit.Framework;
using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.ObjectModel;
using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.UnitTests
{
    [TestFixture]
    class Documentation_Tests
    {
        IFileSystem fileSystem = new MockFileSystem();

        [Test]
        public void New_Documentation_Produces_Default_Single_Page()
        {
            var documentation = new Documentation(null);
            var actualPageCount = documentation.PageCount;
            var actualPage = documentation.CurrentPage;

            Assert.AreEqual(1, documentation.PageCount);
            Assert.Contains("# Features", actualPage.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
        }

        [Test]
        public void New_Documentation_With_Feature_Produces_Single_Page()
        {
            var simpleFeature = new Feature();
            simpleFeature.Name = "My Feature";
            var relPath = "fakedir";
            var location = fileSystem.FileInfo.FromFileName(@"c:\");
            var newNode = new FeatureNode(location, relPath, simpleFeature);
            var featureTree = new Tree(new FolderNode(location, relPath));
            featureTree.Add(newNode);

            var documentation = new Documentation(featureTree);
            var actualPageCount = documentation.PageCount;
            var actualPage = documentation.CurrentPage.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.AreEqual(1, documentation.PageCount);
            Assert.Contains("# Features", actualPage);
            Assert.Contains("### My Feature", actualPage);
        }
    }
}
