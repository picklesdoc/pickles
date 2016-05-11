//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PicklesModule.cs" company="PicklesDoc">
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

using Autofac;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders;
using PicklesDoc.Pickles.DocumentationBuilders.DHTML;
using PicklesDoc.Pickles.DocumentationBuilders.Excel;
using PicklesDoc.Pickles.DocumentationBuilders.HTML;
using PicklesDoc.Pickles.DocumentationBuilders.JSON;
using PicklesDoc.Pickles.DocumentationBuilders.Word;
using PicklesDoc.Pickles.FeatureToggles;
using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.TestFrameworks;
using PicklesDoc.Pickles.TestFrameworks.CucumberJson;
using PicklesDoc.Pickles.TestFrameworks.MsTest;
using PicklesDoc.Pickles.TestFrameworks.NUnit.NUnit2;
using PicklesDoc.Pickles.TestFrameworks.NUnit.NUnit3;
using PicklesDoc.Pickles.TestFrameworks.SpecRun;
using PicklesDoc.Pickles.TestFrameworks.VsTest;
using PicklesDoc.Pickles.TestFrameworks.XUnit.XUnit1;
using PicklesDoc.Pickles.TestFrameworks.XUnit.XUnit2;

namespace PicklesDoc.Pickles
{
    public class PicklesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Configuration>().As<IConfiguration>().SingleInstance();
            builder.RegisterType<DirectoryTreeCrawler>().SingleInstance();
            builder.RegisterType<FeatureParser>().SingleInstance();
            builder.RegisterType<RelevantFileDetector>().SingleInstance();
            builder.RegisterType<FeatureNodeFactory>().SingleInstance();

            builder.RegisterType<HtmlDocumentationBuilder>().SingleInstance();
            builder.RegisterType<WordDocumentationBuilder>().SingleInstance();
            builder.RegisterType<JsonDocumentationBuilder>().SingleInstance();
            builder.RegisterType<ExcelDocumentationBuilder>().SingleInstance();
            builder.RegisterType<DhtmlDocumentationBuilder>().SingleInstance();

            builder.Register<IDocumentationBuilder>(c =>
            {
                var configuration = c.Resolve<IConfiguration>();
                switch (configuration.DocumentationFormat)
                {
                    case DocumentationFormat.Html:
                        return c.Resolve<HtmlDocumentationBuilder>();
                    case DocumentationFormat.Word:
                        return c.Resolve<WordDocumentationBuilder>();
                    case DocumentationFormat.Json:
                        return c.Resolve<JsonDocumentationBuilder>();
                    case DocumentationFormat.Excel:
                        return c.Resolve<ExcelDocumentationBuilder>();
                    case DocumentationFormat.DHtml:
                        return c.Resolve<DhtmlDocumentationBuilder>();
                    default:
                        return c.Resolve<HtmlDocumentationBuilder>();
                }
            }).SingleInstance();

            builder.RegisterType<NUnit2Results>();
            builder.RegisterType<NUnit2SingleResultLoader>();
            builder.RegisterType<NUnit2ScenarioOutlineExampleMatcher>();
            builder.RegisterType<NUnit3ScenarioOutlineExampleMatcher>();
            builder.RegisterType<NUnit3Results>();
            builder.RegisterType<NUnit3SingleResultLoader>();
            builder.RegisterType<XUnit1Results>();
            builder.RegisterType<XUnit1SingleResultLoader>();
            builder.RegisterType<XUnit1ScenarioOutlineExampleMatcher>();
            builder.RegisterType<XUnit2Results>();
            builder.RegisterType<XUnit2SingleResultLoader>();
            builder.RegisterType<XUnit2ScenarioOutlineExampleMatcher>();
            builder.RegisterType<MsTestResults>();
            builder.RegisterType<MsTestSingleResultLoader>();
            builder.RegisterType<MsTestScenarioOutlineExampleMatcher>();
            builder.RegisterType<CucumberJsonResults>();
            builder.RegisterType<CucumberJsonSingleResultLoader>();
            builder.RegisterType<SpecRunResults>();
            builder.RegisterType<SpecRunSingleResultLoader>();
            builder.RegisterType<SpecRunScenarioOutlineExampleMatcher>();
            builder.RegisterType<VsTestResults>();
            builder.RegisterType<VsTestSingleResultLoader>();
            builder.RegisterType<VsTestScenarioOutlineExampleMatcher>();

            builder.Register<ITestResults>(c =>
            {
                var configuration = c.Resolve<IConfiguration>();
                if (!configuration.HasTestResults)
                {
                    return c.Resolve<NullTestResults>();
                }

                switch (configuration.TestResultsFormat)
                {
                    case TestResultsFormat.NUnit:
                        return c.Resolve<NUnit2Results>();
                    case TestResultsFormat.NUnit3:
                        return c.Resolve<NUnit3Results>();
                    case TestResultsFormat.XUnit:
                    case TestResultsFormat.XUnit1:
                        return c.Resolve<XUnit1Results>();
                    case TestResultsFormat.xUnit2:
                        return c.Resolve<XUnit2Results>();
                    case TestResultsFormat.MsTest:
                        return c.Resolve<MsTestResults>();
                    case TestResultsFormat.CucumberJson:
                        return c.Resolve<CucumberJsonResults>();
                    case TestResultsFormat.SpecRun:
                        return c.Resolve<SpecRunResults>();
                    case TestResultsFormat.VsTest:
                        return c.Resolve<VsTestResults>();
                    default:
                        return c.Resolve<NullTestResults>();
                }
            }).SingleInstance();

            builder.RegisterType<LanguageServices>().UsingConstructor(typeof(IConfiguration)).SingleInstance();

            builder.RegisterType<HtmlMarkdownFormatter>().SingleInstance();
            builder.RegisterType<HtmlResourceWriter>().SingleInstance();
            builder.RegisterType<HtmlTableOfContentsFormatter>().SingleInstance();
            builder.RegisterType<HtmlFooterFormatter>().SingleInstance();
            builder.RegisterType<HtmlDocumentFormatter>().SingleInstance();
            builder.RegisterType<HtmlFeatureFormatter>().As<IHtmlFeatureFormatter>().SingleInstance();

            builder.RegisterType<StrikeMarkdownProvider>();
            builder.RegisterType<MarkdownProvider>();

            builder.Register(
                c => FeatureSwitcher.Feature<AlternateMarkdownProvider>.Is().Enabled
                    ? (IMarkdownProvider)c.Resolve<StrikeMarkdownProvider>()
                    : c.Resolve<MarkdownProvider>()).SingleInstance();
        }
    }
}
