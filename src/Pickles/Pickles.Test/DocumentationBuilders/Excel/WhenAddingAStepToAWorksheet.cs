//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenAddingAStepToAWorksheet.cs" company="PicklesDoc">
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
using Autofac;
using ClosedXML.Excel;
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.DocumentationBuilders.Excel;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test.DocumentationBuilders.Excel
{
    [TestFixture]
    public class WhenAddingAStepToAWorksheet : BaseFixture
    {
        [Test]
        public void ThenStepAddedSuccessfully()
        {
            var excelStepFormatter = Container.Resolve<ExcelStepFormatter>();
            var step = new Step { NativeKeyword = "Given", Name = "I have some precondition" };

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("SHEET1");
                int row = 5;
                excelStepFormatter.Format(worksheet, step, ref row);

                Check.That(worksheet.Cell("C5").Value).IsEqualTo(step.NativeKeyword);
                Check.That(worksheet.Cell("D5").Value).IsEqualTo(step.Name);
            }
        }

        [Test]
        public void ThenStepCommentsAreAddedSuccessfully()
        {
            var excelStepFormatter = Container.Resolve<ExcelStepFormatter>();
            var step = new Step
            {
                NativeKeyword = "Given",
                Name = "I have some precondition",
                Comments = new List<Comment>()
                {
                    new Comment()
                    {
                        Text = "# A comment",
                        Type = CommentType.StepComment
                    }
                }
            };

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("SHEET1");
                int row = 5;
                excelStepFormatter.Format(worksheet, step, ref row);

                Check.That(worksheet.Cell("C5").Value).IsEqualTo(step.Comments.First().Text);
                Check.That(worksheet.Cell("C6").Value).IsEqualTo(step.NativeKeyword);
                Check.That(worksheet.Cell("D6").Value).IsEqualTo(step.Name);
            }
        }

        [Test]
        public void ThenMultilineStepCommentsAreAddedSuccessfully()
        {
            var excelStepFormatter = Container.Resolve<ExcelStepFormatter>();
            var step = new Step
            {
                NativeKeyword = "Given",
                Name = "I have some precondition",
                Comments = new List<Comment>()
                {
                    new Comment()
                    {
                        Text = "# A comment - line 1",
                        Type = CommentType.StepComment
                    },
                    new Comment()
                    {
                        Text = "# A comment - line 2",
                        Type = CommentType.StepComment
                    }
                }
            };

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("SHEET1");
                int row = 5;
                excelStepFormatter.Format(worksheet, step, ref row);

                Check.That(worksheet.Cell("C5").Value).IsEqualTo(step.Comments[0].Text);
                Check.That(worksheet.Cell("C6").Value).IsEqualTo(step.Comments[1].Text);
                Check.That(worksheet.Cell("C7").Value).IsEqualTo(step.NativeKeyword);
                Check.That(worksheet.Cell("D7").Value).IsEqualTo(step.Name);
            }
        }

        [Test]
        public void ThenCommentsAfterTheLastStepAreAddedSuccessfully()
        {
            var excelStepFormatter = Container.Resolve<ExcelStepFormatter>();
            var step = new Step
            {
                NativeKeyword = "Given",
                Name = "I have some precondition",
                Comments = new List<Comment>()
                {
                    new Comment()
                    {
                        Text = "# A comment",
                        Type = CommentType.StepComment
                    },
                    new Comment()
                    {
                        Text = "# A comment the last step",
                        Type = CommentType.AfterLastStepComment
                    }
                }
            };

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("SHEET1");
                int row = 5;
                excelStepFormatter.Format(worksheet, step, ref row);

                Check.That(worksheet.Cell("C5").Value).IsEqualTo(step.Comments.First(o => o.Type == CommentType.StepComment).Text);
                Check.That(worksheet.Cell("C6").Value).IsEqualTo(step.NativeKeyword);
                Check.That(worksheet.Cell("D6").Value).IsEqualTo(step.Name);
                Check.That(worksheet.Cell("C7").Value).IsEqualTo(step.Comments.First(o => o.Type == CommentType.AfterLastStepComment).Text);
            }
        }
    }
}
