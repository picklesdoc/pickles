//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlStepFormatter.cs" company="PicklesDoc">
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
using System.Linq;
using System.Xml.Linq;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html
{
    public class HtmlStepFormatter
    {
        private readonly HtmlMultilineStringFormatter htmlMultilineStringFormatter;
        private readonly HtmlTableFormatter htmlTableFormatter;

        private readonly XNamespace xmlns;

        public HtmlStepFormatter(
            HtmlTableFormatter htmlTableFormatter,
            HtmlMultilineStringFormatter htmlMultilineStringFormatter)
        {
            this.htmlTableFormatter = htmlTableFormatter;
            this.htmlMultilineStringFormatter = htmlMultilineStringFormatter;
            this.xmlns = HtmlNamespace.Xhtml;
        }

        protected XElement FormatComments(Step step, CommentType type)
        {
            XElement comment = new XElement(this.xmlns + "span", new XAttribute("class", "comment"));

            foreach (var stepComment in step.Comments.Where(o => o.Type == type))
            {
                comment.Add(stepComment.Text.Trim());
                comment.Add(new XElement(this.xmlns + "br"));
            }
            comment.LastNode.Remove();

            return comment;
        }

        public XElement Format(Step step)
        {
            XElement li;

            XElement beforeStepComments = null;
            XElement afterStepComments = null;
            if (step.Comments.Any(o => o.Type == CommentType.StepComment))
            {
                beforeStepComments = this.FormatComments(step, CommentType.StepComment);
            }
            if (step.Comments.Any(o => o.Type == CommentType.AfterLastStepComment))
            {
                afterStepComments = this.FormatComments(step, CommentType.AfterLastStepComment);
            }

            li = new XElement(
                    this.xmlns + "li",
                    new XAttribute("class", "step"),
                    beforeStepComments,
                    new XElement(this.xmlns + "span", new XAttribute("class", "keyword"), step.NativeKeyword),
                    step.Name,
                    afterStepComments);

            if (step.TableArgument != null)
            {
                li.Add(this.htmlTableFormatter.Format(step.TableArgument));
            }

            if (!string.IsNullOrEmpty(step.DocStringArgument))
            {
                li.Add(this.htmlMultilineStringFormatter.Format(step.DocStringArgument));
            }

            return li;
        }
    }
}
