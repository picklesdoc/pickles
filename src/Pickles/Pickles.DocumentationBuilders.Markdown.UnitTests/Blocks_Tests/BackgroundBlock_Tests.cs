//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="BackgroundBlock_Tests.cs" company="PicklesDoc">
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
    public class BackgroundBlock_Tests
    {
        [Test]
        public void A_New_BackgroundBlock_Has_Background_Heading_On_First_Line()
        {
            var expectedString = "BHF: Hello, World";
            var mockStyle = new MockStylist
            {
                BackgroundHeadingFormat = "BHF: {0}"
            };
            var scenario = new Scenario
            {
                Name = "Hello, World"
            };

            var backgroundBlock = new BackgroundBlock(scenario,mockStyle);
            var actualString = backgroundBlock.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Assert.AreEqual(expectedString, actualString[0]);
            Assert.AreEqual(2, actualString.Length);
        }
    }
}
