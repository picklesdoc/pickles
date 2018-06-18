//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenResolvingTestResults.cs" company="PicklesDoc">
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

using Autofac;

using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.Test;
using PicklesDoc.Pickles.TestFrameworks.CucumberJson;
using PicklesDoc.Pickles.TestFrameworks.MsTest;
using PicklesDoc.Pickles.TestFrameworks.NUnit.NUnit2;
using PicklesDoc.Pickles.TestFrameworks.NUnit.NUnit3;
using PicklesDoc.Pickles.TestFrameworks.SpecRun;
using PicklesDoc.Pickles.TestFrameworks.VsTest;
using PicklesDoc.Pickles.TestFrameworks.XUnit.XUnit1;
using PicklesDoc.Pickles.TestFrameworks.XUnit.XUnit2;

namespace PicklesDoc.Pickles.TestFrameworks.UnitTests
{
    [TestFixture]
    public class WhenResolvingTestResults : BaseFixture
    {
        private static readonly object[] TestFormatCases =
        {
            new object[] { null, null, null, typeof(NullTestResults)},
            new object[] { "results-example-mstest.trx", "MsTest.results-example-mstest.trx", TestResultsFormat.MsTest, typeof(MsTestResults)},
            new object[] { "results-example-nunit.xml", "NUnit.NUnit2.results-example-nunit.xml", TestResultsFormat.NUnit, typeof(NUnit2Results)},
            new object[] { "results-example-nunit3.xml", "NUnit.NUnit3.results-example-nunit3.xml", TestResultsFormat.NUnit3, typeof(NUnit3Results)},
            new object[] { "results-example-xunit.xml", "XUnit.XUnit1.results-example-xunit.xml", TestResultsFormat.XUnit1, typeof(XUnit1Results)},
            new object[] { "results-example-xunit2.xml", "XUnit.XUnit2.results-example-xunit2.xml", TestResultsFormat.xUnit2, typeof(XUnit2Results)},
            new object[] { "results-example-json.json", "CucumberJson.results-example-json.json", TestResultsFormat.CucumberJson, typeof(CucumberJsonResults)},
            new object[] { "results-example-specrun.html", "SpecRun.results-example-specrun.html", TestResultsFormat.SpecRun, typeof(SpecRunResults)},
            new object[] { "results-example-vstest.trx", "VsTest.results-example-vstest.trx", TestResultsFormat.VsTest, typeof(VsTestResults)},
        };

        [Test, TestCaseSource(nameof(TestFormatCases))]
        public void ThenCanResolve(string exampleFilename, string resourceName, TestResultsFormat resultFormat, Type resultType)
        {
            if (exampleFilename != null)
            {
                this.SetFileSystem(exampleFilename, resourceName);
                this.SetConfiguration(exampleFilename, resultFormat);
            }

            var item = Container.Resolve<ITestResults>();

            Check.That(item).IsNotNull();
            Check.That(item).IsInstanceOfType(resultType);
        }

        [Test, TestCaseSource(nameof(TestFormatCases))]
        public void ThenCanResolveAsSingleton(string exampleFilename, string resourceName, TestResultsFormat resultFormat, Type resultType)
        {
            if (exampleFilename != null)
            {
                this.SetFileSystem(exampleFilename, resourceName);
                this.SetConfiguration(exampleFilename, resultFormat);
            }

            var item1 = Container.Resolve<ITestResults>();
            var item2 = Container.Resolve<ITestResults>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOfType(resultType);
            Check.That(item2).IsNotNull();
            Check.That(item2).IsInstanceOfType(resultType);
            Check.That(item1).IsSameReferenceAs(item2);
        }

        private void SetFileSystem(string example, string resource)
        {
            FileSystem.AddFile(example, RetrieveContentOfFileFromResources("PicklesDoc.Pickles.TestFrameworks.UnitTests." + resource));
        }

        private void SetConfiguration(string example, TestResultsFormat format)
        {
            var configuration = this.Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = format;
            configuration.AddTestResultFiles(new[] { this.FileSystem.FileInfo.FromFileName(example) });
        }
    }
}
