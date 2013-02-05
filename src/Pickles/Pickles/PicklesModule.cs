#region License

/*
    Copyright [2011] [Jeffrey Cameron]

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

#endregion

using System;
using Autofac;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders;
using PicklesDoc.Pickles.DocumentationBuilders.DHTML;
using PicklesDoc.Pickles.DocumentationBuilders.DITA;
using PicklesDoc.Pickles.DocumentationBuilders.Excel;
using PicklesDoc.Pickles.DocumentationBuilders.HTML;
using PicklesDoc.Pickles.DocumentationBuilders.JSON;
using PicklesDoc.Pickles.DocumentationBuilders.Word;
using PicklesDoc.Pickles.TestFrameworks;

namespace PicklesDoc.Pickles
{
    public class PicklesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Configuration>().SingleInstance();
            builder.RegisterType<DirectoryTreeCrawler>().SingleInstance();
            builder.RegisterType<FeatureParser>().SingleInstance();
            builder.RegisterType<RelevantFileDetector>().SingleInstance();
            builder.RegisterType<FeatureNodeFactory>().SingleInstance();

            builder.RegisterType<HtmlDocumentationBuilder>().SingleInstance();
            builder.RegisterType<WordDocumentationBuilder>().SingleInstance();
            builder.RegisterType<DitaDocumentationBuilder>().SingleInstance();
            builder.RegisterType<JSONDocumentationBuilder>().SingleInstance();
            builder.RegisterType<ExcelDocumentationBuilder>().SingleInstance();
            builder.RegisterType<DhtmlDocumentationBuilder>().SingleInstance();

            builder.Register<IDocumentationBuilder>(c =>
            {
                var configuration = c.Resolve<Configuration>();
                switch (configuration.DocumentationFormat)
                {
                    case DocumentationFormat.Html: return c.Resolve<HtmlDocumentationBuilder>();
                    case DocumentationFormat.Word: return c.Resolve<WordDocumentationBuilder>();
                    case DocumentationFormat.Dita: return c.Resolve<DitaDocumentationBuilder>();
                    case DocumentationFormat.JSON: return c.Resolve<JSONDocumentationBuilder>();
                    case DocumentationFormat.Excel: return c.Resolve<ExcelDocumentationBuilder>();
                    case DocumentationFormat.DHtml: return c.Resolve<DhtmlDocumentationBuilder>();
                    default: return c.Resolve<HtmlDocumentationBuilder>();
                }
            }).SingleInstance();

            builder.Register<ITestResults>(c =>
                {
                    var configuration = c.Resolve<Configuration>();
                    if (!configuration.HasTestResults) return c.Resolve<NullTestResults>();

                    switch (configuration.TestResultsFormat)
                    {
                        case TestResultsFormat.NUnit: return c.Resolve<NUnitResults>();
                        case TestResultsFormat.xUnit: return c.Resolve<XUnitResults>();
                        case TestResultsFormat.MsTest: return c.Resolve<MsTestResults>();
                        default: return c.Resolve<NullTestResults>();
                    }
                }).SingleInstance();

            builder.RegisterType<LanguageServices>().SingleInstance();

            builder.RegisterType<HtmlMarkdownFormatter>().SingleInstance();
            builder.RegisterType<HtmlResourceWriter>().SingleInstance();
            builder.RegisterType<HtmlTableOfContentsFormatter>().SingleInstance();
            builder.RegisterType<HtmlFooterFormatter>().SingleInstance();
            builder.RegisterType<HtmlDocumentFormatter>().SingleInstance();
            builder.RegisterType<HtmlFeatureFormatter>().As<IHtmlFeatureFormatter>().SingleInstance();
            builder.RegisterType<MarkdownProvider>().SingleInstance();
        }
    }
}
