//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenDoingSomeIntegrationTests.cs" company="PicklesDoc">
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

using NFluent;
using NUnit.Framework;

using PicklesDoc.Pickles.DocumentationBuilders.Dhtml;

namespace PicklesDoc.Pickles.Test.DocumentationBuilders.DHTML
{
    [TestFixture]
    public class WhenDoingSomeIntegrationTests : BaseFixture
    {
        [Test]
        public void TestTheResourceWriter()
        {
            var conf = new Configuration();
            conf.OutputFolder = FileSystem.DirectoryInfo.FromDirectoryName(@"d:\output");
            var resourceWriter = new DhtmlResourceWriter(FileSystem, conf);
            resourceWriter.WriteTo(conf.OutputFolder.FullName);
        }

        [Test]
        public void CanAddFunctionWrapperAroundJson()
        {
            string filePath = @"d:\output\pickledFeatures.json";
            FileSystem.AddFile(filePath, "\r\n[]\r\n");

            var jsonTweaker = new JsonTweaker(FileSystem);
            jsonTweaker.AddJsonPWrapperTo(filePath);

            var expected = "jsonPWrapper (\r\n[]\r\n);";
            var actual = FileSystem.File.ReadAllText(filePath);

            Check.That(actual).IsEqualTo(expected);
        }

        [Test]
        public void CanRenameJsonFile()
        {
            string oldfilePath = @"d:\output\pickledFeatures.json";
            string newFilePath = @"d:\output\pickledFeatures.js";

            FileSystem.AddFile(oldfilePath, "test data");

            var jsonTweaker = new JsonTweaker(FileSystem);
            jsonTweaker.RenameFileTo(oldfilePath, newFilePath);

            var doesNewPathExist = FileSystem.File.Exists(newFilePath);
            Check.That(doesNewPathExist).IsTrue();
            var doesOldPathExist = FileSystem.File.Exists(oldfilePath);
            Check.That(doesOldPathExist).IsFalse();
        }
    }
}
