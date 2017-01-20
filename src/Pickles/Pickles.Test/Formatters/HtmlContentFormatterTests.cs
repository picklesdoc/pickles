//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlContentFormatterTests.cs" company="PicklesDoc">
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
using Moq;
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders.Html;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test.Formatters
{
    [TestFixture]
    public class HtmlContentFormatterTests : BaseFixture
    {
        [Test]
        public void Constructor_NullHtmlFeatureFormatter_ThrowsArgumentNullException()
        {
            Check.ThatCode(() => new HtmlContentFormatter(null, null))
                .Throws<ArgumentNullException>()
                .WithProperty("ParamName", "htmlFeatureFormatter");
        }

        [Test]
        public void Constructor_NullHtmlIndexFormatter_ThrowsArgumentNullException()
        {
            Check.ThatCode(() => new HtmlContentFormatter(new Mock<IHtmlFeatureFormatter>().Object, null))
                .Throws<ArgumentNullException>()
                .WithProperty("ParamName", "htmlIndexFormatter");
        }

        [Test]
        public void Format_ContentIsFeatureNode_UsesHtmlFeatureFormatterWithCorrectArgument()
        {
            var fakeHtmlFeatureFormatter = new Mock<IHtmlFeatureFormatter>();
            var formatter = new HtmlContentFormatter(fakeHtmlFeatureFormatter.Object, new HtmlIndexFormatter());

            var featureNode = new FeatureNode(
                FileSystem.FileInfo.FromFileName(@"c:\temp\test.feature"),
                ".",
                new Feature());

            formatter.Format(featureNode, new INode[0]);

            fakeHtmlFeatureFormatter.Verify(f => f.Format(featureNode.Feature));
        }
    }
}
