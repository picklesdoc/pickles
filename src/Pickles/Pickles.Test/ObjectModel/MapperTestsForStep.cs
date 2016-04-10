//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapperTestsForStep.cs" company="PicklesDoc">
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
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.ObjectModel;
using G = Gherkin.Ast;

namespace PicklesDoc.Pickles.Test.ObjectModel
{
    [TestFixture]
    public class MapperTestsForStep
    {
        private readonly Factory factory = new Factory();

        [Test]
        public void MapToStep_NullStep_ReturnsNull()
        {
            var mapper = this.factory.CreateMapper();

            var result = mapper.MapToStep(null);

            Check.That(result).IsNull();
        }

        [Test]
        public void MapToStep_StepWithoutArgument_ReturnStep()
        {
            var mapper = this.factory.CreateMapper();

            G.Step step = this.factory.CreateStep("Given", "I enter '50' in the calculator");

            var result = mapper.MapToStep(step);

            Check.That(result.Keyword).IsEqualTo(Keyword.Given);
            Check.That(result.NativeKeyword).IsEqualTo("Given");
            Check.That(result.Name).IsEqualTo("I enter '50' in the calculator");
            Check.That(result.DocStringArgument).IsNull();
            Check.That(result.TableArgument).IsNull();
            Check.That(result.Location).IsNull();
            Check.That(result.Comments).IsEmpty();
        }

        [Test]
        public void MapToStep_StepWithWhen_ReturnStep()
        {
            var mapper = this.factory.CreateMapper();

            G.Step step = this.factory.CreateStep("When", "I press 'enter'");

            var result = mapper.MapToStep(step);

            Check.That(result.Keyword).IsEqualTo(Keyword.When);
            Check.That(result.NativeKeyword).IsEqualTo("When");
            Check.That(result.Name).IsEqualTo("I press 'enter'");
            Check.That(result.DocStringArgument).IsNull();
            Check.That(result.TableArgument).IsNull();
            Check.That(result.Location).IsNull();
            Check.That(result.Comments).IsEmpty();
        }

        [Test]
        public void MapToKeyWord_StringGiven_ReturnsGiven()
        {
            var mapper = this.factory.CreateMapper();

            Keyword result = mapper.MapToKeyword("Given");

            Check.That(result).IsEqualTo(Keyword.Given);
        }

        [Test]
        public void MapToKeyWord_StringThen_ReturnsThen()
        {
            var mapper = this.factory.CreateMapper();

            Keyword result = mapper.MapToKeyword("Then");

            Check.That(result).IsEqualTo(Keyword.Then);
        }

        [Test]
        public void MapToStep_StepWithDocStringArgument_ReturnStepWithDocString()
        {
            var mapper = this.factory.CreateMapper();

            G.Step step = this.factory.CreateStep("Then", "I see this value on the screen", "120");

            var result = mapper.MapToStep(step);

            Check.That(result.Keyword).IsEqualTo(Keyword.Then);
            Check.That(result.NativeKeyword).IsEqualTo("Then");
            Check.That(result.Name).IsEqualTo("I see this value on the screen");
            Check.That(result.DocStringArgument).IsEqualTo("120");
            Check.That(result.TableArgument).IsNull();
            Check.That(result.Location).IsNull();
            Check.That(result.Comments).IsEmpty();
        }

        [Test]
        public void MapToStep_StepWithTableArgument_ReturnStepWithTable()
        {
            var mapper = this.factory.CreateMapper();

            G.Step step = this.factory.CreateStep(
                "When",
                "I use this table",
                new[]
                {
                    new[] { "Header 1", "Header 2" },
                    new[] { "Value 1", "Value 2" },
                });

            var result = mapper.MapToStep(step);

            Check.That(result.Keyword).IsEqualTo(Keyword.When);
            Check.That(result.NativeKeyword).IsEqualTo("When");
            Check.That(result.Name).IsEqualTo("I use this table");
            Check.That(result.TableArgument.HeaderRow.Cells).ContainsExactly("Header 1", "Header 2");
            Check.That(result.TableArgument.DataRows).HasSize(1);
            Check.That(result.TableArgument.DataRows[0].Cells).ContainsExactly("Value 1", "Value 2");
            Check.That(result.DocStringArgument).IsNull();
            Check.That(result.Location).IsNull();
            Check.That(result.Comments).IsEmpty();
        }

        [Test]
        public void MapToStep_StepWithLocation_ReturnStepWithLocation()
        {
            var mapper = this.factory.CreateMapper();

            G.Step step = this.factory.CreateStep(
                "Given", "I am on a step", 3, 4
            );

            var result = mapper.MapToStep(step);

            Check.That(result.Keyword).IsEqualTo(Keyword.Given);
            Check.That(result.NativeKeyword).IsEqualTo("Given");
            Check.That(result.Name).IsEqualTo("I am on a step");
            Check.That(result.DocStringArgument).IsNull();
            Check.That(result.TableArgument).IsNull();
            Check.That(result.Location).IsNotNull();
            Check.That(result.Location.Line).IsEqualTo(3);
            Check.That(result.Location.Column).IsEqualTo(4);
            Check.That(result.Comments).IsEmpty();
        }
    }
}
