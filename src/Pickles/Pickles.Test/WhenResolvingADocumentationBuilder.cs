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
using PicklesDoc.Pickles.DocumentationBuilders.Excel;
using PicklesDoc.Pickles.DocumentationBuilders.Html;
using PicklesDoc.Pickles.DocumentationBuilders.Word;

namespace PicklesDoc.Pickles.Test
{
    public class WhenResolvingADocumentationBuilder : BaseFixture
    {
        [Test]
        public void ThenCanResolveIDocumentationBuilderAsHtmlDocumentationBuilderAsSingletonIfTheUserSelectsHtmlOutput()
        {
            var configuration = this.Configuration;
            configuration.DocumentationFormat = DocumentationFormat.Html;

            var item1 = Container.Resolve<IDocumentationBuilder>();
            var item2 = Container.Resolve<IDocumentationBuilder>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOf<HtmlDocumentationBuilder>();
            Check.That(item2).IsNotNull();
            Check.That(item2).IsInstanceOf<HtmlDocumentationBuilder>();
            Check.That(item1).IsSameReferenceThan(item2);
        }

        [Test]
        public void ThenCanResolveIDocumentationBuilderAsWordDocumentationBuilderIfTheUserSelectsWordOutput()
        {
            var configuration = this.Configuration;
            configuration.DocumentationFormat = DocumentationFormat.Word;

            var item = Container.Resolve<IDocumentationBuilder>();

            Check.That(item).IsNotNull();
            Check.That(item).IsInstanceOf<WordDocumentationBuilder>();
        }

        [Test]
        public void ThenCanResolveIDocumentationBuilderAsWordDocumentationBuilderAsSingletonIfTheUserSelectsWordOutput()
        {
            var configuration = this.Configuration;
            configuration.DocumentationFormat = DocumentationFormat.Word;

            var item1 = Container.Resolve<IDocumentationBuilder>();
            var item2 = Container.Resolve<IDocumentationBuilder>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOf<WordDocumentationBuilder>();
            Check.That(item2).IsNotNull();
            Check.That(item2).IsInstanceOf<WordDocumentationBuilder>();
            Check.That(item1).IsSameReferenceThan(item2);
        }

        [Test]
        public void ThenCanResolveIDocumentationBuilderAsExcelDocumentationBuilderIfTheUserSelectsExcelOutput()
        {
            var configuration = this.Configuration;
            configuration.DocumentationFormat = DocumentationFormat.Excel;

            var item = Container.Resolve<IDocumentationBuilder>();

            Check.That(item).IsNotNull();
            Check.That(item).IsInstanceOf<ExcelDocumentationBuilder>();
        }

        [Test]
        public void ThenCanResolveIDocumentationBuilderAsExcelDocumentationBuilderAsSingletonIfTheUserSelectsExcelOutput()
        {
            var configuration = this.Configuration;
            configuration.DocumentationFormat = DocumentationFormat.Excel;

            var item1 = Container.Resolve<IDocumentationBuilder>();
            var item2 = Container.Resolve<IDocumentationBuilder>();

            Check.That(item1).IsNotNull();
            Check.That(item1).IsInstanceOf<ExcelDocumentationBuilder>();
            Check.That(item2).IsNotNull();
            Check.That(item2).IsInstanceOf<ExcelDocumentationBuilder>();
            Check.That(item1).IsSameReferenceThan(item2);
        }
    }
}
