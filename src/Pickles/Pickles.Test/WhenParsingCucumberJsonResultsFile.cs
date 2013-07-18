using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using NUnit.Framework;
using PicklesDoc.Pickles.Parser;
using PicklesDoc.Pickles.TestFrameworks;
using Should;

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    class WhenParsingCucumberJsonResultsFile : BaseFixture
    {
        private const string RESULTS_FILE_NAME = "results-example-json.json";

        [Test]
        public void ThenCanReadFeatureResultSuccesfully()
        {
            // Write out the embedded test results file
            using (var input = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("PicklesDoc.Pickles.Test." + RESULTS_FILE_NAME)))
            using (var output = new StreamWriter(RESULTS_FILE_NAME))
            {
                output.Write(input.ReadToEnd());
            }

            var configuration = Container.Resolve<Configuration>();
            configuration.TestResultsFile = new FileInfo(RESULTS_FILE_NAME);

            var results = Container.Resolve<CucumberJsonResults>();

            var feature = new Feature { Name = "Test Feature" };
            TestResult result = results.GetFeatureResult(feature);

            result.WasExecuted.ShouldBeTrue();
            result.WasSuccessful.ShouldBeFalse();
        }

        [Test]
        public void ThenCanReadScenarioResultSuccessfully()
        {
            // Write out the embedded test results file
            using (var input = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("PicklesDoc.Pickles.Test." + RESULTS_FILE_NAME)))
            using (var output = new StreamWriter(RESULTS_FILE_NAME))
            {
                output.Write(input.ReadToEnd());
            }

            var configuration = Container.Resolve<Configuration>();
            configuration.TestResultsFile = new FileInfo(RESULTS_FILE_NAME);

            var results = Container.Resolve<CucumberJsonResults>();

            var feature = new Feature { Name = "Test Feature" };

            var scenario1 = new Scenario { Name = "Passing", Feature = feature };
            TestResult result1 = results.GetScenarioResult(scenario1);

            result1.WasExecuted.ShouldBeTrue();
            result1.WasSuccessful.ShouldBeTrue();

            var scenario2 = new Scenario { Name = "Failing", Feature = feature };
            TestResult result2 = results.GetScenarioResult(scenario2);

            result2.WasExecuted.ShouldBeTrue();
            result2.WasSuccessful.ShouldBeFalse();
        }
    }
}
