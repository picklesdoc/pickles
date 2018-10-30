//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MarkdownDocumentationBuilder_Tests.cs" company="PicklesDoc">
//  Copyright 2018 Darren Comeau
//  Copyright 2018-present PicklesDoc team and community contributors
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
using NUnit.Framework;
using PicklesDoc.Pickles;
using PicklesDoc.Pickles.DocumentationBuilders;
using PicklesDoc.Pickles.DocumentationBuilders.Markdown;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Reflection;

namespace Pickles.DocumentationBuilders.Markdown.UnitTests
{
    [TestFixture]
    public class MarkdownDocumentationBuilder_Tests
    {
        [Test]
        public void New_MarkdownDocumentationBuilder_Is_An_Implementation_Of_IDocumentationBuilder()
        {
            var documentationBuilder = new MarkdownDocumentationBuilder(null);

            Assert.IsInstanceOf<IDocumentationBuilder>(documentationBuilder);
        }

        [Test]
        public void Autofac_Can_Resolve_MarkdownDocumentationBuilder_Request()
        {
            var container = BuildContainer();

            var markdownDocumentationBuilder = container.Resolve<MarkdownDocumentationBuilder>();

            Assert.IsNotNull(markdownDocumentationBuilder);
        }

        [Test]
        public void When_I_Build_Documentation_A_File_Is_Created()
        {
            var defaultOutputFile = @"c:\output\pickledFeatures.md";

            var container = BuildContainer();
            var markdownDocumentationBuilder = container.Resolve<MarkdownDocumentationBuilder>();
            var fileSystem = (MockFileSystem)container.Resolve<IFileSystem>();

            markdownDocumentationBuilder.Build(null);

            Assert.IsTrue(fileSystem.FileExists(defaultOutputFile));
        }

        [Test]
        public void With_A_Null_Tree_The_Output_Has_Default_Content()
        {
            var defaultOutputFile = @"c:\output\pickledFeatures.md";
            var expectedFile = new string[]
            {
                "# Features",
                "",
                "Generated on: 25 October 2018 at 18:53:00"
            };

            var container = BuildContainer();
            var markdownDocumentationBuilder = container.Resolve<MarkdownDocumentationBuilder>();
            var fileSystem = (MockFileSystem)container.Resolve<IFileSystem>();

            markdownDocumentationBuilder.Build(null);

            var actualfile = fileSystem.File.ReadAllLines(defaultOutputFile);

            Assert.AreEqual(expectedFile, actualfile);
        }

        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<PicklesModule>();
            builder.Register<MockFileSystem>(_ => CreateMockFileSystem()).As<IFileSystem>().SingleInstance();

            var container = builder.Build();

            return container;
        }

        private MockFileSystem CreateMockFileSystem()
        {
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.Directory.SetCurrentDirectory(Assembly.GetExecutingAssembly().Location);
            return mockFileSystem;
        }
    }
}
