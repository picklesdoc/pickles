//  --------------------------------------------------------------------------------------------------------------------ScenarioBlock_Tests
//  <copyright file="StepBlock_Tests.cs" company="PicklesDoc">
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

using NUnit.Framework;
using PicklesDoc.Pickles.DocumentationBuilders.Markdown.Blocks;
using PicklesDoc.Pickles.ObjectModel;
using System;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.UnitTests
{
    [TestFixture]
    public class StepBlock_Tests
    {
        [Test]
        public void The_Step_Is_Formatted()
        {
            var mockStyle = new MockStylist
            {
                StepFormat = "Keyword: {0} Step: {1}"
            };
            var step = new Step() { NativeKeyword = "Natkey ", Name = "I am a step" };

            var stepBlock = new StepBlock(step, mockStyle);
            var actualString = stepBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.AreEqual("Keyword: Natkey Step: I am a step", actualString[0]);
            Assert.AreEqual(2, actualString.Length);
        }

        [Test]
        public void A_Step_Table_Is_Formatted()
        {
            var mockStyle = new MockStylist
            {
                StepFormat = "Keyword: {0} Step: {1}"
            };
            var step = new Step() { NativeKeyword = "Natkey ", Name = "I am a step" };

            var stepTable = new Table();
            stepTable.HeaderRow = new TableRow(new[] { "Col1", "Col2" });
            stepTable.DataRows = new System.Collections.Generic.List<ObjectModel.TableRow>();
            stepTable.DataRows.Add(new TableRow(new[] { "Col1Row1", "Col2Row1" }));
            stepTable.DataRows.Add(new TableRow(new[] { "Col1Row2", "Col2Row2" }));
            step.TableArgument = stepTable;

            var stepBlock = new StepBlock(step, mockStyle);
            var actualString = stepBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.AreEqual("Keyword: Natkey Step: I am a step", actualString[0]);
            Assert.AreEqual(">", actualString[1]);
            Assert.AreEqual("> | Col1 | Col2 |", actualString[2]);
            Assert.AreEqual("> | --- | --- |", actualString[3]);
            Assert.AreEqual("> | Col1Row1 | Col2Row1 |", actualString[4]);
            Assert.AreEqual("> | Col1Row2 | Col2Row2 |", actualString[5]);
            Assert.AreEqual(7, actualString.Length);
        }
    }
}
