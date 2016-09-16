//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WordDocumentationBuilder.cs" company="PicklesDoc">
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
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using NLog;
using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders.Word.TableOfContentsAdder;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word
{
    public class WordDocumentationBuilder : IDocumentationBuilder
    {
        private static readonly Logger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        private readonly IConfiguration configuration;
        private readonly WordFeatureFormatter wordFeatureFormatter;
        private readonly WordFontApplicator wordFontApplicator;
        private readonly WordHeaderFooterFormatter wordHeaderFooterFormatter;

        private readonly IFileSystem fileSystem;

        private readonly WordStyleApplicator wordStyleApplicator;

        public WordDocumentationBuilder(
            IConfiguration configuration,
            WordFeatureFormatter wordFeatureFormatter,
            WordStyleApplicator wordStyleApplicator,
            WordFontApplicator wordFontApplicator,
            WordHeaderFooterFormatter wordHeaderFooterFormatter,
            IFileSystem fileSystem)
        {
            this.configuration = configuration;
            this.wordFeatureFormatter = wordFeatureFormatter;
            this.wordStyleApplicator = wordStyleApplicator;
            this.wordFontApplicator = wordFontApplicator;
            this.wordHeaderFooterFormatter = wordHeaderFooterFormatter;
            this.fileSystem = fileSystem;
        }

        public void Build(Tree features)
        {
            string filename = string.IsNullOrEmpty(this.configuration.SystemUnderTestName)
                ? "features.docx"
                : this.configuration.SystemUnderTestName + ".docx";
            string documentFileName = this.fileSystem.Path.Combine(this.configuration.OutputFolder.FullName, filename);
            if (this.fileSystem.File.Exists(documentFileName))
            {
                try
                {
                    this.fileSystem.File.Delete(documentFileName);
                }
                catch (System.IO.IOException ex)
                {
                    Log.Error("Cannot delete Word file. Is it still open in Word?", ex);
                    return;
                }
            }

            using (
                WordprocessingDocument wordProcessingDocument = WordprocessingDocument.Create(
                    documentFileName,
                    WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainDocumentPart = wordProcessingDocument.AddMainDocumentPart();
                this.wordStyleApplicator.AddStylesPartToPackage(wordProcessingDocument);
                this.wordStyleApplicator.AddStylesWithEffectsPartToPackage(wordProcessingDocument);
                this.wordFontApplicator.AddFontTablePartToPackage(wordProcessingDocument);
                var documentSettingsPart = mainDocumentPart.AddNewPart<DocumentSettingsPart>();
                documentSettingsPart.Settings = new Settings();
                this.wordHeaderFooterFormatter.ApplyHeaderAndFooter(wordProcessingDocument);

                var document = new Document();
                var body = new Body();
                document.Append(body);

                foreach (var node in features)
                {
                    var featureDirectoryTreeNode =
                        node as FeatureNode;
                    if (featureDirectoryTreeNode != null)
                    {
                        this.wordFeatureFormatter.Format(body, featureDirectoryTreeNode);
                    }
                }

                mainDocumentPart.Document = document;
                mainDocumentPart.Document.Save();
            }

            // HACK - Add the table of contents
            using (WordprocessingDocument wordProcessingDocument = WordprocessingDocument.Open(documentFileName, true))
            {
                XElement firstPara = wordProcessingDocument
                    .MainDocumentPart
                    .GetXDocument()
                    .Descendants(W.p)
                    .FirstOrDefault();

                TocAdder.AddToc(wordProcessingDocument, firstPara, @"TOC \o '1-2' \h \z \u", null, 4);
            }
        }
    }
}
