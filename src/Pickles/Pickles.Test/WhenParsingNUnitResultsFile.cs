using System.Reflection;
using NUnit.Framework;
using Autofac;
using PicklesDoc.Pickles.Parser;
using PicklesDoc.Pickles.TestFrameworks;
using Should;

using StreamReader = System.IO.StreamReader;
using StreamWriter = System.IO.StreamWriter;

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    public class WhenParsingNUnitResultsFile : BaseFixture
    {
        private const string RESULTS_FILE_NAME = "results-example-nunit.xml";

        [Test]
        public void ThenCanReadFeatureResultSuccessfully()
        {
            // Write out the embedded test results file
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Addition" };
            TestResult result = results.GetFeatureResult(feature);

            result.WasExecuted.ShouldBeTrue();
            result.WasSuccessful.ShouldBeFalse();
            result.WasNotFound.ShouldBeFalse();
        }

        [Test]
        public void ThenCanReadScenarioOutlineResultSuccessfully()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Addition" };

            var scenarioOutline = new ScenarioOutline { Name = "Adding several numbers", Feature = feature };
            TestResult result = results.GetScenarioOutlineResult(scenarioOutline);

            result.WasExecuted.ShouldBeTrue();
            result.WasSuccessful.ShouldBeTrue();
            result.WasNotFound.ShouldBeFalse();

            TestResult exampleResult1 = results.GetExampleResult(scenarioOutline, new[] { "40", "50", "90" });
            exampleResult1.WasExecuted.ShouldBeTrue();
            exampleResult1.WasSuccessful.ShouldBeTrue();
            exampleResult1.WasNotFound.ShouldBeFalse();

            TestResult exampleResult2 = results.GetExampleResult(scenarioOutline, new[] { "60", "70", "130" });
            exampleResult2.WasExecuted.ShouldBeTrue();
            exampleResult2.WasSuccessful.ShouldBeTrue();
            exampleResult2.WasNotFound.ShouldBeFalse();
        }

        [Test]
        public void ThenCanReadSuccessfulScenarioResultSuccessfully()
        {
            var results = ParseResultsFile();

            var feature = new Feature { Name = "Addition" };

            var passedScenario = new Scenario { Name = "Add two numbers", Feature = feature };
            TestResult result = results.GetScenarioResult(passedScenario);

            result.WasExecuted.ShouldBeTrue();
            result.WasSuccessful.ShouldBeTrue();
            result.WasNotFound.ShouldBeFalse();
        }

        [Test]
        public void ThenCanReadFailedScenarioResultSuccessfully()
        {
            var results = ParseResultsFile();
            var feature = new Feature { Name = "Addition" };
            var scenario = new Scenario { Name = "Fail to add two numbers", Feature = feature };
            TestResult result = results.GetScenarioResult(scenario);

            result.WasExecuted.ShouldBeTrue();
            result.WasSuccessful.ShouldBeFalse();
            result.WasNotFound.ShouldBeFalse();
        }

        [Test]
        public void ThenCanReadIgnoredScenarioResultSuccessfully()
        {
            var results = ParseResultsFile();
            var feature = new Feature { Name = "Addition" };
            var ignoredScenario = new Scenario { Name = "Ignored adding two numbers", Feature = feature };
            var result = results.GetScenarioResult(ignoredScenario);

            result.WasExecuted.ShouldBeFalse();
            result.WasSuccessful.ShouldBeFalse();
            result.WasNotFound.ShouldBeFalse();
        }

        [Test]
        public void ThenCanReadInconclusiveScenarioResultSuccessfully()
        {
            var results = ParseResultsFile();
            var feature = new Feature { Name = "Addition" };
            var inconclusiveScenario = new Scenario
            {
                Name = "Not automated adding two numbers",
                Feature = feature
            };
            var result = results.GetScenarioResult(inconclusiveScenario);

            result.WasExecuted.ShouldBeFalse();
            result.WasSuccessful.ShouldBeFalse();
            result.WasNotFound.ShouldBeFalse();
        }

        [Test]
        public void ThenCanReadInconclusiveFeatureResultSuccessfully()
        {
            var results = ParseResultsFile();
            var result = results.GetFeatureResult(InconclusiveFeature());
            Assert.AreEqual(TestResult.Inconclusive(), result);
        }


        [Test]
        public void ThenCanReadPassedFeatureResultSuccessfully()
        {
            var results = ParseResultsFile();
            var result = results.GetFeatureResult(PassingFeature());
            Assert.AreEqual(TestResult.Passed(), result);
        }

        [Test]
        public void ThenCanReadFailedFeatureResultSuccessfully()
        {
            var results = ParseResultsFile();
            var result = results.GetFeatureResult(FailingFeature());
            Assert.AreEqual(TestResult.Failed(), result);
        }

        private Feature FailingFeature()
        {
            return new Feature() {Name = "Failing"};
        }

        private Feature InconclusiveFeature()
        {
            return new Feature() { Name = "Inconclusive" };
        }

        private Feature PassingFeature()
        {
            return new Feature() { Name = "Passing" };
        }

        [Test]
        public void ThenCanReadNotFoundScenarioCorrectly()
        {
            var results = ParseResultsFile();
            var feature = new Feature { Name = "Addition" };
            var notFoundScenario = new Scenario
            {
                Name = "Not in the file at all!",
                Feature = feature
            };

            var result = results.GetScenarioResult(notFoundScenario);

            result.WasExecuted.ShouldBeFalse();
            result.WasSuccessful.ShouldBeFalse();
            result.WasNotFound.ShouldBeTrue();
        }

        private NUnitResults ParseResultsFile()
        {
            // Write out the embedded test results file
            using (var input = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("PicklesDoc.Pickles.Test." + RESULTS_FILE_NAME)))
            using (var output = new StreamWriter(RESULTS_FILE_NAME))
            {
                output.Write(input.ReadToEnd());
            }

            var configuration = Container.Resolve<Configuration>();
            configuration.TestResultsFile = RealFileSystem.FileInfo.FromFileName(RESULTS_FILE_NAME);

            var results = Container.Resolve<NUnitResults>();
            return results;
        }


    }
}