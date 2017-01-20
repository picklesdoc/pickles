/***************************************************************************

Copyright (c) Microsoft Corporation 2011.

This code is licensed using the Microsoft Public License (Ms-PL).  The text of the license
can be found here:

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx

***************************************************************************/

using System;
using System.Linq;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word.TableOfContentsAdder
{
    public class TocAdder
    {
        private static void AddElementIfMissing(XDocument partXDoc, XElement existing, string newElement)
        {
            if (existing != null)
                return;
            XElement newXElement = XElement.Parse(newElement);
            newXElement.Attributes().Where(a => a.IsNamespaceDeclaration).Remove();
            partXDoc.Root.Add(newXElement);
        }

        private static void UpdateFontTablePart(WordprocessingDocument doc)
        {
            FontTablePart fontTablePart = doc.MainDocumentPart.FontTablePart;
            if (fontTablePart == null)
                throw new OpenXmlPowerToolsException("Document does not contain font table part");
            XDocument fontTableXDoc = fontTablePart.GetXDocument();

            AddElementIfMissing(fontTableXDoc,
                fontTableXDoc
                    .Root
                    .Elements(W.font)
                    .Where(e => (string)e.Attribute(W.name) == "Tahoma")
                    .FirstOrDefault(),
                @"<w:font w:name='Tahoma' xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                     <w:panose1 w:val='020B0604030504040204'/>
                     <w:charset w:val='00'/>
                     <w:family w:val='swiss'/>
                     <w:pitch w:val='variable'/>
                     <w:sig w:usb0='E1002EFF' w:usb1='C000605B' w:usb2='00000029' w:usb3='00000000' w:csb0='000101FF' w:csb1='00000000'/>
                   </w:font>");

            fontTablePart.PutXDocument();
        }

        private static void UpdateAStylePartForToc(XDocument partXDoc)
        {
            AddElementIfMissing(
                partXDoc,
                partXDoc.Root.Elements(W.style)
                    .Where(
                        e =>
                            (string)e.Attribute(W.type) == "paragraph" && (string)e.Attribute(W.styleId) == "TOCHeading")
                    .FirstOrDefault(),
                @"<w:style w:type='paragraph' w:styleId='TOCHeading' xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                    <w:name w:val='TOC Heading'/>
                    <w:basedOn w:val='Heading1'/>
                    <w:next w:val='Normal'/>
                    <w:uiPriority w:val='39'/>
                    <w:semiHidden/>
                    <w:unhideWhenUsed/>
                    <w:qFormat/>
                    <w:pPr>
                      <w:outlineLvl w:val='9'/>
                    </w:pPr>
                    <w:rPr>
                      <w:lang w:eastAsia='ja-JP'/>
                    </w:rPr>
                  </w:style>");

            for (int i = 1; i <= 1; ++i)
            {
                AddElementIfMissing(
                    partXDoc,
                    partXDoc.Root.Elements(W.style)
                        .Where(
                            e =>
                                (string)e.Attribute(W.type) == "paragraph" &&
                                (string)e.Attribute(W.styleId) == ("TOC" + i.ToString()))
                        .FirstOrDefault(),
                    String.Format(
                        @"<w:style w:type='paragraph' w:styleId='TOC{0}' xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                            <w:name w:val='toc {0}'/>
                            <w:basedOn w:val='Normal'/>
                            <w:next w:val='Normal'/>
                            <w:autoRedefine/>
                            <w:uiPriority w:val='39'/>
                            <w:unhideWhenUsed/>
                            <w:pPr>
                              <w:spacing w:after='100'/>
                            </w:pPr>
                          </w:style>",
                        i));
            }
            for (int i = 2; i <= 6; ++i)
            {
                AddElementIfMissing(
                    partXDoc,
                    partXDoc.Root.Elements(W.style)
                        .Where(
                            e =>
                                (string)e.Attribute(W.type) == "paragraph" &&
                                (string)e.Attribute(W.styleId) == ("TOC" + i.ToString()))
                        .FirstOrDefault(),
                    String.Format(
                        @"<w:style w:type='paragraph' w:styleId='TOC{0}' xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                            <w:name w:val='toc {0}'/>
                            <w:basedOn w:val='Normal'/>
                            <w:next w:val='Normal'/>
                            <w:autoRedefine/>
                            <w:uiPriority w:val='39'/>
                            <w:unhideWhenUsed/>
                            <w:pPr>
                              <w:tabs>
                                <w:tab w:val='right' w:leader='dot' w:pos='9350'/>
                              </w:tabs>
                              <w:spacing w:after='100'/>
                              <w:ind w:left='720'/>
                            </w:pPr>
                          </w:style>",
                        i));
            }

            AddElementIfMissing(
                partXDoc,
                partXDoc.Root.Elements(W.style)
                    .Where(
                        e =>
                            (string)e.Attribute(W.type) == "character" && (string)e.Attribute(W.styleId) == "Hyperlink")
                    .FirstOrDefault(),
                @"<w:style w:type='character' w:styleId='Hyperlink' xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                     <w:name w:val='Hyperlink'/>
                     <w:basedOn w:val='DefaultParagraphFont'/>
                     <w:uiPriority w:val='99'/>
                     <w:unhideWhenUsed/>
                     <w:rPr>
                       <w:color w:val='0000FF' w:themeColor='hyperlink'/>
                       <w:u w:val='single'/>
                     </w:rPr>
                   </w:style>");

            AddElementIfMissing(
                partXDoc,
                partXDoc.Root.Elements(W.style)
                    .Where(
                        e =>
                            (string)e.Attribute(W.type) == "paragraph" && (string)e.Attribute(W.styleId) == "BalloonText")
                    .FirstOrDefault(),
                @"<w:style w:type='paragraph' w:styleId='BalloonText' xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                    <w:name w:val='Balloon Text'/>
                    <w:basedOn w:val='Normal'/>
                    <w:link w:val='BalloonTextChar'/>
                    <w:uiPriority w:val='99'/>
                    <w:semiHidden/>
                    <w:unhideWhenUsed/>
                    <w:pPr>
                      <w:spacing w:after='0' w:line='240' w:lineRule='auto'/>
                    </w:pPr>
                    <w:rPr>
                      <w:rFonts w:ascii='Tahoma' w:hAnsi='Tahoma' w:cs='Tahoma'/>
                      <w:sz w:val='16'/>
                      <w:szCs w:val='16'/>
                    </w:rPr>
                  </w:style>");

            AddElementIfMissing(
                partXDoc,
                partXDoc.Root.Elements(W.style)
                    .Where(e => (string)e.Attribute(W.type) == "character" &&
                                (bool?)e.Attribute(W.customStyle) == true &&
                                (string)e.Attribute(W.styleId) == "BalloonTextChar")
                    .FirstOrDefault(),
                @"<w:style w:type='character' w:customStyle='1' w:styleId='BalloonTextChar' xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                    <w:name w:val='Balloon Text Char'/>
                    <w:basedOn w:val='DefaultParagraphFont'/>
                    <w:link w:val='BalloonText'/>
                    <w:uiPriority w:val='99'/>
                    <w:semiHidden/>
                    <w:rPr>
                      <w:rFonts w:ascii='Tahoma' w:hAnsi='Tahoma' w:cs='Tahoma'/>
                      <w:sz w:val='16'/>
                      <w:szCs w:val='16'/>
                    </w:rPr>
                  </w:style>");
        }

        private static void UpdateStylesPartForToc(WordprocessingDocument doc)
        {
            StylesPart stylesPart = doc.MainDocumentPart.StyleDefinitionsPart;
            if (stylesPart == null)
                throw new OpenXmlPowerToolsException("Document does not contain styles part");
            XDocument stylesXDoc = stylesPart.GetXDocument();
            UpdateAStylePartForToc(stylesXDoc);
            stylesPart.PutXDocument();
        }

        private static void UpdateStylesWithEffectsPartForToc(WordprocessingDocument doc)
        {
            StylesWithEffectsPart stylesWithEffectsPart = doc.MainDocumentPart.StylesWithEffectsPart;
            if (stylesWithEffectsPart == null)
                throw new OpenXmlPowerToolsException("Document does not contain styles with effects part");
            XDocument stylesWithEffectsXDoc = stylesWithEffectsPart.GetXDocument();
            UpdateAStylePartForToc(stylesWithEffectsXDoc);
            stylesWithEffectsPart.PutXDocument();
        }

        private static void UpdateAStylePartForTof(XDocument partXDoc)
        {
            AddElementIfMissing(
                partXDoc,
                partXDoc.Root.Elements(W.style)
                    .Where(
                        e =>
                            (string)e.Attribute(W.type) == "paragraph" &&
                            (string)e.Attribute(W.styleId) == "TableofFigures")
                    .FirstOrDefault(),
                @"<w:style w:type='paragraph' w:styleId='TableofFigures' xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                    <w:name w:val='table of figures'/>
                    <w:basedOn w:val='Normal'/>
                    <w:next w:val='Normal'/>
                    <w:uiPriority w:val='99'/>
                    <w:unhideWhenUsed/>
                    <w:pPr>
                      <w:spacing w:after='0'/>
                    </w:pPr>
                  </w:style>");

            AddElementIfMissing(
                partXDoc,
                partXDoc.Root.Elements(W.style)
                    .Where(
                        e =>
                            (string)e.Attribute(W.type) == "character" && (string)e.Attribute(W.styleId) == "Hyperlink")
                    .FirstOrDefault(),
                @"<w:style w:type='character' w:styleId='Hyperlink' xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                     <w:name w:val='Hyperlink'/>
                     <w:basedOn w:val='DefaultParagraphFont'/>
                     <w:uiPriority w:val='99'/>
                     <w:unhideWhenUsed/>
                     <w:rPr>
                       <w:color w:val='0000FF' w:themeColor='hyperlink'/>
                       <w:u w:val='single'/>
                     </w:rPr>
                   </w:style>");
        }

        private static void UpdateStylesPartForTof(WordprocessingDocument doc)
        {
            StylesPart stylesPart = doc.MainDocumentPart.StyleDefinitionsPart;
            if (stylesPart == null)
                throw new OpenXmlPowerToolsException("Document does not contain styles part");
            XDocument stylesXDoc = stylesPart.GetXDocument();
            UpdateAStylePartForTof(stylesXDoc);
            stylesPart.PutXDocument();
        }

        private static void UpdateStylesWithEffectsPartForTof(WordprocessingDocument doc)
        {
            StylesWithEffectsPart stylesWithEffectsPart = doc.MainDocumentPart.StylesWithEffectsPart;
            if (stylesWithEffectsPart == null)
                throw new OpenXmlPowerToolsException("Document does not contain styles with effects part");
            XDocument stylesWithEffectsXDoc = stylesWithEffectsPart.GetXDocument();
            UpdateAStylePartForTof(stylesWithEffectsXDoc);
            stylesWithEffectsPart.PutXDocument();
        }

        public static void AddToc(WordprocessingDocument doc, XElement addBefore, string switches, string title,
            int? rightTabPos)
        {
            UpdateFontTablePart(doc);
            UpdateStylesPartForToc(doc);
            UpdateStylesWithEffectsPartForToc(doc);

            if (title == null)
                title = "Contents";
            if (rightTabPos == null)
                rightTabPos = 9350;

            // {0} tocTitle (default = "Contents")
            // {1} rightTabPosition (default = 9350)
            // {2} switches

            String xmlString =
                @"<w:sdt xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
  <w:sdtPr>
    <w:docPartObj>
      <w:docPartGallery w:val='Table of Contents'/>
      <w:docPartUnique/>
    </w:docPartObj>
  </w:sdtPr>
  <w:sdtEndPr>
    <w:rPr>
     <w:rFonts w:asciiTheme='minorHAnsi' w:cstheme='minorBidi' w:eastAsiaTheme='minorHAnsi' w:hAnsiTheme='minorHAnsi'/>
     <w:color w:val='auto'/>
     <w:sz w:val='22'/>
     <w:szCs w:val='22'/>
     <w:lang w:eastAsia='en-US'/>
    </w:rPr>
  </w:sdtEndPr>
  <w:sdtContent>
    <w:p>
      <w:pPr>
        <w:pStyle w:val='TOCHeading'/>
      </w:pPr>
      <w:r>
        <w:t>{0}</w:t>
      </w:r>
    </w:p>
    <w:p>
      <w:pPr>
        <w:pStyle w:val='TOC1'/>
        <w:tabs>
          <w:tab w:val='right' w:leader='dot' w:pos='{1}'/>
        </w:tabs>
        <w:rPr>
          <w:noProof/>
        </w:rPr>
      </w:pPr>
      <w:r>
        <w:fldChar w:fldCharType='begin' w:dirty='true'/>
      </w:r>
      <w:r>
        <w:instrText xml:space='preserve'> {2} </w:instrText>
      </w:r>
      <w:r>
        <w:fldChar w:fldCharType='separate'/>
      </w:r>
    </w:p>
    <w:p>
      <w:r>
        <w:rPr>
          <w:b/>
          <w:bCs/>
          <w:noProof/>
        </w:rPr>
        <w:fldChar w:fldCharType='end'/>
      </w:r>
    </w:p>
  </w:sdtContent>
</w:sdt>";

            XElement sdt = XElement.Parse(String.Format(xmlString, title, rightTabPos, switches));
            XDocument mainXDoc = doc.MainDocumentPart.GetXDocument();
            addBefore.AddBeforeSelf(sdt);
            doc.MainDocumentPart.PutXDocument();

            XDocument settingsXDoc = doc.MainDocumentPart.DocumentSettingsPart.GetXDocument();
            XElement updateFields = settingsXDoc.Descendants(W.updateFields).FirstOrDefault();
            if (updateFields != null)
                updateFields.Attribute(W.val).Value = "true";
            else
            {
                updateFields = new XElement(W.updateFields,
                    new XAttribute(W.val, "true"));
                settingsXDoc.Root.Add(updateFields);
            }
            doc.MainDocumentPart.DocumentSettingsPart.PutXDocument();
        }

        public static void AddTof(WordprocessingDocument doc, XElement addBefore, string switches, int? rightTabPos)
        {
            UpdateFontTablePart(doc);
            UpdateStylesPartForTof(doc);
            UpdateStylesWithEffectsPartForTof(doc);

            if (rightTabPos == null)
                rightTabPos = 9350;

            // {0} rightTabPosition (default = 9350)
            // {1} switches

            string xmlString =
                @"<w:p xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
  <w:pPr>
    <w:pStyle w:val='TableofFigures'/>
    <w:tabs>
      <w:tab w:val='right' w:leader='dot' w:pos='{0}'/>
    </w:tabs>
    <w:rPr>
      <w:noProof/>
    </w:rPr>
  </w:pPr>
  <w:r>
    <w:fldChar w:fldCharType='begin' dirty='true'/>
  </w:r>
  <w:r>
    <w:instrText xml:space='preserve'> {1} </w:instrText>
  </w:r>
  <w:r>
    <w:fldChar w:fldCharType='separate'/>
  </w:r>
  <w:r>
    <w:fldChar w:fldCharType='end'/>
  </w:r>
</w:p>";
            XElement paragraph = XElement.Parse(String.Format(xmlString, rightTabPos, switches));
            XDocument mainXDoc = doc.MainDocumentPart.GetXDocument();
            addBefore.AddBeforeSelf(paragraph);
            doc.MainDocumentPart.PutXDocument();

            XDocument settingsXDoc = doc.MainDocumentPart.DocumentSettingsPart.GetXDocument();
            XElement updateFields = settingsXDoc.Descendants(W.updateFields).FirstOrDefault();
            if (updateFields != null)
                updateFields.Attribute(W.val).Value = "true";
            else
            {
                updateFields = new XElement(W.updateFields,
                    new XAttribute(W.val, "true"));
                settingsXDoc.Root.Add(updateFields);
            }
            doc.MainDocumentPart.DocumentSettingsPart.PutXDocument();
        }
    }
}
