using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Autofac;
using PicklesDoc.Pickles.Parser;
using PicklesDoc.Pickles.TestFrameworks;
using Should;

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    public class WhenParsingMsTestResultsFile : BaseFixture
    {
        #region Setup/Teardown
        private const string RESULTS_FILE_NAME = "results-example-mstest.trx";

        #endregion

        [Test]
        public void ThenCanReadBackgroundResultSuccessfully()
        {
            var background = new Scenario {Name = "Background", Feature = AdditionFeature()};
            var feature = AdditionFeature();
            feature.AddBackground(background);
            var results = ParseResultsFile();

            TestResult result = results.GetScenarioResult(background);

            result.WasExecuted.ShouldBeFalse();
            result.WasSuccessful.ShouldBeFalse();
        }

        [Test]
        public void ThenCanReadFeatureResultSuccessfully()
        {
            var results = ParseResultsFile();
            
            TestResult result = results.GetFeatureResult(AdditionFeature());

            result.WasExecuted.ShouldBeTrue();
            result.WasSuccessful.ShouldBeFalse();
        }

        [Test]
        public void ThenCanReadScenarioOutlineResultSuccessfully()
        {
            var results = ParseResultsFile();
            var scenarioOutline = new ScenarioOutline {Name = "Adding several numbers", Feature = AdditionFeature()};
            
            TestResult result = results.GetScenarioOutlineResult(scenarioOutline);

            result.WasExecuted.ShouldBeTrue();
            result.WasSuccessful.ShouldBeTrue();
        }

        [Test]
        public void ThenCanReadSuccessfulScenarioResultSuccessfully()
        {
            var results = ParseResultsFile();
            var passedScenario = new Scenario { Name = "Add two numbers", Feature = AdditionFeature() };

            TestResult result = results.GetScenarioResult(passedScenario);

            result.WasExecuted.ShouldBeTrue();
            result.WasSuccessful.ShouldBeTrue();
            result.WasNotFound.ShouldBeFalse();
        }

        [Test]
        public void ThenCanReadFailedScenarioResultSuccessfully()
        {
            var results = ParseResultsFile();
            var scenario = new Scenario { Name = "Fail to add two numbers", Feature = AdditionFeature() };
            TestResult result = results.GetScenarioResult(scenario);

            result.WasExecuted.ShouldBeTrue();
            result.WasSuccessful.ShouldBeFalse();
            result.WasNotFound.ShouldBeFalse();
        }

        [Test]
        public void ThenCanReadIgnoredScenarioResultSuccessfully()
        {
            var results = ParseResultsFile();
            var ignoredScenario = new Scenario { Name = "Ignored adding two numbers", Feature = AdditionFeature() };
            
            var result = results.GetScenarioResult(ignoredScenario);

            result.WasExecuted.ShouldBeFalse();
            result.WasSuccessful.ShouldBeFalse();
            result.WasNotFound.ShouldBeFalse();
        }

        [Test]
        public void ThenCanReadInconclusiveScenarioResultSuccessfully()
        {
            var results = ParseResultsFile();
            
            var inconclusiveScenario = new Scenario
            {
                Name = "Not automated adding two numbers",
                Feature = AdditionFeature()
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
            var feature = new Feature { Name = "Not Automated At All" };

            var result = results.GetFeatureResult(feature);

            result.WasExecuted.ShouldBeFalse();
            result.WasSuccessful.ShouldBeFalse();
            result.WasNotFound.ShouldBeFalse();
        }

        private MsTestResults ParseResultsFile()
        {
            // Write out the embedded test results file
            using (var input = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("PicklesDoc.Pickles.Test." + RESULTS_FILE_NAME)))
            using (var output = new StreamWriter(RESULTS_FILE_NAME))
            {
                output.Write(input.ReadToEnd());
            }

            var configuration = Container.Resolve<Configuration>();
            configuration.TestResultsFile = new FileInfo(RESULTS_FILE_NAME);

            var results = Container.Resolve<MsTestResults>();
            return results;
        }

        private Feature AdditionFeature()
        {
            return new Feature() { Name = "Addition" };
        }


    }
}