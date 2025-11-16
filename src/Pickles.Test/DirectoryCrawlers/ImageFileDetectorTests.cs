//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ImageFileDetectorTests.cs" company="PicklesDoc">
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
using System.IO.Abstractions.TestingHelpers;
using NFluent;
using NUnit.Framework;

using PicklesDoc.Pickles.DirectoryCrawler;

namespace PicklesDoc.Pickles.Test.DirectoryCrawlers
{
    [TestFixture]
    public class ImageFileDetectorTests
    {
        [Test]
        public void IsRelevant_ByDefault_ReturnsFalse()
        {
            ImageFileDetector fileDetector = CreateImageFileDetector();

            bool isRelevant = fileDetector.IsRelevant(null);

            Check.That(isRelevant).IsFalse();
        }

        private static ImageFileDetector CreateImageFileDetector()
        {
            return new ImageFileDetector();
        }

        [TestCase("image.png")]
        [TestCase("image.jpg")]
        [TestCase("image.bmp")]
        [TestCase("image.gif")]
        public void IsRelevant_ValidFile_ReturnsTrue(string imageName)
        {
            ImageFileDetector fileDetector = CreateImageFileDetector();

            var fileSystem = new MockFileSystem();
            var file = fileSystem.FileInfo.New(imageName);

            bool isRelevant = fileDetector.IsRelevant(file);

            Check.That(isRelevant).IsTrue();
        }

        [Test]
        public void IsRelevant_InvalidFile_ReturnsFalse()
        {
            ImageFileDetector fileDetector = CreateImageFileDetector();

            var fileSystem = new MockFileSystem();
            var file = fileSystem.FileInfo.New("image.pdf");

            bool isRelevant = fileDetector.IsRelevant(file);

            Check.That(isRelevant).IsFalse();
        }

        [Test]
        public void IsRelevant_ValidFileWithMixedCaseExtension_ReturnsTrue()
        {
            ImageFileDetector fileDetector = CreateImageFileDetector();

            var fileSystem = new MockFileSystem();
            var file = fileSystem.FileInfo.New("image.PnG");

            bool isRelevant = fileDetector.IsRelevant(file);

            Check.That(isRelevant).IsTrue();
        }
    }
}
