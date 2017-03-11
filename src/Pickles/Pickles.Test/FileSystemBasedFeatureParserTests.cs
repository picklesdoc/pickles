//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FileSystemBasedFeatureParserTests.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    public class FileSystemBasedFeatureParserTests : BaseFixture
    {
        [Test]
        public void Parse_InvalidFeatureFile_ThrowsFeatureParseExceptionWithFilename()
        {
            FileSystem.AddFile(@"c:\temp\featurefile.feature", new MockFileData("Invalid feature file"));
            var parser = new FileSystemBasedFeatureParser(new FeatureParser(Configuration), FileSystem);

            Check.ThatCode(() => parser.Parse(@"c:\temp\featurefile.feature")).Throws<FeatureParseException>()
                .WithMessage(@"There was an error parsing the feature file here: c:\temp\featurefile.feature" +
                             Environment.NewLine + @"Errormessage was: 'Unable to parse feature'");
        }


        [Test]
        public void Parse_WesternEuropeanEncoding()
        {
            var parser = new FileSystemBasedFeatureParser(new FeatureParser(Configuration), FileSystem);
            AddFakeFolderAndFiles(@"Encodings", new string[0]);
            //AddFakeFolderAndFiles(@"Encodings\UTF8", new[] { "UTF8WithBOM.feature", "UTF8WithoutBOM.feature" });
            AddFakeFolderAndFiles(@"Encodings\WesternEuropean", new[] { "WesternEuropean1252.feature" });

            parser.Parse(@"c:\temp\FakeFolderStructures\Encodings\WesternEuropean\WesternEuropean1252.feature");
        }
    }
}