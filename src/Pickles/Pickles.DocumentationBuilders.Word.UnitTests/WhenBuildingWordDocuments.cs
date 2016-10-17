//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenBuildingWordDocuments.cs" company="PicklesDoc">
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
using System.IO;
using System.Linq;

using Autofac;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using NUnit.Framework;

using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.Test;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word.UnitTests
{
    [TestFixture]
    public class WhenBuildingWordDocuments : BaseFixture
    {
        private WordDocumentationBuilder builder;
        private Tree features;
        private const string RootPath = FileSystemPrefix + @"OrderingTests";

        [SetUp]
        public void Setup()
        {
            AddFakeFolderStructures();

            Configuration.OutputFolder = this.FileSystem.DirectoryInfo.FromDirectoryName(FileSystemPrefix);
            this.features = Container.Resolve<DirectoryTreeCrawler>().Crawl(RootPath);
            this.builder = Container.Resolve<WordDocumentationBuilder>();
        }

        [Test]
        public void ShouldOutputFeaturesInParsedOrder()
        {
            string[] expectedSequence = {
                "a-a",
                "a-a-a",
                "a-a-b",
                "a-b",
                "a-b-a",
                "a-b-b",
                "b-a",
                "b-a-a",
                "b-a-b",
                "b-b",
                "b-b-a",
                "b-b-b",
            };

            this.builder.Build(this.features);

            var outputPath = Path.Combine(Configuration.OutputFolder.FullName, "features.docx");

            using (var stream = this.FileSystem.File.OpenRead(outputPath))
            {
                var doc = WordprocessingDocument.Open(stream, false);
                var body = doc.MainDocumentPart.Document.Body;
                var headings = body.Where(IsHeading).Select(InnerText);

                Assert.That(headings, Is.EquivalentTo(expectedSequence));
            }

        }

        private static string InnerText(OpenXmlElement part)
        {
            return part.InnerText;
        }

        private static bool IsHeading(OpenXmlElement part)
        {
            return part.OfType<ParagraphProperties>().Any(IsHeading);
        }

        private static bool IsHeading(ParagraphProperties property)
        {
            return property.OfType<ParagraphStyleId>().Any(IsHeading);
        }

        private static bool IsHeading(ParagraphStyleId styleId)
        {
            return styleId.Val.HasValue && styleId.Val.Value.StartsWith("Heading");
        }
    }
}
