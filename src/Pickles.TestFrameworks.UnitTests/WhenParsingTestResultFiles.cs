//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenParsingTestResultFiles.cs" company="PicklesDoc">
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
using System.Reflection;

using Autofac;

using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.Test;

namespace PicklesDoc.Pickles.TestFrameworks.UnitTests
{
    public abstract class WhenParsingTestResultFiles<TResults> : BaseFixture
        where TResults : ITestResults
    {
        private readonly string[] resultsFileNames;

        protected WhenParsingTestResultFiles(string resultsFileName)
        {
            this.resultsFileNames = resultsFileName.Split(';');
        }

        protected ITestResults ParseResultsFile()
        {
            this.AddTestResultsToConfiguration();

            var results = Container.Resolve<TResults>();
            return results;
        }

        protected void AddTestResultsToConfiguration()
        {
            foreach (var fileName in this.resultsFileNames)
            {
                // Write out the embedded test results file
                using (var input = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("PicklesDoc.Pickles.TestFrameworks.UnitTests." + fileName)))
                {
                    FileSystem.AddFile(fileName, new MockFileData(input.ReadToEnd()));
                }
            }

            var configuration = Container.Resolve<IConfiguration>();

            configuration.AddTestResultFiles(this.resultsFileNames.Select(f => FileSystem.FileInfo.New(f)));
        }
    }
}
