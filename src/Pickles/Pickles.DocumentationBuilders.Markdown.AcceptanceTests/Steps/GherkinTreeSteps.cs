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
            var lastFeature = TryToGetLastFeature();

            var featureTree = TryToGetTheTree();

            var lastNodeIndexLocation = featureTree.ChildNodes.Count - 1;
            foreach (var row in table.Rows)
            {
                lastFeature.Description = string.Concat(
                    lastFeature.Description,
                    row["Description"],
                    Environment.NewLine
                    );
            }
        }

        [Given(@"I have a scenario called '(.*)'")]
        public void GivenIHaveAScenarioCalled(string scenarioName)
        {
            var lastFeature = TryToGetLastFeature();

            var scenario = new Scenario
            {
                Name = scenarioName
            };

            lastFeature.AddFeatureElement(scenario);
        }

        [Given(@"I have the tags")]
        [Given(@"I have the feature tags")]
        public void GivenIHaveTheTags(TechTalk.SpecFlow.Table table)
        {
            var lastFeature = TryToGetLastFeature();

            foreach (var row in table.Rows)
            {
                lastFeature.Tags.Add(
                    row["Tag"]
                    );
            }
        }

        [Given(@"I have the scenario tags")]
        public void GivenIHaveTheScenarioTags(TechTalk.SpecFlow.Table table)
        {
            var lastScenario = TryToGetLastScenario();

            foreach (var row in table.Rows)
            {
                lastScenario.Tags.Add(
                    row["Tag"]
                    );
            }
        }

        [Given(@"I have the scenario steps")]
        public void GivenIHaveTheScenarioSteps(TechTalk.SpecFlow.Table table)
        {
            var lastScenario = TryToGetLastScenario();

            foreach (var row in table.Rows)
            {
                var step = new Step()
                {
                    NativeKeyword = row["Keyword"],
                    Name = row["step"]
                };
                lastScenario.Steps.Add(step);
            }
        }

        [Given(@"I have the scenario step with table '(Given )(.*)'")]
        public void Given_I_Have_The_Scenario_Step_With_Table(string keyword, string stepName, TechTalk.SpecFlow.Table table)
        {
            var lastScenario = TryToGetLastScenario();

            var step = new Step()
            {
                NativeKeyword = keyword,
                Name = stepName
            };
            
            var stepTable = new ObjectModel.Table();

            stepTable.HeaderRow = new ObjectModel.TableRow(table.Header);
            stepTable.DataRows = new System.Collections.Generic.List<ObjectModel.TableRow>();
            foreach (var row in table.Rows)
            {
                stepTable.DataRows.Add(new ObjectModel.TableRow(row.Values));
            }

            step.TableArgument = stepTable;

            lastScenario.Steps.Add(step);
        }

        private Scenario TryToGetLastScenario()
        {
            var lastFeature = TryToGetLastFeature();

            var lastScenario = lastFeature.FeatureElements[lastFeature.FeatureElements.Count - 1];

            return lastScenario as Scenario;
        }
        private Feature TryToGetLastFeature()
        {
            var featureTree = TryToGetTheTree();

            var lastNodeIndexLocation = featureTree.ChildNodes.Count - 1;

            var lastFeature = ((FeatureNode)featureTree.ChildNodes[lastNodeIndexLocation].Data).Feature;

            return lastFeature;
        }

        private Tree TryToGetTheTree()
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

            return featureTree;
        }
    }
}
