//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StepToJsonStepMapperTests.cs" company="PicklesDoc">
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

using System.Collections.Generic;
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.DocumentationBuilders.JSON;
using PicklesDoc.Pickles.DocumentationBuilders.JSON.Mapper;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test.ObjectModel.Json
{
    [TestFixture]
    public class StepToJsonStepMapperTests
    {
        [Test]
        public void Map_NullStep_ReturnsNull()
        {
            var mapper = CreateMapper();

            JsonStep actual = mapper.Map(null);

            Check.That(actual).IsNull();
        }

        private static StepToJsonStepMapper CreateMapper()
        {
            return new StepToJsonStepMapper();
        }

        [Test]
        public void Map_NonNullStep_ReturnsJsonStep()
        {
            var step = new Step();

            var mapper = CreateMapper();

            var actual = mapper.Map(step);

            Check.That(actual).IsNotNull();
        }

        [Test]
        public void Map_WithKeyword_ReturnsCorrectKeyword()
        {
            var step = new Step { Keyword = Keyword.But };

            var mapper = CreateMapper();

            var actual = mapper.Map(step);

            Check.That(actual.Keyword).IsEqualTo(JsonKeyword.But);
        }

        [Test]
        public void Map_WithNativeKeyword_ReturnsNativeKeyword()
        {
            var step = new Step { NativeKeyword = "But" };

            var mapper = CreateMapper();

            var actual = mapper.Map(step);

            Check.That(actual.NativeKeyword).IsEqualTo("But");
        }

        [Test]
        public void Map_WithName_ReturnsName()
        {
            var step = new Step { Name = "I run this step" };

            var mapper = CreateMapper();

            var actual = mapper.Map(step);

            Check.That(actual.Name).IsEqualTo("I run this step");
        }

        [Test]
        public void Map_WithTable_ReturnsTableAndNullDocString()
        {
            var step = new Step
            {
                TableArgument = new Table
                {
                    HeaderRow = new TableRow("row 1", "row 2")
                }
            };

            var mapper = CreateMapper();

            var actual = mapper.Map(step);

            Check.That(actual.TableArgument.HeaderRow).ContainsExactly("row 1", "row 2");
            Check.That(actual.DocStringArgument).IsNull();
        }

        [Test]
        public void Map_WithDocString_ReturnsDocStringAndNoTableArgument()
        {
            var step = new Step { DocStringArgument = "Some longer text" };

            var mapper = CreateMapper();

            var actual = mapper.Map(step);

            Check.That(actual.DocStringArgument).IsEqualTo("Some longer text");
            Check.That(actual.TableArgument).IsNull();
        }

        [Test]
        public void Map_WithNullComments_ReturnsEmptyStepCommentsList()
        {
            var step = new Step { Comments = null };

            var mapper = CreateMapper();

            var actual = mapper.Map(step);

            Check.That(actual.StepComments.Count).IsEqualTo(0);
        }

        [Test]
        public void Map_WithStepComments_ReturnsThoseComments()
        {
            var step = new Step
            {
                Comments = new List<Comment>()
                           {
                               new Comment { Text = "My Comment", Type = CommentType.StepComment }
                           }
            };

            var mapper = CreateMapper();

            var actual = mapper.Map(step);

            Check.That(actual.StepComments[0].Text).IsEqualTo("My Comment");
        }

        [Test]
        public void Map_WithoutStepComments_ReturnsEmptyList()
        {
            var step = new Step
            {
                Comments = new List<Comment>
                           {
                               new Comment { Text = "My AfterLastStepComment Comment", Type = CommentType.AfterLastStepComment },
                               new Comment { Text = "My Normal comment", Type = CommentType.Normal }
                           }
            };

            var mapper = CreateMapper();

            var actual = mapper.Map(step);

            Check.That(actual.StepComments.Count).IsEqualTo(0);
        }

        [Test]
        public void Map_WithNullComments_ReturnsAfterLastStepCommentsList()
        {
            var step = new Step { Comments = null };

            var mapper = CreateMapper();

            var actual = mapper.Map(step);

            Check.That(actual.AfterLastStepComments.Count).IsEqualTo(0);
        }

        [Test]
        public void Map_WithAfterLastStepComments_ReturnsThoseComments()
        {
            var step = new Step
            {
                Comments = new List<Comment>
                           {
                               new Comment { Text = "My AfterLastStepComment Comment", Type = CommentType.AfterLastStepComment },
                           }
            };

            var mapper = CreateMapper();

            var actual = mapper.Map(step);

            Check.That(actual.AfterLastStepComments[0].Text).IsEqualTo("My AfterLastStepComment Comment");
        }

        [Test]
        public void Map_WithoutAfterLastStepComments_ReturnsEmptyList()
        {
            var step = new Step
            {
                Comments = new List<Comment>
                           {
                               new Comment { Text = "My Step Comment", Type = CommentType.StepComment },
                               new Comment { Text = "My Normal comment", Type = CommentType.Normal }
                           }
            };

            var mapper = CreateMapper();

            var actual = mapper.Map(step);

            Check.That(actual.AfterLastStepComments.Count).IsEqualTo(0);
        }
    }
}