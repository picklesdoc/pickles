//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="RelevantFileDetectorTests.cs" company="PicklesDoc">
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

using System.IO.Abstractions.TestingHelpers;
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.DirectoryCrawler;

namespace PicklesDoc.Pickles.Test.DirectoryCrawlers
{
    [TestFixture]
    public class RelevantFileDetectorTests
    {
        [Test]
        public void IsRelevant_ImageFileInfo_ReturnsTrue()
        {
            var fileDetector = new RelevantFileDetector();

            var fileSystem = new MockFileSystem();
            var file = fileSystem.FileInfo.New("image.png");

            bool isRelevant = fileDetector.IsRelevant(file);

            Check.That(isRelevant).IsTrue();
        }
    }
}
