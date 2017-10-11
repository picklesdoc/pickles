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
        private const string TestResultsResourcePrefix = "PicklesDoc.Pickles.TestFrameworks.UnitTests.";

        [Test]
        public void ThenCanResolveAsSingletonWhenNoTestResultsSelected()
        {
            var item1 = Container.Resolve<ITestResults>();
            var item2 = Container.Resolve<ITestResults>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOf<NullTestResults>();
            Check.That(item2).IsNotNull();
            Check.That(item2).IsInstanceOf<NullTestResults>();
            Check.That(item1).IsSameReferenceAs(item2);
        }

        [Test]
        public void ThenCanResolveAsSingletonWhenTestResultsAreMsTest()
        {
            FileSystem.AddFile("results-example-mstest.trx", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "MsTest.results-example-mstest.trx"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.MsTest;
            configuration.AddTestResultFiles(new[] { FileSystem.FileInfo.FromFileName("results-example-mstest.trx") });

            var item1 = Container.Resolve<ITestResults>();
            var item2 = Container.Resolve<ITestResults>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOf<MsTestResults>();
            Check.That(item2).IsNotNull();
            Check.That(item2).IsInstanceOf<MsTestResults>();
            Check.That(item1).IsSameReferenceAs(item2);
        }

        [Test]
        public void ThenCanResolveAsSingletonWhenTestResultsAreNUnit()
        {
            FileSystem.AddFile("results-example-nunit.xml", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "NUnit.NUnit2.results-example-nunit.xml"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.NUnit;
            configuration.AddTestResultFile(FileSystem.FileInfo.FromFileName("results-example-nunit.xml"));

            var item1 = Container.Resolve<ITestResults>();
            var item2 = Container.Resolve<ITestResults>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOf<NUnit2Results>();
            Check.That(item2).IsNotNull();
            Check.That(item2).IsInstanceOf<NUnit2Results>();
            Check.That(item1).IsSameReferenceAs(item2);
        }

        [Test]
        public void ThenCanResolveAsSingletonWhenTestResultsAreNUnit3()
        {
            FileSystem.AddFile("results-example-nunit3.xml", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "NUnit.NUnit3.results-example-nunit3.xml"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.NUnit3;
            configuration.AddTestResultFile(FileSystem.FileInfo.FromFileName("results-example-nunit3.xml"));

            var item1 = Container.Resolve<ITestResults>();
            var item2 = Container.Resolve<ITestResults>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOf<NUnit3Results>();
            Check.That(item2).IsNotNull();
            Check.That(item2).IsInstanceOf<NUnit3Results>();
            Check.That(item1).IsSameReferenceAs(item2);
        }

        [Test]
        public void ThenCanResolveAsSingletonWhenTestResultsArexUnit()
        {
            FileSystem.AddFile("results-example-xunit.xml", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "XUnit.XUnit1.results-example-xunit.xml"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.XUnit1;
            configuration.AddTestResultFile(FileSystem.FileInfo.FromFileName("results-example-xunit.xml"));

            var item1 = Container.Resolve<ITestResults>();
            var item2 = Container.Resolve<ITestResults>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOf<XUnit1Results>();
            Check.That(item2).IsNotNull();
            Check.That(item2).IsInstanceOf<XUnit1Results>();
            Check.That(item1).IsSameReferenceAs(item2);
        }

        [Test]
        public void ThenCanResolveAsSingletonWhenTestResultsArexUnit2()
        {
            FileSystem.AddFile("results-example-xunit2.xml", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "XUnit.XUnit2.results-example-xunit2.xml"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.xUnit2;
            configuration.AddTestResultFile(FileSystem.FileInfo.FromFileName("results-example-xunit2.xml"));

            var item1 = Container.Resolve<ITestResults>();
            var item2 = Container.Resolve<ITestResults>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOf<XUnit2Results>();
            Check.That(item2).IsNotNull();
            Check.That(item2).IsInstanceOf<XUnit2Results>();
            Check.That(item1).IsSameReferenceAs(item2);
        }

        [Test]
        public void ThenCanResolveAsSingletonWhenTestResultsAreCucumberJson()
        {
            FileSystem.AddFile("results-example-json.json", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "CucumberJson.results-example-json.json"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.CucumberJson;
            configuration.AddTestResultFile(FileSystem.FileInfo.FromFileName("results-example-json.json"));

            var item1 = Container.Resolve<ITestResults>();
            var item2 = Container.Resolve<ITestResults>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOf<CucumberJsonResults>();
            Check.That(item2).IsNotNull();
            Check.That(item2).IsInstanceOf<CucumberJsonResults>();
            Check.That(item1).IsSameReferenceAs(item2);
        }

        [Test]
        public void ThenCanResolveAsSingletonWhenTestResultsAreSpecrun()
        {
            FileSystem.AddFile("results-example-specrun.html", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "SpecRun.results-example-specrun.html"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.SpecRun;
            configuration.AddTestResultFile(FileSystem.FileInfo.FromFileName("results-example-specrun.html"));

            var item1 = Container.Resolve<ITestResults>();
            var item2 = Container.Resolve<ITestResults>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOf<SpecRunResults>();
            Check.That(item2).IsNotNull();
            Check.That(item2).IsInstanceOf<SpecRunResults>();
            Check.That(item1).IsSameReferenceAs(item2);
        }

        [Test]
        public void ThenCanResolveAsSingletonWhenTestResultsAreVsTest()
        {
            FileSystem.AddFile("results-example-vstest.trx", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "VsTest.results-example-vstest.trx"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.VsTest;
            configuration.AddTestResultFiles(new[] { FileSystem.FileInfo.FromFileName("results-example-vstest.trx") });

            var item1 = Container.Resolve<ITestResults>();
            var item2 = Container.Resolve<ITestResults>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOf<VsTestResults>();
            Check.That(item2).IsNotNull();
            Check.That(item2).IsInstanceOf<VsTestResults>();
            Check.That(item1).IsSameReferenceAs(item2);
        }

        [Test]
        public void ThenCanResolveWhenNoTestResultsSelected()
        {
            var item = Container.Resolve<ITestResults>();

            Check.That(item).IsNotNull();
            Check.That(item).IsInstanceOf<NullTestResults>();
        }

        [Test]
        public void ThenCanResolveWhenTestResultsAreMsTest()
        {
            FileSystem.AddFile("results-example-mstest.trx", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "MsTest.results-example-mstest.trx"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.MsTest;
            configuration.AddTestResultFile(FileSystem.FileInfo.FromFileName("results-example-mstest.trx"));

            var item = Container.Resolve<ITestResults>();

            Check.That(item).IsNotNull();
            Check.That(item).IsInstanceOf<MsTestResults>();
        }

        [Test]
        public void ThenCanResolveWhenTestResultsAreNUnit()
        {
            FileSystem.AddFile("results-example-nunit.xml", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "NUnit.NUnit2.results-example-nunit.xml"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.NUnit;
            configuration.AddTestResultFile(FileSystem.FileInfo.FromFileName("results-example-nunit.xml"));

            var item = Container.Resolve<ITestResults>();

            Check.That(item).IsNotNull();
            Check.That(item).IsInstanceOf<NUnit2Results>();
        }

        [Test]
        public void ThenCanResolveWhenTestResultsAreNUnit3()
        {
            FileSystem.AddFile("results-example-nunit3.xml", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "NUnit.NUnit3.results-example-nunit3.xml"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.NUnit3;
            configuration.AddTestResultFile(FileSystem.FileInfo.FromFileName("results-example-nunit3.xml"));

            var item1 = Container.Resolve<ITestResults>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOf<NUnit3Results>();
        }

        [Test]
        public void ThenCanResolveWhenTestResultsArexUnit()
        {
            FileSystem.AddFile("results-example-xunit.xml", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "XUnit.XUnit1.results-example-xunit.xml"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.XUnit1;
            configuration.AddTestResultFile(FileSystem.FileInfo.FromFileName("results-example-xunit.xml"));

            var item = Container.Resolve<ITestResults>();

            Check.That(item).IsNotNull();
            Check.That(item).IsInstanceOf<XUnit1Results>();
        }

        [Test]
        public void ThenCanResolveWhenTestResultsArexUnit2()
        {
            FileSystem.AddFile("results-example-xunit2.xml", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "XUnit.XUnit2.results-example-xunit2.xml"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.xUnit2;
            configuration.AddTestResultFile(FileSystem.FileInfo.FromFileName("results-example-xunit2.xml"));

            var item1 = Container.Resolve<ITestResults>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOf<XUnit2Results>();
        }

        [Test]
        public void ThenCanResolveWhenTestResultsAreCucumberJson()
        {
            FileSystem.AddFile("results-example-json.json", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "CucumberJson.results-example-json.json"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.CucumberJson;
            configuration.AddTestResultFile(FileSystem.FileInfo.FromFileName("results-example-json.json"));

            var item = Container.Resolve<ITestResults>();

            Check.That(item).IsNotNull();
            Check.That(item).IsInstanceOf<CucumberJsonResults>();
        }

        [Test]
        public void ThenCanResolveWhenTestResultsAreSpecrun()
        {
            FileSystem.AddFile("results-example-specrun.html", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "SpecRun.results-example-specrun.html"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.SpecRun;
            configuration.AddTestResultFile(FileSystem.FileInfo.FromFileName("results-example-specrun.html"));

            var item = Container.Resolve<ITestResults>();

            Check.That(item).IsNotNull();
            Check.That(item).IsInstanceOf<SpecRunResults>();
        }

        [Test]
        public void ThenCanResolveWhenTestResultsAreVsTest()
        {
            FileSystem.AddFile("results-example-vstest.trx", RetrieveContentOfFileFromResources(TestResultsResourcePrefix + "VsTest.results-example-vstest.trx"));

            var configuration = Container.Resolve<IConfiguration>();
            configuration.TestResultsFormat = TestResultsFormat.VsTest;
            configuration.AddTestResultFiles(new[] { FileSystem.FileInfo.FromFileName("results-example-vstest.trx") });

            var item1 = Container.Resolve<ITestResults>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOf<VsTestResults>();
        }
    }
}
