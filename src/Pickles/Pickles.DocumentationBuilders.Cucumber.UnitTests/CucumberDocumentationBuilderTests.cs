//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CucumberDocumentationBuilderTests.cs" company="PicklesDoc">
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

using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Reflection;
using NUnit.Framework;
using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;

namespace PicklesDoc.Pickles.DocumentationBuilders.Cucumber.UnitTests
{
    [TestFixture]
    public class CucumberDocumentationBuilderTests
    {
        [Test]
        public void GIVEN_MacOS_test_When_create_document_Then_there_is_no_error()
        {
            var featureDescription =
         @"Feature: Clearing Screen
            In order to restart a new set of calculations
            As a math idiot
            I want to be able to clear the screen

                @workflow @slow
            Scenario: Clear the screen
                Given I have entered 50 into the calculator
                And I have entered 70 into the calculator
                When I press C
                Then the screen should be empty
            ";

            var fileSystem = new FileSystem();
            fileSystem.Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var IFileSystemInfo = fileSystem.DirectoryInfo.FromDirectoryName(@"./");
            var configuration = new Configuration
            {
                OutputFolder = IFileSystemInfo
            };
            var builder = new CucumberDocumentationBuilder(configuration, fileSystem);
            FeatureParser parser = new FeatureParser(configuration);
            var feature = parser.Parse(new StringReader(featureDescription));
            var tree =  new Tree(new FeatureNode(IFileSystemInfo, string.Empty, feature));
            builder.Build(tree);
        }
    }
}