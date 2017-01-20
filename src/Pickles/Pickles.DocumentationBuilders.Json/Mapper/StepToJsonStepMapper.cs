//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StepToJsonStepMapper.cs" company="PicklesDoc">
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
using System.Collections.Generic;
using System.Linq;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Json.Mapper
{
    public class StepToJsonStepMapper
    {
        private readonly KeywordToJsonKeywordMapper keywordMapper;
        private readonly TableToJsonTableMapper tableMapper;
        private readonly CommentToJsonCommentMapper commentMapper;

        public StepToJsonStepMapper()
        {
            this.keywordMapper = new KeywordToJsonKeywordMapper();
            this.tableMapper = new TableToJsonTableMapper();
            this.commentMapper = new CommentToJsonCommentMapper();
        }
        public JsonStep Map(Step step)
        {
            if (step == null)
            {
                return null;
            }

            var result = new JsonStep
            {
                Keyword = this.keywordMapper.Map(step.Keyword),
                NativeKeyword = step.NativeKeyword,
                Name = step.Name,
                TableArgument = this.tableMapper.Map(step.TableArgument),
                DocStringArgument = step.DocStringArgument,
                StepComments = new List<JsonComment>(),
                AfterLastStepComments = new List<JsonComment>()
            };

            var comments = (step.Comments ?? new List<Comment>()).ToArray();
            result.StepComments.AddRange(comments.Where(s => s.Type == CommentType.StepComment).Select(this.commentMapper.Map));
            result.AfterLastStepComments.AddRange(comments.Where(s => s.Type == CommentType.AfterLastStepComment).Select(this.commentMapper.Map));

            return result;
        }
    }
}