//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenCrawlingFoldersForFeatures.cs" company="PicklesDoc">
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
using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    public class WhenCrawlingFoldersForFeatures : BaseFixture
    {
        [Test]
        public void Then_can_crawl_all_folders_including_subfolders_for_features_successfully()
        {
            this.AddFakeFolderStructures();

            var rootPath = FileSystem.DirectoryInfo.FromDirectoryName(FileSystemPrefix + @"FeatureCrawlerTests");
            Tree features = Container.Resolve<DirectoryTreeCrawler>().Crawl(rootPath, new ParsingReport());

            Check.That(features).IsNotNull();

            INode indexMd = features.ChildNodes[0].Data;
            Check.That(indexMd).IsNotNull();
            Check.That(indexMd.Name).IsEqualTo("This is an index written in Markdown");
            Check.That(indexMd.RelativePathFromRoot).IsEqualTo("index.md");
            Check.That(indexMd).IsInstanceOf<MarkdownNode>();

            INode levelOneFeature = features.ChildNodes[1].Data;
            Check.That(levelOneFeature).IsNotNull();
            Check.That(levelOneFeature.Name).IsEqualTo("Addition");
            Check.That(levelOneFeature.RelativePathFromRoot).IsEqualTo("LevelOne.feature");
            Check.That(levelOneFeature).IsInstanceOf<FeatureNode>();

            INode image = features.ChildNodes[2].Data;
            Check.That(image).IsNotNull();
            Check.That(image.Name).IsEqualTo("image.png");
            Check.That(image.RelativePathFromRoot).IsEqualTo("image.png");
            Check.That(image).IsInstanceOf<ImageNode>();

            INode levelOneRemoveTagsToHide = features.ChildNodes[3].Data;
            Check.That(levelOneRemoveTagsToHide).IsNotNull();
            Check.That(levelOneRemoveTagsToHide.Name).IsEqualTo("LevelOneRemoveTagsToHide");
            Check.That(levelOneRemoveTagsToHide.RelativePathFromRoot).IsEqualTo("LevelOneRemoveTagsToHide.feature");
            Check.That(levelOneRemoveTagsToHide).IsInstanceOf<FeatureNode>();

            INode subLevelOneDirectory = features.ChildNodes[4].Data;
            Check.That(subLevelOneDirectory).IsNotNull();
            Check.That(subLevelOneDirectory.Name).IsEqualTo("Sub Level One");
            Check.That(subLevelOneDirectory.RelativePathFromRoot).IsEqualTo(@"SubLevelOne\");
            Check.That(subLevelOneDirectory).IsInstanceOf<FolderNode>();

            Tree subLevelOneNode = features.ChildNodes[4];
            Check.That(subLevelOneNode.ChildNodes.Count).IsEqualTo(3);

            INode levelOneSublevelOneFeature = subLevelOneNode.ChildNodes[0].Data;
            Check.That(levelOneSublevelOneFeature).IsNotNull();
            Check.That(levelOneSublevelOneFeature.Name).IsEqualTo("Addition");
            Check.That(levelOneSublevelOneFeature.RelativePathFromRoot).IsEqualTo(@"SubLevelOne\LevelOneSublevelOne.feature");
            Check.That(levelOneSublevelOneFeature).IsInstanceOf<FeatureNode>();

            INode levelOneSublevelTwoFeature = subLevelOneNode.ChildNodes[1].Data;
            Check.That(levelOneSublevelTwoFeature).IsNotNull();
            Check.That(levelOneSublevelTwoFeature.Name).IsEqualTo("Addition");
            Check.That(levelOneSublevelTwoFeature.RelativePathFromRoot).IsEqualTo(@"SubLevelOne\LevelOneSublevelTwo.feature");
            Check.That(levelOneSublevelTwoFeature).IsInstanceOf<FeatureNode>();

            Tree subLevelTwoNode = subLevelOneNode.ChildNodes[2];
            Check.That(subLevelTwoNode.ChildNodes.Count).IsEqualTo(1);

            INode subLevelTwoDirectory = subLevelOneNode.ChildNodes[2].Data;
            Check.That(subLevelTwoDirectory).IsNotNull();
            Check.That(subLevelTwoDirectory.Name).IsEqualTo("Sub Level Two");
            Check.That(subLevelTwoDirectory.RelativePathFromRoot).IsEqualTo(@"SubLevelOne\SubLevelTwo\");
            Check.That(subLevelTwoDirectory).IsInstanceOf<FolderNode>();

            INode levelOneSublevelOneSubLevelTwoDirectory = subLevelOneNode.ChildNodes[2].ChildNodes[0].Data;
            Check.That(levelOneSublevelOneSubLevelTwoDirectory).IsNotNull();
            Check.That(levelOneSublevelOneSubLevelTwoDirectory.Name).IsEqualTo("Addition");
            Check.That(levelOneSublevelOneSubLevelTwoDirectory.RelativePathFromRoot).IsEqualTo(@"SubLevelOne\SubLevelTwo\LevelOneSublevelOneSubLevelTwo.feature");
            Check.That(levelOneSublevelOneSubLevelTwoDirectory).IsInstanceOf<FeatureNode>();
        }
    }
}
