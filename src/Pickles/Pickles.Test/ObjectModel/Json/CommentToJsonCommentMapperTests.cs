//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CommentToJsonCommentMapperTests.cs" company="PicklesDoc">
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

using NFluent;
using NUnit.Framework;

using PicklesDoc.Pickles.DocumentationBuilders.Json;
using PicklesDoc.Pickles.DocumentationBuilders.Json.Mapper;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test.ObjectModel.Json
{
    [TestFixture]
    public class CommentToJsonCommentMapperTests
    {
        [Test]
        public void Map_Null_ReturnsNull()
        {
            var mapper = CreateMapper();

            JsonComment result = mapper.Map(null);

            Check.That(result).IsNull();
        }

        private static CommentToJsonCommentMapper CreateMapper()
        {
            return new CommentToJsonCommentMapper();
        }

        [Test]
        public void Map_NotNull_ReturnsJsonCommentWithSameText()
        {
            Comment comment = new Comment { Text = "Some comment" };

            var mapper = CreateMapper();

            var result = mapper.Map(comment);

            Check.That(result.Text).IsEqualTo("Some comment");
        }
    }
}