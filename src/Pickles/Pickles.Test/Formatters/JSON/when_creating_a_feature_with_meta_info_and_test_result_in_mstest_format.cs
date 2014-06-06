using System;
using System.Collections.Generic;
using System.Linq;

using NGenerics.DataStructures.Trees;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using Autofac;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders.JSON;
using PicklesDoc.Pickles.Test.Helpers;
using PicklesDoc.Pickles.TestFrameworks;

namespace PicklesDoc.Pickles.Test.Formatters.JSON
{
    public class when_creating_a_feature_with_meta_info_and_test_result_in_mstest_format : BaseFixture
    {
        public string Setup()
        {
            const string OUTPUT_DIRECTORY = FileSystemPrefix + @"JSONFeatureOutput";
            const string ROOT_PATH = FileSystemPrefix + @"JSON\Features";
            const string testResultFilePath = FileSystemPrefix + @"JSON\results-example-failing-and-pasing-mstest.trx";

            string filePath = FileSystem.Path.Combine(OUTPUT_DIRECTORY, JSONDocumentationBuilder.JsonFileName);

            AddFakeFolderAndFiles("JSON", new[] { "results-example-failing-and-pasing-mstest.trx" });
            AddFakeFolderAndFiles(
                @"JSON\Features",
                new[]
                    {
                      "OneScenarioTransferingMoneyBetweenAccountsFailing.feature",
                      "TransferBetweenAccounts_WithSuccess.feature",
                      "TwoScenariosTransferingFundsOneFailingOneSuccess.feature",
                      "TwoScenariosTransferingMoneyBetweenAccoutsWithSuccess.feature",
                    });

            var resultFile = RetrieveContentOfFileFromResources(ResourcePrefix + "JSON.results-example-failing-and-pasing-mstest.trx");
            FileSystem.AddFile(testResultFilePath, resultFile);

            GeneralTree<INode> features = Container.Resolve<DirectoryTreeCrawler>().Crawl(ROOT_PATH);

            var outputDirectory = FileSystem.DirectoryInfo.FromDirectoryName(OUTPUT_DIRECTORY);
            if (!outputDirectory.Exists) outputDirectory.Create();

            var configuration = new Configuration
                                {
                                    OutputFolder = FileSystem.DirectoryInfo.FromDirectoryName(OUTPUT_DIRECTORY),
                                    DocumentationFormat = DocumentationFormat.JSON,
                                    TestResultsFiles = new[] { FileSystem.FileInfo.FromFileName(testResultFilePath) },
                                    TestResultsFormat = TestResultsFormat.MsTest,
                                    SystemUnderTestName = "SUT Name",
                                    SystemUnderTestVersion = "SUT Version"
                                };

            ITestResults testResults = new MsTestResults(configuration);
            var jsonDocumentationBuilder = new JSONDocumentationBuilder(configuration, testResults, FileSystem);
            jsonDocumentationBuilder.Build(features);
            string content = FileSystem.File.ReadAllText(filePath);

            return content;
        }

        [Test]
        public void it_should_contain_result_keys_in_the_json_document()
        {
            string content = this.Setup();

            content.AssertJsonContainsKey("Result");
        }

        [Test]
        public void it_should_contain_the_sut_info_in_the_json_document()
        {
            string content = this.Setup();

            var jsonObj = JObject.Parse(content);

            var configuration = jsonObj["Configuration"];

            Assert.That(configuration["SutName"].ToString(), Is.EqualTo("SUT Name"));
            Assert.That(configuration["SutVersion"].ToString(), Is.EqualTo("SUT Version"));
        }

        [Test]
        public void it_should_indicate_WasSuccessful_is_true()
        {
            string content = this.Setup();

            var jsonObj = JObject.Parse(content);


            IEnumerable<JToken> featureJsonElement = from feat in jsonObj["Features"]
                                                     where
                                                         feat["Feature"]["Name"].Value<string>().Equals(
                                                             "Two more scenarios transfering funds between accounts")
                                                     select feat;

            Assert.IsTrue(featureJsonElement.ElementAt(0)["Result"]["WasSuccessful"].Value<bool>());
        }

        [Test]
        public void it_should_indicate_WasSuccessful_is_true_for_the_other_success_feature()
        {
            string content = this.Setup();

            var jsonObj = JObject.Parse(content);


            IEnumerable<JToken> featureJsonElement = from feat in jsonObj["Features"]
                                                     where
                                                         feat["Feature"]["Name"].Value<string>().Equals(
                                                             "Transfer funds between accounts")
                                                     select feat;

            Assert.IsTrue(featureJsonElement.ElementAt(0)["Result"]["WasSuccessful"].Value<bool>());
        }

        [Test]
        public void it_should_indicate_WasSuccessful_is_false_for_failing_scenario()
        {
            string content = this.Setup();

            var jsonObj = JObject.Parse(content);


            IEnumerable<JToken> featureJsonElement = from feat in jsonObj["Features"]
                                                     where
                                                         feat["Feature"]["Name"].Value<string>().Equals(
                                                             "Transfer funds between accounts onc scenario and FAILING")
                                                     select feat;

            Assert.IsFalse(featureJsonElement.ElementAt(0)["Result"]["WasSuccessful"].Value<bool>());
        }


        [Test]
        public void it_should_indicate_WasSuccessful_is_false_for_another_failing_scenario()
        {
            string content = this.Setup();

            var jsonObj = JObject.Parse(content);


            IEnumerable<JToken> featureJsonElement = from feat in jsonObj["Features"]
                                                     where
                                                         feat["Feature"]["Name"].Value<string>().Equals(
                                                             "Two more scenarios transfering funds between accounts - one failng and one succeding")
                                                     select feat;

            Assert.IsFalse(featureJsonElement.ElementAt(0)["Result"]["WasSuccessful"].Value<bool>());
        }


        [Test]
        public void it_should_contain_WasSuccessful_key_in_Json_document()
        {
            string content = this.Setup();

            var jsonObj = JObject.Parse(content);

            Assert.IsNotEmpty(jsonObj["Features"][0]["Result"]["WasSuccessful"].ToString());
        }


        [Test]
        public void it_should_WasSuccessful_false_for_feature_X_Json_document()
        {
            string content = this.Setup();

            var jsonObj = JObject.Parse(content);

            Assert.IsNotEmpty(jsonObj["Features"][0]["Result"]["WasSuccessful"].ToString());
        }
    }
}