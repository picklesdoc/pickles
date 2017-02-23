//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapperTestsForExample.cs" company="PicklesDoc">
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
    public class MapperTestsForExample
    {
        private readonly Factory factory = new Factory();

        [Test]
        public void MapToExample_NullExamples_ReturnsNullExample()
        {
            var mapper = this.factory.CreateMapper();

            Example example = mapper.MapToExample(null);

            Check.That(example).IsNull();
        }

        [Test]
        public void MapToExample_RegularExamples_ReturnsCorrectExample()
        {
            var examples = this.factory.CreateExamples(
                "Examples",
                "My Description",
                new[] { "Header 1", "Header 2" },
                new[]
                {
                    new[] { "Row 1, Value 1", "Row 2, Value 2" },
                    new[] { "Row 2, Value 1", "Row 2, Value 2" }
                });

            var mapper = this.factory.CreateMapper();

            var result = mapper.MapToExample(examples);

            Check.That(result.Name).IsEqualTo("Examples");
            Check.That(result.Description).IsEqualTo("My Description");
            Check.That(result.TableArgument.HeaderRow.Cells).ContainsExactly("Header 1", "Header 2");
            Check.That(result.TableArgument.DataRows.Count).IsEqualTo(2);
            Check.That(result.TableArgument.DataRows[0].Cells).ContainsExactly("Row 1, Value 1", "Row 2, Value 2");
            Check.That(result.TableArgument.DataRows[1].Cells).ContainsExactly("Row 2, Value 1", "Row 2, Value 2");
        }

        [Test]
        public void MapToExample_RegularWithTagsExamples_ReturnsCorrectExample()
        {
          var examples = this.factory.CreateExamples(
              "Examples",
              "My Description",
              new[] { "Header 1", "Header 2" },
              new[]
              {
                        new[] { "Row 1, Value 1", "Row 2, Value 2" },
                        new[] { "Row 2, Value 1", "Row 2, Value 2" }
              },
              new[] { "tag1", "tag2" }
              );

          var mapper = this.factory.CreateMapper();

          var result = mapper.MapToExample(examples);

          Check.That(result.Name).IsEqualTo("Examples");
          Check.That(result.Description).IsEqualTo("My Description");
          Check.That(result.TableArgument.HeaderRow.Cells).ContainsExactly("Header 1", "Header 2");
          Check.That(result.TableArgument.DataRows.Count).IsEqualTo(2);
          Check.That(result.TableArgument.DataRows[0].Cells).ContainsExactly("Row 1, Value 1", "Row 2, Value 2");
          Check.That(result.TableArgument.DataRows[1].Cells).ContainsExactly("Row 2, Value 1", "Row 2, Value 2");
          Check.That(result.Tags).ContainsExactly("tag1", "tag2");
        }
    }
}
