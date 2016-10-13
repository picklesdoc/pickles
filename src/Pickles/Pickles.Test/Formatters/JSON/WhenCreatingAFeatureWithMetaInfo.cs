//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenCreatingAFeatureWithMetaInfo.cs" company="PicklesDoc">
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
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders.Json;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test.Formatters.JSON
{
    public class WhenCreatingAFeatureWithMetaInfo : BaseFixture
    {
        private const string RelativePath = @"AcceptanceTest";
        private const string RootPath = FileSystemPrefix + @"AcceptanceTest";
        private const string FeaturePath = @"AdvancedFeature.feature";

        private FeatureNode featureDirectoryNode;
        private FileInfoBase featureFileInfo;
        private JsonFeatureWithMetaInfo featureWithMeta;
        private Feature testFeature;

        public void Setup()
        {
            this.testFeature = new Feature { Name = "Test" };
            this.featureFileInfo = this.FileSystem.FileInfo.FromFileName(FileSystem.Path.Combine(RootPath, FeaturePath));
            this.featureDirectoryNode = new FeatureNode(this.featureFileInfo, RelativePath, this.testFeature);

            this.featureWithMeta = new JsonFeatureWithMetaInfo(this.featureDirectoryNode);
        }

        [Test]
        public void ItShouldContainTheFeature()
        {
            this.Setup();

            Check.That(this.featureWithMeta.Feature).IsNotNull();
            Check.That(this.featureWithMeta.Feature.Name).IsEqualTo("Test");
        }

        [Test]
        public void ItShouldContainTheRelativePath()
        {
            this.Setup();

            Check.That(this.featureWithMeta.RelativeFolder).IsEqualTo(RelativePath);
        }
    }
}
