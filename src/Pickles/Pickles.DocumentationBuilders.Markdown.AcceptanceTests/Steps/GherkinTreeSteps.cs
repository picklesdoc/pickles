//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GherkinTreeSteps.cs" company="PicklesDoc">
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

using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.Test;
using System;
using TechTalk.SpecFlow;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.AcceptanceTests.Steps
{
    [Binding]
    public sealed class GherkinTreeSteps : BaseFixture
    {
        [Given(@"I have a feature called '(.*)'")]
        public void GivenIHaveAFeatureCalled(string p0)
        {
            var newFeature = new Feature();
            newFeature.Name = p0;

            var relPath = "fakedir";
            var location = FileSystem.FileInfo.FromFileName(@"c:\");

            INode newNode = new FeatureNode(location, relPath, newFeature);

            Tree featureTree = null;

            if (ScenarioContext.Current.ContainsKey("Feature Tree"))
            {
                featureTree = (Tree)ScenarioContext.Current["Feature Tree"];
            }
            else
            {
                INode rootNode = new FolderNode(location, relPath);
                featureTree = new Tree(rootNode);
            }


            featureTree.Add(newNode);

            ScenarioContext.Current["Feature Tree"] = featureTree;
        }

        [Given(@"I have the description")]
        public void GivenIHaveTheDescription(TechTalk.SpecFlow.Table table)
        {
            Tree featureTree = null;

            try
            {
                featureTree = (Tree)ScenarioContext.Current["Feature Tree"];
            }
            catch (Exception e)
            {
                throw new SpecFlowException("Failed to load the Feature Tree context, ensure you called the step \"I have a feature called '<p0>'\" first", e);
            }

            var lastNodeIndexLocation = featureTree.ChildNodes.Count - 1;
            foreach (var row in table.Rows)
            {
                ((FeatureNode)featureTree.ChildNodes[lastNodeIndexLocation].Data).Feature.Description = string.Concat(
                    ((FeatureNode)featureTree.ChildNodes[lastNodeIndexLocation].Data).Feature.Description,
                    row["Description"],
                    Environment.NewLine
                    );
            }

            ScenarioContext.Current["Feature Tree"] = featureTree;
        }
    }
}
