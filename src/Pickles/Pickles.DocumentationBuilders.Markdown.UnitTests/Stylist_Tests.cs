//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Stylist_Tests.cs" company="PicklesDoc">
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
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.UnitTests
{
    [TestFixture]
    public class Stylist_Tests
    {
        [Test]
        public void A_Stylist_Can_Style_A_Title()
        { 
            var expectedStyle = "# Title";
            var style = new Stylist();

            var actualStyle = style.AsTitle("Title");

            Assert.AreEqual(expectedStyle, actualStyle);
        }

        [Test]
        public void A_Stylist_Can_Style_A_Feature_Heading()
        {
            var expectedStyle = "### My Styled Feature";
            var style = new Stylist();

            var actualStyle = style.AsFeatureHeading("My Styled Feature");

            Assert.AreEqual(expectedStyle, actualStyle);
        }

        [Test]
        public void A_Stylist_Can_Style_A_Background_Heading()
        {
            var expectedStyle = "#### Background: My Styled Background";
            var style = new Stylist();

            var actualStyle = style.AsBackgroundHeading("My Styled Background");

            Assert.AreEqual(expectedStyle, actualStyle);
        }

        [Test]
        public void A_Stylist_Can_Style_A_Background_Heading_Without_A_Name()
        {
            var expectedStyle = "#### Background:";
            var style = new Stylist();

            var actualStyle = style.AsBackgroundHeading(null);

            Assert.AreEqual(expectedStyle, actualStyle);
        }

        [Test]
        public void A_Stylist_Can_Style_A_Scenario_Heading()
        {
            var expectedStyle = "#### Scenario: My Styled Scenario";
            var style = new Stylist();

            var actualStyle = style.AsScenarioHeading("My Styled Scenario");

            Assert.AreEqual(expectedStyle, actualStyle);
        }

        [Test]
        public void A_Stylist_Can_Style_A_Scenario_Heading_With_Result()
        {
            var expectedStyle = "#### Scenario: ![Passed](pass.png) My Styled Scenario";
            var style = new Stylist();

            var actualStyle = style.AsScenarioHeading("My Styled Scenario", TestResult.Passed);

            Assert.AreEqual(expectedStyle, actualStyle);
        }

        [Test]
        public void A_Stylist_Can_Style_A_ScenarioOutline_Heading()
        {
            var expectedStyle = "#### Scenario Outline: My Styled Scenario Outline";
            var style = new Stylist();

            var actualStyle = style.AsScenarioOutlineHeading("My Styled Scenario Outline");

            Assert.AreEqual(expectedStyle, actualStyle);
        }

        [Test]
        public void A_Stylist_Can_Style_An_Example_Heading()
        {
            var expectedStyle = "> Examples: My Styled Example";
            var style = new Stylist();

            var actualStyle = style.AsExampleHeading("My Styled Example");

            Assert.AreEqual(expectedStyle, actualStyle);
        }

        [Test]
        public void A_Stylist_Can_Style_An_Example_Heading_Without_A_Name()
        {
            var expectedStyle = "> Examples:";
            var style = new Stylist();

            var actualStyle = style.AsExampleHeading(string.Empty);

            Assert.AreEqual(expectedStyle, actualStyle);
        }

        [Test]
        public void A_Stylist_Can_Style_A_Tag()
        {
            var expectedStyle = "*`@Tag`*";
            var style = new Stylist();

            var actualStyle = style.AsTag("@Tag");

            Assert.AreEqual(expectedStyle, actualStyle);
        }

        [Test]
        public void A_Stylist_Can_Style_A_Step()
        {
            var expectedStyle = "> **Keyword** Step";
            var style = new Stylist();

            var actualStyle = style.AsStep("Keyword", "Step");

            Assert.AreEqual(expectedStyle, actualStyle);
        }

        [Test]
        public void A_Stylist_Can_Style_A_Step_With_Angle_Brakets()
        {
            var expectedStyle = @"> **Keyword** Step \<placeholder\>";
            var style = new Stylist();

            var actualStyle = style.AsStep("Keyword", "Step <placeholder>");

            Assert.AreEqual(expectedStyle, actualStyle);
        }

        [Test]
        public void A_Stylist_Can_Style_A_Step_Table()
        {
            var expectedStyle = "> | HeadingOne | HeadingTwo |";
            var style = new Stylist();

            var actualStyle = style.AsStepTable("{0}HeadingOne{0}HeadingTwo{0}");

            Assert.AreEqual(expectedStyle, actualStyle);
        }

        [Test]
        public void A_Stylist_Can_Style_A_StepLine()
        {
            var style = new Stylist();

            Assert.AreEqual(">", style.AsStepLine(string.Empty));
            Assert.AreEqual("> Text", style.AsStepLine("Text"));
        }

        [Test]
        public void A_Stylist_Can_Style_A_Result()
        {
            var style = new Stylist();

            Assert.AreEqual("![Passed](pass.png)", style.AsResult(TestResult.Passed));
            Assert.AreEqual("![Failed](fail.png)", style.AsResult(TestResult.Failed));
            Assert.AreEqual("![Inconclusive](inconclusive.png)", style.AsResult(TestResult.Inconclusive));
        }

        [Test]
        public void A_Stylist_Can_Style_A_Table_Result_Heading()
        {
            var style = new Stylist();

            Assert.AreEqual("Result", style.TableResultHeading);
        }
    }
}
