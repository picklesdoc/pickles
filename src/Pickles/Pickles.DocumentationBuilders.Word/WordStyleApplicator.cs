//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WordStyleApplicator.cs" company="PicklesDoc">
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
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word
{
    /// <summary>
    /// Applies the word styles.
    /// </summary>
    /// <remarks>Most of the code in this class was generated using the Open XML Productivity Tool</remarks>
    public class WordStyleApplicator
    {
        // Apply a style to a paragraph.
        public void ApplyStyleToParagraph(WordprocessingDocument doc, string styleid, string stylename, Paragraph p)
        {
            // If the paragraph has no ParagraphProperties object, create one.
            if (p.Elements<ParagraphProperties>().Count() == 0)
            {
                p.PrependChild(new ParagraphProperties());
            }

            // Get the paragraph properties element of the paragraph.
            ParagraphProperties pPr = p.Elements<ParagraphProperties>().First();

            // Get the Styles part for this document.
            StyleDefinitionsPart part =
                doc.MainDocumentPart.StyleDefinitionsPart;

            // If the Styles part does not exist, add it and then add the style.
            if (part == null)
            {
                part = this.AddStylesPartToPackage(doc);
                AddNewStyle(part, styleid, stylename);
            }
            else
            {
                // If the style is not in the document, add it.
                if (this.IsStyleIdInDocument(doc, styleid) != true)
                {
                    // No match on styleid, so let's try style name.
                    string styleidFromName = this.GetStyleIdFromStyleName(doc, stylename);
                    if (styleidFromName == null)
                    {
                        AddNewStyle(part, styleid, stylename);
                    }
                    else
                    {
                        styleid = styleidFromName;
                    }
                }
            }

            // Set the style of the paragraph.
            pPr.ParagraphStyleId = new ParagraphStyleId { Val = styleid };
        }

        // Return true if the style id is in the document, false otherwise.
        public bool IsStyleIdInDocument(WordprocessingDocument doc, string styleid)
        {
            // Get access to the Styles element for this document.
            Styles s = doc.MainDocumentPart.StyleDefinitionsPart.Styles;

            // Check that there are styles and how many.
            int n = s.Elements<Style>().Count();
            if (n == 0)
            {
                return false;
            }

            // Look for a match on styleid.
            Style style = s.Elements<Style>()
                .Where(st => (st.StyleId == styleid) && (st.Type == StyleValues.Paragraph))
                .FirstOrDefault();
            if (style == null)
            {
                return false;
            }

            return true;
        }

        // Return styleid that matches the styleName, or null when there's no match.
        public string GetStyleIdFromStyleName(WordprocessingDocument doc, string styleName)
        {
            StyleDefinitionsPart stylePart = doc.MainDocumentPart.StyleDefinitionsPart;
            string styleId = stylePart.Styles.Descendants<StyleName>()
                .Where(s => s.Val.Value.Equals(styleName) &&
                            (((Style)s.Parent).Type == StyleValues.Paragraph))
                .Select(n => ((Style)n.Parent).StyleId).FirstOrDefault();
            return styleId;
        }

        // Create a new style with the specified styleid and stylename and add it to the specified
        // style definitions part.
        private static void AddNewStyle(StyleDefinitionsPart styleDefinitionsPart, string styleid, string stylename)
        {
            // Get access to the root element of the styles part.
            Styles styles = styleDefinitionsPart.Styles;

            // Create a new paragraph style and specify some of the properties.
            var style = new Style
            {
                Type = StyleValues.Paragraph,
                StyleId = styleid,
                CustomStyle = true
            };
            var styleName1 = new StyleName { Val = stylename };
            var basedOn1 = new BasedOn { Val = "Normal" };
            var nextParagraphStyle1 = new NextParagraphStyle { Val = "Normal" };
            style.Append(styleName1);
            style.Append(basedOn1);
            style.Append(nextParagraphStyle1);

            // Create the StyleRunProperties object and specify some of the run properties.
            var styleRunProperties1 = new StyleRunProperties();
            var bold1 = new Bold();
            var color1 = new Color { ThemeColor = ThemeColorValues.Accent2 };
            var font1 = new RunFonts { Ascii = "Lucida Console" };
            var italic1 = new Italic();

            // Specify a 12 point size.
            var fontSize1 = new FontSize { Val = "24" };
            styleRunProperties1.Append(bold1);
            styleRunProperties1.Append(color1);
            styleRunProperties1.Append(font1);
            styleRunProperties1.Append(fontSize1);
            styleRunProperties1.Append(italic1);

            // Add the run properties to the style.
            style.Append(styleRunProperties1);

            // Add the style to the styles part.
            styles.Append(style);
        }

        // Add a StylesDefinitionsPart to the document.  Returns a reference to it.
        public StyleDefinitionsPart AddStylesPartToPackage(WordprocessingDocument doc)
        {
            var part = doc.MainDocumentPart.AddNewPart<StyleDefinitionsPart>();
            this.GeneratePartContent(part);
            var root = new Styles();
            root.Save(part);
            return part;
        }

        public StylesWithEffectsPart AddStylesWithEffectsPartToPackage(WordprocessingDocument doc)
        {
            var part = doc.MainDocumentPart.AddNewPart<StylesWithEffectsPart>();
            var root = new Styles();
            root.Save(part);
            return part;
        }

        // Generates content of part.
        private void GeneratePartContent(StyleDefinitionsPart part)
        {
            var styles1 = new Styles();
            styles1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            styles1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

            var docDefaults1 = new DocDefaults();

            var runPropertiesDefault1 = new RunPropertiesDefault();

            var runPropertiesBaseStyle1 = new RunPropertiesBaseStyle();
            var runFonts1 = new RunFonts
            {
                AsciiTheme = ThemeFontValues.MinorHighAnsi,
                HighAnsiTheme = ThemeFontValues.MinorHighAnsi,
                EastAsiaTheme = ThemeFontValues.MinorHighAnsi,
                ComplexScriptTheme = ThemeFontValues.MinorBidi
            };
            var fontSize1 = new FontSize { Val = "22" };
            var fontSizeComplexScript1 = new FontSizeComplexScript { Val = "22" };
            var languages1 = new Languages { Val = "en-US", EastAsia = "en-US", Bidi = "ar-SA" };

            runPropertiesBaseStyle1.Append(runFonts1);
            runPropertiesBaseStyle1.Append(fontSize1);
            runPropertiesBaseStyle1.Append(fontSizeComplexScript1);
            runPropertiesBaseStyle1.Append(languages1);

            runPropertiesDefault1.Append(runPropertiesBaseStyle1);

            var paragraphPropertiesDefault1 = new ParagraphPropertiesDefault();

            var paragraphPropertiesBaseStyle1 = new ParagraphPropertiesBaseStyle();
            var spacingBetweenLines1 = new SpacingBetweenLines
            { After = "200", Line = "276", LineRule = LineSpacingRuleValues.Auto };

            paragraphPropertiesBaseStyle1.Append(spacingBetweenLines1);

            paragraphPropertiesDefault1.Append(paragraphPropertiesBaseStyle1);

            docDefaults1.Append(runPropertiesDefault1);
            docDefaults1.Append(paragraphPropertiesDefault1);

            var latentStyles1 = new LatentStyles
            {
                DefaultLockedState = false,
                DefaultUiPriority = 99,
                DefaultSemiHidden = true,
                DefaultUnhideWhenUsed = true,
                DefaultPrimaryStyle = false,
                Count = 267
            };
            var latentStyleExceptionInfo1 = new LatentStyleExceptionInfo
            {
                Name = "Normal",
                UiPriority = 0,
                SemiHidden = false,
                UnhideWhenUsed = false,
                PrimaryStyle = true
            };
            var latentStyleExceptionInfo2 = new LatentStyleExceptionInfo
            {
                Name = "heading 1",
                UiPriority = 9,
                SemiHidden = false,
                UnhideWhenUsed = false,
                PrimaryStyle = true
            };
            var latentStyleExceptionInfo3 = new LatentStyleExceptionInfo
            { Name = "heading 2", UiPriority = 9, PrimaryStyle = true };
            var latentStyleExceptionInfo4 = new LatentStyleExceptionInfo
            { Name = "heading 3", UiPriority = 9, PrimaryStyle = true };
            var latentStyleExceptionInfo5 = new LatentStyleExceptionInfo
            { Name = "heading 4", UiPriority = 9, PrimaryStyle = true };
            var latentStyleExceptionInfo6 = new LatentStyleExceptionInfo
            { Name = "heading 5", UiPriority = 9, PrimaryStyle = true };
            var latentStyleExceptionInfo7 = new LatentStyleExceptionInfo
            { Name = "heading 6", UiPriority = 9, PrimaryStyle = true };
            var latentStyleExceptionInfo8 = new LatentStyleExceptionInfo
            { Name = "heading 7", UiPriority = 9, PrimaryStyle = true };
            var latentStyleExceptionInfo9 = new LatentStyleExceptionInfo
            { Name = "heading 8", UiPriority = 9, PrimaryStyle = true };
            var latentStyleExceptionInfo10 = new LatentStyleExceptionInfo
            { Name = "heading 9", UiPriority = 9, PrimaryStyle = true };
            var latentStyleExceptionInfo11 = new LatentStyleExceptionInfo { Name = "toc 1", UiPriority = 39 };
            var latentStyleExceptionInfo12 = new LatentStyleExceptionInfo { Name = "toc 2", UiPriority = 39 };
            var latentStyleExceptionInfo13 = new LatentStyleExceptionInfo { Name = "toc 3", UiPriority = 39 };
            var latentStyleExceptionInfo14 = new LatentStyleExceptionInfo { Name = "toc 4", UiPriority = 39 };
            var latentStyleExceptionInfo15 = new LatentStyleExceptionInfo { Name = "toc 5", UiPriority = 39 };
            var latentStyleExceptionInfo16 = new LatentStyleExceptionInfo { Name = "toc 6", UiPriority = 39 };
            var latentStyleExceptionInfo17 = new LatentStyleExceptionInfo { Name = "toc 7", UiPriority = 39 };
            var latentStyleExceptionInfo18 = new LatentStyleExceptionInfo { Name = "toc 8", UiPriority = 39 };
            var latentStyleExceptionInfo19 = new LatentStyleExceptionInfo { Name = "toc 9", UiPriority = 39 };
            var latentStyleExceptionInfo20 = new LatentStyleExceptionInfo
            { Name = "caption", UiPriority = 35, PrimaryStyle = true };
            var latentStyleExceptionInfo21 = new LatentStyleExceptionInfo
            {
                Name = "Title",
                UiPriority = 10,
                SemiHidden = false,
                UnhideWhenUsed = false,
                PrimaryStyle = true
            };
            var latentStyleExceptionInfo22 = new LatentStyleExceptionInfo
            { Name = "Default Paragraph Font", UiPriority = 1 };
            var latentStyleExceptionInfo23 = new LatentStyleExceptionInfo
            {
                Name = "Subtitle",
                UiPriority = 11,
                SemiHidden = false,
                UnhideWhenUsed = false,
                PrimaryStyle = true
            };
            var latentStyleExceptionInfo24 = new LatentStyleExceptionInfo
            {
                Name = "Strong",
                UiPriority = 22,
                SemiHidden = false,
                UnhideWhenUsed = false,
                PrimaryStyle = true
            };
            var latentStyleExceptionInfo25 = new LatentStyleExceptionInfo
            {
                Name = "Emphasis",
                UiPriority = 20,
                SemiHidden = false,
                UnhideWhenUsed = false,
                PrimaryStyle = true
            };
            var latentStyleExceptionInfo26 = new LatentStyleExceptionInfo
            {
                Name = "Table Grid",
                UiPriority = 59,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo27 = new LatentStyleExceptionInfo
            { Name = "Placeholder Text", UnhideWhenUsed = false };
            var latentStyleExceptionInfo28 = new LatentStyleExceptionInfo
            {
                Name = "No Spacing",
                UiPriority = 1,
                SemiHidden = false,
                UnhideWhenUsed = false,
                PrimaryStyle = true
            };
            var latentStyleExceptionInfo29 = new LatentStyleExceptionInfo
            {
                Name = "Light Shading",
                UiPriority = 60,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo30 = new LatentStyleExceptionInfo
            {
                Name = "Light List",
                UiPriority = 61,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo31 = new LatentStyleExceptionInfo
            {
                Name = "Light Grid",
                UiPriority = 62,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo32 = new LatentStyleExceptionInfo
            {
                Name = "Medium Shading 1",
                UiPriority = 63,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo33 = new LatentStyleExceptionInfo
            {
                Name = "Medium Shading 2",
                UiPriority = 64,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo34 = new LatentStyleExceptionInfo
            {
                Name = "Medium List 1",
                UiPriority = 65,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo35 = new LatentStyleExceptionInfo
            {
                Name = "Medium List 2",
                UiPriority = 66,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo36 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 1",
                UiPriority = 67,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo37 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 2",
                UiPriority = 68,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo38 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 3",
                UiPriority = 69,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo39 = new LatentStyleExceptionInfo
            {
                Name = "Dark List",
                UiPriority = 70,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo40 = new LatentStyleExceptionInfo
            {
                Name = "Colorful Shading",
                UiPriority = 71,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo41 = new LatentStyleExceptionInfo
            {
                Name = "Colorful List",
                UiPriority = 72,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo42 = new LatentStyleExceptionInfo
            {
                Name = "Colorful Grid",
                UiPriority = 73,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo43 = new LatentStyleExceptionInfo
            {
                Name = "Light Shading Accent 1",
                UiPriority = 60,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo44 = new LatentStyleExceptionInfo
            {
                Name = "Light List Accent 1",
                UiPriority = 61,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo45 = new LatentStyleExceptionInfo
            {
                Name = "Light Grid Accent 1",
                UiPriority = 62,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo46 = new LatentStyleExceptionInfo
            {
                Name = "Medium Shading 1 Accent 1",
                UiPriority = 63,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo47 = new LatentStyleExceptionInfo
            {
                Name = "Medium Shading 2 Accent 1",
                UiPriority = 64,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo48 = new LatentStyleExceptionInfo
            {
                Name = "Medium List 1 Accent 1",
                UiPriority = 65,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo49 = new LatentStyleExceptionInfo { Name = "Revision", UnhideWhenUsed = false };
            var latentStyleExceptionInfo50 = new LatentStyleExceptionInfo
            {
                Name = "List Paragraph",
                UiPriority = 34,
                SemiHidden = false,
                UnhideWhenUsed = false,
                PrimaryStyle = true
            };
            var latentStyleExceptionInfo51 = new LatentStyleExceptionInfo
            {
                Name = "Quote",
                UiPriority = 29,
                SemiHidden = false,
                UnhideWhenUsed = false,
                PrimaryStyle = true
            };
            var latentStyleExceptionInfo52 = new LatentStyleExceptionInfo
            {
                Name = "Intense Quote",
                UiPriority = 30,
                SemiHidden = false,
                UnhideWhenUsed = false,
                PrimaryStyle = true
            };
            var latentStyleExceptionInfo53 = new LatentStyleExceptionInfo
            {
                Name = "Medium List 2 Accent 1",
                UiPriority = 66,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo54 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 1 Accent 1",
                UiPriority = 67,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo55 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 2 Accent 1",
                UiPriority = 68,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo56 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 3 Accent 1",
                UiPriority = 69,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo57 = new LatentStyleExceptionInfo
            {
                Name = "Dark List Accent 1",
                UiPriority = 70,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo58 = new LatentStyleExceptionInfo
            {
                Name = "Colorful Shading Accent 1",
                UiPriority = 71,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo59 = new LatentStyleExceptionInfo
            {
                Name = "Colorful List Accent 1",
                UiPriority = 72,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo60 = new LatentStyleExceptionInfo
            {
                Name = "Colorful Grid Accent 1",
                UiPriority = 73,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo61 = new LatentStyleExceptionInfo
            {
                Name = "Light Shading Accent 2",
                UiPriority = 60,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo62 = new LatentStyleExceptionInfo
            {
                Name = "Light List Accent 2",
                UiPriority = 61,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo63 = new LatentStyleExceptionInfo
            {
                Name = "Light Grid Accent 2",
                UiPriority = 62,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo64 = new LatentStyleExceptionInfo
            {
                Name = "Medium Shading 1 Accent 2",
                UiPriority = 63,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo65 = new LatentStyleExceptionInfo
            {
                Name = "Medium Shading 2 Accent 2",
                UiPriority = 64,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo66 = new LatentStyleExceptionInfo
            {
                Name = "Medium List 1 Accent 2",
                UiPriority = 65,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo67 = new LatentStyleExceptionInfo
            {
                Name = "Medium List 2 Accent 2",
                UiPriority = 66,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo68 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 1 Accent 2",
                UiPriority = 67,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo69 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 2 Accent 2",
                UiPriority = 68,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo70 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 3 Accent 2",
                UiPriority = 69,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo71 = new LatentStyleExceptionInfo
            {
                Name = "Dark List Accent 2",
                UiPriority = 70,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo72 = new LatentStyleExceptionInfo
            {
                Name = "Colorful Shading Accent 2",
                UiPriority = 71,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo73 = new LatentStyleExceptionInfo
            {
                Name = "Colorful List Accent 2",
                UiPriority = 72,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo74 = new LatentStyleExceptionInfo
            {
                Name = "Colorful Grid Accent 2",
                UiPriority = 73,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo75 = new LatentStyleExceptionInfo
            {
                Name = "Light Shading Accent 3",
                UiPriority = 60,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo76 = new LatentStyleExceptionInfo
            {
                Name = "Light List Accent 3",
                UiPriority = 61,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo77 = new LatentStyleExceptionInfo
            {
                Name = "Light Grid Accent 3",
                UiPriority = 62,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo78 = new LatentStyleExceptionInfo
            {
                Name = "Medium Shading 1 Accent 3",
                UiPriority = 63,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo79 = new LatentStyleExceptionInfo
            {
                Name = "Medium Shading 2 Accent 3",
                UiPriority = 64,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo80 = new LatentStyleExceptionInfo
            {
                Name = "Medium List 1 Accent 3",
                UiPriority = 65,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo81 = new LatentStyleExceptionInfo
            {
                Name = "Medium List 2 Accent 3",
                UiPriority = 66,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo82 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 1 Accent 3",
                UiPriority = 67,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo83 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 2 Accent 3",
                UiPriority = 68,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo84 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 3 Accent 3",
                UiPriority = 69,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo85 = new LatentStyleExceptionInfo
            {
                Name = "Dark List Accent 3",
                UiPriority = 70,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo86 = new LatentStyleExceptionInfo
            {
                Name = "Colorful Shading Accent 3",
                UiPriority = 71,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo87 = new LatentStyleExceptionInfo
            {
                Name = "Colorful List Accent 3",
                UiPriority = 72,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo88 = new LatentStyleExceptionInfo
            {
                Name = "Colorful Grid Accent 3",
                UiPriority = 73,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo89 = new LatentStyleExceptionInfo
            {
                Name = "Light Shading Accent 4",
                UiPriority = 60,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo90 = new LatentStyleExceptionInfo
            {
                Name = "Light List Accent 4",
                UiPriority = 61,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo91 = new LatentStyleExceptionInfo
            {
                Name = "Light Grid Accent 4",
                UiPriority = 62,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo92 = new LatentStyleExceptionInfo
            {
                Name = "Medium Shading 1 Accent 4",
                UiPriority = 63,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo93 = new LatentStyleExceptionInfo
            {
                Name = "Medium Shading 2 Accent 4",
                UiPriority = 64,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo94 = new LatentStyleExceptionInfo
            {
                Name = "Medium List 1 Accent 4",
                UiPriority = 65,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo95 = new LatentStyleExceptionInfo
            {
                Name = "Medium List 2 Accent 4",
                UiPriority = 66,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo96 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 1 Accent 4",
                UiPriority = 67,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo97 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 2 Accent 4",
                UiPriority = 68,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo98 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 3 Accent 4",
                UiPriority = 69,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo99 = new LatentStyleExceptionInfo
            {
                Name = "Dark List Accent 4",
                UiPriority = 70,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo100 = new LatentStyleExceptionInfo
            {
                Name = "Colorful Shading Accent 4",
                UiPriority = 71,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo101 = new LatentStyleExceptionInfo
            {
                Name = "Colorful List Accent 4",
                UiPriority = 72,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo102 = new LatentStyleExceptionInfo
            {
                Name = "Colorful Grid Accent 4",
                UiPriority = 73,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo103 = new LatentStyleExceptionInfo
            {
                Name = "Light Shading Accent 5",
                UiPriority = 60,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo104 = new LatentStyleExceptionInfo
            {
                Name = "Light List Accent 5",
                UiPriority = 61,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo105 = new LatentStyleExceptionInfo
            {
                Name = "Light Grid Accent 5",
                UiPriority = 62,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo106 = new LatentStyleExceptionInfo
            {
                Name = "Medium Shading 1 Accent 5",
                UiPriority = 63,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo107 = new LatentStyleExceptionInfo
            {
                Name = "Medium Shading 2 Accent 5",
                UiPriority = 64,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo108 = new LatentStyleExceptionInfo
            {
                Name = "Medium List 1 Accent 5",
                UiPriority = 65,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo109 = new LatentStyleExceptionInfo
            {
                Name = "Medium List 2 Accent 5",
                UiPriority = 66,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo110 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 1 Accent 5",
                UiPriority = 67,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo111 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 2 Accent 5",
                UiPriority = 68,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo112 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 3 Accent 5",
                UiPriority = 69,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo113 = new LatentStyleExceptionInfo
            {
                Name = "Dark List Accent 5",
                UiPriority = 70,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo114 = new LatentStyleExceptionInfo
            {
                Name = "Colorful Shading Accent 5",
                UiPriority = 71,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo115 = new LatentStyleExceptionInfo
            {
                Name = "Colorful List Accent 5",
                UiPriority = 72,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo116 = new LatentStyleExceptionInfo
            {
                Name = "Colorful Grid Accent 5",
                UiPriority = 73,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo117 = new LatentStyleExceptionInfo
            {
                Name = "Light Shading Accent 6",
                UiPriority = 60,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo118 = new LatentStyleExceptionInfo
            {
                Name = "Light List Accent 6",
                UiPriority = 61,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo119 = new LatentStyleExceptionInfo
            {
                Name = "Light Grid Accent 6",
                UiPriority = 62,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo120 = new LatentStyleExceptionInfo
            {
                Name = "Medium Shading 1 Accent 6",
                UiPriority = 63,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo121 = new LatentStyleExceptionInfo
            {
                Name = "Medium Shading 2 Accent 6",
                UiPriority = 64,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo122 = new LatentStyleExceptionInfo
            {
                Name = "Medium List 1 Accent 6",
                UiPriority = 65,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo123 = new LatentStyleExceptionInfo
            {
                Name = "Medium List 2 Accent 6",
                UiPriority = 66,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo124 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 1 Accent 6",
                UiPriority = 67,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo125 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 2 Accent 6",
                UiPriority = 68,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo126 = new LatentStyleExceptionInfo
            {
                Name = "Medium Grid 3 Accent 6",
                UiPriority = 69,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo127 = new LatentStyleExceptionInfo
            {
                Name = "Dark List Accent 6",
                UiPriority = 70,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo128 = new LatentStyleExceptionInfo
            {
                Name = "Colorful Shading Accent 6",
                UiPriority = 71,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo129 = new LatentStyleExceptionInfo
            {
                Name = "Colorful List Accent 6",
                UiPriority = 72,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo130 = new LatentStyleExceptionInfo
            {
                Name = "Colorful Grid Accent 6",
                UiPriority = 73,
                SemiHidden = false,
                UnhideWhenUsed = false
            };
            var latentStyleExceptionInfo131 = new LatentStyleExceptionInfo
            {
                Name = "Subtle Emphasis",
                UiPriority = 19,
                SemiHidden = false,
                UnhideWhenUsed = false,
                PrimaryStyle = true
            };
            var latentStyleExceptionInfo132 = new LatentStyleExceptionInfo
            {
                Name = "Intense Emphasis",
                UiPriority = 21,
                SemiHidden = false,
                UnhideWhenUsed = false,
                PrimaryStyle = true
            };
            var latentStyleExceptionInfo133 = new LatentStyleExceptionInfo
            {
                Name = "Subtle Reference",
                UiPriority = 31,
                SemiHidden = false,
                UnhideWhenUsed = false,
                PrimaryStyle = true
            };
            var latentStyleExceptionInfo134 = new LatentStyleExceptionInfo
            {
                Name = "Intense Reference",
                UiPriority = 32,
                SemiHidden = false,
                UnhideWhenUsed = false,
                PrimaryStyle = true
            };
            var latentStyleExceptionInfo135 = new LatentStyleExceptionInfo
            {
                Name = "Book Title",
                UiPriority = 33,
                SemiHidden = false,
                UnhideWhenUsed = false,
                PrimaryStyle = true
            };
            var latentStyleExceptionInfo136 = new LatentStyleExceptionInfo { Name = "Bibliography", UiPriority = 37 };
            var latentStyleExceptionInfo137 = new LatentStyleExceptionInfo
            { Name = "TOC Heading", UiPriority = 39, PrimaryStyle = true };

            latentStyles1.Append(latentStyleExceptionInfo1);
            latentStyles1.Append(latentStyleExceptionInfo2);
            latentStyles1.Append(latentStyleExceptionInfo3);
            latentStyles1.Append(latentStyleExceptionInfo4);
            latentStyles1.Append(latentStyleExceptionInfo5);
            latentStyles1.Append(latentStyleExceptionInfo6);
            latentStyles1.Append(latentStyleExceptionInfo7);
            latentStyles1.Append(latentStyleExceptionInfo8);
            latentStyles1.Append(latentStyleExceptionInfo9);
            latentStyles1.Append(latentStyleExceptionInfo10);
            latentStyles1.Append(latentStyleExceptionInfo11);
            latentStyles1.Append(latentStyleExceptionInfo12);
            latentStyles1.Append(latentStyleExceptionInfo13);
            latentStyles1.Append(latentStyleExceptionInfo14);
            latentStyles1.Append(latentStyleExceptionInfo15);
            latentStyles1.Append(latentStyleExceptionInfo16);
            latentStyles1.Append(latentStyleExceptionInfo17);
            latentStyles1.Append(latentStyleExceptionInfo18);
            latentStyles1.Append(latentStyleExceptionInfo19);
            latentStyles1.Append(latentStyleExceptionInfo20);
            latentStyles1.Append(latentStyleExceptionInfo21);
            latentStyles1.Append(latentStyleExceptionInfo22);
            latentStyles1.Append(latentStyleExceptionInfo23);
            latentStyles1.Append(latentStyleExceptionInfo24);
            latentStyles1.Append(latentStyleExceptionInfo25);
            latentStyles1.Append(latentStyleExceptionInfo26);
            latentStyles1.Append(latentStyleExceptionInfo27);
            latentStyles1.Append(latentStyleExceptionInfo28);
            latentStyles1.Append(latentStyleExceptionInfo29);
            latentStyles1.Append(latentStyleExceptionInfo30);
            latentStyles1.Append(latentStyleExceptionInfo31);
            latentStyles1.Append(latentStyleExceptionInfo32);
            latentStyles1.Append(latentStyleExceptionInfo33);
            latentStyles1.Append(latentStyleExceptionInfo34);
            latentStyles1.Append(latentStyleExceptionInfo35);
            latentStyles1.Append(latentStyleExceptionInfo36);
            latentStyles1.Append(latentStyleExceptionInfo37);
            latentStyles1.Append(latentStyleExceptionInfo38);
            latentStyles1.Append(latentStyleExceptionInfo39);
            latentStyles1.Append(latentStyleExceptionInfo40);
            latentStyles1.Append(latentStyleExceptionInfo41);
            latentStyles1.Append(latentStyleExceptionInfo42);
            latentStyles1.Append(latentStyleExceptionInfo43);
            latentStyles1.Append(latentStyleExceptionInfo44);
            latentStyles1.Append(latentStyleExceptionInfo45);
            latentStyles1.Append(latentStyleExceptionInfo46);
            latentStyles1.Append(latentStyleExceptionInfo47);
            latentStyles1.Append(latentStyleExceptionInfo48);
            latentStyles1.Append(latentStyleExceptionInfo49);
            latentStyles1.Append(latentStyleExceptionInfo50);
            latentStyles1.Append(latentStyleExceptionInfo51);
            latentStyles1.Append(latentStyleExceptionInfo52);
            latentStyles1.Append(latentStyleExceptionInfo53);
            latentStyles1.Append(latentStyleExceptionInfo54);
            latentStyles1.Append(latentStyleExceptionInfo55);
            latentStyles1.Append(latentStyleExceptionInfo56);
            latentStyles1.Append(latentStyleExceptionInfo57);
            latentStyles1.Append(latentStyleExceptionInfo58);
            latentStyles1.Append(latentStyleExceptionInfo59);
            latentStyles1.Append(latentStyleExceptionInfo60);
            latentStyles1.Append(latentStyleExceptionInfo61);
            latentStyles1.Append(latentStyleExceptionInfo62);
            latentStyles1.Append(latentStyleExceptionInfo63);
            latentStyles1.Append(latentStyleExceptionInfo64);
            latentStyles1.Append(latentStyleExceptionInfo65);
            latentStyles1.Append(latentStyleExceptionInfo66);
            latentStyles1.Append(latentStyleExceptionInfo67);
            latentStyles1.Append(latentStyleExceptionInfo68);
            latentStyles1.Append(latentStyleExceptionInfo69);
            latentStyles1.Append(latentStyleExceptionInfo70);
            latentStyles1.Append(latentStyleExceptionInfo71);
            latentStyles1.Append(latentStyleExceptionInfo72);
            latentStyles1.Append(latentStyleExceptionInfo73);
            latentStyles1.Append(latentStyleExceptionInfo74);
            latentStyles1.Append(latentStyleExceptionInfo75);
            latentStyles1.Append(latentStyleExceptionInfo76);
            latentStyles1.Append(latentStyleExceptionInfo77);
            latentStyles1.Append(latentStyleExceptionInfo78);
            latentStyles1.Append(latentStyleExceptionInfo79);
            latentStyles1.Append(latentStyleExceptionInfo80);
            latentStyles1.Append(latentStyleExceptionInfo81);
            latentStyles1.Append(latentStyleExceptionInfo82);
            latentStyles1.Append(latentStyleExceptionInfo83);
            latentStyles1.Append(latentStyleExceptionInfo84);
            latentStyles1.Append(latentStyleExceptionInfo85);
            latentStyles1.Append(latentStyleExceptionInfo86);
            latentStyles1.Append(latentStyleExceptionInfo87);
            latentStyles1.Append(latentStyleExceptionInfo88);
            latentStyles1.Append(latentStyleExceptionInfo89);
            latentStyles1.Append(latentStyleExceptionInfo90);
            latentStyles1.Append(latentStyleExceptionInfo91);
            latentStyles1.Append(latentStyleExceptionInfo92);
            latentStyles1.Append(latentStyleExceptionInfo93);
            latentStyles1.Append(latentStyleExceptionInfo94);
            latentStyles1.Append(latentStyleExceptionInfo95);
            latentStyles1.Append(latentStyleExceptionInfo96);
            latentStyles1.Append(latentStyleExceptionInfo97);
            latentStyles1.Append(latentStyleExceptionInfo98);
            latentStyles1.Append(latentStyleExceptionInfo99);
            latentStyles1.Append(latentStyleExceptionInfo100);
            latentStyles1.Append(latentStyleExceptionInfo101);
            latentStyles1.Append(latentStyleExceptionInfo102);
            latentStyles1.Append(latentStyleExceptionInfo103);
            latentStyles1.Append(latentStyleExceptionInfo104);
            latentStyles1.Append(latentStyleExceptionInfo105);
            latentStyles1.Append(latentStyleExceptionInfo106);
            latentStyles1.Append(latentStyleExceptionInfo107);
            latentStyles1.Append(latentStyleExceptionInfo108);
            latentStyles1.Append(latentStyleExceptionInfo109);
            latentStyles1.Append(latentStyleExceptionInfo110);
            latentStyles1.Append(latentStyleExceptionInfo111);
            latentStyles1.Append(latentStyleExceptionInfo112);
            latentStyles1.Append(latentStyleExceptionInfo113);
            latentStyles1.Append(latentStyleExceptionInfo114);
            latentStyles1.Append(latentStyleExceptionInfo115);
            latentStyles1.Append(latentStyleExceptionInfo116);
            latentStyles1.Append(latentStyleExceptionInfo117);
            latentStyles1.Append(latentStyleExceptionInfo118);
            latentStyles1.Append(latentStyleExceptionInfo119);
            latentStyles1.Append(latentStyleExceptionInfo120);
            latentStyles1.Append(latentStyleExceptionInfo121);
            latentStyles1.Append(latentStyleExceptionInfo122);
            latentStyles1.Append(latentStyleExceptionInfo123);
            latentStyles1.Append(latentStyleExceptionInfo124);
            latentStyles1.Append(latentStyleExceptionInfo125);
            latentStyles1.Append(latentStyleExceptionInfo126);
            latentStyles1.Append(latentStyleExceptionInfo127);
            latentStyles1.Append(latentStyleExceptionInfo128);
            latentStyles1.Append(latentStyleExceptionInfo129);
            latentStyles1.Append(latentStyleExceptionInfo130);
            latentStyles1.Append(latentStyleExceptionInfo131);
            latentStyles1.Append(latentStyleExceptionInfo132);
            latentStyles1.Append(latentStyleExceptionInfo133);
            latentStyles1.Append(latentStyleExceptionInfo134);
            latentStyles1.Append(latentStyleExceptionInfo135);
            latentStyles1.Append(latentStyleExceptionInfo136);
            latentStyles1.Append(latentStyleExceptionInfo137);

            var style1 = new Style { Type = StyleValues.Paragraph, StyleId = "Normal", Default = true };
            var styleName1 = new StyleName { Val = "Normal" };
            var primaryStyle1 = new PrimaryStyle();
            var rsid1 = new Rsid { Val = "00BA40EF" };

            var styleParagraphProperties1 = new StyleParagraphProperties();
            var spacingBetweenLines2 = new SpacingBetweenLines { After = "0" };

            styleParagraphProperties1.Append(spacingBetweenLines2);

            style1.Append(styleName1);
            style1.Append(primaryStyle1);
            style1.Append(rsid1);
            style1.Append(styleParagraphProperties1);

            var style2 = new Style { Type = StyleValues.Paragraph, StyleId = "Heading1" };
            var styleName2 = new StyleName { Val = "heading 1" };
            var basedOn1 = new BasedOn { Val = "Normal" };
            var nextParagraphStyle1 = new NextParagraphStyle { Val = "Normal" };
            var linkedStyle1 = new LinkedStyle { Val = "Heading1Char" };
            var uiPriority1 = new UIPriority { Val = 9 };
            var primaryStyle2 = new PrimaryStyle();
            var rsid2 = new Rsid { Val = "0016335E" };

            var styleParagraphProperties2 = new StyleParagraphProperties();
            var keepNext1 = new KeepNext();
            var keepLines1 = new KeepLines();
            var spacingBetweenLines3 = new SpacingBetweenLines { Before = "480" };
            var outlineLevel1 = new OutlineLevel { Val = 0 };

            styleParagraphProperties2.Append(keepNext1);
            styleParagraphProperties2.Append(keepLines1);
            styleParagraphProperties2.Append(spacingBetweenLines3);
            styleParagraphProperties2.Append(outlineLevel1);

            var styleRunProperties1 = new StyleRunProperties();
            var runFonts2 = new RunFonts
            {
                AsciiTheme = ThemeFontValues.MajorHighAnsi,
                HighAnsiTheme = ThemeFontValues.MajorHighAnsi,
                EastAsiaTheme = ThemeFontValues.MajorEastAsia,
                ComplexScriptTheme = ThemeFontValues.MajorBidi
            };
            var bold1 = new Bold();
            var boldComplexScript1 = new BoldComplexScript();
            var color1 = new Color { Val = "365F91", ThemeColor = ThemeColorValues.Accent1, ThemeShade = "BF" };
            var fontSize2 = new FontSize { Val = "28" };
            var fontSizeComplexScript2 = new FontSizeComplexScript { Val = "28" };

            styleRunProperties1.Append(runFonts2);
            styleRunProperties1.Append(bold1);
            styleRunProperties1.Append(boldComplexScript1);
            styleRunProperties1.Append(color1);
            styleRunProperties1.Append(fontSize2);
            styleRunProperties1.Append(fontSizeComplexScript2);

            style2.Append(styleName2);
            style2.Append(basedOn1);
            style2.Append(nextParagraphStyle1);
            style2.Append(linkedStyle1);
            style2.Append(uiPriority1);
            style2.Append(primaryStyle2);
            style2.Append(rsid2);
            style2.Append(styleParagraphProperties2);
            style2.Append(styleRunProperties1);

            var style3 = new Style { Type = StyleValues.Paragraph, StyleId = "Heading2" };
            var styleName3 = new StyleName { Val = "heading 2" };
            var basedOn2 = new BasedOn { Val = "Normal" };
            var nextParagraphStyle2 = new NextParagraphStyle { Val = "Normal" };
            var linkedStyle2 = new LinkedStyle { Val = "Heading2Char" };
            var uiPriority2 = new UIPriority { Val = 9 };
            var unhideWhenUsed1 = new UnhideWhenUsed();
            var primaryStyle3 = new PrimaryStyle();
            var rsid3 = new Rsid { Val = "0016335E" };

            var styleParagraphProperties3 = new StyleParagraphProperties();
            var keepNext2 = new KeepNext();
            var keepLines2 = new KeepLines();
            var spacingBetweenLines4 = new SpacingBetweenLines { Before = "200" };
            var outlineLevel2 = new OutlineLevel { Val = 1 };

            styleParagraphProperties3.Append(keepNext2);
            styleParagraphProperties3.Append(keepLines2);
            styleParagraphProperties3.Append(spacingBetweenLines4);
            styleParagraphProperties3.Append(outlineLevel2);

            var styleRunProperties2 = new StyleRunProperties();
            var runFonts3 = new RunFonts
            {
                AsciiTheme = ThemeFontValues.MajorHighAnsi,
                HighAnsiTheme = ThemeFontValues.MajorHighAnsi,
                EastAsiaTheme = ThemeFontValues.MajorEastAsia,
                ComplexScriptTheme = ThemeFontValues.MajorBidi
            };
            var bold2 = new Bold();
            var boldComplexScript2 = new BoldComplexScript();
            var color2 = new Color { Val = "4F81BD", ThemeColor = ThemeColorValues.Accent1 };
            var fontSize3 = new FontSize { Val = "26" };
            var fontSizeComplexScript3 = new FontSizeComplexScript { Val = "26" };

            styleRunProperties2.Append(runFonts3);
            styleRunProperties2.Append(bold2);
            styleRunProperties2.Append(boldComplexScript2);
            styleRunProperties2.Append(color2);
            styleRunProperties2.Append(fontSize3);
            styleRunProperties2.Append(fontSizeComplexScript3);

            style3.Append(styleName3);
            style3.Append(basedOn2);
            style3.Append(nextParagraphStyle2);
            style3.Append(linkedStyle2);
            style3.Append(uiPriority2);
            style3.Append(unhideWhenUsed1);
            style3.Append(primaryStyle3);
            style3.Append(rsid3);
            style3.Append(styleParagraphProperties3);
            style3.Append(styleRunProperties2);

            var style4 = new Style { Type = StyleValues.Paragraph, StyleId = "Heading3" };
            var styleName4 = new StyleName { Val = "heading 3" };
            var basedOn3 = new BasedOn { Val = "Normal" };
            var nextParagraphStyle3 = new NextParagraphStyle { Val = "Normal" };
            var linkedStyle3 = new LinkedStyle { Val = "Heading3Char" };
            var uiPriority3 = new UIPriority { Val = 9 };
            var unhideWhenUsed2 = new UnhideWhenUsed();
            var primaryStyle4 = new PrimaryStyle();
            var rsid4 = new Rsid { Val = "0016335E" };

            var styleParagraphProperties4 = new StyleParagraphProperties();
            var keepNext3 = new KeepNext();
            var keepLines3 = new KeepLines();
            var spacingBetweenLines5 = new SpacingBetweenLines { Before = "200" };
            var outlineLevel3 = new OutlineLevel { Val = 2 };

            styleParagraphProperties4.Append(keepNext3);
            styleParagraphProperties4.Append(keepLines3);
            styleParagraphProperties4.Append(spacingBetweenLines5);
            styleParagraphProperties4.Append(outlineLevel3);

            var styleRunProperties3 = new StyleRunProperties();
            var runFonts4 = new RunFonts
            {
                AsciiTheme = ThemeFontValues.MajorHighAnsi,
                HighAnsiTheme = ThemeFontValues.MajorHighAnsi,
                EastAsiaTheme = ThemeFontValues.MajorEastAsia,
                ComplexScriptTheme = ThemeFontValues.MajorBidi
            };
            var bold3 = new Bold();
            var boldComplexScript3 = new BoldComplexScript();
            var color3 = new Color { Val = "4F81BD", ThemeColor = ThemeColorValues.Accent1 };

            styleRunProperties3.Append(runFonts4);
            styleRunProperties3.Append(bold3);
            styleRunProperties3.Append(boldComplexScript3);
            styleRunProperties3.Append(color3);

            style4.Append(styleName4);
            style4.Append(basedOn3);
            style4.Append(nextParagraphStyle3);
            style4.Append(linkedStyle3);
            style4.Append(uiPriority3);
            style4.Append(unhideWhenUsed2);
            style4.Append(primaryStyle4);
            style4.Append(rsid4);
            style4.Append(styleParagraphProperties4);
            style4.Append(styleRunProperties3);

            var style5 = new Style { Type = StyleValues.Character, StyleId = "DefaultParagraphFont", Default = true };
            var styleName5 = new StyleName { Val = "Default Paragraph Font" };
            var uiPriority4 = new UIPriority { Val = 1 };
            var semiHidden1 = new SemiHidden();
            var unhideWhenUsed3 = new UnhideWhenUsed();

            style5.Append(styleName5);
            style5.Append(uiPriority4);
            style5.Append(semiHidden1);
            style5.Append(unhideWhenUsed3);

            var style6 = new Style { Type = StyleValues.Table, StyleId = "TableNormal", Default = true };
            var styleName6 = new StyleName { Val = "Normal Table" };
            var uiPriority5 = new UIPriority { Val = 99 };
            var semiHidden2 = new SemiHidden();
            var unhideWhenUsed4 = new UnhideWhenUsed();
            var primaryStyle5 = new PrimaryStyle();

            var styleTableProperties1 = new StyleTableProperties();
            var tableIndentation1 = new TableIndentation { Width = 0, Type = TableWidthUnitValues.Dxa };

            var tableCellMarginDefault1 = new TableCellMarginDefault();
            var topMargin1 = new TopMargin { Width = "0", Type = TableWidthUnitValues.Dxa };
            var tableCellLeftMargin1 = new TableCellLeftMargin { Width = 108, Type = TableWidthValues.Dxa };
            var bottomMargin1 = new BottomMargin { Width = "0", Type = TableWidthUnitValues.Dxa };
            var tableCellRightMargin1 = new TableCellRightMargin { Width = 108, Type = TableWidthValues.Dxa };

            tableCellMarginDefault1.Append(topMargin1);
            tableCellMarginDefault1.Append(tableCellLeftMargin1);
            tableCellMarginDefault1.Append(bottomMargin1);
            tableCellMarginDefault1.Append(tableCellRightMargin1);

            styleTableProperties1.Append(tableIndentation1);
            styleTableProperties1.Append(tableCellMarginDefault1);

            style6.Append(styleName6);
            style6.Append(uiPriority5);
            style6.Append(semiHidden2);
            style6.Append(unhideWhenUsed4);
            style6.Append(primaryStyle5);
            style6.Append(styleTableProperties1);

            var style7 = new Style { Type = StyleValues.Numbering, StyleId = "NoList", Default = true };
            var styleName7 = new StyleName { Val = "No List" };
            var uiPriority6 = new UIPriority { Val = 99 };
            var semiHidden3 = new SemiHidden();
            var unhideWhenUsed5 = new UnhideWhenUsed();

            style7.Append(styleName7);
            style7.Append(uiPriority6);
            style7.Append(semiHidden3);
            style7.Append(unhideWhenUsed5);

            var style8 = new Style { Type = StyleValues.Character, StyleId = "Heading1Char", CustomStyle = true };
            var styleName8 = new StyleName { Val = "Heading 1 Char" };
            var basedOn4 = new BasedOn { Val = "DefaultParagraphFont" };
            var linkedStyle4 = new LinkedStyle { Val = "Heading1" };
            var uiPriority7 = new UIPriority { Val = 9 };
            var rsid5 = new Rsid { Val = "0016335E" };

            var styleRunProperties4 = new StyleRunProperties();
            var runFonts5 = new RunFonts
            {
                AsciiTheme = ThemeFontValues.MajorHighAnsi,
                HighAnsiTheme = ThemeFontValues.MajorHighAnsi,
                EastAsiaTheme = ThemeFontValues.MajorEastAsia,
                ComplexScriptTheme = ThemeFontValues.MajorBidi
            };
            var bold4 = new Bold();
            var boldComplexScript4 = new BoldComplexScript();
            var color4 = new Color { Val = "365F91", ThemeColor = ThemeColorValues.Accent1, ThemeShade = "BF" };
            var fontSize4 = new FontSize { Val = "28" };
            var fontSizeComplexScript4 = new FontSizeComplexScript { Val = "28" };

            styleRunProperties4.Append(runFonts5);
            styleRunProperties4.Append(bold4);
            styleRunProperties4.Append(boldComplexScript4);
            styleRunProperties4.Append(color4);
            styleRunProperties4.Append(fontSize4);
            styleRunProperties4.Append(fontSizeComplexScript4);

            style8.Append(styleName8);
            style8.Append(basedOn4);
            style8.Append(linkedStyle4);
            style8.Append(uiPriority7);
            style8.Append(rsid5);
            style8.Append(styleRunProperties4);

            var style9 = new Style { Type = StyleValues.Character, StyleId = "Heading2Char", CustomStyle = true };
            var styleName9 = new StyleName { Val = "Heading 2 Char" };
            var basedOn5 = new BasedOn { Val = "DefaultParagraphFont" };
            var linkedStyle5 = new LinkedStyle { Val = "Heading2" };
            var uiPriority8 = new UIPriority { Val = 9 };
            var rsid6 = new Rsid { Val = "0016335E" };

            var styleRunProperties5 = new StyleRunProperties();
            var runFonts6 = new RunFonts
            {
                AsciiTheme = ThemeFontValues.MajorHighAnsi,
                HighAnsiTheme = ThemeFontValues.MajorHighAnsi,
                EastAsiaTheme = ThemeFontValues.MajorEastAsia,
                ComplexScriptTheme = ThemeFontValues.MajorBidi
            };
            var bold5 = new Bold();
            var boldComplexScript5 = new BoldComplexScript();
            var color5 = new Color { Val = "4F81BD", ThemeColor = ThemeColorValues.Accent1 };
            var fontSize5 = new FontSize { Val = "26" };
            var fontSizeComplexScript5 = new FontSizeComplexScript { Val = "26" };

            styleRunProperties5.Append(runFonts6);
            styleRunProperties5.Append(bold5);
            styleRunProperties5.Append(boldComplexScript5);
            styleRunProperties5.Append(color5);
            styleRunProperties5.Append(fontSize5);
            styleRunProperties5.Append(fontSizeComplexScript5);

            style9.Append(styleName9);
            style9.Append(basedOn5);
            style9.Append(linkedStyle5);
            style9.Append(uiPriority8);
            style9.Append(rsid6);
            style9.Append(styleRunProperties5);

            var style10 = new Style { Type = StyleValues.Character, StyleId = "Heading3Char", CustomStyle = true };
            var styleName10 = new StyleName { Val = "Heading 3 Char" };
            var basedOn6 = new BasedOn { Val = "DefaultParagraphFont" };
            var linkedStyle6 = new LinkedStyle { Val = "Heading3" };
            var uiPriority9 = new UIPriority { Val = 9 };
            var rsid7 = new Rsid { Val = "0016335E" };

            var styleRunProperties6 = new StyleRunProperties();
            var runFonts7 = new RunFonts
            {
                AsciiTheme = ThemeFontValues.MajorHighAnsi,
                HighAnsiTheme = ThemeFontValues.MajorHighAnsi,
                EastAsiaTheme = ThemeFontValues.MajorEastAsia,
                ComplexScriptTheme = ThemeFontValues.MajorBidi
            };
            var bold6 = new Bold();
            var boldComplexScript6 = new BoldComplexScript();
            var color6 = new Color { Val = "4F81BD", ThemeColor = ThemeColorValues.Accent1 };

            styleRunProperties6.Append(runFonts7);
            styleRunProperties6.Append(bold6);
            styleRunProperties6.Append(boldComplexScript6);
            styleRunProperties6.Append(color6);

            style10.Append(styleName10);
            style10.Append(basedOn6);
            style10.Append(linkedStyle6);
            style10.Append(uiPriority9);
            style10.Append(rsid7);
            style10.Append(styleRunProperties6);

            var style11 = new Style { Type = StyleValues.Table, StyleId = "TableGrid" };
            var styleName11 = new StyleName { Val = "Table Grid" };
            var basedOn7 = new BasedOn { Val = "TableNormal" };
            var uiPriority10 = new UIPriority { Val = 59 };
            var rsid8 = new Rsid { Val = "00BA40EF" };

            var styleParagraphProperties5 = new StyleParagraphProperties();
            var spacingBetweenLines6 = new SpacingBetweenLines
            { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            styleParagraphProperties5.Append(spacingBetweenLines6);

            var styleTableProperties2 = new StyleTableProperties();
            var tableIndentation2 = new TableIndentation { Width = 0, Type = TableWidthUnitValues.Dxa };

            var tableBorders1 = new TableBorders();
            var topBorder1 = new TopBorder { Val = BorderValues.Single, Color = "auto", Size = 4U, Space = 0U };
            var leftBorder1 = new LeftBorder { Val = BorderValues.Single, Color = "auto", Size = 4U, Space = 0U };
            var bottomBorder1 = new BottomBorder { Val = BorderValues.Single, Color = "auto", Size = 4U, Space = 0U };
            var rightBorder1 = new RightBorder { Val = BorderValues.Single, Color = "auto", Size = 4U, Space = 0U };
            var insideHorizontalBorder1 = new InsideHorizontalBorder
            { Val = BorderValues.Single, Color = "auto", Size = 4U, Space = 0U };
            var insideVerticalBorder1 = new InsideVerticalBorder
            { Val = BorderValues.Single, Color = "auto", Size = 4U, Space = 0U };

            tableBorders1.Append(topBorder1);
            tableBorders1.Append(leftBorder1);
            tableBorders1.Append(bottomBorder1);
            tableBorders1.Append(rightBorder1);
            tableBorders1.Append(insideHorizontalBorder1);
            tableBorders1.Append(insideVerticalBorder1);

            var tableCellMarginDefault2 = new TableCellMarginDefault();
            var topMargin2 = new TopMargin { Width = "0", Type = TableWidthUnitValues.Dxa };
            var tableCellLeftMargin2 = new TableCellLeftMargin { Width = 108, Type = TableWidthValues.Dxa };
            var bottomMargin2 = new BottomMargin { Width = "0", Type = TableWidthUnitValues.Dxa };
            var tableCellRightMargin2 = new TableCellRightMargin { Width = 108, Type = TableWidthValues.Dxa };

            tableCellMarginDefault2.Append(topMargin2);
            tableCellMarginDefault2.Append(tableCellLeftMargin2);
            tableCellMarginDefault2.Append(bottomMargin2);
            tableCellMarginDefault2.Append(tableCellRightMargin2);

            styleTableProperties2.Append(tableIndentation2);
            styleTableProperties2.Append(tableBorders1);
            styleTableProperties2.Append(tableCellMarginDefault2);

            style11.Append(styleName11);
            style11.Append(basedOn7);
            style11.Append(uiPriority10);
            style11.Append(rsid8);
            style11.Append(styleParagraphProperties5);
            style11.Append(styleTableProperties2);

            var style12 = new Style { Type = StyleValues.Table, StyleId = "LightList-Accent1" };
            var styleName12 = new StyleName { Val = "Light List Accent 1" };
            var basedOn8 = new BasedOn { Val = "TableNormal" };
            var uiPriority11 = new UIPriority { Val = 61 };
            var rsid9 = new Rsid { Val = "00BA40EF" };

            var styleParagraphProperties6 = new StyleParagraphProperties();
            var spacingBetweenLines7 = new SpacingBetweenLines
            { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            styleParagraphProperties6.Append(spacingBetweenLines7);

            var styleTableProperties3 = new StyleTableProperties();
            var tableStyleRowBandSize1 = new TableStyleRowBandSize { Val = 1 };
            var tableStyleColumnBandSize1 = new TableStyleColumnBandSize { Val = 1 };
            var tableIndentation3 = new TableIndentation { Width = 0, Type = TableWidthUnitValues.Dxa };

            var tableBorders2 = new TableBorders();
            var topBorder2 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var leftBorder2 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder2 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var rightBorder2 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };

            tableBorders2.Append(topBorder2);
            tableBorders2.Append(leftBorder2);
            tableBorders2.Append(bottomBorder2);
            tableBorders2.Append(rightBorder2);

            var tableCellMarginDefault3 = new TableCellMarginDefault();
            var topMargin3 = new TopMargin { Width = "0", Type = TableWidthUnitValues.Dxa };
            var tableCellLeftMargin3 = new TableCellLeftMargin { Width = 108, Type = TableWidthValues.Dxa };
            var bottomMargin3 = new BottomMargin { Width = "0", Type = TableWidthUnitValues.Dxa };
            var tableCellRightMargin3 = new TableCellRightMargin { Width = 108, Type = TableWidthValues.Dxa };

            tableCellMarginDefault3.Append(topMargin3);
            tableCellMarginDefault3.Append(tableCellLeftMargin3);
            tableCellMarginDefault3.Append(bottomMargin3);
            tableCellMarginDefault3.Append(tableCellRightMargin3);

            styleTableProperties3.Append(tableStyleRowBandSize1);
            styleTableProperties3.Append(tableStyleColumnBandSize1);
            styleTableProperties3.Append(tableIndentation3);
            styleTableProperties3.Append(tableBorders2);
            styleTableProperties3.Append(tableCellMarginDefault3);

            var tableStyleProperties1 = new TableStyleProperties { Type = TableStyleOverrideValues.FirstRow };

            var styleParagraphProperties7 = new StyleParagraphProperties();
            var spacingBetweenLines8 = new SpacingBetweenLines
            {
                Before = "0",
                After = "0",
                Line = "240",
                LineRule = LineSpacingRuleValues.Auto
            };

            styleParagraphProperties7.Append(spacingBetweenLines8);

            var runPropertiesBaseStyle2 = new RunPropertiesBaseStyle();
            var bold7 = new Bold();
            var boldComplexScript7 = new BoldComplexScript();
            var color7 = new Color { Val = "FFFFFF", ThemeColor = ThemeColorValues.Background1 };

            runPropertiesBaseStyle2.Append(bold7);
            runPropertiesBaseStyle2.Append(boldComplexScript7);
            runPropertiesBaseStyle2.Append(color7);
            var tableStyleConditionalFormattingTableProperties1 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties1 =
                new TableStyleConditionalFormattingTableCellProperties();
            var shading1 = new Shading
            {
                Val = ShadingPatternValues.Clear,
                Color = "auto",
                Fill = "4F81BD",
                ThemeFill = ThemeColorValues.Accent1
            };

            tableStyleConditionalFormattingTableCellProperties1.Append(shading1);

            tableStyleProperties1.Append(styleParagraphProperties7);
            tableStyleProperties1.Append(runPropertiesBaseStyle2);
            tableStyleProperties1.Append(tableStyleConditionalFormattingTableProperties1);
            tableStyleProperties1.Append(tableStyleConditionalFormattingTableCellProperties1);

            var tableStyleProperties2 = new TableStyleProperties { Type = TableStyleOverrideValues.LastRow };

            var styleParagraphProperties8 = new StyleParagraphProperties();
            var spacingBetweenLines9 = new SpacingBetweenLines
            {
                Before = "0",
                After = "0",
                Line = "240",
                LineRule = LineSpacingRuleValues.Auto
            };

            styleParagraphProperties8.Append(spacingBetweenLines9);

            var runPropertiesBaseStyle3 = new RunPropertiesBaseStyle();
            var bold8 = new Bold();
            var boldComplexScript8 = new BoldComplexScript();

            runPropertiesBaseStyle3.Append(bold8);
            runPropertiesBaseStyle3.Append(boldComplexScript8);
            var tableStyleConditionalFormattingTableProperties2 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties2 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders1 = new TableCellBorders();
            var topBorder3 = new TopBorder
            {
                Val = BorderValues.Double,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 6U,
                Space = 0U
            };
            var leftBorder3 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder3 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var rightBorder3 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };

            tableCellBorders1.Append(topBorder3);
            tableCellBorders1.Append(leftBorder3);
            tableCellBorders1.Append(bottomBorder3);
            tableCellBorders1.Append(rightBorder3);

            tableStyleConditionalFormattingTableCellProperties2.Append(tableCellBorders1);

            tableStyleProperties2.Append(styleParagraphProperties8);
            tableStyleProperties2.Append(runPropertiesBaseStyle3);
            tableStyleProperties2.Append(tableStyleConditionalFormattingTableProperties2);
            tableStyleProperties2.Append(tableStyleConditionalFormattingTableCellProperties2);

            var tableStyleProperties3 = new TableStyleProperties { Type = TableStyleOverrideValues.FirstColumn };

            var runPropertiesBaseStyle4 = new RunPropertiesBaseStyle();
            var bold9 = new Bold();
            var boldComplexScript9 = new BoldComplexScript();

            runPropertiesBaseStyle4.Append(bold9);
            runPropertiesBaseStyle4.Append(boldComplexScript9);

            tableStyleProperties3.Append(runPropertiesBaseStyle4);

            var tableStyleProperties4 = new TableStyleProperties { Type = TableStyleOverrideValues.LastColumn };

            var runPropertiesBaseStyle5 = new RunPropertiesBaseStyle();
            var bold10 = new Bold();
            var boldComplexScript10 = new BoldComplexScript();

            runPropertiesBaseStyle5.Append(bold10);
            runPropertiesBaseStyle5.Append(boldComplexScript10);

            tableStyleProperties4.Append(runPropertiesBaseStyle5);

            var tableStyleProperties5 = new TableStyleProperties { Type = TableStyleOverrideValues.Band1Vertical };
            var tableStyleConditionalFormattingTableProperties3 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties3 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders2 = new TableCellBorders();
            var topBorder4 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var leftBorder4 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder4 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var rightBorder4 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };

            tableCellBorders2.Append(topBorder4);
            tableCellBorders2.Append(leftBorder4);
            tableCellBorders2.Append(bottomBorder4);
            tableCellBorders2.Append(rightBorder4);

            tableStyleConditionalFormattingTableCellProperties3.Append(tableCellBorders2);

            tableStyleProperties5.Append(tableStyleConditionalFormattingTableProperties3);
            tableStyleProperties5.Append(tableStyleConditionalFormattingTableCellProperties3);

            var tableStyleProperties6 = new TableStyleProperties { Type = TableStyleOverrideValues.Band1Horizontal };
            var tableStyleConditionalFormattingTableProperties4 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties4 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders3 = new TableCellBorders();
            var topBorder5 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var leftBorder5 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder5 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var rightBorder5 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };

            tableCellBorders3.Append(topBorder5);
            tableCellBorders3.Append(leftBorder5);
            tableCellBorders3.Append(bottomBorder5);
            tableCellBorders3.Append(rightBorder5);

            tableStyleConditionalFormattingTableCellProperties4.Append(tableCellBorders3);

            tableStyleProperties6.Append(tableStyleConditionalFormattingTableProperties4);
            tableStyleProperties6.Append(tableStyleConditionalFormattingTableCellProperties4);

            style12.Append(styleName12);
            style12.Append(basedOn8);
            style12.Append(uiPriority11);
            style12.Append(rsid9);
            style12.Append(styleParagraphProperties6);
            style12.Append(styleTableProperties3);
            style12.Append(tableStyleProperties1);
            style12.Append(tableStyleProperties2);
            style12.Append(tableStyleProperties3);
            style12.Append(tableStyleProperties4);
            style12.Append(tableStyleProperties5);
            style12.Append(tableStyleProperties6);

            var style13 = new Style { Type = StyleValues.Table, StyleId = "MediumGrid3-Accent1" };
            var styleName13 = new StyleName { Val = "Medium Grid 3 Accent 1" };
            var basedOn9 = new BasedOn { Val = "TableNormal" };
            var uiPriority12 = new UIPriority { Val = 69 };
            var rsid10 = new Rsid { Val = "00BA40EF" };

            var styleParagraphProperties9 = new StyleParagraphProperties();
            var spacingBetweenLines10 = new SpacingBetweenLines
            { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            styleParagraphProperties9.Append(spacingBetweenLines10);

            var styleTableProperties4 = new StyleTableProperties();
            var tableStyleRowBandSize2 = new TableStyleRowBandSize { Val = 1 };
            var tableStyleColumnBandSize2 = new TableStyleColumnBandSize { Val = 1 };
            var tableIndentation4 = new TableIndentation { Width = 0, Type = TableWidthUnitValues.Dxa };

            var tableBorders3 = new TableBorders();
            var topBorder6 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var leftBorder6 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder6 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var rightBorder6 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var insideHorizontalBorder2 = new InsideHorizontalBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 6U,
                Space = 0U
            };
            var insideVerticalBorder2 = new InsideVerticalBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 6U,
                Space = 0U
            };

            tableBorders3.Append(topBorder6);
            tableBorders3.Append(leftBorder6);
            tableBorders3.Append(bottomBorder6);
            tableBorders3.Append(rightBorder6);
            tableBorders3.Append(insideHorizontalBorder2);
            tableBorders3.Append(insideVerticalBorder2);

            var tableCellMarginDefault4 = new TableCellMarginDefault();
            var topMargin4 = new TopMargin { Width = "0", Type = TableWidthUnitValues.Dxa };
            var tableCellLeftMargin4 = new TableCellLeftMargin { Width = 108, Type = TableWidthValues.Dxa };
            var bottomMargin4 = new BottomMargin { Width = "0", Type = TableWidthUnitValues.Dxa };
            var tableCellRightMargin4 = new TableCellRightMargin { Width = 108, Type = TableWidthValues.Dxa };

            tableCellMarginDefault4.Append(topMargin4);
            tableCellMarginDefault4.Append(tableCellLeftMargin4);
            tableCellMarginDefault4.Append(bottomMargin4);
            tableCellMarginDefault4.Append(tableCellRightMargin4);

            styleTableProperties4.Append(tableStyleRowBandSize2);
            styleTableProperties4.Append(tableStyleColumnBandSize2);
            styleTableProperties4.Append(tableIndentation4);
            styleTableProperties4.Append(tableBorders3);
            styleTableProperties4.Append(tableCellMarginDefault4);

            var styleTableCellProperties1 = new StyleTableCellProperties();
            var shading2 = new Shading
            {
                Val = ShadingPatternValues.Clear,
                Color = "auto",
                Fill = "D3DFEE",
                ThemeFill = ThemeColorValues.Accent1,
                ThemeFillTint = "3F"
            };

            styleTableCellProperties1.Append(shading2);

            var tableStyleProperties7 = new TableStyleProperties { Type = TableStyleOverrideValues.FirstRow };

            var runPropertiesBaseStyle6 = new RunPropertiesBaseStyle();
            var bold11 = new Bold();
            var boldComplexScript11 = new BoldComplexScript();
            var italic1 = new Italic { Val = false };
            var italicComplexScript1 = new ItalicComplexScript { Val = false };
            var color8 = new Color { Val = "FFFFFF", ThemeColor = ThemeColorValues.Background1 };

            runPropertiesBaseStyle6.Append(bold11);
            runPropertiesBaseStyle6.Append(boldComplexScript11);
            runPropertiesBaseStyle6.Append(italic1);
            runPropertiesBaseStyle6.Append(italicComplexScript1);
            runPropertiesBaseStyle6.Append(color8);
            var tableStyleConditionalFormattingTableProperties5 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties5 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders4 = new TableCellBorders();
            var topBorder7 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var leftBorder7 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder7 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 24U,
                Space = 0U
            };
            var rightBorder7 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var insideHorizontalBorder3 = new InsideHorizontalBorder { Val = BorderValues.Nil };
            var insideVerticalBorder3 = new InsideVerticalBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };

            tableCellBorders4.Append(topBorder7);
            tableCellBorders4.Append(leftBorder7);
            tableCellBorders4.Append(bottomBorder7);
            tableCellBorders4.Append(rightBorder7);
            tableCellBorders4.Append(insideHorizontalBorder3);
            tableCellBorders4.Append(insideVerticalBorder3);
            var shading3 = new Shading
            {
                Val = ShadingPatternValues.Clear,
                Color = "auto",
                Fill = "4F81BD",
                ThemeFill = ThemeColorValues.Accent1
            };

            tableStyleConditionalFormattingTableCellProperties5.Append(tableCellBorders4);
            tableStyleConditionalFormattingTableCellProperties5.Append(shading3);

            tableStyleProperties7.Append(runPropertiesBaseStyle6);
            tableStyleProperties7.Append(tableStyleConditionalFormattingTableProperties5);
            tableStyleProperties7.Append(tableStyleConditionalFormattingTableCellProperties5);

            var tableStyleProperties8 = new TableStyleProperties { Type = TableStyleOverrideValues.LastRow };

            var runPropertiesBaseStyle7 = new RunPropertiesBaseStyle();
            var bold12 = new Bold();
            var boldComplexScript12 = new BoldComplexScript();
            var italic2 = new Italic { Val = false };
            var italicComplexScript2 = new ItalicComplexScript { Val = false };
            var color9 = new Color { Val = "FFFFFF", ThemeColor = ThemeColorValues.Background1 };

            runPropertiesBaseStyle7.Append(bold12);
            runPropertiesBaseStyle7.Append(boldComplexScript12);
            runPropertiesBaseStyle7.Append(italic2);
            runPropertiesBaseStyle7.Append(italicComplexScript2);
            runPropertiesBaseStyle7.Append(color9);
            var tableStyleConditionalFormattingTableProperties6 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties6 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders5 = new TableCellBorders();
            var topBorder8 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 24U,
                Space = 0U
            };
            var leftBorder8 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder8 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var rightBorder8 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var insideHorizontalBorder4 = new InsideHorizontalBorder { Val = BorderValues.Nil };
            var insideVerticalBorder4 = new InsideVerticalBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };

            tableCellBorders5.Append(topBorder8);
            tableCellBorders5.Append(leftBorder8);
            tableCellBorders5.Append(bottomBorder8);
            tableCellBorders5.Append(rightBorder8);
            tableCellBorders5.Append(insideHorizontalBorder4);
            tableCellBorders5.Append(insideVerticalBorder4);
            var shading4 = new Shading
            {
                Val = ShadingPatternValues.Clear,
                Color = "auto",
                Fill = "4F81BD",
                ThemeFill = ThemeColorValues.Accent1
            };

            tableStyleConditionalFormattingTableCellProperties6.Append(tableCellBorders5);
            tableStyleConditionalFormattingTableCellProperties6.Append(shading4);

            tableStyleProperties8.Append(runPropertiesBaseStyle7);
            tableStyleProperties8.Append(tableStyleConditionalFormattingTableProperties6);
            tableStyleProperties8.Append(tableStyleConditionalFormattingTableCellProperties6);

            var tableStyleProperties9 = new TableStyleProperties { Type = TableStyleOverrideValues.FirstColumn };

            var runPropertiesBaseStyle8 = new RunPropertiesBaseStyle();
            var bold13 = new Bold();
            var boldComplexScript13 = new BoldComplexScript();
            var italic3 = new Italic { Val = false };
            var italicComplexScript3 = new ItalicComplexScript { Val = false };
            var color10 = new Color { Val = "FFFFFF", ThemeColor = ThemeColorValues.Background1 };

            runPropertiesBaseStyle8.Append(bold13);
            runPropertiesBaseStyle8.Append(boldComplexScript13);
            runPropertiesBaseStyle8.Append(italic3);
            runPropertiesBaseStyle8.Append(italicComplexScript3);
            runPropertiesBaseStyle8.Append(color10);
            var tableStyleConditionalFormattingTableProperties7 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties7 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders6 = new TableCellBorders();
            var leftBorder9 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var rightBorder9 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 24U,
                Space = 0U
            };
            var insideHorizontalBorder5 = new InsideHorizontalBorder { Val = BorderValues.Nil };
            var insideVerticalBorder5 = new InsideVerticalBorder { Val = BorderValues.Nil };

            tableCellBorders6.Append(leftBorder9);
            tableCellBorders6.Append(rightBorder9);
            tableCellBorders6.Append(insideHorizontalBorder5);
            tableCellBorders6.Append(insideVerticalBorder5);
            var shading5 = new Shading
            {
                Val = ShadingPatternValues.Clear,
                Color = "auto",
                Fill = "4F81BD",
                ThemeFill = ThemeColorValues.Accent1
            };

            tableStyleConditionalFormattingTableCellProperties7.Append(tableCellBorders6);
            tableStyleConditionalFormattingTableCellProperties7.Append(shading5);

            tableStyleProperties9.Append(runPropertiesBaseStyle8);
            tableStyleProperties9.Append(tableStyleConditionalFormattingTableProperties7);
            tableStyleProperties9.Append(tableStyleConditionalFormattingTableCellProperties7);

            var tableStyleProperties10 = new TableStyleProperties { Type = TableStyleOverrideValues.LastColumn };

            var runPropertiesBaseStyle9 = new RunPropertiesBaseStyle();
            var bold14 = new Bold();
            var boldComplexScript14 = new BoldComplexScript();
            var italic4 = new Italic { Val = false };
            var italicComplexScript4 = new ItalicComplexScript { Val = false };
            var color11 = new Color { Val = "FFFFFF", ThemeColor = ThemeColorValues.Background1 };

            runPropertiesBaseStyle9.Append(bold14);
            runPropertiesBaseStyle9.Append(boldComplexScript14);
            runPropertiesBaseStyle9.Append(italic4);
            runPropertiesBaseStyle9.Append(italicComplexScript4);
            runPropertiesBaseStyle9.Append(color11);
            var tableStyleConditionalFormattingTableProperties8 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties8 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders7 = new TableCellBorders();
            var topBorder9 = new TopBorder { Val = BorderValues.Nil };
            var leftBorder10 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 24U,
                Space = 0U
            };
            var bottomBorder9 = new BottomBorder { Val = BorderValues.Nil };
            var rightBorder10 = new RightBorder { Val = BorderValues.Nil };
            var insideHorizontalBorder6 = new InsideHorizontalBorder { Val = BorderValues.Nil };
            var insideVerticalBorder6 = new InsideVerticalBorder { Val = BorderValues.Nil };

            tableCellBorders7.Append(topBorder9);
            tableCellBorders7.Append(leftBorder10);
            tableCellBorders7.Append(bottomBorder9);
            tableCellBorders7.Append(rightBorder10);
            tableCellBorders7.Append(insideHorizontalBorder6);
            tableCellBorders7.Append(insideVerticalBorder6);
            var shading6 = new Shading
            {
                Val = ShadingPatternValues.Clear,
                Color = "auto",
                Fill = "4F81BD",
                ThemeFill = ThemeColorValues.Accent1
            };

            tableStyleConditionalFormattingTableCellProperties8.Append(tableCellBorders7);
            tableStyleConditionalFormattingTableCellProperties8.Append(shading6);

            tableStyleProperties10.Append(runPropertiesBaseStyle9);
            tableStyleProperties10.Append(tableStyleConditionalFormattingTableProperties8);
            tableStyleProperties10.Append(tableStyleConditionalFormattingTableCellProperties8);

            var tableStyleProperties11 = new TableStyleProperties { Type = TableStyleOverrideValues.Band1Vertical };
            var tableStyleConditionalFormattingTableProperties9 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties9 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders8 = new TableCellBorders();
            var topBorder10 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var leftBorder11 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder10 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var rightBorder11 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var insideHorizontalBorder7 = new InsideHorizontalBorder { Val = BorderValues.Nil };
            var insideVerticalBorder7 = new InsideVerticalBorder { Val = BorderValues.Nil };

            tableCellBorders8.Append(topBorder10);
            tableCellBorders8.Append(leftBorder11);
            tableCellBorders8.Append(bottomBorder10);
            tableCellBorders8.Append(rightBorder11);
            tableCellBorders8.Append(insideHorizontalBorder7);
            tableCellBorders8.Append(insideVerticalBorder7);
            var shading7 = new Shading
            {
                Val = ShadingPatternValues.Clear,
                Color = "auto",
                Fill = "A7BFDE",
                ThemeFill = ThemeColorValues.Accent1,
                ThemeFillTint = "7F"
            };

            tableStyleConditionalFormattingTableCellProperties9.Append(tableCellBorders8);
            tableStyleConditionalFormattingTableCellProperties9.Append(shading7);

            tableStyleProperties11.Append(tableStyleConditionalFormattingTableProperties9);
            tableStyleProperties11.Append(tableStyleConditionalFormattingTableCellProperties9);

            var tableStyleProperties12 = new TableStyleProperties { Type = TableStyleOverrideValues.Band1Horizontal };
            var tableStyleConditionalFormattingTableProperties10 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties10 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders9 = new TableCellBorders();
            var topBorder11 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var leftBorder12 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder11 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var rightBorder12 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var insideHorizontalBorder8 = new InsideHorizontalBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };
            var insideVerticalBorder8 = new InsideVerticalBorder
            {
                Val = BorderValues.Single,
                Color = "FFFFFF",
                ThemeColor = ThemeColorValues.Background1,
                Size = 8U,
                Space = 0U
            };

            tableCellBorders9.Append(topBorder11);
            tableCellBorders9.Append(leftBorder12);
            tableCellBorders9.Append(bottomBorder11);
            tableCellBorders9.Append(rightBorder12);
            tableCellBorders9.Append(insideHorizontalBorder8);
            tableCellBorders9.Append(insideVerticalBorder8);
            var shading8 = new Shading
            {
                Val = ShadingPatternValues.Clear,
                Color = "auto",
                Fill = "A7BFDE",
                ThemeFill = ThemeColorValues.Accent1,
                ThemeFillTint = "7F"
            };

            tableStyleConditionalFormattingTableCellProperties10.Append(tableCellBorders9);
            tableStyleConditionalFormattingTableCellProperties10.Append(shading8);

            tableStyleProperties12.Append(tableStyleConditionalFormattingTableProperties10);
            tableStyleProperties12.Append(tableStyleConditionalFormattingTableCellProperties10);

            style13.Append(styleName13);
            style13.Append(basedOn9);
            style13.Append(uiPriority12);
            style13.Append(rsid10);
            style13.Append(styleParagraphProperties9);
            style13.Append(styleTableProperties4);
            style13.Append(styleTableCellProperties1);
            style13.Append(tableStyleProperties7);
            style13.Append(tableStyleProperties8);
            style13.Append(tableStyleProperties9);
            style13.Append(tableStyleProperties10);
            style13.Append(tableStyleProperties11);
            style13.Append(tableStyleProperties12);

            var style14 = new Style { Type = StyleValues.Table, StyleId = "MediumShading1-Accent1" };
            var styleName14 = new StyleName { Val = "Medium Shading 1 Accent 1" };
            var basedOn10 = new BasedOn { Val = "TableNormal" };
            var uiPriority13 = new UIPriority { Val = 63 };
            var rsid11 = new Rsid { Val = "00BA40EF" };

            var styleParagraphProperties10 = new StyleParagraphProperties();
            var spacingBetweenLines11 = new SpacingBetweenLines
            { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            styleParagraphProperties10.Append(spacingBetweenLines11);

            var styleTableProperties5 = new StyleTableProperties();
            var tableStyleRowBandSize3 = new TableStyleRowBandSize { Val = 1 };
            var tableStyleColumnBandSize3 = new TableStyleColumnBandSize { Val = 1 };
            var tableIndentation5 = new TableIndentation { Width = 0, Type = TableWidthUnitValues.Dxa };

            var tableBorders4 = new TableBorders();
            var topBorder12 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "7BA0CD",
                ThemeColor = ThemeColorValues.Accent1,
                ThemeTint = "BF",
                Size = 8U,
                Space = 0U
            };
            var leftBorder13 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "7BA0CD",
                ThemeColor = ThemeColorValues.Accent1,
                ThemeTint = "BF",
                Size = 8U,
                Space = 0U
            };
            var bottomBorder12 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "7BA0CD",
                ThemeColor = ThemeColorValues.Accent1,
                ThemeTint = "BF",
                Size = 8U,
                Space = 0U
            };
            var rightBorder13 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "7BA0CD",
                ThemeColor = ThemeColorValues.Accent1,
                ThemeTint = "BF",
                Size = 8U,
                Space = 0U
            };
            var insideHorizontalBorder9 = new InsideHorizontalBorder
            {
                Val = BorderValues.Single,
                Color = "7BA0CD",
                ThemeColor = ThemeColorValues.Accent1,
                ThemeTint = "BF",
                Size = 8U,
                Space = 0U
            };

            tableBorders4.Append(topBorder12);
            tableBorders4.Append(leftBorder13);
            tableBorders4.Append(bottomBorder12);
            tableBorders4.Append(rightBorder13);
            tableBorders4.Append(insideHorizontalBorder9);

            var tableCellMarginDefault5 = new TableCellMarginDefault();
            var topMargin5 = new TopMargin { Width = "0", Type = TableWidthUnitValues.Dxa };
            var tableCellLeftMargin5 = new TableCellLeftMargin { Width = 108, Type = TableWidthValues.Dxa };
            var bottomMargin5 = new BottomMargin { Width = "0", Type = TableWidthUnitValues.Dxa };
            var tableCellRightMargin5 = new TableCellRightMargin { Width = 108, Type = TableWidthValues.Dxa };

            tableCellMarginDefault5.Append(topMargin5);
            tableCellMarginDefault5.Append(tableCellLeftMargin5);
            tableCellMarginDefault5.Append(bottomMargin5);
            tableCellMarginDefault5.Append(tableCellRightMargin5);

            styleTableProperties5.Append(tableStyleRowBandSize3);
            styleTableProperties5.Append(tableStyleColumnBandSize3);
            styleTableProperties5.Append(tableIndentation5);
            styleTableProperties5.Append(tableBorders4);
            styleTableProperties5.Append(tableCellMarginDefault5);

            var tableStyleProperties13 = new TableStyleProperties { Type = TableStyleOverrideValues.FirstRow };

            var styleParagraphProperties11 = new StyleParagraphProperties();
            var spacingBetweenLines12 = new SpacingBetweenLines
            {
                Before = "0",
                After = "0",
                Line = "240",
                LineRule = LineSpacingRuleValues.Auto
            };

            styleParagraphProperties11.Append(spacingBetweenLines12);

            var runPropertiesBaseStyle10 = new RunPropertiesBaseStyle();
            var bold15 = new Bold();
            var boldComplexScript15 = new BoldComplexScript();
            var color12 = new Color { Val = "FFFFFF", ThemeColor = ThemeColorValues.Background1 };

            runPropertiesBaseStyle10.Append(bold15);
            runPropertiesBaseStyle10.Append(boldComplexScript15);
            runPropertiesBaseStyle10.Append(color12);
            var tableStyleConditionalFormattingTableProperties11 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties11 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders10 = new TableCellBorders();
            var topBorder13 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "7BA0CD",
                ThemeColor = ThemeColorValues.Accent1,
                ThemeTint = "BF",
                Size = 8U,
                Space = 0U
            };
            var leftBorder14 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "7BA0CD",
                ThemeColor = ThemeColorValues.Accent1,
                ThemeTint = "BF",
                Size = 8U,
                Space = 0U
            };
            var bottomBorder13 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "7BA0CD",
                ThemeColor = ThemeColorValues.Accent1,
                ThemeTint = "BF",
                Size = 8U,
                Space = 0U
            };
            var rightBorder14 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "7BA0CD",
                ThemeColor = ThemeColorValues.Accent1,
                ThemeTint = "BF",
                Size = 8U,
                Space = 0U
            };
            var insideHorizontalBorder10 = new InsideHorizontalBorder { Val = BorderValues.Nil };
            var insideVerticalBorder9 = new InsideVerticalBorder { Val = BorderValues.Nil };

            tableCellBorders10.Append(topBorder13);
            tableCellBorders10.Append(leftBorder14);
            tableCellBorders10.Append(bottomBorder13);
            tableCellBorders10.Append(rightBorder14);
            tableCellBorders10.Append(insideHorizontalBorder10);
            tableCellBorders10.Append(insideVerticalBorder9);
            var shading9 = new Shading
            {
                Val = ShadingPatternValues.Clear,
                Color = "auto",
                Fill = "4F81BD",
                ThemeFill = ThemeColorValues.Accent1
            };

            tableStyleConditionalFormattingTableCellProperties11.Append(tableCellBorders10);
            tableStyleConditionalFormattingTableCellProperties11.Append(shading9);

            tableStyleProperties13.Append(styleParagraphProperties11);
            tableStyleProperties13.Append(runPropertiesBaseStyle10);
            tableStyleProperties13.Append(tableStyleConditionalFormattingTableProperties11);
            tableStyleProperties13.Append(tableStyleConditionalFormattingTableCellProperties11);

            var tableStyleProperties14 = new TableStyleProperties { Type = TableStyleOverrideValues.LastRow };

            var styleParagraphProperties12 = new StyleParagraphProperties();
            var spacingBetweenLines13 = new SpacingBetweenLines
            {
                Before = "0",
                After = "0",
                Line = "240",
                LineRule = LineSpacingRuleValues.Auto
            };

            styleParagraphProperties12.Append(spacingBetweenLines13);

            var runPropertiesBaseStyle11 = new RunPropertiesBaseStyle();
            var bold16 = new Bold();
            var boldComplexScript16 = new BoldComplexScript();

            runPropertiesBaseStyle11.Append(bold16);
            runPropertiesBaseStyle11.Append(boldComplexScript16);
            var tableStyleConditionalFormattingTableProperties12 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties12 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders11 = new TableCellBorders();
            var topBorder14 = new TopBorder
            {
                Val = BorderValues.Double,
                Color = "7BA0CD",
                ThemeColor = ThemeColorValues.Accent1,
                ThemeTint = "BF",
                Size = 6U,
                Space = 0U
            };
            var leftBorder15 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "7BA0CD",
                ThemeColor = ThemeColorValues.Accent1,
                ThemeTint = "BF",
                Size = 8U,
                Space = 0U
            };
            var bottomBorder14 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "7BA0CD",
                ThemeColor = ThemeColorValues.Accent1,
                ThemeTint = "BF",
                Size = 8U,
                Space = 0U
            };
            var rightBorder15 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "7BA0CD",
                ThemeColor = ThemeColorValues.Accent1,
                ThemeTint = "BF",
                Size = 8U,
                Space = 0U
            };
            var insideHorizontalBorder11 = new InsideHorizontalBorder { Val = BorderValues.Nil };
            var insideVerticalBorder10 = new InsideVerticalBorder { Val = BorderValues.Nil };

            tableCellBorders11.Append(topBorder14);
            tableCellBorders11.Append(leftBorder15);
            tableCellBorders11.Append(bottomBorder14);
            tableCellBorders11.Append(rightBorder15);
            tableCellBorders11.Append(insideHorizontalBorder11);
            tableCellBorders11.Append(insideVerticalBorder10);

            tableStyleConditionalFormattingTableCellProperties12.Append(tableCellBorders11);

            tableStyleProperties14.Append(styleParagraphProperties12);
            tableStyleProperties14.Append(runPropertiesBaseStyle11);
            tableStyleProperties14.Append(tableStyleConditionalFormattingTableProperties12);
            tableStyleProperties14.Append(tableStyleConditionalFormattingTableCellProperties12);

            var tableStyleProperties15 = new TableStyleProperties { Type = TableStyleOverrideValues.FirstColumn };

            var runPropertiesBaseStyle12 = new RunPropertiesBaseStyle();
            var bold17 = new Bold();
            var boldComplexScript17 = new BoldComplexScript();

            runPropertiesBaseStyle12.Append(bold17);
            runPropertiesBaseStyle12.Append(boldComplexScript17);

            tableStyleProperties15.Append(runPropertiesBaseStyle12);

            var tableStyleProperties16 = new TableStyleProperties { Type = TableStyleOverrideValues.LastColumn };

            var runPropertiesBaseStyle13 = new RunPropertiesBaseStyle();
            var bold18 = new Bold();
            var boldComplexScript18 = new BoldComplexScript();

            runPropertiesBaseStyle13.Append(bold18);
            runPropertiesBaseStyle13.Append(boldComplexScript18);

            tableStyleProperties16.Append(runPropertiesBaseStyle13);

            var tableStyleProperties17 = new TableStyleProperties { Type = TableStyleOverrideValues.Band1Vertical };
            var tableStyleConditionalFormattingTableProperties13 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties13 =
                new TableStyleConditionalFormattingTableCellProperties();
            var shading10 = new Shading
            {
                Val = ShadingPatternValues.Clear,
                Color = "auto",
                Fill = "D3DFEE",
                ThemeFill = ThemeColorValues.Accent1,
                ThemeFillTint = "3F"
            };

            tableStyleConditionalFormattingTableCellProperties13.Append(shading10);

            tableStyleProperties17.Append(tableStyleConditionalFormattingTableProperties13);
            tableStyleProperties17.Append(tableStyleConditionalFormattingTableCellProperties13);

            var tableStyleProperties18 = new TableStyleProperties { Type = TableStyleOverrideValues.Band1Horizontal };
            var tableStyleConditionalFormattingTableProperties14 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties14 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders12 = new TableCellBorders();
            var insideHorizontalBorder12 = new InsideHorizontalBorder { Val = BorderValues.Nil };
            var insideVerticalBorder11 = new InsideVerticalBorder { Val = BorderValues.Nil };

            tableCellBorders12.Append(insideHorizontalBorder12);
            tableCellBorders12.Append(insideVerticalBorder11);
            var shading11 = new Shading
            {
                Val = ShadingPatternValues.Clear,
                Color = "auto",
                Fill = "D3DFEE",
                ThemeFill = ThemeColorValues.Accent1,
                ThemeFillTint = "3F"
            };

            tableStyleConditionalFormattingTableCellProperties14.Append(tableCellBorders12);
            tableStyleConditionalFormattingTableCellProperties14.Append(shading11);

            tableStyleProperties18.Append(tableStyleConditionalFormattingTableProperties14);
            tableStyleProperties18.Append(tableStyleConditionalFormattingTableCellProperties14);

            var tableStyleProperties19 = new TableStyleProperties { Type = TableStyleOverrideValues.Band2Horizontal };
            var tableStyleConditionalFormattingTableProperties15 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties15 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders13 = new TableCellBorders();
            var insideHorizontalBorder13 = new InsideHorizontalBorder { Val = BorderValues.Nil };
            var insideVerticalBorder12 = new InsideVerticalBorder { Val = BorderValues.Nil };

            tableCellBorders13.Append(insideHorizontalBorder13);
            tableCellBorders13.Append(insideVerticalBorder12);

            tableStyleConditionalFormattingTableCellProperties15.Append(tableCellBorders13);

            tableStyleProperties19.Append(tableStyleConditionalFormattingTableProperties15);
            tableStyleProperties19.Append(tableStyleConditionalFormattingTableCellProperties15);

            style14.Append(styleName14);
            style14.Append(basedOn10);
            style14.Append(uiPriority13);
            style14.Append(rsid11);
            style14.Append(styleParagraphProperties10);
            style14.Append(styleTableProperties5);
            style14.Append(tableStyleProperties13);
            style14.Append(tableStyleProperties14);
            style14.Append(tableStyleProperties15);
            style14.Append(tableStyleProperties16);
            style14.Append(tableStyleProperties17);
            style14.Append(tableStyleProperties18);
            style14.Append(tableStyleProperties19);

            var style15 = new Style { Type = StyleValues.Table, StyleId = "LightGrid-Accent1" };
            var styleName15 = new StyleName { Val = "Light Grid Accent 1" };
            var basedOn11 = new BasedOn { Val = "TableNormal" };
            var uiPriority14 = new UIPriority { Val = 62 };
            var rsid12 = new Rsid { Val = "00BA40EF" };

            var styleParagraphProperties13 = new StyleParagraphProperties();
            var spacingBetweenLines14 = new SpacingBetweenLines
            { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            styleParagraphProperties13.Append(spacingBetweenLines14);

            var styleTableProperties6 = new StyleTableProperties();
            var tableStyleRowBandSize4 = new TableStyleRowBandSize { Val = 1 };
            var tableStyleColumnBandSize4 = new TableStyleColumnBandSize { Val = 1 };
            var tableIndentation6 = new TableIndentation { Width = 0, Type = TableWidthUnitValues.Dxa };

            var tableBorders5 = new TableBorders();
            var topBorder15 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var leftBorder16 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder15 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var rightBorder16 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var insideHorizontalBorder14 = new InsideHorizontalBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var insideVerticalBorder13 = new InsideVerticalBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };

            tableBorders5.Append(topBorder15);
            tableBorders5.Append(leftBorder16);
            tableBorders5.Append(bottomBorder15);
            tableBorders5.Append(rightBorder16);
            tableBorders5.Append(insideHorizontalBorder14);
            tableBorders5.Append(insideVerticalBorder13);

            var tableCellMarginDefault6 = new TableCellMarginDefault();
            var topMargin6 = new TopMargin { Width = "0", Type = TableWidthUnitValues.Dxa };
            var tableCellLeftMargin6 = new TableCellLeftMargin { Width = 108, Type = TableWidthValues.Dxa };
            var bottomMargin6 = new BottomMargin { Width = "0", Type = TableWidthUnitValues.Dxa };
            var tableCellRightMargin6 = new TableCellRightMargin { Width = 108, Type = TableWidthValues.Dxa };

            tableCellMarginDefault6.Append(topMargin6);
            tableCellMarginDefault6.Append(tableCellLeftMargin6);
            tableCellMarginDefault6.Append(bottomMargin6);
            tableCellMarginDefault6.Append(tableCellRightMargin6);

            styleTableProperties6.Append(tableStyleRowBandSize4);
            styleTableProperties6.Append(tableStyleColumnBandSize4);
            styleTableProperties6.Append(tableIndentation6);
            styleTableProperties6.Append(tableBorders5);
            styleTableProperties6.Append(tableCellMarginDefault6);

            var tableStyleProperties20 = new TableStyleProperties { Type = TableStyleOverrideValues.FirstRow };

            var styleParagraphProperties14 = new StyleParagraphProperties();
            var spacingBetweenLines15 = new SpacingBetweenLines
            {
                Before = "0",
                After = "0",
                Line = "240",
                LineRule = LineSpacingRuleValues.Auto
            };

            styleParagraphProperties14.Append(spacingBetweenLines15);

            var runPropertiesBaseStyle14 = new RunPropertiesBaseStyle();
            var runFonts8 = new RunFonts
            {
                AsciiTheme = ThemeFontValues.MajorHighAnsi,
                HighAnsiTheme = ThemeFontValues.MajorHighAnsi,
                EastAsiaTheme = ThemeFontValues.MajorEastAsia,
                ComplexScriptTheme = ThemeFontValues.MajorBidi
            };
            var bold19 = new Bold();
            var boldComplexScript19 = new BoldComplexScript();

            runPropertiesBaseStyle14.Append(runFonts8);
            runPropertiesBaseStyle14.Append(bold19);
            runPropertiesBaseStyle14.Append(boldComplexScript19);
            var tableStyleConditionalFormattingTableProperties16 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties16 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders14 = new TableCellBorders();
            var topBorder16 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var leftBorder17 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder16 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 18U,
                Space = 0U
            };
            var rightBorder17 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var insideHorizontalBorder15 = new InsideHorizontalBorder { Val = BorderValues.Nil };
            var insideVerticalBorder14 = new InsideVerticalBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };

            tableCellBorders14.Append(topBorder16);
            tableCellBorders14.Append(leftBorder17);
            tableCellBorders14.Append(bottomBorder16);
            tableCellBorders14.Append(rightBorder17);
            tableCellBorders14.Append(insideHorizontalBorder15);
            tableCellBorders14.Append(insideVerticalBorder14);

            tableStyleConditionalFormattingTableCellProperties16.Append(tableCellBorders14);

            tableStyleProperties20.Append(styleParagraphProperties14);
            tableStyleProperties20.Append(runPropertiesBaseStyle14);
            tableStyleProperties20.Append(tableStyleConditionalFormattingTableProperties16);
            tableStyleProperties20.Append(tableStyleConditionalFormattingTableCellProperties16);

            var tableStyleProperties21 = new TableStyleProperties { Type = TableStyleOverrideValues.LastRow };

            var styleParagraphProperties15 = new StyleParagraphProperties();
            var spacingBetweenLines16 = new SpacingBetweenLines
            {
                Before = "0",
                After = "0",
                Line = "240",
                LineRule = LineSpacingRuleValues.Auto
            };

            styleParagraphProperties15.Append(spacingBetweenLines16);

            var runPropertiesBaseStyle15 = new RunPropertiesBaseStyle();
            var runFonts9 = new RunFonts
            {
                AsciiTheme = ThemeFontValues.MajorHighAnsi,
                HighAnsiTheme = ThemeFontValues.MajorHighAnsi,
                EastAsiaTheme = ThemeFontValues.MajorEastAsia,
                ComplexScriptTheme = ThemeFontValues.MajorBidi
            };
            var bold20 = new Bold();
            var boldComplexScript20 = new BoldComplexScript();

            runPropertiesBaseStyle15.Append(runFonts9);
            runPropertiesBaseStyle15.Append(bold20);
            runPropertiesBaseStyle15.Append(boldComplexScript20);
            var tableStyleConditionalFormattingTableProperties17 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties17 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders15 = new TableCellBorders();
            var topBorder17 = new TopBorder
            {
                Val = BorderValues.Double,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 6U,
                Space = 0U
            };
            var leftBorder18 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder17 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var rightBorder18 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var insideHorizontalBorder16 = new InsideHorizontalBorder { Val = BorderValues.Nil };
            var insideVerticalBorder15 = new InsideVerticalBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };

            tableCellBorders15.Append(topBorder17);
            tableCellBorders15.Append(leftBorder18);
            tableCellBorders15.Append(bottomBorder17);
            tableCellBorders15.Append(rightBorder18);
            tableCellBorders15.Append(insideHorizontalBorder16);
            tableCellBorders15.Append(insideVerticalBorder15);

            tableStyleConditionalFormattingTableCellProperties17.Append(tableCellBorders15);

            tableStyleProperties21.Append(styleParagraphProperties15);
            tableStyleProperties21.Append(runPropertiesBaseStyle15);
            tableStyleProperties21.Append(tableStyleConditionalFormattingTableProperties17);
            tableStyleProperties21.Append(tableStyleConditionalFormattingTableCellProperties17);

            var tableStyleProperties22 = new TableStyleProperties { Type = TableStyleOverrideValues.FirstColumn };

            var runPropertiesBaseStyle16 = new RunPropertiesBaseStyle();
            var runFonts10 = new RunFonts
            {
                AsciiTheme = ThemeFontValues.MajorHighAnsi,
                HighAnsiTheme = ThemeFontValues.MajorHighAnsi,
                EastAsiaTheme = ThemeFontValues.MajorEastAsia,
                ComplexScriptTheme = ThemeFontValues.MajorBidi
            };
            var bold21 = new Bold();
            var boldComplexScript21 = new BoldComplexScript();

            runPropertiesBaseStyle16.Append(runFonts10);
            runPropertiesBaseStyle16.Append(bold21);
            runPropertiesBaseStyle16.Append(boldComplexScript21);

            tableStyleProperties22.Append(runPropertiesBaseStyle16);

            var tableStyleProperties23 = new TableStyleProperties { Type = TableStyleOverrideValues.LastColumn };

            var runPropertiesBaseStyle17 = new RunPropertiesBaseStyle();
            var runFonts11 = new RunFonts
            {
                AsciiTheme = ThemeFontValues.MajorHighAnsi,
                HighAnsiTheme = ThemeFontValues.MajorHighAnsi,
                EastAsiaTheme = ThemeFontValues.MajorEastAsia,
                ComplexScriptTheme = ThemeFontValues.MajorBidi
            };
            var bold22 = new Bold();
            var boldComplexScript22 = new BoldComplexScript();

            runPropertiesBaseStyle17.Append(runFonts11);
            runPropertiesBaseStyle17.Append(bold22);
            runPropertiesBaseStyle17.Append(boldComplexScript22);
            var tableStyleConditionalFormattingTableProperties18 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties18 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders16 = new TableCellBorders();
            var topBorder18 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var leftBorder19 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder18 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var rightBorder19 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };

            tableCellBorders16.Append(topBorder18);
            tableCellBorders16.Append(leftBorder19);
            tableCellBorders16.Append(bottomBorder18);
            tableCellBorders16.Append(rightBorder19);

            tableStyleConditionalFormattingTableCellProperties18.Append(tableCellBorders16);

            tableStyleProperties23.Append(runPropertiesBaseStyle17);
            tableStyleProperties23.Append(tableStyleConditionalFormattingTableProperties18);
            tableStyleProperties23.Append(tableStyleConditionalFormattingTableCellProperties18);

            var tableStyleProperties24 = new TableStyleProperties { Type = TableStyleOverrideValues.Band1Vertical };
            var tableStyleConditionalFormattingTableProperties19 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties19 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders17 = new TableCellBorders();
            var topBorder19 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var leftBorder20 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder19 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var rightBorder20 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };

            tableCellBorders17.Append(topBorder19);
            tableCellBorders17.Append(leftBorder20);
            tableCellBorders17.Append(bottomBorder19);
            tableCellBorders17.Append(rightBorder20);
            var shading12 = new Shading
            {
                Val = ShadingPatternValues.Clear,
                Color = "auto",
                Fill = "D3DFEE",
                ThemeFill = ThemeColorValues.Accent1,
                ThemeFillTint = "3F"
            };

            tableStyleConditionalFormattingTableCellProperties19.Append(tableCellBorders17);
            tableStyleConditionalFormattingTableCellProperties19.Append(shading12);

            tableStyleProperties24.Append(tableStyleConditionalFormattingTableProperties19);
            tableStyleProperties24.Append(tableStyleConditionalFormattingTableCellProperties19);

            var tableStyleProperties25 = new TableStyleProperties { Type = TableStyleOverrideValues.Band1Horizontal };
            var tableStyleConditionalFormattingTableProperties20 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties20 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders18 = new TableCellBorders();
            var topBorder20 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var leftBorder21 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder20 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var rightBorder21 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var insideVerticalBorder16 = new InsideVerticalBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };

            tableCellBorders18.Append(topBorder20);
            tableCellBorders18.Append(leftBorder21);
            tableCellBorders18.Append(bottomBorder20);
            tableCellBorders18.Append(rightBorder21);
            tableCellBorders18.Append(insideVerticalBorder16);
            var shading13 = new Shading
            {
                Val = ShadingPatternValues.Clear,
                Color = "auto",
                Fill = "D3DFEE",
                ThemeFill = ThemeColorValues.Accent1,
                ThemeFillTint = "3F"
            };

            tableStyleConditionalFormattingTableCellProperties20.Append(tableCellBorders18);
            tableStyleConditionalFormattingTableCellProperties20.Append(shading13);

            tableStyleProperties25.Append(tableStyleConditionalFormattingTableProperties20);
            tableStyleProperties25.Append(tableStyleConditionalFormattingTableCellProperties20);

            var tableStyleProperties26 = new TableStyleProperties { Type = TableStyleOverrideValues.Band2Horizontal };
            var tableStyleConditionalFormattingTableProperties21 = new TableStyleConditionalFormattingTableProperties();

            var tableStyleConditionalFormattingTableCellProperties21 =
                new TableStyleConditionalFormattingTableCellProperties();

            var tableCellBorders19 = new TableCellBorders();
            var topBorder21 = new TopBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var leftBorder22 = new LeftBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var bottomBorder21 = new BottomBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var rightBorder22 = new RightBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };
            var insideVerticalBorder17 = new InsideVerticalBorder
            {
                Val = BorderValues.Single,
                Color = "4F81BD",
                ThemeColor = ThemeColorValues.Accent1,
                Size = 8U,
                Space = 0U
            };

            tableCellBorders19.Append(topBorder21);
            tableCellBorders19.Append(leftBorder22);
            tableCellBorders19.Append(bottomBorder21);
            tableCellBorders19.Append(rightBorder22);
            tableCellBorders19.Append(insideVerticalBorder17);

            tableStyleConditionalFormattingTableCellProperties21.Append(tableCellBorders19);

            tableStyleProperties26.Append(tableStyleConditionalFormattingTableProperties21);
            tableStyleProperties26.Append(tableStyleConditionalFormattingTableCellProperties21);

            style15.Append(styleName15);
            style15.Append(basedOn11);
            style15.Append(uiPriority14);
            style15.Append(rsid12);
            style15.Append(styleParagraphProperties13);
            style15.Append(styleTableProperties6);
            style15.Append(tableStyleProperties20);
            style15.Append(tableStyleProperties21);
            style15.Append(tableStyleProperties22);
            style15.Append(tableStyleProperties23);
            style15.Append(tableStyleProperties24);
            style15.Append(tableStyleProperties25);
            style15.Append(tableStyleProperties26);

            var style16 = new Style { Type = StyleValues.Paragraph, StyleId = "Quote" };
            var styleName16 = new StyleName { Val = "Quote" };
            var basedOn12 = new BasedOn { Val = "Normal" };
            var nextParagraphStyle4 = new NextParagraphStyle { Val = "Normal" };
            var linkedStyle7 = new LinkedStyle { Val = "QuoteChar" };
            var uiPriority15 = new UIPriority { Val = 29 };
            var primaryStyle6 = new PrimaryStyle();
            var rsid13 = new Rsid { Val = "00851582" };

            var styleParagraphProperties16 = new StyleParagraphProperties();

            var paragraphBorders1 = new ParagraphBorders();
            var topBorder22 = new TopBorder { Val = BorderValues.Single, Color = "auto", Size = 4U, Space = 1U };
            var leftBorder23 = new LeftBorder { Val = BorderValues.Single, Color = "auto", Size = 4U, Space = 4U };
            var bottomBorder22 = new BottomBorder { Val = BorderValues.Single, Color = "auto", Size = 4U, Space = 1U };
            var rightBorder23 = new RightBorder { Val = BorderValues.Single, Color = "auto", Size = 4U, Space = 4U };

            paragraphBorders1.Append(topBorder22);
            paragraphBorders1.Append(leftBorder23);
            paragraphBorders1.Append(bottomBorder22);
            paragraphBorders1.Append(rightBorder23);

            styleParagraphProperties16.Append(paragraphBorders1);

            var styleRunProperties7 = new StyleRunProperties();
            var italic5 = new Italic();
            var italicComplexScript5 = new ItalicComplexScript();
            var color13 = new Color { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            styleRunProperties7.Append(italic5);
            styleRunProperties7.Append(italicComplexScript5);
            styleRunProperties7.Append(color13);

            style16.Append(styleName16);
            style16.Append(basedOn12);
            style16.Append(nextParagraphStyle4);
            style16.Append(linkedStyle7);
            style16.Append(uiPriority15);
            style16.Append(primaryStyle6);
            style16.Append(rsid13);
            style16.Append(styleParagraphProperties16);
            style16.Append(styleRunProperties7);

            var style17 = new Style { Type = StyleValues.Character, StyleId = "QuoteChar", CustomStyle = true };
            var styleName17 = new StyleName { Val = "Quote Char" };
            var basedOn13 = new BasedOn { Val = "DefaultParagraphFont" };
            var linkedStyle8 = new LinkedStyle { Val = "Quote" };
            var uiPriority16 = new UIPriority { Val = 29 };
            var rsid14 = new Rsid { Val = "00851582" };

            var styleRunProperties8 = new StyleRunProperties();
            var italic6 = new Italic();
            var italicComplexScript6 = new ItalicComplexScript();
            var color14 = new Color { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            styleRunProperties8.Append(italic6);
            styleRunProperties8.Append(italicComplexScript6);
            styleRunProperties8.Append(color14);

            style17.Append(styleName17);
            style17.Append(basedOn13);
            style17.Append(linkedStyle8);
            style17.Append(uiPriority16);
            style17.Append(rsid14);
            style17.Append(styleRunProperties8);

            var style18 = new Style { Type = StyleValues.Paragraph, StyleId = "TOCHeading" };
            var styleName18 = new StyleName { Val = "TOC Heading" };
            var basedOn14 = new BasedOn { Val = "Heading1" };
            var nextParagraphStyle5 = new NextParagraphStyle { Val = "Normal" };
            var uiPriority17 = new UIPriority { Val = 39 };
            var semiHidden4 = new SemiHidden();
            var unhideWhenUsed6 = new UnhideWhenUsed();
            var primaryStyle7 = new PrimaryStyle();
            var rsid15 = new Rsid { Val = "00A636FD" };

            var styleParagraphProperties17 = new StyleParagraphProperties();
            var outlineLevel4 = new OutlineLevel { Val = 9 };

            styleParagraphProperties17.Append(outlineLevel4);

            style18.Append(styleName18);
            style18.Append(basedOn14);
            style18.Append(nextParagraphStyle5);
            style18.Append(uiPriority17);
            style18.Append(semiHidden4);
            style18.Append(unhideWhenUsed6);
            style18.Append(primaryStyle7);
            style18.Append(rsid15);
            style18.Append(styleParagraphProperties17);

            var style19 = new Style { Type = StyleValues.Paragraph, StyleId = "TOC1" };
            var styleName19 = new StyleName { Val = "toc 1" };
            var basedOn15 = new BasedOn { Val = "Normal" };
            var nextParagraphStyle6 = new NextParagraphStyle { Val = "Normal" };
            var autoRedefine1 = new AutoRedefine();
            var uiPriority18 = new UIPriority { Val = 39 };
            var unhideWhenUsed7 = new UnhideWhenUsed();
            var rsid16 = new Rsid { Val = "00A636FD" };

            var styleParagraphProperties18 = new StyleParagraphProperties();
            var spacingBetweenLines17 = new SpacingBetweenLines { After = "100" };

            styleParagraphProperties18.Append(spacingBetweenLines17);

            style19.Append(styleName19);
            style19.Append(basedOn15);
            style19.Append(nextParagraphStyle6);
            style19.Append(autoRedefine1);
            style19.Append(uiPriority18);
            style19.Append(unhideWhenUsed7);
            style19.Append(rsid16);
            style19.Append(styleParagraphProperties18);

            var style20 = new Style { Type = StyleValues.Paragraph, StyleId = "TOC2" };
            var styleName20 = new StyleName { Val = "toc 2" };
            var basedOn16 = new BasedOn { Val = "Normal" };
            var nextParagraphStyle7 = new NextParagraphStyle { Val = "Normal" };
            var autoRedefine2 = new AutoRedefine();
            var uiPriority19 = new UIPriority { Val = 39 };
            var unhideWhenUsed8 = new UnhideWhenUsed();
            var rsid17 = new Rsid { Val = "00A636FD" };

            var styleParagraphProperties19 = new StyleParagraphProperties();
            var spacingBetweenLines18 = new SpacingBetweenLines { After = "100" };
            var indentation1 = new Indentation { Left = "220" };

            styleParagraphProperties19.Append(spacingBetweenLines18);
            styleParagraphProperties19.Append(indentation1);

            style20.Append(styleName20);
            style20.Append(basedOn16);
            style20.Append(nextParagraphStyle7);
            style20.Append(autoRedefine2);
            style20.Append(uiPriority19);
            style20.Append(unhideWhenUsed8);
            style20.Append(rsid17);
            style20.Append(styleParagraphProperties19);

            var style21 = new Style { Type = StyleValues.Paragraph, StyleId = "TOC3" };
            var styleName21 = new StyleName { Val = "toc 3" };
            var basedOn17 = new BasedOn { Val = "Normal" };
            var nextParagraphStyle8 = new NextParagraphStyle { Val = "Normal" };
            var autoRedefine3 = new AutoRedefine();
            var uiPriority20 = new UIPriority { Val = 39 };
            var unhideWhenUsed9 = new UnhideWhenUsed();
            var rsid18 = new Rsid { Val = "00A636FD" };

            var styleParagraphProperties20 = new StyleParagraphProperties();
            var spacingBetweenLines19 = new SpacingBetweenLines { After = "100" };
            var indentation2 = new Indentation { Left = "440" };

            styleParagraphProperties20.Append(spacingBetweenLines19);
            styleParagraphProperties20.Append(indentation2);

            style21.Append(styleName21);
            style21.Append(basedOn17);
            style21.Append(nextParagraphStyle8);
            style21.Append(autoRedefine3);
            style21.Append(uiPriority20);
            style21.Append(unhideWhenUsed9);
            style21.Append(rsid18);
            style21.Append(styleParagraphProperties20);

            var style22 = new Style { Type = StyleValues.Character, StyleId = "Hyperlink" };
            var styleName22 = new StyleName { Val = "Hyperlink" };
            var basedOn18 = new BasedOn { Val = "DefaultParagraphFont" };
            var uiPriority21 = new UIPriority { Val = 99 };
            var unhideWhenUsed10 = new UnhideWhenUsed();
            var rsid19 = new Rsid { Val = "00A636FD" };

            var styleRunProperties9 = new StyleRunProperties();
            var color15 = new Color { Val = "0000FF", ThemeColor = ThemeColorValues.Hyperlink };
            var underline1 = new Underline { Val = UnderlineValues.Single };

            styleRunProperties9.Append(color15);
            styleRunProperties9.Append(underline1);

            style22.Append(styleName22);
            style22.Append(basedOn18);
            style22.Append(uiPriority21);
            style22.Append(unhideWhenUsed10);
            style22.Append(rsid19);
            style22.Append(styleRunProperties9);

            var style23 = new Style { Type = StyleValues.Paragraph, StyleId = "BalloonText" };
            var styleName23 = new StyleName { Val = "Balloon Text" };
            var basedOn19 = new BasedOn { Val = "Normal" };
            var linkedStyle9 = new LinkedStyle { Val = "BalloonTextChar" };
            var uiPriority22 = new UIPriority { Val = 99 };
            var semiHidden5 = new SemiHidden();
            var unhideWhenUsed11 = new UnhideWhenUsed();
            var rsid20 = new Rsid { Val = "00A636FD" };

            var styleParagraphProperties21 = new StyleParagraphProperties();
            var spacingBetweenLines20 = new SpacingBetweenLines { Line = "240", LineRule = LineSpacingRuleValues.Auto };

            styleParagraphProperties21.Append(spacingBetweenLines20);

            var styleRunProperties10 = new StyleRunProperties();
            var runFonts12 = new RunFonts { Ascii = "Tahoma", HighAnsi = "Tahoma", ComplexScript = "Tahoma" };
            var fontSize6 = new FontSize { Val = "16" };
            var fontSizeComplexScript6 = new FontSizeComplexScript { Val = "16" };

            styleRunProperties10.Append(runFonts12);
            styleRunProperties10.Append(fontSize6);
            styleRunProperties10.Append(fontSizeComplexScript6);

            style23.Append(styleName23);
            style23.Append(basedOn19);
            style23.Append(linkedStyle9);
            style23.Append(uiPriority22);
            style23.Append(semiHidden5);
            style23.Append(unhideWhenUsed11);
            style23.Append(rsid20);
            style23.Append(styleParagraphProperties21);
            style23.Append(styleRunProperties10);

            var style24 = new Style { Type = StyleValues.Character, StyleId = "BalloonTextChar", CustomStyle = true };
            var styleName24 = new StyleName { Val = "Balloon Text Char" };
            var basedOn20 = new BasedOn { Val = "DefaultParagraphFont" };
            var linkedStyle10 = new LinkedStyle { Val = "BalloonText" };
            var uiPriority23 = new UIPriority { Val = 99 };
            var semiHidden6 = new SemiHidden();
            var rsid21 = new Rsid { Val = "00A636FD" };

            var styleRunProperties11 = new StyleRunProperties();
            var runFonts13 = new RunFonts { Ascii = "Tahoma", HighAnsi = "Tahoma", ComplexScript = "Tahoma" };
            var fontSize7 = new FontSize { Val = "16" };
            var fontSizeComplexScript7 = new FontSizeComplexScript { Val = "16" };

            styleRunProperties11.Append(runFonts13);
            styleRunProperties11.Append(fontSize7);
            styleRunProperties11.Append(fontSizeComplexScript7);

            style24.Append(styleName24);
            style24.Append(basedOn20);
            style24.Append(linkedStyle10);
            style24.Append(uiPriority23);
            style24.Append(semiHidden6);
            style24.Append(rsid21);
            style24.Append(styleRunProperties11);

            var style25 = new Style { Type = StyleValues.Paragraph, StyleId = "Passed", CustomStyle = true };
            var styleName25 = new StyleName { Val = "Passed" };
            var basedOn21 = new BasedOn { Val = "Normal" };
            var linkedStyle11 = new LinkedStyle { Val = "PassedChar" };
            var primaryStyle8 = new PrimaryStyle();
            var rsid22 = new Rsid { Val = "005217FA" };

            var styleParagraphProperties22 = new StyleParagraphProperties();
            var shading14 = new Shading { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "00B050" };
            var justification1 = new Justification { Val = JustificationValues.Center };

            styleParagraphProperties22.Append(shading14);
            styleParagraphProperties22.Append(justification1);

            style25.Append(styleName25);
            style25.Append(basedOn21);
            style25.Append(linkedStyle11);
            style25.Append(primaryStyle8);
            style25.Append(rsid22);
            style25.Append(styleParagraphProperties22);

            var style26 = new Style { Type = StyleValues.Paragraph, StyleId = "Failed", CustomStyle = true };
            var styleName26 = new StyleName { Val = "Failed" };
            var basedOn22 = new BasedOn { Val = "Passed" };
            var linkedStyle12 = new LinkedStyle { Val = "FailedChar" };
            var primaryStyle9 = new PrimaryStyle();
            var rsid23 = new Rsid { Val = "005217FA" };

            var styleParagraphProperties23 = new StyleParagraphProperties();
            var shading15 = new Shading { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FF0000" };

            styleParagraphProperties23.Append(shading15);

            style26.Append(styleName26);
            style26.Append(basedOn22);
            style26.Append(linkedStyle12);
            style26.Append(primaryStyle9);
            style26.Append(rsid23);
            style26.Append(styleParagraphProperties23);

            var style27 = new Style { Type = StyleValues.Character, StyleId = "PassedChar", CustomStyle = true };
            var styleName27 = new StyleName { Val = "Passed Char" };
            var basedOn23 = new BasedOn { Val = "DefaultParagraphFont" };
            var linkedStyle13 = new LinkedStyle { Val = "Passed" };
            var rsid24 = new Rsid { Val = "005217FA" };

            var styleRunProperties12 = new StyleRunProperties();
            var shading16 = new Shading { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "00B050" };

            styleRunProperties12.Append(shading16);

            style27.Append(styleName27);
            style27.Append(basedOn23);
            style27.Append(linkedStyle13);
            style27.Append(rsid24);
            style27.Append(styleRunProperties12);

            var style28 = new Style { Type = StyleValues.Character, StyleId = "FailedChar", CustomStyle = true };
            var styleName28 = new StyleName { Val = "Failed Char" };
            var basedOn24 = new BasedOn { Val = "PassedChar" };
            var linkedStyle14 = new LinkedStyle { Val = "Failed" };
            var rsid25 = new Rsid { Val = "005217FA" };

            var styleRunProperties13 = new StyleRunProperties();
            var shading17 = new Shading { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FF0000" };

            styleRunProperties13.Append(shading17);

            style28.Append(styleName28);
            style28.Append(basedOn24);
            style28.Append(linkedStyle14);
            style28.Append(rsid25);
            style28.Append(styleRunProperties13);

            styles1.Append(docDefaults1);
            styles1.Append(latentStyles1);
            styles1.Append(style1);
            styles1.Append(style2);
            styles1.Append(style3);
            styles1.Append(style4);
            styles1.Append(style5);
            styles1.Append(style6);
            styles1.Append(style7);
            styles1.Append(style8);
            styles1.Append(style9);
            styles1.Append(style10);
            styles1.Append(style11);
            styles1.Append(style12);
            styles1.Append(style13);
            styles1.Append(style14);
            styles1.Append(style15);
            styles1.Append(style16);
            styles1.Append(style17);
            styles1.Append(style18);
            styles1.Append(style19);
            styles1.Append(style20);
            styles1.Append(style21);
            styles1.Append(style22);
            styles1.Append(style23);
            styles1.Append(style24);
            styles1.Append(style25);
            styles1.Append(style26);
            styles1.Append(style27);
            styles1.Append(style28);

            styles1.Append(this.GenerateHeaderStyle());
            styles1.Append(this.GenerateHeaderCharStyle());
            styles1.Append(this.GenerateFooterStyle());
            styles1.Append(this.GenerateFooterCharStyle());

            part.Styles = styles1;
        }

        public Style GenerateHeaderStyle()
        {
            var style1 = new Style { Type = StyleValues.Paragraph, StyleId = "Header" };
            var styleName1 = new StyleName { Val = "header" };
            var basedOn1 = new BasedOn { Val = "Normal" };
            var linkedStyle1 = new LinkedStyle { Val = "HeaderChar" };
            var uiPriority1 = new UIPriority { Val = 99 };
            var unhideWhenUsed1 = new UnhideWhenUsed();
            var rsid1 = new Rsid { Val = "005641D2" };

            var styleParagraphProperties1 = new StyleParagraphProperties();

            var tabs1 = new Tabs();
            var tabStop1 = new TabStop { Val = TabStopValues.Center, Position = 4680 };
            var tabStop2 = new TabStop { Val = TabStopValues.Right, Position = 9360 };

            tabs1.Append(tabStop1);
            tabs1.Append(tabStop2);
            var spacingBetweenLines1 = new SpacingBetweenLines { Line = "240", LineRule = LineSpacingRuleValues.Auto };

            styleParagraphProperties1.Append(tabs1);
            styleParagraphProperties1.Append(spacingBetweenLines1);

            style1.Append(styleName1);
            style1.Append(basedOn1);
            style1.Append(linkedStyle1);
            style1.Append(uiPriority1);
            style1.Append(unhideWhenUsed1);
            style1.Append(rsid1);
            style1.Append(styleParagraphProperties1);
            return style1;
        }

        public Style GenerateHeaderCharStyle()
        {
            var style1 = new Style { Type = StyleValues.Character, StyleId = "HeaderChar", CustomStyle = true };
            var styleName1 = new StyleName { Val = "Header Char" };
            var basedOn1 = new BasedOn { Val = "DefaultParagraphFont" };
            var linkedStyle1 = new LinkedStyle { Val = "Header" };
            var uiPriority1 = new UIPriority { Val = 99 };
            var rsid1 = new Rsid { Val = "005641D2" };

            style1.Append(styleName1);
            style1.Append(basedOn1);
            style1.Append(linkedStyle1);
            style1.Append(uiPriority1);
            style1.Append(rsid1);
            return style1;
        }

        public Style GenerateFooterStyle()
        {
            var style1 = new Style { Type = StyleValues.Paragraph, StyleId = "Footer" };
            var styleName1 = new StyleName { Val = "footer" };
            var basedOn1 = new BasedOn { Val = "Normal" };
            var linkedStyle1 = new LinkedStyle { Val = "FooterChar" };
            var uiPriority1 = new UIPriority { Val = 99 };
            var semiHidden1 = new SemiHidden();
            var unhideWhenUsed1 = new UnhideWhenUsed();
            var rsid1 = new Rsid { Val = "005641D2" };

            var styleParagraphProperties1 = new StyleParagraphProperties();

            var tabs1 = new Tabs();
            var tabStop1 = new TabStop { Val = TabStopValues.Center, Position = 4680 };
            var tabStop2 = new TabStop { Val = TabStopValues.Right, Position = 9360 };

            tabs1.Append(tabStop1);
            tabs1.Append(tabStop2);
            var spacingBetweenLines1 = new SpacingBetweenLines { Line = "240", LineRule = LineSpacingRuleValues.Auto };

            styleParagraphProperties1.Append(tabs1);
            styleParagraphProperties1.Append(spacingBetweenLines1);

            style1.Append(styleName1);
            style1.Append(basedOn1);
            style1.Append(linkedStyle1);
            style1.Append(uiPriority1);
            style1.Append(semiHidden1);
            style1.Append(unhideWhenUsed1);
            style1.Append(rsid1);
            style1.Append(styleParagraphProperties1);
            return style1;
        }

        public Style GenerateFooterCharStyle()
        {
            var style1 = new Style { Type = StyleValues.Character, StyleId = "FooterChar", CustomStyle = true };
            var styleName1 = new StyleName { Val = "Footer Char" };
            var basedOn1 = new BasedOn { Val = "DefaultParagraphFont" };
            var linkedStyle1 = new LinkedStyle { Val = "Footer" };
            var uiPriority1 = new UIPriority { Val = 99 };
            var semiHidden1 = new SemiHidden();
            var rsid1 = new Rsid { Val = "005641D2" };

            style1.Append(styleName1);
            style1.Append(basedOn1);
            style1.Append(linkedStyle1);
            style1.Append(uiPriority1);
            style1.Append(semiHidden1);
            style1.Append(rsid1);
            return style1;
        }
    }
}
