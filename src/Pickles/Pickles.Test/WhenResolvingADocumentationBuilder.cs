//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenResolvingADocumentationBuilder.cs" company="PicklesDoc">
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
using PicklesDoc.Pickles.DocumentationBuilders;
using PicklesDoc.Pickles.DocumentationBuilders.Dhtml;
using PicklesDoc.Pickles.DocumentationBuilders.Excel;
using PicklesDoc.Pickles.DocumentationBuilders.Html;
using PicklesDoc.Pickles.DocumentationBuilders.Json;
using PicklesDoc.Pickles.DocumentationBuilders.Word;
using PicklesDoc.Pickles.DocumentationBuilders.Cucumber;

namespace PicklesDoc.Pickles.Test
{
    public class WhenResolvingADocumentationBuilder : BaseFixture
    {
        private static readonly object[] DocumentationFormatCases =
        {
            new object[] { DocumentationFormat.Html, typeof(HtmlDocumentationBuilder) },
            new object[] { DocumentationFormat.Word, typeof(WordDocumentationBuilder) },
            new object[] { DocumentationFormat.Excel, typeof(ExcelDocumentationBuilder) },
            new object[] { DocumentationFormat.DHtml, typeof(DhtmlDocumentationBuilder) },
            new object[] { DocumentationFormat.Json, typeof(JsonDocumentationBuilder) },
            new object[] { DocumentationFormat.Cucumber, typeof(CucumberDocumentationBuilder) },
        };

        [Test]
        [TestCaseSource(nameof(DocumentationFormatCases))]
        public void ThenCanResolveTheSelectedIDocumentationBuilder(DocumentationFormat documentationFormat, Type builderType)
        {
            this.SetDocumentationFormat(documentationFormat);

            var item = Container.Resolve<IDocumentationBuilder>();

            Check.That(item).IsNotNull();
            Check.That(item).IsInstanceOfType(builderType);
        }

        [Test]
        [TestCaseSource(nameof(DocumentationFormatCases))]
        public void ThenCanResolveTheSelectedIDocumentationBuilderAsSingleton(DocumentationFormat documentationFormat, Type builderType)
        {
            this.SetDocumentationFormat(documentationFormat);

            var item1 = Container.Resolve<IDocumentationBuilder>();
            var item2 = Container.Resolve<IDocumentationBuilder>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOfType(builderType);
            Check.That(item2).IsNotNull();
            Check.That(item2).IsInstanceOfType(builderType);
            Check.That(item1).IsSameReferenceAs(item2);
        }

        private void SetDocumentationFormat(DocumentationFormat documentationFormat)
        {
            var configuration = this.Configuration;
            configuration.DocumentationFormat = documentationFormat;
        }
    }
}