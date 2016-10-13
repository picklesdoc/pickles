//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TableOfContentsShouldBeCreatedFromAFolderStructure.cs" company="PicklesDoc">
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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Autofac;
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders.Html;
using PicklesDoc.Pickles.Test.Helpers;

namespace PicklesDoc.Pickles.Test.Formatters
{
    [TestFixture]
    public class TableOfContentsShouldBeCreatedFromAFolderStructure : BaseFixture
    {
        private const string RootPath = FileSystemPrefix + @"FeatureCrawlerTests";
        private XElement toc;

        public void Setup()
        {
            this.AddFakeFolderStructures();

            Tree features = Container.Resolve<DirectoryTreeCrawler>().Crawl(RootPath);

            var formatter = new HtmlTableOfContentsFormatter(null, this.FileSystem);
            this.toc = formatter.Format(
                features.ChildNodes[0].Data.OriginalLocationUrl,
                features,
                FileSystem.DirectoryInfo.FromDirectoryName(RootPath));
        }

        [Test]
        public void Can_crawl_directory_tree_for_features_successfully()
        {
            this.Setup();

            XElement ul = this.toc.FindFirstDescendantWithName("ul");
            XElement ul2 = ul.FindFirstDescendantWithName("ul");
            Check.That(ul2.HasElements).IsTrue();

            // Assert that a feature file is appropriately set deeper down in the TOC
            XElement li2 = ul2.FindFirstDescendantWithName("li");
            Check.That(li2).IsNotNull();

            XElement anchorInLI2 = li2.Elements().First();
            Check.That(anchorInLI2.HasAttributes).IsTrue();
            Check.That(anchorInLI2.Attribute("href").Value).IsEqualTo("SubLevelOne/LevelOneSublevelOne.html");
            Check.That(anchorInLI2.Value).IsEqualTo("Addition");
        }

        [Test]
        public void TableOfContent_Must_Contain_Link_To_Home()
        {
            this.Setup();

            XElement home =
                this.toc.Descendants().SingleOrDefault(
                    d => d.Attributes().Any(a => a.Name.LocalName == "id" && a.Value == "root"));

            Check.That(home).IsNotNull();
        }

        [Test]
        public void TableOfContent_Must_Contain_One_Paragraph_With_Current_Class()
        {
            this.Setup();

            XElement span =
                this.toc.Descendants().Where(e => e.Name.LocalName == "span").SingleOrDefault(
                    e => e.Attributes().Any(a => a.Name.LocalName == "class" && a.Value == "current"));

            Check.That(span).IsNotNull();
        }

        [Test]
        public void TableOfContent_Must_Link_Folder_Nodes_To_That_Folders_Index_File()
        {
            this.Setup();

            XElement directory =
                this.toc.Descendants().First(
                    d =>
                        d.Name.LocalName == "div" &&
                        d.Attributes().Any(a => a.Name.LocalName == "class" && a.Value == "directory"));
            XElement link = directory.Descendants().First();

            Check.That(link.Name.LocalName).IsEqualTo("a");
            XAttribute href = link.Attributes().Single(a => a.Name.LocalName == "href");
            Check.That(href.Value).IsEqualTo("SubLevelOne/index.html");
        }

        [Test]
        public void Ul_Element_Must_Contain_Li_Children_Only()
        {
            this.Setup();

            IEnumerable<XElement> childrenOfUl = this.toc.Elements().First().Elements();

            int numberOfChildren = childrenOfUl.Count();
            int numberOfLiChildren = childrenOfUl.Count(e => e.Name.LocalName == "li");

            Check.That(numberOfLiChildren).IsEqualTo(numberOfChildren);
        }

        [Test]
        public void FirstUlNodeShouldBeIndex()
        {
            this.Setup();

            XElement ul = this.toc.FindFirstDescendantWithName("ul");

            // Assert that the first feature is appropriately set in the TOC
            Check.That(ul).IsNotNull();
            Check.That(ul.HasElements).IsTrue();

            XElement li1 = ul.Descendants().FirstOrDefault(d => d.Name.LocalName == "li");
            Check.That(li1).IsNotNull();

            XElement anchorInLI1 = li1.Elements().First();
            Check.That(anchorInLI1.HasAttributes).IsTrue();
            Check.That(anchorInLI1.Attribute("class").Value).IsEqualTo("current");
            Check.That(anchorInLI1.Value).IsEqualTo("Home");
        }

        [Test]
        public void TocShouldBeSetWithCorrectAttributes()
        {
            this.Setup();

            Check.That(this.toc).IsNotNull();
            Check.That(this.toc.Attributes("id").First().Value).IsEqualTo("toc");
        }
    }
}
