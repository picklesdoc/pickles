//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="BodyExtensions.cs" company="PicklesDoc">
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

using DocumentFormat.OpenXml.Wordprocessing;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word.Extensions
{
    public static class BodyExtensions
    {
        public static void GenerateParagraph(this Body body, string text, string styleId)
        {
            var paragraph = new Paragraph
            {
                RsidParagraphAddition = "00CC1B7A",
                RsidParagraphProperties = "0016335E",
                RsidRunAdditionDefault = "0016335E"
            };

            var paragraphProperties = new ParagraphProperties();
            var paragraphStyleId = new ParagraphStyleId { Val = styleId };

            paragraphProperties.Append(paragraphStyleId);

            var run1 = new Run();
            var text1 = new Text();
            text1.Text = text;

            run1.Append(text1);

            paragraph.Append(paragraphProperties);
            paragraph.Append(run1);

            body.Append(paragraph);
        }

        public static void InsertPageBreak(this Body body)
        {
            body.Append(new Paragraph(new Run(new Break { Type = BreakValues.Page })));
        }
    }
}
