/***************************************************************************

Copyright (c) Microsoft Corporation 2011.

This code is licensed using the Microsoft Public License (Ms-PL).  The text of the license
can be found here:

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx

 This code was modified to use System.IO.Abstractions in the appropriate places.

***************************************************************************/

using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;

using FileMode = System.IO.FileMode;
using FileAccess = System.IO.FileAccess;
using MemoryStream = System.IO.MemoryStream;
using Stream = System.IO.Stream;
using StreamReader = System.IO.StreamReader;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word.TableOfContentsAdder
{
    public class WmlDocument
    {
        private readonly IFileSystem fileSystem;

        public WmlDocument(IFileSystem fileSystem, WmlDocument original)
        {
            this.fileSystem = fileSystem;
            this.DocumentByteArray = new byte[original.DocumentByteArray.Length];
            Array.Copy(original.DocumentByteArray, this.DocumentByteArray, original.DocumentByteArray.Length);
            this.FileName = original.FileName;
        }

        public WmlDocument(IFileSystem fileSystem, string fileName)
        {
            this.fileSystem = fileSystem;
            this.FileName = this.FileName;
            this.DocumentByteArray = this.fileSystem.File.ReadAllBytes(fileName);
        }

        public WmlDocument(IFileSystem fileSystem, byte[] byteArray)
        {
            this.fileSystem = fileSystem;
            this.DocumentByteArray = new byte[byteArray.Length];
            Array.Copy(byteArray, this.DocumentByteArray, byteArray.Length);
            this.FileName = null;
        }

        public string FileName { get; }
        public byte[] DocumentByteArray { get; }

        public WordprocessingDocument GetWordprocessingDocument()
        {
            var mem = new MemoryStream();
            mem.Write(this.DocumentByteArray, 0, this.DocumentByteArray.Length);
            WordprocessingDocument doc = WordprocessingDocument.Open(mem, true);
            return doc;
        }

        public void SaveAs(string fileName)
        {
            this.fileSystem.File.WriteAllBytes(fileName, this.DocumentByteArray);
        }

        public void Save()
        {
            if (this.FileName == null)
                throw new OpenXmlPowerToolsException(
                    "Attempting to Save a document that has no file name.  Use SaveAs instead.");
            this.fileSystem.File.WriteAllBytes(this.FileName, this.DocumentByteArray);
        }
    }

    public static class A
    {
        public static XNamespace a =
            "http://schemas.openxmlformats.org/drawingml/2006/main";

        public static XName accent1 = a + "accent1";
        public static XName accent2 = a + "accent2";
        public static XName accent3 = a + "accent3";
        public static XName accent4 = a + "accent4";
        public static XName accent5 = a + "accent5";
        public static XName accent6 = a + "accent6";
        public static XName ahLst = a + "ahLst";
        public static XName alpha = a + "alpha";
        public static XName alphaMod = a + "alphaMod";
        public static XName alphaModFix = a + "alphaModFix";
        public static XName alphaOff = a + "alphaOff";
        public static XName alphaRepl = a + "alphaRepl";
        public static XName anchor = a + "anchor";
        public static XName arcTo = a + "arcTo";
        public static XName avLst = a + "avLst";
        public static XName backdrop = a + "backdrop";
        public static XName bevelB = a + "bevelB";
        public static XName bevelT = a + "bevelT";
        public static XName bgClr = a + "bgClr";
        public static XName bgFillStyleLst = a + "bgFillStyleLst";
        public static XName biLevel = a + "biLevel";
        public static XName blip = a + "blip";
        public static XName blipFill = a + "blipFill";
        public static XName bodyPr = a + "bodyPr";
        public static XName br = a + "br";
        public static XName buChar = a + "buChar";
        public static XName buClr = a + "buClr";
        public static XName buFont = a + "buFont";
        public static XName camera = a + "camera";
        public static XName chExt = a + "chExt";
        public static XName chOff = a + "chOff";
        public static XName close = a + "close";
        public static XName clrChange = a + "clrChange";
        public static XName clrFrom = a + "clrFrom";
        public static XName clrScheme = a + "clrScheme";
        public static XName clrTo = a + "clrTo";
        public static XName cNvCxnSpPr = a + "cNvCxnSpPr";
        public static XName cNvGraphicFramePr = a + "cNvGraphicFramePr";
        public static XName cNvGrpSpPr = a + "cNvGrpSpPr";
        public static XName cNvPicPr = a + "cNvPicPr";
        public static XName cNvPr = a + "cNvPr";
        public static XName cNvSpPr = a + "cNvSpPr";
        public static XName contourClr = a + "contourClr";
        public static XName cs = a + "cs";
        public static XName cubicBezTo = a + "cubicBezTo";
        public static XName custGeom = a + "custGeom";
        public static XName cxn = a + "cxn";
        public static XName cxnLst = a + "cxnLst";
        public static XName cxnSp = a + "cxnSp";
        public static XName defPPr = a + "defPPr";
        public static XName defRPr = a + "defRPr";
        public static XName dk1 = a + "dk1";
        public static XName dk2 = a + "dk2";
        public static XName duotone = a + "duotone";
        public static XName ea = a + "ea";
        public static XName effectLst = a + "effectLst";
        public static XName effectRef = a + "effectRef";
        public static XName effectStyle = a + "effectStyle";
        public static XName effectStyleLst = a + "effectStyleLst";
        public static XName endParaRPr = a + "endParaRPr";
        public static XName ext = a + "ext";
        public static XName extLst = a + "extLst";
        public static XName extraClrSchemeLst = a + "extraClrSchemeLst";
        public static XName extrusionClr = a + "extrusionClr";
        public static XName fgClr = a + "fgClr";
        public static XName fillOverlay = a + "fillOverlay";
        public static XName fillRect = a + "fillRect";
        public static XName fillRef = a + "fillRef";
        public static XName fillStyleLst = a + "fillStyleLst";
        public static XName fillToRect = a + "fillToRect";
        public static XName flatTx = a + "flatTx";
        public static XName fld = a + "fld";
        public static XName fmtScheme = a + "fmtScheme";
        public static XName folHlink = a + "folHlink";
        public static XName font = a + "font";
        public static XName fontRef = a + "fontRef";
        public static XName fontScheme = a + "fontScheme";
        public static XName gamma = a + "gamma";
        public static XName gd = a + "gd";
        public static XName gdLst = a + "gdLst";
        public static XName glow = a + "glow";
        public static XName gradFill = a + "gradFill";
        public static XName graphic = a + "graphic";
        public static XName graphicData = a + "graphicData";
        public static XName graphicFrame = a + "graphicFrame";
        public static XName graphicFrameLocks = a + "graphicFrameLocks";
        public static XName grayscl = a + "grayscl";
        public static XName grpSp = a + "grpSp";
        public static XName grpSpPr = a + "grpSpPr";
        public static XName gs = a + "gs";
        public static XName gsLst = a + "gsLst";
        public static XName headEnd = a + "headEnd";
        public static XName hlink = a + "hlink";
        public static XName hlinkClick = a + "hlinkClick";
        public static XName hslClr = a + "hslClr";
        public static XName hueMod = a + "hueMod";
        public static XName hueOff = a + "hueOff";
        public static XName innerShdw = a + "innerShdw";
        public static XName invGamma = a + "invGamma";
        public static XName latin = a + "latin";
        public static XName lightRig = a + "lightRig";
        public static XName lin = a + "lin";
        public static XName ln = a + "ln";
        public static XName lnDef = a + "lnDef";
        public static XName lnRef = a + "lnRef";
        public static XName lnSpc = a + "lnSpc";
        public static XName lnStyleLst = a + "lnStyleLst";
        public static XName lnTo = a + "lnTo";
        public static XName lstStyle = a + "lstStyle";
        public static XName lt1 = a + "lt1";
        public static XName lt2 = a + "lt2";
        public static XName lum = a + "lum";
        public static XName lumMod = a + "lumMod";
        public static XName lumOff = a + "lumOff";
        public static XName lvl1pPr = a + "lvl1pPr";
        public static XName lvl2pPr = a + "lvl2pPr";
        public static XName lvl3pPr = a + "lvl3pPr";
        public static XName lvl4pPr = a + "lvl4pPr";
        public static XName lvl5pPr = a + "lvl5pPr";
        public static XName lvl6pPr = a + "lvl6pPr";
        public static XName lvl7pPr = a + "lvl7pPr";
        public static XName lvl8pPr = a + "lvl8pPr";
        public static XName lvl9pPr = a + "lvl9pPr";
        public static XName majorFont = a + "majorFont";
        public static XName minorFont = a + "minorFont";
        public static XName miter = a + "miter";
        public static XName moveTo = a + "moveTo";
        public static XName noAutofit = a + "noAutofit";
        public static XName noFill = a + "noFill";
        public static XName norm = a + "norm";
        public static XName nvCxnSpPr = a + "nvCxnSpPr";
        public static XName nvGraphicFramePr = a + "nvGraphicFramePr";
        public static XName nvGrpSpPr = a + "nvGrpSpPr";
        public static XName nvPicPr = a + "nvPicPr";
        public static XName nvSpPr = a + "nvSpPr";
        public static XName objectDefaults = a + "objectDefaults";
        public static XName off = a + "off";
        public static XName outerShdw = a + "outerShdw";
        public static XName p = a + "p";
        public static XName path = a + "path";
        public static XName pathLst = a + "pathLst";
        public static XName pattFill = a + "pattFill";
        public static XName pic = a + "pic";
        public static XName picLocks = a + "picLocks";
        public static XName pos = a + "pos";
        public static XName pPr = a + "pPr";
        public static XName prstClr = a + "prstClr";
        public static XName prstDash = a + "prstDash";
        public static XName prstGeom = a + "prstGeom";
        public static XName prstTxWarp = a + "prstTxWarp";
        public static XName pt = a + "pt";
        public static XName r = a + "r";
        public static XName rect = a + "rect";
        public static XName reflection = a + "reflection";
        public static XName relIds = a + "relIds";
        public static XName rot = a + "rot";
        public static XName round = a + "round";
        public static XName rPr = a + "rPr";
        public static XName satMod = a + "satMod";
        public static XName satOff = a + "satOff";
        public static XName scene3d = a + "scene3d";
        public static XName schemeClr = a + "schemeClr";
        public static XName scrgbClr = a + "scrgbClr";
        public static XName shade = a + "shade";
        public static XName softEdge = a + "softEdge";
        public static XName solidFill = a + "solidFill";
        public static XName sp = a + "sp";
        public static XName sp3d = a + "sp3d";
        public static XName spAutoFit = a + "spAutoFit";
        public static XName spcAft = a + "spcAft";
        public static XName spcBef = a + "spcBef";
        public static XName spcPct = a + "spcPct";
        public static XName spDef = a + "spDef";
        public static XName spLocks = a + "spLocks";
        public static XName spPr = a + "spPr";
        public static XName srcRect = a + "srcRect";
        public static XName srgbClr = a + "srgbClr";
        public static XName stCxn = a + "stCxn";
        public static XName stretch = a + "stretch";
        public static XName style = a + "style";
        public static XName sx = a + "sx";
        public static XName sy = a + "sy";
        public static XName sysClr = a + "sysClr";
        public static XName t = a + "t";
        public static XName tailEnd = a + "tailEnd";
        public static XName theme = a + "theme";
        public static XName themeElements = a + "themeElements";
        public static XName tile = a + "tile";
        public static XName tileRect = a + "tileRect";
        public static XName tint = a + "tint";
        public static XName txBody = a + "txBody";
        public static XName txSp = a + "txSp";
        public static XName up = a + "up";
        public static XName useSpRect = a + "useSpRect";
        public static XName xfrm = a + "xfrm";
    }

    public static class ACTIVEX
    {
        public static XNamespace activex =
            "http://schemas.microsoft.com/office/2006/activeX";

        public static XName classid = activex + "classid";
        public static XName font = activex + "font";
        public static XName license = activex + "license";
        public static XName name = activex + "name";
        public static XName ocx = activex + "ocx";
        public static XName ocxPr = activex + "ocxPr";
        public static XName persistence = activex + "persistence";
        public static XName value = activex + "value";
    }

    public static class BIBLIO
    {
        public static XNamespace biblio =
            "http://schemas.microsoft.com/office/word/2004/10/bibliography";

        public static XName AlbumTitle = biblio + "AlbumTitle";
        public static XName Artist = biblio + "Artist";
        public static XName Author = biblio + "Author";
        public static XName City = biblio + "City";
        public static XName Comments = biblio + "Comments";
        public static XName Composer = biblio + "Composer";
        public static XName Conductor = biblio + "Conductor";
        public static XName ConferenceName = biblio + "ConferenceName";
        public static XName Country = biblio + "Country";
        public static XName Day = biblio + "Day";
        public static XName DayAccessed = biblio + "DayAccessed";
        public static XName Editor = biblio + "Editor";
        public static XName First = biblio + "First";
        public static XName Guid = biblio + "Guid";
        public static XName InternetSiteTitle = biblio + "InternetSiteTitle";
        public static XName Inventor = biblio + "Inventor";
        public static XName Last = biblio + "Last";
        public static XName LCID = biblio + "LCID";
        public static XName Main = biblio + "Main";
        public static XName Medium = biblio + "Medium";
        public static XName Middle = biblio + "Middle";
        public static XName Month = biblio + "Month";
        public static XName MonthAccessed = biblio + "MonthAccessed";
        public static XName NameList = biblio + "NameList";
        public static XName NumberVolumes = biblio + "NumberVolumes";
        public static XName Pages = biblio + "Pages";
        public static XName PatentNumber = biblio + "PatentNumber";
        public static XName Performer = biblio + "Performer";
        public static XName Person = biblio + "Person";
        public static XName ProducerName = biblio + "ProducerName";
        public static XName ProductionCompany = biblio + "ProductionCompany";
        public static XName Publisher = biblio + "Publisher";
        public static XName RefOrder = biblio + "RefOrder";
        public static XName ShortTitle = biblio + "ShortTitle";
        public static XName Source = biblio + "Source";
        public static XName Sources = biblio + "Sources";
        public static XName SourceType = biblio + "SourceType";
        public static XName Tag = biblio + "Tag";
        public static XName Title = biblio + "Title";
        public static XName Translator = biblio + "Translator";
        public static XName Type = biblio + "Type";
        public static XName URL = biblio + "URL";
        public static XName Version = biblio + "Version";
        public static XName Volume = biblio + "Volume";
        public static XName Year = biblio + "Year";
        public static XName YearAccessed = biblio + "YearAccessed";
    }

    public static class C
    {
        public static XNamespace c =
            "http://schemas.openxmlformats.org/drawingml/2006/chart";

        public static XName area3DChart = c + "area3DChart";
        public static XName areaChart = c + "areaChart";
        public static XName auto = c + "auto";
        public static XName autoTitleDeleted = c + "autoTitleDeleted";
        public static XName autoUpdate = c + "autoUpdate";
        public static XName axId = c + "axId";
        public static XName axPos = c + "axPos";
        public static XName backWall = c + "backWall";
        public static XName backward = c + "backward";
        public static XName bandFmt = c + "bandFmt";
        public static XName bandFmts = c + "bandFmts";
        public static XName bar3DChart = c + "bar3DChart";
        public static XName barChart = c + "barChart";
        public static XName barDir = c + "barDir";
        public static XName baseTimeUnit = c + "baseTimeUnit";
        public static XName bubble3D = c + "bubble3D";
        public static XName bubbleChart = c + "bubbleChart";
        public static XName bubbleScale = c + "bubbleScale";
        public static XName bubbleSize = c + "bubbleSize";
        public static XName builtInUnit = c + "builtInUnit";
        public static XName cat = c + "cat";
        public static XName catAx = c + "catAx";
        public static XName chart = c + "chart";
        public static XName chartSpace = c + "chartSpace";
        public static XName crossAx = c + "crossAx";
        public static XName crossBetween = c + "crossBetween";
        public static XName crosses = c + "crosses";
        public static XName crossesAt = c + "crossesAt";
        public static XName custUnit = c + "custUnit";
        public static XName date1904 = c + "date1904";
        public static XName dateAx = c + "dateAx";
        public static XName delete = c + "delete";
        public static XName depthPercent = c + "depthPercent";
        public static XName dispBlanksAs = c + "dispBlanksAs";
        public static XName dispEq = c + "dispEq";
        public static XName dispRSqr = c + "dispRSqr";
        public static XName dispUnits = c + "dispUnits";
        public static XName dispUnitsLbl = c + "dispUnitsLbl";
        public static XName dLbl = c + "dLbl";
        public static XName dLblPos = c + "dLblPos";
        public static XName dLbls = c + "dLbls";
        public static XName doughnutChart = c + "doughnutChart";
        public static XName downBars = c + "downBars";
        public static XName dPt = c + "dPt";
        public static XName dropLines = c + "dropLines";
        public static XName dTable = c + "dTable";
        public static XName errBars = c + "errBars";
        public static XName errBarType = c + "errBarType";
        public static XName errDir = c + "errDir";
        public static XName errValType = c + "errValType";
        public static XName explosion = c + "explosion";
        public static XName externalData = c + "externalData";
        public static XName f = c + "f";
        public static XName firstSliceAng = c + "firstSliceAng";
        public static XName floor = c + "floor";
        public static XName formatCode = c + "formatCode";
        public static XName forward = c + "forward";
        public static XName gapDepth = c + "gapDepth";
        public static XName gapWidth = c + "gapWidth";
        public static XName grouping = c + "grouping";
        public static XName h = c + "h";
        public static XName hiLowLines = c + "hiLowLines";
        public static XName holeSize = c + "holeSize";
        public static XName hPercent = c + "hPercent";
        public static XName idx = c + "idx";
        public static XName intercept = c + "intercept";
        public static XName invertIfNegative = c + "invertIfNegative";
        public static XName lang = c + "lang";
        public static XName layout = c + "layout";
        public static XName layoutTarget = c + "layoutTarget";
        public static XName lblAlgn = c + "lblAlgn";
        public static XName lblOffset = c + "lblOffset";
        public static XName leaderLines = c + "leaderLines";
        public static XName legend = c + "legend";
        public static XName legendEntry = c + "legendEntry";
        public static XName legendPos = c + "legendPos";
        public static XName line3DChart = c + "line3DChart";
        public static XName lineChart = c + "lineChart";
        public static XName logBase = c + "logBase";
        public static XName lvl = c + "lvl";
        public static XName majorGridlines = c + "majorGridlines";
        public static XName majorTickMark = c + "majorTickMark";
        public static XName majorTimeUnit = c + "majorTimeUnit";
        public static XName majorUnit = c + "majorUnit";
        public static XName manualLayout = c + "manualLayout";
        public static XName marker = c + "marker";
        public static XName max = c + "max";
        public static XName min = c + "min";
        public static XName minorGridlines = c + "minorGridlines";
        public static XName minorTickMark = c + "minorTickMark";
        public static XName minorTimeUnit = c + "minorTimeUnit";
        public static XName minorUnit = c + "minorUnit";
        public static XName minus = c + "minus";
        public static XName multiLvlStrCache = c + "multiLvlStrCache";
        public static XName multiLvlStrRef = c + "multiLvlStrRef";
        public static XName noEndCap = c + "noEndCap";
        public static XName noMultiLvlLbl = c + "noMultiLvlLbl";
        public static XName numCache = c + "numCache";
        public static XName numFmt = c + "numFmt";
        public static XName numLit = c + "numLit";
        public static XName numRef = c + "numRef";
        public static XName ofPieChart = c + "ofPieChart";
        public static XName ofPieType = c + "ofPieType";
        public static XName order = c + "order";
        public static XName orientation = c + "orientation";
        public static XName overlap = c + "overlap";
        public static XName overlay = c + "overlay";
        public static XName period = c + "period";
        public static XName perspective = c + "perspective";
        public static XName pie3DChart = c + "pie3DChart";
        public static XName pieChart = c + "pieChart";
        public static XName plotArea = c + "plotArea";
        public static XName plotVisOnly = c + "plotVisOnly";
        public static XName plus = c + "plus";
        public static XName pt = c + "pt";
        public static XName ptCount = c + "ptCount";
        public static XName radarChart = c + "radarChart";
        public static XName radarStyle = c + "radarStyle";
        public static XName rAngAx = c + "rAngAx";
        public static XName rich = c + "rich";
        public static XName rotX = c + "rotX";
        public static XName rotY = c + "rotY";
        public static XName roundedCorners = c + "roundedCorners";
        public static XName scaling = c + "scaling";
        public static XName scatterChart = c + "scatterChart";
        public static XName scatterStyle = c + "scatterStyle";
        public static XName secondPieSize = c + "secondPieSize";
        public static XName separator = c + "separator";
        public static XName ser = c + "ser";
        public static XName serAx = c + "serAx";
        public static XName serLines = c + "serLines";
        public static XName shape = c + "shape";
        public static XName showBubbleSize = c + "showBubbleSize";
        public static XName showCatName = c + "showCatName";
        public static XName showDLblsOverMax = c + "showDLblsOverMax";
        public static XName showHorzBorder = c + "showHorzBorder";
        public static XName showKeys = c + "showKeys";
        public static XName showLeaderLines = c + "showLeaderLines";
        public static XName showLegendKey = c + "showLegendKey";
        public static XName showNegBubbles = c + "showNegBubbles";
        public static XName showOutline = c + "showOutline";
        public static XName showPercent = c + "showPercent";
        public static XName showSerName = c + "showSerName";
        public static XName showVal = c + "showVal";
        public static XName showVertBorder = c + "showVertBorder";
        public static XName sideWall = c + "sideWall";
        public static XName size = c + "size";
        public static XName smooth = c + "smooth";
        public static XName splitPos = c + "splitPos";
        public static XName splitType = c + "splitType";
        public static XName spPr = c + "spPr";
        public static XName stockChart = c + "stockChart";
        public static XName strCache = c + "strCache";
        public static XName strLit = c + "strLit";
        public static XName strRef = c + "strRef";
        public static XName style = c + "style";
        public static XName surface3DChart = c + "surface3DChart";
        public static XName surfaceChart = c + "surfaceChart";
        public static XName symbol = c + "symbol";
        public static XName thickness = c + "thickness";
        public static XName tickLblPos = c + "tickLblPos";
        public static XName tickLblSkip = c + "tickLblSkip";
        public static XName tickMarkSkip = c + "tickMarkSkip";
        public static XName title = c + "title";
        public static XName trendline = c + "trendline";
        public static XName trendlineLbl = c + "trendlineLbl";
        public static XName trendlineType = c + "trendlineType";
        public static XName tx = c + "tx";
        public static XName txPr = c + "txPr";
        public static XName upBars = c + "upBars";
        public static XName upDownBars = c + "upDownBars";
        public static XName userShapes = c + "userShapes";
        public static XName v = c + "v";
        public static XName val = c + "val";
        public static XName valAx = c + "valAx";
        public static XName varyColors = c + "varyColors";
        public static XName view3D = c + "view3D";
        public static XName w = c + "w";
        public static XName wireframe = c + "wireframe";
        public static XName x = c + "x";
        public static XName xMode = c + "xMode";
        public static XName xVal = c + "xVal";
        public static XName y = c + "y";
        public static XName yMode = c + "yMode";
        public static XName yVal = c + "yVal";
    }

    public static class DGM
    {
        public static XNamespace dgm =
            "http://schemas.openxmlformats.org/drawingml/2006/diagram";

        public static XName adj = dgm + "adj";
        public static XName adjLst = dgm + "adjLst";
        public static XName alg = dgm + "alg";
        public static XName animLvl = dgm + "animLvl";
        public static XName animOne = dgm + "animOne";
        public static XName bg = dgm + "bg";
        public static XName bulletEnabled = dgm + "bulletEnabled";
        public static XName cat = dgm + "cat";
        public static XName catLst = dgm + "catLst";
        public static XName chMax = dgm + "chMax";
        public static XName choose = dgm + "choose";
        public static XName chPref = dgm + "chPref";
        public static XName clrData = dgm + "clrData";
        public static XName colorsDef = dgm + "colorsDef";
        public static XName constr = dgm + "constr";
        public static XName constrLst = dgm + "constrLst";
        public static XName cxn = dgm + "cxn";
        public static XName cxnLst = dgm + "cxnLst";
        public static XName dataModel = dgm + "dataModel";
        public static XName desc = dgm + "desc";
        public static XName dir = dgm + "dir";
        public static XName effectClrLst = dgm + "effectClrLst";
        public static XName _else = dgm + "else";
        public static XName extLst = dgm + "extLst";
        public static XName fillClrLst = dgm + "fillClrLst";
        public static XName forEach = dgm + "forEach";
        public static XName hierBranch = dgm + "hierBranch";
        public static XName _if = dgm + "if";
        public static XName layoutDef = dgm + "layoutDef";
        public static XName layoutNode = dgm + "layoutNode";
        public static XName linClrLst = dgm + "linClrLst";
        public static XName orgChart = dgm + "orgChart";
        public static XName param = dgm + "param";
        public static XName presLayoutVars = dgm + "presLayoutVars";
        public static XName presOf = dgm + "presOf";
        public static XName prSet = dgm + "prSet";
        public static XName pt = dgm + "pt";
        public static XName ptLst = dgm + "ptLst";
        public static XName relIds = dgm + "relIds";
        public static XName resizeHandles = dgm + "resizeHandles";
        public static XName rule = dgm + "rule";
        public static XName ruleLst = dgm + "ruleLst";
        public static XName sampData = dgm + "sampData";
        public static XName scene3d = dgm + "scene3d";
        public static XName shape = dgm + "shape";
        public static XName sp3d = dgm + "sp3d";
        public static XName spPr = dgm + "spPr";
        public static XName style = dgm + "style";
        public static XName styleData = dgm + "styleData";
        public static XName styleDef = dgm + "styleDef";
        public static XName styleLbl = dgm + "styleLbl";
        public static XName t = dgm + "t";
        public static XName title = dgm + "title";
        public static XName txEffectClrLst = dgm + "txEffectClrLst";
        public static XName txFillClrLst = dgm + "txFillClrLst";
        public static XName txLinClrLst = dgm + "txLinClrLst";
        public static XName txPr = dgm + "txPr";
        public static XName varLst = dgm + "varLst";
        public static XName whole = dgm + "whole";
    }

    public static class DIGSIG
    {
        public static XNamespace digsig =
            "http://schemas.microsoft.com/office/2006/digsig";

        public static XName ApplicationVersion = digsig + "ApplicationVersion";
        public static XName ColorDepth = digsig + "ColorDepth";
        public static XName HorizontalResolution = digsig + "HorizontalResolution";
        public static XName ManifestHashAlgorithm = digsig + "ManifestHashAlgorithm";
        public static XName Monitors = digsig + "Monitors";
        public static XName OfficeVersion = digsig + "OfficeVersion";
        public static XName SetupID = digsig + "SetupID";
        public static XName SignatureComments = digsig + "SignatureComments";
        public static XName SignatureImage = digsig + "SignatureImage";
        public static XName SignatureInfoV1 = digsig + "SignatureInfoV1";
        public static XName SignatureProviderDetails = digsig + "SignatureProviderDetails";
        public static XName SignatureProviderId = digsig + "SignatureProviderId";
        public static XName SignatureProviderUrl = digsig + "SignatureProviderUrl";
        public static XName SignatureText = digsig + "SignatureText";
        public static XName SignatureType = digsig + "SignatureType";
        public static XName VerticalResolution = digsig + "VerticalResolution";
        public static XName WindowsVersion = digsig + "WindowsVersion";
    }

    public static class DS
    {
        public static XNamespace ds =
            "http://schemas.openxmlformats.org/officeDocument/2006/customXml";

        public static XName datastoreItem = ds + "datastoreItem";
        public static XName itemID = ds + "itemID";
        public static XName schemaRef = ds + "schemaRef";
        public static XName schemaRefs = ds + "schemaRefs";
        public static XName uri = ds + "uri";
    }

    public static class INK
    {
        public static XNamespace ink =
            "http://schemas.microsoft.com/ink/2010/main";

        public static XName context = ink + "context";
        public static XName sourceLink = ink + "sourceLink";
    }

    public static class M
    {
        public static XNamespace m =
            "http://schemas.openxmlformats.org/officeDocument/2006/math";

        public static XName acc = m + "acc";
        public static XName accPr = m + "accPr";
        public static XName aln = m + "aln";
        public static XName alnAt = m + "alnAt";
        public static XName alnScr = m + "alnScr";
        public static XName argPr = m + "argPr";
        public static XName argSz = m + "argSz";
        public static XName bar = m + "bar";
        public static XName barPr = m + "barPr";
        public static XName baseJc = m + "baseJc";
        public static XName begChr = m + "begChr";
        public static XName borderBox = m + "borderBox";
        public static XName borderBoxPr = m + "borderBoxPr";
        public static XName box = m + "box";
        public static XName boxPr = m + "boxPr";
        public static XName brk = m + "brk";
        public static XName brkBin = m + "brkBin";
        public static XName brkBinSub = m + "brkBinSub";
        public static XName cGp = m + "cGp";
        public static XName cGpRule = m + "cGpRule";
        public static XName chr = m + "chr";
        public static XName count = m + "count";
        public static XName cSp = m + "cSp";
        public static XName ctrlPr = m + "ctrlPr";
        public static XName d = m + "d";
        public static XName defJc = m + "defJc";
        public static XName deg = m + "deg";
        public static XName degHide = m + "degHide";
        public static XName den = m + "den";
        public static XName diff = m + "diff";
        public static XName dispDef = m + "dispDef";
        public static XName dPr = m + "dPr";
        public static XName e = m + "e";
        public static XName endChr = m + "endChr";
        public static XName eqArr = m + "eqArr";
        public static XName eqArrPr = m + "eqArrPr";
        public static XName f = m + "f";
        public static XName fName = m + "fName";
        public static XName fPr = m + "fPr";
        public static XName func = m + "func";
        public static XName funcPr = m + "funcPr";
        public static XName groupChr = m + "groupChr";
        public static XName groupChrPr = m + "groupChrPr";
        public static XName grow = m + "grow";
        public static XName hideBot = m + "hideBot";
        public static XName hideLeft = m + "hideLeft";
        public static XName hideRight = m + "hideRight";
        public static XName hideTop = m + "hideTop";
        public static XName interSp = m + "interSp";
        public static XName intLim = m + "intLim";
        public static XName intraSp = m + "intraSp";
        public static XName jc = m + "jc";
        public static XName lim = m + "lim";
        public static XName limLoc = m + "limLoc";
        public static XName limLow = m + "limLow";
        public static XName limLowPr = m + "limLowPr";
        public static XName limUpp = m + "limUpp";
        public static XName limUppPr = m + "limUppPr";
        public static XName lit = m + "lit";
        public static XName lMargin = m + "lMargin";
        public static XName _m = m + "m";
        public static XName mathFont = m + "mathFont";
        public static XName mathPr = m + "mathPr";
        public static XName maxDist = m + "maxDist";
        public static XName mc = m + "mc";
        public static XName mcJc = m + "mcJc";
        public static XName mcPr = m + "mcPr";
        public static XName mcs = m + "mcs";
        public static XName mPr = m + "mPr";
        public static XName mr = m + "mr";
        public static XName nary = m + "nary";
        public static XName naryLim = m + "naryLim";
        public static XName naryPr = m + "naryPr";
        public static XName noBreak = m + "noBreak";
        public static XName nor = m + "nor";
        public static XName num = m + "num";
        public static XName objDist = m + "objDist";
        public static XName oMath = m + "oMath";
        public static XName oMathPara = m + "oMathPara";
        public static XName oMathParaPr = m + "oMathParaPr";
        public static XName opEmu = m + "opEmu";
        public static XName phant = m + "phant";
        public static XName phantPr = m + "phantPr";
        public static XName plcHide = m + "plcHide";
        public static XName pos = m + "pos";
        public static XName postSp = m + "postSp";
        public static XName preSp = m + "preSp";
        public static XName r = m + "r";
        public static XName rad = m + "rad";
        public static XName radPr = m + "radPr";
        public static XName rMargin = m + "rMargin";
        public static XName rPr = m + "rPr";
        public static XName rSp = m + "rSp";
        public static XName rSpRule = m + "rSpRule";
        public static XName scr = m + "scr";
        public static XName sepChr = m + "sepChr";
        public static XName show = m + "show";
        public static XName shp = m + "shp";
        public static XName smallFrac = m + "smallFrac";
        public static XName sPre = m + "sPre";
        public static XName sPrePr = m + "sPrePr";
        public static XName sSub = m + "sSub";
        public static XName sSubPr = m + "sSubPr";
        public static XName sSubSup = m + "sSubSup";
        public static XName sSubSupPr = m + "sSubSupPr";
        public static XName sSup = m + "sSup";
        public static XName sSupPr = m + "sSupPr";
        public static XName strikeBLTR = m + "strikeBLTR";
        public static XName strikeH = m + "strikeH";
        public static XName strikeTLBR = m + "strikeTLBR";
        public static XName strikeV = m + "strikeV";
        public static XName sty = m + "sty";
        public static XName sub = m + "sub";
        public static XName subHide = m + "subHide";
        public static XName sup = m + "sup";
        public static XName supHide = m + "supHide";
        public static XName t = m + "t";
        public static XName transp = m + "transp";
        public static XName type = m + "type";
        public static XName val = m + "val";
        public static XName vertJc = m + "vertJc";
        public static XName wrapIndent = m + "wrapIndent";
        public static XName wrapRight = m + "wrapRight";
        public static XName zeroAsc = m + "zeroAsc";
        public static XName zeroDesc = m + "zeroDesc";
        public static XName zeroWid = m + "zeroWid";
    }

    public static class MC
    {
        public static XNamespace mc =
            "http://schemas.openxmlformats.org/markup-compatibility/2006";

        public static XName AlternateContent = mc + "AlternateContent";
        public static XName Choice = mc + "Choice";
        public static XName Fallback = mc + "Fallback";
        public static XName Ignorable = mc + "Ignorable";
        public static XName PreserveAttributes = mc + "PreserveAttributes";
    }

    public static class NoNamespace
    {
        public static XName a = "a";
        public static XName accentbar = "accentbar";
        public static XName adj = "adj";
        public static XName adjusthandles = "adjusthandles";
        public static XName algn = "algn";
        public static XName Algorithm = "Algorithm";
        public static XName alignshape = "alignshape";
        public static XName allowcomments = "allowcomments";
        public static XName allowOverlap = "allowOverlap";
        public static XName alt = "alt";
        public static XName altLang = "altLang";
        public static XName amount = "amount";
        public static XName amt = "amt";
        public static XName anchor = "anchor";
        public static XName anchorCtr = "anchorCtr";
        public static XName anchorx = "anchorx";
        public static XName anchory = "anchory";
        public static XName ang = "ang";
        public static XName angle = "angle";
        public static XName annotation = "annotation";
        public static XName arcsize = "arcsize";
        public static XName arg = "arg";
        public static XName arrowok = "arrowok";
        public static XName aspect = "aspect";
        public static XName aspectratio = "aspectratio";
        public static XName attributeFormDefault = "attributeFormDefault";
        public static XName autoformat = "autoformat";
        public static XName autolayout = "autolayout";
        public static XName autorotationcenter = "autorotationcenter";
        public static XName axis = "axis";
        public static XName b = "b";
        public static XName backdepth = "backdepth";
        public static XName _base = "base";
        public static XName baseline = "baseline";
        public static XName baseType = "baseType";
        public static XName behindDoc = "behindDoc";
        public static XName bilevel = "bilevel";
        public static XName bIns = "bIns";
        public static XName blacklevel = "blacklevel";
        public static XName blend = "blend";
        public static XName blipPhldr = "blipPhldr";
        public static XName blockDefault = "blockDefault";
        public static XName blurRad = "blurRad";
        public static XName bright = "bright";
        public static XName brightness = "brightness";
        public static XName brushRef = "brushRef";
        public static XName bwMode = "bwMode";
        public static XName cap = "cap";
        public static XName channel = "channel";
        public static XName _char = "char";
        public static XName charset = "charset";
        public static XName chksum = "chksum";
        public static XName chOrder = "chOrder";
        public static XName chromakey = "chromakey";
        public static XName _class = "class";
        public static XName cmpd = "cmpd";
        public static XName cnt = "cnt";
        public static XName color = "color";
        public static XName color2 = "color2";
        public static XName colors = "colors";
        public static XName colorTemp = "colorTemp";
        public static XName compatLnSpc = "compatLnSpc";
        public static XName connectloc = "connectloc";
        public static XName constrainbounds = "constrainbounds";
        public static XName contextRef = "contextRef";
        public static XName contourW = "contourW";
        public static XName contrast = "contrast";
        public static XName control1 = "control1";
        public static XName control2 = "control2";
        public static XName coordorigin = "coordorigin";
        public static XName coordsize = "coordsize";
        public static XName cropbottom = "cropbottom";
        public static XName cropleft = "cropleft";
        public static XName cropping = "cropping";
        public static XName cropright = "cropright";
        public static XName croptop = "croptop";
        public static XName csCatId = "csCatId";
        public static XName cstate = "cstate";
        public static XName csTypeId = "csTypeId";
        public static XName custAng = "custAng";
        public static XName custLinFactNeighborX = "custLinFactNeighborX";
        public static XName custLinFactNeighborY = "custLinFactNeighborY";
        public static XName custLinFactY = "custLinFactY";
        public static XName custScaleX = "custScaleX";
        public static XName custScaleY = "custScaleY";
        public static XName custT = "custT";
        public static XName cx = "cx";
        public static XName cxnId = "cxnId";
        public static XName cy = "cy";
        public static XName d = "d";
        public static XName dashstyle = "dashstyle";
        public static XName data = "data";
        public static XName defTabSz = "defTabSz";
        public static XName descr = "descr";
        public static XName destId = "destId";
        public static XName destOrd = "destOrd";
        public static XName dgmbasetextscale = "dgmbasetextscale";
        public static XName dgmfontsize = "dgmfontsize";
        public static XName dgmscalex = "dgmscalex";
        public static XName dgmscaley = "dgmscaley";
        public static XName dgmstyle = "dgmstyle";
        public static XName diffusity = "diffusity";
        public static XName dir = "dir";
        public static XName direction = "direction";
        public static XName dirty = "dirty";
        public static XName dist = "dist";
        public static XName distance = "distance";
        public static XName distB = "distB";
        public static XName distL = "distL";
        public static XName distR = "distR";
        public static XName distT = "distT";
        public static XName documentManagement = "documentManagement";
        public static XName dpi = "dpi";
        public static XName DrawAspect = "DrawAspect";
        public static XName drop = "drop";
        public static XName dropauto = "dropauto";
        public static XName dx = "dx";
        public static XName dy = "dy";
        public static XName dz = "dz";
        public static XName eaLnBrk = "eaLnBrk";
        public static XName edge = "edge";
        public static XName editas = "editas";
        public static XName edited = "edited";
        public static XName elementFormDefault = "elementFormDefault";
        public static XName embosscolor = "embosscolor";
        public static XName end = "end";
        public static XName endA = "endA";
        public static XName endangle = "endangle";
        public static XName endarrow = "endarrow";
        public static XName endarrowlength = "endarrowlength";
        public static XName endarrowwidth = "endarrowwidth";
        public static XName endcap = "endcap";
        public static XName endPos = "endPos";
        public static XName eqn = "eqn";
        public static XName equationxml = "equationxml";
        public static XName extrusioncolor = "extrusioncolor";
        public static XName extrusionH = "extrusionH";
        public static XName facet = "facet";
        public static XName fact = "fact";
        public static XName fill = "fill";
        public static XName fillcolor = "fillcolor";
        public static XName filled = "filled";
        public static XName fillok = "fillok";
        public static XName filltype = "filltype";
        public static XName fitpath = "fitpath";
        public static XName fitshape = "fitshape";
        public static XName _fixed = "fixed";
        public static XName flip = "flip";
        public static XName flipH = "flipH";
        public static XName flipV = "flipV";
        public static XName fLocksText = "fLocksText";
        public static XName fmla = "fmla";
        public static XName fmtid = "fmtid";
        public static XName focus = "focus";
        public static XName focusposition = "focusposition";
        public static XName focussize = "focussize";
        public static XName foo = "foo";
        public static XName _for = "for";
        public static XName forceAA = "forceAA";
        public static XName foredepth = "foredepth";
        public static XName formatCode = "formatCode";
        public static XName forName = "forName";
        public static XName fov = "fov";
        public static XName from = "from";
        public static XName fromWordArt = "fromWordArt";
        public static XName func = "func";
        public static XName g = "g";
        public static XName gain = "gain";
        public static XName gamma = "gamma";
        public static XName gap = "gap";
        public static XName gradientshapeok = "gradientshapeok";
        public static XName grayscale = "grayscale";
        public static XName grouping = "grouping";
        public static XName h = "h";
        public static XName hangingPunct = "hangingPunct";
        public static XName height = "height";
        public static XName hidden = "hidden";
        public static XName hideGeom = "hideGeom";
        public static XName hideLastTrans = "hideLastTrans";
        public static XName horzOverflow = "horzOverflow";
        public static XName hotPoints = "hotPoints";
        public static XName hR = "hR";
        public static XName href = "href";
        public static XName hue = "hue";
        public static XName i = "i";
        public static XName id = "id";
        public static XName Id = "Id";
        public static XName idcntr = "idcntr";
        public static XName iddest = "iddest";
        public static XName idQ = "idQ";
        public static XName idref = "idref";
        public static XName idsrc = "idsrc";
        public static XName idx = "idx";
        public static XName image = "image";
        public static XName imagealignshape = "imagealignshape";
        public static XName imageaspect = "imageaspect";
        public static XName imagesize = "imagesize";
        public static XName indent = "indent";
        public static XName inset = "inset";
        public static XName insetpen = "insetpen";
        public static XName insetpenok = "insetpenok";
        public static XName invx = "invx";
        public static XName invy = "invy";
        public static XName issignatureline = "issignatureline";
        public static XName joinstyle = "joinstyle";
        public static XName kern = "kern";
        public static XName kumimoji = "kumimoji";
        public static XName kx = "kx";
        public static XName ky = "ky";
        public static XName l = "l";
        public static XName label = "label";
        public static XName lang = "lang";
        public static XName lastClr = "lastClr";
        public static XName lastView = "lastView";
        public static XName lat = "lat";
        public static XName latinLnBrk = "latinLnBrk";
        public static XName layoutInCell = "layoutInCell";
        public static XName len = "len";
        public static XName length = "length";
        public static XName lengthspecified = "lengthspecified";
        public static XName lightface = "lightface";
        public static XName lightharsh = "lightharsh";
        public static XName lightharsh2 = "lightharsh2";
        public static XName lightlevel = "lightlevel";
        public static XName lightlevel2 = "lightlevel2";
        public static XName lightposition = "lightposition";
        public static XName lightposition2 = "lightposition2";
        public static XName lim = "lim";
        public static XName limo = "limo";
        public static XName linestyle = "linestyle";
        public static XName linkTarget = "linkTarget";
        public static XName lIns = "lIns";
        public static XName loCatId = "loCatId";
        public static XName locked = "locked";
        public static XName lockrotationcenter = "lockrotationcenter";
        public static XName lon = "lon";
        public static XName loTypeId = "loTypeId";
        public static XName lum = "lum";
        public static XName lvl = "lvl";
        public static XName macro = "macro";
        public static XName map = "map";
        public static XName marL = "marL";
        public static XName marR = "marR";
        public static XName matrix = "matrix";
        public static XName max = "max";
        public static XName maxOccurs = "maxOccurs";
        public static XName metal = "metal";
        public static XName meth = "meth";
        public static XName method = "method";
        public static XName minOccurs = "minOccurs";
        public static XName minusx = "minusx";
        public static XName minusy = "minusy";
        public static XName minVer = "minVer";
        public static XName miterlimit = "miterlimit";
        public static XName modelId = "modelId";
        public static XName moveWith = "moveWith";
        public static XName n = "n";
        public static XName name = "name";
        public static XName _namespace = "namespace";
        public static XName _new = "new";
        public static XName nillable = "nillable";
        public static XName noChangeArrowheads = "noChangeArrowheads";
        public static XName noChangeAspect = "noChangeAspect";
        public static XName noChangeShapeType = "noChangeShapeType";
        public static XName noCrop = "noCrop";
        public static XName noDrilldown = "noDrilldown";
        public static XName noGrp = "noGrp";
        public static XName noMove = "noMove";
        public static XName noResize = "noResize";
        public static XName noRot = "noRot";
        public static XName noSelect = "noSelect";
        public static XName noTextEdit = "noTextEdit";
        public static XName numCol = "numCol";
        public static XName ObjectID = "ObjectID";
        public static XName ObjectType = "ObjectType";
        public static XName obscured = "obscured";
        public static XName offset = "offset";
        public static XName offset2 = "offset2";
        public static XName old = "old";
        public static XName on = "on";
        public static XName op = "op";
        public static XName opacity = "opacity";
        public static XName orient = "orient";
        public static XName orientation = "orientation";
        public static XName orientationangle = "orientationangle";
        public static XName origin = "origin";
        public static XName parTransId = "parTransId";
        public static XName path = "path";
        public static XName phldr = "phldr";
        public static XName phldrT = "phldrT";
        public static XName pid = "pid";
        public static XName pitchFamily = "pitchFamily";
        public static XName points = "points";
        public static XName polar = "polar";
        public static XName pos = "pos";
        public static XName position = "position";
        public static XName preferRelativeResize = "preferRelativeResize";
        public static XName presAssocID = "presAssocID";
        public static XName presId = "presId";
        public static XName presName = "presName";
        public static XName presStyleCnt = "presStyleCnt";
        public static XName presStyleIdx = "presStyleIdx";
        public static XName presStyleLbl = "presStyleLbl";
        public static XName pri = "pri";
        public static XName print = "print";
        public static XName ProgID = "ProgID";
        public static XName provid = "provid";
        public static XName prst = "prst";
        public static XName prstMaterial = "prstMaterial";
        public static XName ptType = "ptType";
        public static XName qsCatId = "qsCatId";
        public static XName qsTypeId = "qsTypeId";
        public static XName r = "r";
        public static XName rad = "rad";
        public static XName radiusrange = "radiusrange";
        public static XName rasterOp = "rasterOp";
        public static XName recolor = "recolor";
        public static XName recolortarget = "recolortarget";
        public static XName _ref = "ref";
        public static XName refFor = "refFor";
        public static XName refForName = "refForName";
        public static XName refPtType = "refPtType";
        public static XName refType = "refType";
        public static XName relativeFrom = "relativeFrom";
        public static XName relativeHeight = "relativeHeight";
        public static XName relId = "relId";
        public static XName render = "render";
        public static XName Requires = "Requires";
        public static XName Resolved = "Resolved";
        public static XName rev = "rev";
        public static XName reverse = "reverse";
        public static XName rig = "rig";
        public static XName rIns = "rIns";
        public static XName rot = "rot";
        public static XName rotate = "rotate";
        public static XName rotatedBoundingBox = "rotatedBoundingBox";
        public static XName rotation = "rotation";
        public static XName rotationangle = "rotationangle";
        public static XName rotationcenter = "rotationcenter";
        public static XName rotWithShape = "rotWithShape";
        public static XName rtl = "rtl";
        public static XName rtlCol = "rtlCol";
        public static XName sat = "sat";
        public static XName scaled = "scaled";
        public static XName scaling = "scaling";
        public static XName schemaLocation = "schemaLocation";
        public static XName script = "script";
        public static XName SelectedStyle = "SelectedStyle";
        public static XName selection = "selection";
        public static XName semanticType = "semanticType";
        public static XName seq = "seq";
        public static XName shadow = "shadow";
        public static XName shadowcolor = "shadowcolor";
        public static XName shadowok = "shadowok";
        public static XName ShapeID = "ShapeID";
        public static XName shapeName = "shapeName";
        public static XName shapetype = "shapetype";
        public static XName shininess = "shininess";
        public static XName showOutlineIcons = "showOutlineIcons";
        public static XName showsigndate = "showsigndate";
        public static XName sibTransId = "sibTransId";
        public static XName side = "side";
        public static XName signinginstructionsset = "signinginstructionsset";
        public static XName simplePos = "simplePos";
        public static XName size = "size";
        public static XName skewamt = "skewamt";
        public static XName skewangle = "skewangle";
        public static XName smtClean = "smtClean";
        public static XName SourceId = "SourceId";
        public static XName sourceLinked = "sourceLinked";
        public static XName spc = "spc";
        public static XName spcCol = "spcCol";
        public static XName spcFirstLastPara = "spcFirstLastPara";
        public static XName specularity = "specularity";
        public static XName spid = "spid";
        public static XName spidmax = "spidmax";
        public static XName src = "src";
        public static XName srcId = "srcId";
        public static XName srcOrd = "srcOrd";
        public static XName st = "st";
        public static XName stA = "stA";
        public static XName stAng = "stAng";
        public static XName start = "start";
        public static XName startangle = "startangle";
        public static XName startarrow = "startarrow";
        public static XName startarrowlength = "startarrowlength";
        public static XName startarrowwidth = "startarrowwidth";
        public static XName step = "step";
        public static XName strike = "strike";
        public static XName _string = "string";
        public static XName stroke = "stroke";
        public static XName strokecolor = "strokecolor";
        public static XName stroked = "stroked";
        public static XName strokeok = "strokeok";
        public static XName strokeweight = "strokeweight";
        public static XName style = "style";
        public static XName styleLbl = "styleLbl";
        public static XName StyleName = "StyleName";
        public static XName swAng = "swAng";
        public static XName _switch = "switch";
        public static XName sx = "sx";
        public static XName sy = "sy";
        public static XName sz = "sz";
        public static XName t = "t";
        public static XName target = "target";
        public static XName Target = "Target";
        public static XName targetNamespace = "targetNamespace";
        public static XName text = "text";
        public static XName textborder = "textborder";
        public static XName textboxrect = "textboxrect";
        public static XName textlink = "textlink";
        public static XName textpathok = "textpathok";
        public static XName tgtFrame = "tgtFrame";
        public static XName thresh = "thresh";
        public static XName tIns = "tIns";
        public static XName tip = "tip";
        public static XName title = "title";
        public static XName titleOptions = "titleOptions";
        public static XName to = "to";
        public static XName tooltip = "tooltip";
        public static XName trans = "trans";
        public static XName trim = "trim";
        public static XName tx = "tx";
        public static XName txBox = "txBox";
        public static XName txbxSeq = "txbxSeq";
        public static XName txbxStory = "txbxStory";
        public static XName ty = "ty";
        public static XName type = "type";
        public static XName Type = "Type";
        public static XName typeface = "typeface";
        public static XName u = "u";
        public static XName ungrouping = "ungrouping";
        public static XName uniqueId = "uniqueId";
        public static XName units = "units";
        public static XName UpdateMode = "UpdateMode";
        public static XName upright = "upright";
        public static XName uri = "uri";
        public static XName URI = "URI";
        public static XName useDef = "useDef";
        public static XName v = "v";
        public static XName val = "val";
        public static XName value = "value";
        public static XName varScale = "varScale";
        public static XName version = "version";
        public static XName vert = "vert";
        public static XName verticies = "verticies";
        public static XName vertOverflow = "vertOverflow";
        public static XName viewpoint = "viewpoint";
        public static XName viewpointorigin = "viewpointorigin";
        public static XName visible = "visible";
        public static XName w = "w";
        public static XName weight = "weight";
        public static XName width = "width";
        public static XName wR = "wR";
        public static XName wrap = "wrap";
        public static XName wrapcoords = "wrapcoords";
        public static XName wrapText = "wrapText";
        public static XName x = "x";
        public static XName x1 = "x1";
        public static XName x2 = "x2";
        public static XName xrange = "xrange";
        public static XName xscale = "xscale";
        public static XName y = "y";
        public static XName y1 = "y1";
        public static XName y2 = "y2";
        public static XName yrange = "yrange";
        public static XName z = "z";
        public static XName zoom = "zoom";
        public static XName zOrderOff = "zOrderOff";
    }

    public static class O
    {
        public static XNamespace o =
            "urn:schemas-microsoft-com:office:office";

        public static XName allowincell = o + "allowincell";
        public static XName allowoverlap = o + "allowoverlap";
        public static XName althref = o + "althref";
        public static XName borderbottomcolor = o + "borderbottomcolor";
        public static XName borderleftcolor = o + "borderleftcolor";
        public static XName borderrightcolor = o + "borderrightcolor";
        public static XName bordertopcolor = o + "bordertopcolor";
        public static XName bottom = o + "bottom";
        public static XName bullet = o + "bullet";
        public static XName button = o + "button";
        public static XName bwmode = o + "bwmode";
        public static XName bwnormal = o + "bwnormal";
        public static XName bwpure = o + "bwpure";
        public static XName callout = o + "callout";
        public static XName clip = o + "clip";
        public static XName clippath = o + "clippath";
        public static XName cliptowrap = o + "cliptowrap";
        public static XName colormenu = o + "colormenu";
        public static XName colormru = o + "colormru";
        public static XName column = o + "column";
        public static XName complex = o + "complex";
        public static XName connectangles = o + "connectangles";
        public static XName connectlocs = o + "connectlocs";
        public static XName connectortype = o + "connectortype";
        public static XName connecttype = o + "connecttype";
        public static XName detectmouseclick = o + "detectmouseclick";
        public static XName dgmlayout = o + "dgmlayout";
        public static XName dgmlayoutmru = o + "dgmlayoutmru";
        public static XName dgmnodekind = o + "dgmnodekind";
        public static XName diagram = o + "diagram";
        public static XName doubleclicknotify = o + "doubleclicknotify";
        public static XName entry = o + "entry";
        public static XName extrusion = o + "extrusion";
        public static XName extrusionok = o + "extrusionok";
        public static XName FieldCodes = o + "FieldCodes";
        public static XName fill = o + "fill";
        public static XName forcedash = o + "forcedash";
        public static XName gfxdata = o + "gfxdata";
        public static XName hr = o + "hr";
        public static XName hralign = o + "hralign";
        public static XName href = o + "href";
        public static XName hrnoshade = o + "hrnoshade";
        public static XName hrpct = o + "hrpct";
        public static XName hrstd = o + "hrstd";
        public static XName idmap = o + "idmap";
        public static XName ink = o + "ink";
        public static XName insetmode = o + "insetmode";
        public static XName left = o + "left";
        public static XName LinkType = o + "LinkType";
        public static XName _lock = o + "lock";
        public static XName LockedField = o + "LockedField";
        public static XName master = o + "master";
        public static XName ole = o + "ole";
        public static XName oleicon = o + "oleicon";
        public static XName OLEObject = o + "OLEObject";
        public static XName oned = o + "oned";
        public static XName opacity2 = o + "opacity2";
        public static XName preferrelative = o + "preferrelative";
        public static XName proxy = o + "proxy";
        public static XName r = o + "r";
        public static XName regroupid = o + "regroupid";
        public static XName regrouptable = o + "regrouptable";
        public static XName rel = o + "rel";
        public static XName relationtable = o + "relationtable";
        public static XName right = o + "right";
        public static XName rules = o + "rules";
        public static XName shapedefaults = o + "shapedefaults";
        public static XName shapelayout = o + "shapelayout";
        public static XName signatureline = o + "signatureline";
        public static XName singleclick = o + "singleclick";
        public static XName skew = o + "skew";
        public static XName spid = o + "spid";
        public static XName spt = o + "spt";
        public static XName suggestedsigner = o + "suggestedsigner";
        public static XName suggestedsigner2 = o + "suggestedsigner2";
        public static XName suggestedsigneremail = o + "suggestedsigneremail";
        public static XName tablelimits = o + "tablelimits";
        public static XName tableproperties = o + "tableproperties";
        public static XName targetscreensize = o + "targetscreensize";
        public static XName title = o + "title";
        public static XName top = o + "top";
        public static XName userdrawn = o + "userdrawn";
        public static XName userhidden = o + "userhidden";
        public static XName v = o + "v";
    }

    public static class Pic
    {
        public static XNamespace pic =
            "http://schemas.openxmlformats.org/drawingml/2006/picture";

        public static XName blipFill = pic + "blipFill";
        public static XName cNvPicPr = pic + "cNvPicPr";
        public static XName cNvPr = pic + "cNvPr";
        public static XName nvPicPr = pic + "nvPicPr";
        public static XName _pic = pic + "pic";
        public static XName spPr = pic + "spPr";
    }

    public static class R
    {
        public static XNamespace r =
            "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

        public static XName blip = r + "blip";
        public static XName cs = r + "cs";
        public static XName dm = r + "dm";
        public static XName embed = r + "embed";
        public static XName href = r + "href";
        public static XName id = r + "id";
        public static XName link = r + "link";
        public static XName lo = r + "lo";
        public static XName pict = r + "pict";
        public static XName qs = r + "qs";
    }

    public static class VML
    {
        public static XNamespace vml =
            "urn:schemas-microsoft-com:vml";

        public static XName arc = vml + "arc";
        public static XName background = vml + "background";
        public static XName curve = vml + "curve";
        public static XName ext = vml + "ext";
        public static XName f = vml + "f";
        public static XName fill = vml + "fill";
        public static XName formulas = vml + "formulas";
        public static XName group = vml + "group";
        public static XName h = vml + "h";
        public static XName handles = vml + "handles";
        public static XName image = vml + "image";
        public static XName imagedata = vml + "imagedata";
        public static XName line = vml + "line";
        public static XName oval = vml + "oval";
        public static XName path = vml + "path";
        public static XName polyline = vml + "polyline";
        public static XName rect = vml + "rect";
        public static XName roundrect = vml + "roundrect";
        public static XName shadow = vml + "shadow";
        public static XName shape = vml + "shape";
        public static XName shapetype = vml + "shapetype";
        public static XName stroke = vml + "stroke";
        public static XName textbox = vml + "textbox";
        public static XName textpath = vml + "textpath";
    }

    public static class W
    {
        public static XNamespace w =
            "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        public static XName abstractNum = w + "abstractNum";
        public static XName abstractNumId = w + "abstractNumId";
        public static XName accent1 = w + "accent1";
        public static XName accent2 = w + "accent2";
        public static XName accent3 = w + "accent3";
        public static XName accent4 = w + "accent4";
        public static XName accent5 = w + "accent5";
        public static XName accent6 = w + "accent6";
        public static XName active = w + "active";
        public static XName activeRecord = w + "activeRecord";
        public static XName activeWritingStyle = w + "activeWritingStyle";
        public static XName actualPg = w + "actualPg";
        public static XName addressFieldName = w + "addressFieldName";
        public static XName adjustLineHeightInTable = w + "adjustLineHeightInTable";
        public static XName adjustRightInd = w + "adjustRightInd";
        public static XName after = w + "after";
        public static XName afterAutospacing = w + "afterAutospacing";
        public static XName afterLines = w + "afterLines";
        public static XName algIdExt = w + "algIdExt";
        public static XName algIdExtSource = w + "algIdExtSource";
        public static XName alias = w + "alias";
        public static XName aliases = w + "aliases";
        public static XName alignBordersAndEdges = w + "alignBordersAndEdges";
        public static XName alignment = w + "alignment";
        public static XName alignTablesRowByRow = w + "alignTablesRowByRow";
        public static XName allowPNG = w + "allowPNG";
        public static XName allowSpaceOfSameStyleInTable = w + "allowSpaceOfSameStyleInTable";
        public static XName altChunk = w + "altChunk";
        public static XName altChunkPr = w + "altChunkPr";
        public static XName altName = w + "altName";
        public static XName alwaysMergeEmptyNamespace = w + "alwaysMergeEmptyNamespace";
        public static XName alwaysShowPlaceholderText = w + "alwaysShowPlaceholderText";
        public static XName anchor = w + "anchor";
        public static XName anchorLock = w + "anchorLock";
        public static XName annotationRef = w + "annotationRef";
        public static XName applyBreakingRules = w + "applyBreakingRules";
        public static XName appName = w + "appName";
        public static XName ascii = w + "ascii";
        public static XName asciiTheme = w + "asciiTheme";
        public static XName attachedSchema = w + "attachedSchema";
        public static XName attachedTemplate = w + "attachedTemplate";
        public static XName attr = w + "attr";
        public static XName author = w + "author";
        public static XName autofitToFirstFixedWidthCell = w + "autofitToFirstFixedWidthCell";
        public static XName autoFormatOverride = w + "autoFormatOverride";
        public static XName autoHyphenation = w + "autoHyphenation";
        public static XName autoRedefine = w + "autoRedefine";
        public static XName autoSpaceDE = w + "autoSpaceDE";
        public static XName autoSpaceDN = w + "autoSpaceDN";
        public static XName autoSpaceLikeWord95 = w + "autoSpaceLikeWord95";
        public static XName b = w + "b";
        public static XName background = w + "background";
        public static XName balanceSingleByteDoubleByteWidth = w + "balanceSingleByteDoubleByteWidth";
        public static XName bar = w + "bar";
        public static XName basedOn = w + "basedOn";
        public static XName bCs = w + "bCs";
        public static XName bdo = w + "bdo";
        public static XName bdr = w + "bdr";
        public static XName before = w + "before";
        public static XName beforeAutospacing = w + "beforeAutospacing";
        public static XName beforeLines = w + "beforeLines";
        public static XName behavior = w + "behavior";
        public static XName behaviors = w + "behaviors";
        public static XName between = w + "between";
        public static XName bg1 = w + "bg1";
        public static XName bg2 = w + "bg2";
        public static XName bibliography = w + "bibliography";
        public static XName bidi = w + "bidi";
        public static XName bidiVisual = w + "bidiVisual";
        public static XName blockQuote = w + "blockQuote";
        public static XName body = w + "body";
        public static XName bodyDiv = w + "bodyDiv";
        public static XName bookFoldPrinting = w + "bookFoldPrinting";
        public static XName bookFoldPrintingSheets = w + "bookFoldPrintingSheets";
        public static XName bookFoldRevPrinting = w + "bookFoldRevPrinting";
        public static XName bookmarkEnd = w + "bookmarkEnd";
        public static XName bookmarkStart = w + "bookmarkStart";
        public static XName bordersDoNotSurroundFooter = w + "bordersDoNotSurroundFooter";
        public static XName bordersDoNotSurroundHeader = w + "bordersDoNotSurroundHeader";
        public static XName bottom = w + "bottom";
        public static XName bottomFromText = w + "bottomFromText";
        public static XName br = w + "br";
        public static XName cachedColBalance = w + "cachedColBalance";
        public static XName calcOnExit = w + "calcOnExit";
        public static XName calendar = w + "calendar";
        public static XName cantSplit = w + "cantSplit";
        public static XName caps = w + "caps";
        public static XName category = w + "category";
        public static XName cellDel = w + "cellDel";
        public static XName cellIns = w + "cellIns";
        public static XName cellMerge = w + "cellMerge";
        public static XName chapSep = w + "chapSep";
        public static XName chapStyle = w + "chapStyle";
        public static XName _char = w + "char";
        public static XName characterSpacingControl = w + "characterSpacingControl";
        public static XName charset = w + "charset";
        public static XName charSpace = w + "charSpace";
        public static XName checkBox = w + "checkBox";
        public static XName _checked = w + "checked";
        public static XName checkErrors = w + "checkErrors";
        public static XName checkStyle = w + "checkStyle";
        public static XName citation = w + "citation";
        public static XName clear = w + "clear";
        public static XName clickAndTypeStyle = w + "clickAndTypeStyle";
        public static XName clrSchemeMapping = w + "clrSchemeMapping";
        public static XName cnfStyle = w + "cnfStyle";
        public static XName code = w + "code";
        public static XName col = w + "col";
        public static XName colDelim = w + "colDelim";
        public static XName colFirst = w + "colFirst";
        public static XName colLast = w + "colLast";
        public static XName color = w + "color";
        public static XName cols = w + "cols";
        public static XName column = w + "column";
        public static XName combine = w + "combine";
        public static XName combineBrackets = w + "combineBrackets";
        public static XName comboBox = w + "comboBox";
        public static XName comment = w + "comment";
        public static XName commentRangeEnd = w + "commentRangeEnd";
        public static XName commentRangeStart = w + "commentRangeStart";
        public static XName commentReference = w + "commentReference";
        public static XName comments = w + "comments";
        public static XName compat = w + "compat";
        public static XName compatSetting = w + "compatSetting";
        public static XName connectString = w + "connectString";
        public static XName consecutiveHyphenLimit = w + "consecutiveHyphenLimit";
        public static XName contextualSpacing = w + "contextualSpacing";
        public static XName continuationSeparator = w + "continuationSeparator";
        public static XName control = w + "control";
        public static XName convMailMergeEsc = w + "convMailMergeEsc";
        public static XName count = w + "count";
        public static XName countBy = w + "countBy";
        public static XName cr = w + "cr";
        public static XName cryptAlgorithmClass = w + "cryptAlgorithmClass";
        public static XName cryptAlgorithmSid = w + "cryptAlgorithmSid";
        public static XName cryptAlgorithmType = w + "cryptAlgorithmType";
        public static XName cryptProvider = w + "cryptProvider";
        public static XName cryptProviderType = w + "cryptProviderType";
        public static XName cryptProviderTypeExt = w + "cryptProviderTypeExt";
        public static XName cryptProviderTypeExtSource = w + "cryptProviderTypeExtSource";
        public static XName cryptSpinCount = w + "cryptSpinCount";
        public static XName cs = w + "cs";
        public static XName csb0 = w + "csb0";
        public static XName csb1 = w + "csb1";
        public static XName cstheme = w + "cstheme";
        public static XName customMarkFollows = w + "customMarkFollows";
        public static XName customStyle = w + "customStyle";
        public static XName customXml = w + "customXml";
        public static XName customXmlDelRangeEnd = w + "customXmlDelRangeEnd";
        public static XName customXmlDelRangeStart = w + "customXmlDelRangeStart";
        public static XName customXmlInsRangeEnd = w + "customXmlInsRangeEnd";
        public static XName customXmlInsRangeStart = w + "customXmlInsRangeStart";
        public static XName customXmlMoveFromRangeEnd = w + "customXmlMoveFromRangeEnd";
        public static XName customXmlMoveFromRangeStart = w + "customXmlMoveFromRangeStart";
        public static XName customXmlMoveToRangeEnd = w + "customXmlMoveToRangeEnd";
        public static XName customXmlMoveToRangeStart = w + "customXmlMoveToRangeStart";
        public static XName customXmlPr = w + "customXmlPr";
        public static XName dataBinding = w + "dataBinding";
        public static XName dataSource = w + "dataSource";
        public static XName dataType = w + "dataType";
        public static XName date = w + "date";
        public static XName dateFormat = w + "dateFormat";
        public static XName dayLong = w + "dayLong";
        public static XName dayShort = w + "dayShort";
        public static XName ddList = w + "ddList";
        public static XName decimalSymbol = w + "decimalSymbol";
        public static XName _default = w + "default";
        public static XName defaultTableStyle = w + "defaultTableStyle";
        public static XName defaultTabStop = w + "defaultTabStop";
        public static XName defLockedState = w + "defLockedState";
        public static XName defQFormat = w + "defQFormat";
        public static XName defSemiHidden = w + "defSemiHidden";
        public static XName defUIPriority = w + "defUIPriority";
        public static XName defUnhideWhenUsed = w + "defUnhideWhenUsed";
        public static XName del = w + "del";
        public static XName delInstrText = w + "delInstrText";
        public static XName delText = w + "delText";
        public static XName description = w + "description";
        public static XName destination = w + "destination";
        public static XName dirty = w + "dirty";
        public static XName displacedByCustomXml = w + "displacedByCustomXml";
        public static XName display = w + "display";
        public static XName displayBackgroundShape = w + "displayBackgroundShape";
        public static XName displayHangulFixedWidth = w + "displayHangulFixedWidth";
        public static XName displayHorizontalDrawingGridEvery = w + "displayHorizontalDrawingGridEvery";
        public static XName displayText = w + "displayText";
        public static XName displayVerticalDrawingGridEvery = w + "displayVerticalDrawingGridEvery";
        public static XName distance = w + "distance";
        public static XName div = w + "div";
        public static XName divBdr = w + "divBdr";
        public static XName divId = w + "divId";
        public static XName divs = w + "divs";
        public static XName divsChild = w + "divsChild";
        public static XName dllVersion = w + "dllVersion";
        public static XName docDefaults = w + "docDefaults";
        public static XName docGrid = w + "docGrid";
        public static XName docLocation = w + "docLocation";
        public static XName docPart = w + "docPart";
        public static XName docPartBody = w + "docPartBody";
        public static XName docPartCategory = w + "docPartCategory";
        public static XName docPartGallery = w + "docPartGallery";
        public static XName docPartList = w + "docPartList";
        public static XName docPartObj = w + "docPartObj";
        public static XName docPartPr = w + "docPartPr";
        public static XName docParts = w + "docParts";
        public static XName docPartUnique = w + "docPartUnique";
        public static XName document = w + "document";
        public static XName documentProtection = w + "documentProtection";
        public static XName documentType = w + "documentType";
        public static XName docVar = w + "docVar";
        public static XName docVars = w + "docVars";
        public static XName doNotAutoCompressPictures = w + "doNotAutoCompressPictures";
        public static XName doNotAutofitConstrainedTables = w + "doNotAutofitConstrainedTables";
        public static XName doNotBreakConstrainedForcedTable = w + "doNotBreakConstrainedForcedTable";
        public static XName doNotBreakWrappedTables = w + "doNotBreakWrappedTables";
        public static XName doNotDemarcateInvalidXml = w + "doNotDemarcateInvalidXml";
        public static XName doNotDisplayPageBoundaries = w + "doNotDisplayPageBoundaries";
        public static XName doNotEmbedSmartTags = w + "doNotEmbedSmartTags";
        public static XName doNotExpandShiftReturn = w + "doNotExpandShiftReturn";
        public static XName doNotHyphenateCaps = w + "doNotHyphenateCaps";
        public static XName doNotIncludeSubdocsInStats = w + "doNotIncludeSubdocsInStats";
        public static XName doNotLeaveBackslashAlone = w + "doNotLeaveBackslashAlone";
        public static XName doNotOrganizeInFolder = w + "doNotOrganizeInFolder";
        public static XName doNotRelyOnCSS = w + "doNotRelyOnCSS";
        public static XName doNotSaveAsSingleFile = w + "doNotSaveAsSingleFile";
        public static XName doNotShadeFormData = w + "doNotShadeFormData";
        public static XName doNotSnapToGridInCell = w + "doNotSnapToGridInCell";
        public static XName doNotSuppressBlankLines = w + "doNotSuppressBlankLines";
        public static XName doNotSuppressIndentation = w + "doNotSuppressIndentation";
        public static XName doNotSuppressParagraphBorders = w + "doNotSuppressParagraphBorders";
        public static XName doNotTrackFormatting = w + "doNotTrackFormatting";
        public static XName doNotTrackMoves = w + "doNotTrackMoves";
        public static XName doNotUseEastAsianBreakRules = w + "doNotUseEastAsianBreakRules";
        public static XName doNotUseHTMLParagraphAutoSpacing = w + "doNotUseHTMLParagraphAutoSpacing";
        public static XName doNotUseIndentAsNumberingTabStop = w + "doNotUseIndentAsNumberingTabStop";
        public static XName doNotUseLongFileNames = w + "doNotUseLongFileNames";
        public static XName doNotUseMarginsForDrawingGridOrigin = w + "doNotUseMarginsForDrawingGridOrigin";
        public static XName doNotValidateAgainstSchema = w + "doNotValidateAgainstSchema";
        public static XName doNotVertAlignCellWithSp = w + "doNotVertAlignCellWithSp";
        public static XName doNotVertAlignInTxbx = w + "doNotVertAlignInTxbx";
        public static XName doNotWrapTextWithPunct = w + "doNotWrapTextWithPunct";
        public static XName drawing = w + "drawing";
        public static XName drawingGridHorizontalOrigin = w + "drawingGridHorizontalOrigin";
        public static XName drawingGridHorizontalSpacing = w + "drawingGridHorizontalSpacing";
        public static XName drawingGridVerticalOrigin = w + "drawingGridVerticalOrigin";
        public static XName drawingGridVerticalSpacing = w + "drawingGridVerticalSpacing";
        public static XName dropCap = w + "dropCap";
        public static XName dropDownList = w + "dropDownList";
        public static XName dstrike = w + "dstrike";
        public static XName dxaOrig = w + "dxaOrig";
        public static XName dyaOrig = w + "dyaOrig";
        public static XName dynamicAddress = w + "dynamicAddress";
        public static XName eastAsia = w + "eastAsia";
        public static XName eastAsianLayout = w + "eastAsianLayout";
        public static XName eastAsiaTheme = w + "eastAsiaTheme";
        public static XName ed = w + "ed";
        public static XName edGrp = w + "edGrp";
        public static XName edit = w + "edit";
        public static XName effect = w + "effect";
        public static XName element = w + "element";
        public static XName em = w + "em";
        public static XName embedBold = w + "embedBold";
        public static XName embedBoldItalic = w + "embedBoldItalic";
        public static XName embedItalic = w + "embedItalic";
        public static XName embedRegular = w + "embedRegular";
        public static XName embedSystemFonts = w + "embedSystemFonts";
        public static XName embedTrueTypeFonts = w + "embedTrueTypeFonts";
        public static XName emboss = w + "emboss";
        public static XName enabled = w + "enabled";
        public static XName encoding = w + "encoding";
        public static XName endnote = w + "endnote";
        public static XName endnotePr = w + "endnotePr";
        public static XName endnoteRef = w + "endnoteRef";
        public static XName endnoteReference = w + "endnoteReference";
        public static XName endnotes = w + "endnotes";
        public static XName enforcement = w + "enforcement";
        public static XName entryMacro = w + "entryMacro";
        public static XName equalWidth = w + "equalWidth";
        public static XName equation = w + "equation";
        public static XName evenAndOddHeaders = w + "evenAndOddHeaders";
        public static XName exitMacro = w + "exitMacro";
        public static XName family = w + "family";
        public static XName ffData = w + "ffData";
        public static XName fHdr = w + "fHdr";
        public static XName fieldMapData = w + "fieldMapData";
        public static XName fill = w + "fill";
        public static XName first = w + "first";
        public static XName firstLine = w + "firstLine";
        public static XName firstLineChars = w + "firstLineChars";
        public static XName fitText = w + "fitText";
        public static XName flatBorders = w + "flatBorders";
        public static XName fldChar = w + "fldChar";
        public static XName fldCharType = w + "fldCharType";
        public static XName fldData = w + "fldData";
        public static XName fldLock = w + "fldLock";
        public static XName fldSimple = w + "fldSimple";
        public static XName fmt = w + "fmt";
        public static XName followedHyperlink = w + "followedHyperlink";
        public static XName font = w + "font";
        public static XName fontKey = w + "fontKey";
        public static XName fonts = w + "fonts";
        public static XName fontSz = w + "fontSz";
        public static XName footer = w + "footer";
        public static XName footerReference = w + "footerReference";
        public static XName footnote = w + "footnote";
        public static XName footnoteLayoutLikeWW8 = w + "footnoteLayoutLikeWW8";
        public static XName footnotePr = w + "footnotePr";
        public static XName footnoteRef = w + "footnoteRef";
        public static XName footnoteReference = w + "footnoteReference";
        public static XName footnotes = w + "footnotes";
        public static XName forceUpgrade = w + "forceUpgrade";
        public static XName forgetLastTabAlignment = w + "forgetLastTabAlignment";
        public static XName format = w + "format";
        public static XName formatting = w + "formatting";
        public static XName formProt = w + "formProt";
        public static XName formsDesign = w + "formsDesign";
        public static XName frame = w + "frame";
        public static XName frameLayout = w + "frameLayout";
        public static XName framePr = w + "framePr";
        public static XName frameset = w + "frameset";
        public static XName framesetSplitbar = w + "framesetSplitbar";
        public static XName ftr = w + "ftr";
        public static XName fullDate = w + "fullDate";
        public static XName gallery = w + "gallery";
        public static XName glossaryDocument = w + "glossaryDocument";
        public static XName grammar = w + "grammar";
        public static XName gridAfter = w + "gridAfter";
        public static XName gridBefore = w + "gridBefore";
        public static XName gridCol = w + "gridCol";
        public static XName gridSpan = w + "gridSpan";
        public static XName group = w + "group";
        public static XName growAutofit = w + "growAutofit";
        public static XName guid = w + "guid";
        public static XName gutter = w + "gutter";
        public static XName gutterAtTop = w + "gutterAtTop";
        public static XName h = w + "h";
        public static XName hAnchor = w + "hAnchor";
        public static XName hanging = w + "hanging";
        public static XName hangingChars = w + "hangingChars";
        public static XName hAnsi = w + "hAnsi";
        public static XName hAnsiTheme = w + "hAnsiTheme";
        public static XName hash = w + "hash";
        public static XName hdr = w + "hdr";
        public static XName hdrShapeDefaults = w + "hdrShapeDefaults";
        public static XName header = w + "header";
        public static XName headerReference = w + "headerReference";
        public static XName headerSource = w + "headerSource";
        public static XName helpText = w + "helpText";
        public static XName hidden = w + "hidden";
        public static XName hideGrammaticalErrors = w + "hideGrammaticalErrors";
        public static XName hideMark = w + "hideMark";
        public static XName hideSpellingErrors = w + "hideSpellingErrors";
        public static XName highlight = w + "highlight";
        public static XName hint = w + "hint";
        public static XName history = w + "history";
        public static XName hMerge = w + "hMerge";
        public static XName horzAnchor = w + "horzAnchor";
        public static XName hps = w + "hps";
        public static XName hpsBaseText = w + "hpsBaseText";
        public static XName hpsRaise = w + "hpsRaise";
        public static XName hRule = w + "hRule";
        public static XName hSpace = w + "hSpace";
        public static XName hyperlink = w + "hyperlink";
        public static XName hyphenationZone = w + "hyphenationZone";
        public static XName i = w + "i";
        public static XName iCs = w + "iCs";
        public static XName id = w + "id";
        public static XName ignoreMixedContent = w + "ignoreMixedContent";
        public static XName ilvl = w + "ilvl";
        public static XName imprint = w + "imprint";
        public static XName ind = w + "ind";
        public static XName initials = w + "initials";
        public static XName inkAnnotations = w + "inkAnnotations";
        public static XName ins = w + "ins";
        public static XName insDel = w + "insDel";
        public static XName insideH = w + "insideH";
        public static XName insideV = w + "insideV";
        public static XName instr = w + "instr";
        public static XName instrText = w + "instrText";
        public static XName isLgl = w + "isLgl";
        public static XName jc = w + "jc";
        public static XName keepLines = w + "keepLines";
        public static XName keepNext = w + "keepNext";
        public static XName kern = w + "kern";
        public static XName kinsoku = w + "kinsoku";
        public static XName lang = w + "lang";
        public static XName lastRenderedPageBreak = w + "lastRenderedPageBreak";
        public static XName lastValue = w + "lastValue";
        public static XName latentStyles = w + "latentStyles";
        public static XName layoutRawTableWidth = w + "layoutRawTableWidth";
        public static XName layoutTableRowsApart = w + "layoutTableRowsApart";
        public static XName leader = w + "leader";
        public static XName left = w + "left";
        public static XName leftChars = w + "leftChars";
        public static XName leftFromText = w + "leftFromText";
        public static XName legacy = w + "legacy";
        public static XName legacyIndent = w + "legacyIndent";
        public static XName legacySpace = w + "legacySpace";
        public static XName lid = w + "lid";
        public static XName line = w + "line";
        public static XName linePitch = w + "linePitch";
        public static XName lineRule = w + "lineRule";
        public static XName lines = w + "lines";
        public static XName lineWrapLikeWord6 = w + "lineWrapLikeWord6";
        public static XName link = w + "link";
        public static XName linkedToFile = w + "linkedToFile";
        public static XName linkStyles = w + "linkStyles";
        public static XName linkToQuery = w + "linkToQuery";
        public static XName listEntry = w + "listEntry";
        public static XName listItem = w + "listItem";
        public static XName listSeparator = w + "listSeparator";
        public static XName lnNumType = w + "lnNumType";
        public static XName _lock = w + "lock";
        public static XName locked = w + "locked";
        public static XName lsdException = w + "lsdException";
        public static XName lvl = w + "lvl";
        public static XName lvlJc = w + "lvlJc";
        public static XName lvlOverride = w + "lvlOverride";
        public static XName lvlPicBulletId = w + "lvlPicBulletId";
        public static XName lvlRestart = w + "lvlRestart";
        public static XName lvlText = w + "lvlText";
        public static XName mailAsAttachment = w + "mailAsAttachment";
        public static XName mailMerge = w + "mailMerge";
        public static XName mailSubject = w + "mailSubject";
        public static XName mainDocumentType = w + "mainDocumentType";
        public static XName mappedName = w + "mappedName";
        public static XName marBottom = w + "marBottom";
        public static XName marH = w + "marH";
        public static XName markup = w + "markup";
        public static XName marLeft = w + "marLeft";
        public static XName marRight = w + "marRight";
        public static XName marTop = w + "marTop";
        public static XName marW = w + "marW";
        public static XName matchSrc = w + "matchSrc";
        public static XName maxLength = w + "maxLength";
        public static XName mirrorIndents = w + "mirrorIndents";
        public static XName mirrorMargins = w + "mirrorMargins";
        public static XName monthLong = w + "monthLong";
        public static XName monthShort = w + "monthShort";
        public static XName moveFrom = w + "moveFrom";
        public static XName moveFromRangeEnd = w + "moveFromRangeEnd";
        public static XName moveFromRangeStart = w + "moveFromRangeStart";
        public static XName moveTo = w + "moveTo";
        public static XName moveToRangeEnd = w + "moveToRangeEnd";
        public static XName moveToRangeStart = w + "moveToRangeStart";
        public static XName multiLevelType = w + "multiLevelType";
        public static XName multiLine = w + "multiLine";
        public static XName mwSmallCaps = w + "mwSmallCaps";
        public static XName name = w + "name";
        public static XName namespaceuri = w + "namespaceuri";
        public static XName next = w + "next";
        public static XName nlCheck = w + "nlCheck";
        public static XName noBorder = w + "noBorder";
        public static XName noBreakHyphen = w + "noBreakHyphen";
        public static XName noColumnBalance = w + "noColumnBalance";
        public static XName noEndnote = w + "noEndnote";
        public static XName noExtraLineSpacing = w + "noExtraLineSpacing";
        public static XName noLeading = w + "noLeading";
        public static XName noLineBreaksAfter = w + "noLineBreaksAfter";
        public static XName noLineBreaksBefore = w + "noLineBreaksBefore";
        public static XName noProof = w + "noProof";
        public static XName noPunctuationKerning = w + "noPunctuationKerning";
        public static XName noResizeAllowed = w + "noResizeAllowed";
        public static XName noSpaceRaiseLower = w + "noSpaceRaiseLower";
        public static XName noTabHangInd = w + "noTabHangInd";
        public static XName notTrueType = w + "notTrueType";
        public static XName noWrap = w + "noWrap";
        public static XName nsid = w + "nsid";
        public static XName _null = w + "null";
        public static XName num = w + "num";
        public static XName numbering = w + "numbering";
        public static XName numberingChange = w + "numberingChange";
        public static XName numFmt = w + "numFmt";
        public static XName numId = w + "numId";
        public static XName numIdMacAtCleanup = w + "numIdMacAtCleanup";
        public static XName numPicBullet = w + "numPicBullet";
        public static XName numPicBulletId = w + "numPicBulletId";
        public static XName numPr = w + "numPr";
        public static XName numRestart = w + "numRestart";
        public static XName numStart = w + "numStart";
        public static XName numStyleLink = w + "numStyleLink";
        public static XName _object = w + "object";
        public static XName odso = w + "odso";
        public static XName offsetFrom = w + "offsetFrom";
        public static XName oMath = w + "oMath";
        public static XName optimizeForBrowser = w + "optimizeForBrowser";
        public static XName orient = w + "orient";
        public static XName original = w + "original";
        public static XName other = w + "other";
        public static XName outline = w + "outline";
        public static XName outlineLvl = w + "outlineLvl";
        public static XName overflowPunct = w + "overflowPunct";
        public static XName p = w + "p";
        public static XName pageBreakBefore = w + "pageBreakBefore";
        public static XName panose1 = w + "panose1";
        public static XName paperSrc = w + "paperSrc";
        public static XName pBdr = w + "pBdr";
        public static XName percent = w + "percent";
        public static XName permEnd = w + "permEnd";
        public static XName permStart = w + "permStart";
        public static XName personal = w + "personal";
        public static XName personalCompose = w + "personalCompose";
        public static XName personalReply = w + "personalReply";
        public static XName pgBorders = w + "pgBorders";
        public static XName pgMar = w + "pgMar";
        public static XName pgNum = w + "pgNum";
        public static XName pgNumType = w + "pgNumType";
        public static XName pgSz = w + "pgSz";
        public static XName pict = w + "pict";
        public static XName picture = w + "picture";
        public static XName pitch = w + "pitch";
        public static XName pixelsPerInch = w + "pixelsPerInch";
        public static XName placeholder = w + "placeholder";
        public static XName pos = w + "pos";
        public static XName position = w + "position";
        public static XName pPr = w + "pPr";
        public static XName pPrChange = w + "pPrChange";
        public static XName pPrDefault = w + "pPrDefault";
        public static XName prefixMappings = w + "prefixMappings";
        public static XName printBodyTextBeforeHeader = w + "printBodyTextBeforeHeader";
        public static XName printColBlack = w + "printColBlack";
        public static XName printerSettings = w + "printerSettings";
        public static XName printFormsData = w + "printFormsData";
        public static XName printFractionalCharacterWidth = w + "printFractionalCharacterWidth";
        public static XName printPostScriptOverText = w + "printPostScriptOverText";
        public static XName printTwoOnOne = w + "printTwoOnOne";
        public static XName proofErr = w + "proofErr";
        public static XName proofState = w + "proofState";
        public static XName pStyle = w + "pStyle";
        public static XName ptab = w + "ptab";
        public static XName qFormat = w + "qFormat";
        public static XName query = w + "query";
        public static XName r = w + "r";
        public static XName readModeInkLockDown = w + "readModeInkLockDown";
        public static XName recipientData = w + "recipientData";
        public static XName recipients = w + "recipients";
        public static XName recommended = w + "recommended";
        public static XName relativeTo = w + "relativeTo";
        public static XName relyOnVML = w + "relyOnVML";
        public static XName removeDateAndTime = w + "removeDateAndTime";
        public static XName removePersonalInformation = w + "removePersonalInformation";
        public static XName restart = w + "restart";
        public static XName result = w + "result";
        public static XName revisionView = w + "revisionView";
        public static XName rFonts = w + "rFonts";
        public static XName right = w + "right";
        public static XName rightChars = w + "rightChars";
        public static XName rightFromText = w + "rightFromText";
        public static XName rPr = w + "rPr";
        public static XName rPrChange = w + "rPrChange";
        public static XName rPrDefault = w + "rPrDefault";
        public static XName rsid = w + "rsid";
        public static XName rsidDel = w + "rsidDel";
        public static XName rsidP = w + "rsidP";
        public static XName rsidR = w + "rsidR";
        public static XName rsidRDefault = w + "rsidRDefault";
        public static XName rsidRoot = w + "rsidRoot";
        public static XName rsidRPr = w + "rsidRPr";
        public static XName rsids = w + "rsids";
        public static XName rsidSect = w + "rsidSect";
        public static XName rsidTr = w + "rsidTr";
        public static XName rStyle = w + "rStyle";
        public static XName rt = w + "rt";
        public static XName rtl = w + "rtl";
        public static XName rtlGutter = w + "rtlGutter";
        public static XName ruby = w + "ruby";
        public static XName rubyAlign = w + "rubyAlign";
        public static XName rubyBase = w + "rubyBase";
        public static XName rubyPr = w + "rubyPr";
        public static XName salt = w + "salt";
        public static XName saveFormsData = w + "saveFormsData";
        public static XName saveInvalidXml = w + "saveInvalidXml";
        public static XName savePreviewPicture = w + "savePreviewPicture";
        public static XName saveSmartTagsAsXml = w + "saveSmartTagsAsXml";
        public static XName saveSubsetFonts = w + "saveSubsetFonts";
        public static XName saveThroughXslt = w + "saveThroughXslt";
        public static XName saveXmlDataOnly = w + "saveXmlDataOnly";
        public static XName scrollbar = w + "scrollbar";
        public static XName sdt = w + "sdt";
        public static XName sdtContent = w + "sdtContent";
        public static XName sdtEndPr = w + "sdtEndPr";
        public static XName sdtPr = w + "sdtPr";
        public static XName sectPr = w + "sectPr";
        public static XName sectPrChange = w + "sectPrChange";
        public static XName selectFldWithFirstOrLastChar = w + "selectFldWithFirstOrLastChar";
        public static XName semiHidden = w + "semiHidden";
        public static XName sep = w + "sep";
        public static XName separator = w + "separator";
        public static XName settings = w + "settings";
        public static XName shadow = w + "shadow";
        public static XName shapeDefaults = w + "shapeDefaults";
        public static XName shapeid = w + "shapeid";
        public static XName shapeLayoutLikeWW8 = w + "shapeLayoutLikeWW8";
        public static XName shd = w + "shd";
        public static XName showBreaksInFrames = w + "showBreaksInFrames";
        public static XName showEnvelope = w + "showEnvelope";
        public static XName showingPlcHdr = w + "showingPlcHdr";
        public static XName showXMLTags = w + "showXMLTags";
        public static XName sig = w + "sig";
        public static XName size = w + "size";
        public static XName sizeAuto = w + "sizeAuto";
        public static XName smallCaps = w + "smallCaps";
        public static XName smartTag = w + "smartTag";
        public static XName smartTagPr = w + "smartTagPr";
        public static XName smartTagType = w + "smartTagType";
        public static XName snapToGrid = w + "snapToGrid";
        public static XName softHyphen = w + "softHyphen";
        public static XName solutionID = w + "solutionID";
        public static XName sourceFileName = w + "sourceFileName";
        public static XName space = w + "space";
        public static XName spaceForUL = w + "spaceForUL";
        public static XName spacing = w + "spacing";
        public static XName spacingInWholePoints = w + "spacingInWholePoints";
        public static XName specVanish = w + "specVanish";
        public static XName spelling = w + "spelling";
        public static XName splitPgBreakAndParaMark = w + "splitPgBreakAndParaMark";
        public static XName src = w + "src";
        public static XName start = w + "start";
        public static XName startOverride = w + "startOverride";
        public static XName statusText = w + "statusText";
        public static XName storeItemID = w + "storeItemID";
        public static XName storeMappedDataAs = w + "storeMappedDataAs";
        public static XName strictFirstAndLastChars = w + "strictFirstAndLastChars";
        public static XName strike = w + "strike";
        public static XName style = w + "style";
        public static XName styleId = w + "styleId";
        public static XName styleLink = w + "styleLink";
        public static XName styleLockQFSet = w + "styleLockQFSet";
        public static XName styleLockTheme = w + "styleLockTheme";
        public static XName stylePaneFormatFilter = w + "stylePaneFormatFilter";
        public static XName stylePaneSortMethod = w + "stylePaneSortMethod";
        public static XName styles = w + "styles";
        public static XName subDoc = w + "subDoc";
        public static XName subFontBySize = w + "subFontBySize";
        public static XName subsetted = w + "subsetted";
        public static XName suff = w + "suff";
        public static XName summaryLength = w + "summaryLength";
        public static XName suppressAutoHyphens = w + "suppressAutoHyphens";
        public static XName suppressBottomSpacing = w + "suppressBottomSpacing";
        public static XName suppressLineNumbers = w + "suppressLineNumbers";
        public static XName suppressOverlap = w + "suppressOverlap";
        public static XName suppressSpacingAtTopOfPage = w + "suppressSpacingAtTopOfPage";
        public static XName suppressSpBfAfterPgBrk = w + "suppressSpBfAfterPgBrk";
        public static XName suppressTopSpacing = w + "suppressTopSpacing";
        public static XName suppressTopSpacingWP = w + "suppressTopSpacingWP";
        public static XName swapBordersFacingPages = w + "swapBordersFacingPages";
        public static XName sym = w + "sym";
        public static XName sz = w + "sz";
        public static XName szCs = w + "szCs";
        public static XName t = w + "t";
        public static XName t1 = w + "t1";
        public static XName t2 = w + "t2";
        public static XName tab = w + "tab";
        public static XName table = w + "table";
        public static XName tabs = w + "tabs";
        public static XName tag = w + "tag";
        public static XName targetScreenSz = w + "targetScreenSz";
        public static XName tbl = w + "tbl";
        public static XName tblBorders = w + "tblBorders";
        public static XName tblCellMar = w + "tblCellMar";
        public static XName tblCellSpacing = w + "tblCellSpacing";
        public static XName tblGrid = w + "tblGrid";
        public static XName tblGridChange = w + "tblGridChange";
        public static XName tblHeader = w + "tblHeader";
        public static XName tblInd = w + "tblInd";
        public static XName tblLayout = w + "tblLayout";
        public static XName tblLook = w + "tblLook";
        public static XName tblOverlap = w + "tblOverlap";
        public static XName tblpPr = w + "tblpPr";
        public static XName tblPr = w + "tblPr";
        public static XName tblPrChange = w + "tblPrChange";
        public static XName tblPrEx = w + "tblPrEx";
        public static XName tblPrExChange = w + "tblPrExChange";
        public static XName tblpX = w + "tblpX";
        public static XName tblpXSpec = w + "tblpXSpec";
        public static XName tblpY = w + "tblpY";
        public static XName tblpYSpec = w + "tblpYSpec";
        public static XName tblStyle = w + "tblStyle";
        public static XName tblStyleColBandSize = w + "tblStyleColBandSize";
        public static XName tblStylePr = w + "tblStylePr";
        public static XName tblStyleRowBandSize = w + "tblStyleRowBandSize";
        public static XName tblW = w + "tblW";
        public static XName tc = w + "tc";
        public static XName tcBorders = w + "tcBorders";
        public static XName tcFitText = w + "tcFitText";
        public static XName tcMar = w + "tcMar";
        public static XName tcPr = w + "tcPr";
        public static XName tcPrChange = w + "tcPrChange";
        public static XName tcW = w + "tcW";
        public static XName temporary = w + "temporary";
        public static XName tentative = w + "tentative";
        public static XName text = w + "text";
        public static XName textAlignment = w + "textAlignment";
        public static XName textboxTightWrap = w + "textboxTightWrap";
        public static XName textDirection = w + "textDirection";
        public static XName textInput = w + "textInput";
        public static XName tgtFrame = w + "tgtFrame";
        public static XName themeColor = w + "themeColor";
        public static XName themeFill = w + "themeFill";
        public static XName themeFillShade = w + "themeFillShade";
        public static XName themeFillTint = w + "themeFillTint";
        public static XName themeFontLang = w + "themeFontLang";
        public static XName themeShade = w + "themeShade";
        public static XName themeTint = w + "themeTint";
        public static XName titlePg = w + "titlePg";
        public static XName tl2br = w + "tl2br";
        public static XName tmpl = w + "tmpl";
        public static XName tooltip = w + "tooltip";
        public static XName top = w + "top";
        public static XName topFromText = w + "topFromText";
        public static XName topLinePunct = w + "topLinePunct";
        public static XName tplc = w + "tplc";
        public static XName tr = w + "tr";
        public static XName tr2bl = w + "tr2bl";
        public static XName trackRevisions = w + "trackRevisions";
        public static XName trHeight = w + "trHeight";
        public static XName trPr = w + "trPr";
        public static XName trPrChange = w + "trPrChange";
        public static XName truncateFontHeightsLikeWP6 = w + "truncateFontHeightsLikeWP6";
        public static XName txbxContent = w + "txbxContent";
        public static XName type = w + "type";
        public static XName types = w + "types";
        public static XName u = w + "u";
        public static XName udl = w + "udl";
        public static XName uiCompat97To2003 = w + "uiCompat97To2003";
        public static XName uiPriority = w + "uiPriority";
        public static XName ulTrailSpace = w + "ulTrailSpace";
        public static XName underlineTabInNumList = w + "underlineTabInNumList";
        public static XName unhideWhenUsed = w + "unhideWhenUsed";
        public static XName uniqueTag = w + "uniqueTag";
        public static XName updateFields = w + "updateFields";
        public static XName uri = w + "uri";
        public static XName url = w + "url";
        public static XName usb0 = w + "usb0";
        public static XName usb1 = w + "usb1";
        public static XName usb2 = w + "usb2";
        public static XName usb3 = w + "usb3";
        public static XName useAltKinsokuLineBreakRules = w + "useAltKinsokuLineBreakRules";
        public static XName useAnsiKerningPairs = w + "useAnsiKerningPairs";
        public static XName useFELayout = w + "useFELayout";
        public static XName useNormalStyleForList = w + "useNormalStyleForList";
        public static XName usePrinterMetrics = w + "usePrinterMetrics";
        public static XName useSingleBorderforContiguousCells = w + "useSingleBorderforContiguousCells";
        public static XName useWord2002TableStyleRules = w + "useWord2002TableStyleRules";
        public static XName useWord97LineBreakRules = w + "useWord97LineBreakRules";
        public static XName useXSLTWhenSaving = w + "useXSLTWhenSaving";
        public static XName val = w + "val";
        public static XName vAlign = w + "vAlign";
        public static XName value = w + "value";
        public static XName vAnchor = w + "vAnchor";
        public static XName vanish = w + "vanish";
        public static XName vendorID = w + "vendorID";
        public static XName vert = w + "vert";
        public static XName vertAlign = w + "vertAlign";
        public static XName vertAnchor = w + "vertAnchor";
        public static XName vertCompress = w + "vertCompress";
        public static XName view = w + "view";
        public static XName viewMergedData = w + "viewMergedData";
        public static XName vMerge = w + "vMerge";
        public static XName vMergeOrig = w + "vMergeOrig";
        public static XName vSpace = w + "vSpace";
        public static XName _w = w + "w";
        public static XName wAfter = w + "wAfter";
        public static XName wBefore = w + "wBefore";
        public static XName webHidden = w + "webHidden";
        public static XName webSettings = w + "webSettings";
        public static XName widowControl = w + "widowControl";
        public static XName wordWrap = w + "wordWrap";
        public static XName wpJustification = w + "wpJustification";
        public static XName wpSpaceWidth = w + "wpSpaceWidth";
        public static XName wrap = w + "wrap";
        public static XName wrapTrailSpaces = w + "wrapTrailSpaces";
        public static XName writeProtection = w + "writeProtection";
        public static XName x = w + "x";
        public static XName xAlign = w + "xAlign";
        public static XName xpath = w + "xpath";
        public static XName y = w + "y";
        public static XName yAlign = w + "yAlign";
        public static XName yearLong = w + "yearLong";
        public static XName yearShort = w + "yearShort";
        public static XName zoom = w + "zoom";
        public static XName zOrder = w + "zOrder";

        public static XName[] BlockLevelContentContainers =
        {
            body,
            tc,
            txbxContent,
            hdr,
            ftr,
            endnote,
            footnote
        };

        public static XName[] SubRunLevelContent =
        {
            br,
            cr,
            dayLong,
            dayShort,
            drawing,
            drawing,
            monthLong,
            monthShort,
            noBreakHyphen,
            ptab,
            pgNum,
            pict,
            softHyphen,
            sym,
            t,
            tab,
            yearLong,
            yearShort,
            MC.AlternateContent,
        };
    }

    public static class W10
    {
        public static XNamespace w10 =
            "urn:schemas-microsoft-com:office:word";

        public static XName anchorlock = w10 + "anchorlock";
        public static XName borderbottom = w10 + "borderbottom";
        public static XName borderleft = w10 + "borderleft";
        public static XName borderright = w10 + "borderright";
        public static XName bordertop = w10 + "bordertop";
        public static XName wrap = w10 + "wrap";
    }

    public static class W14
    {
        public static XNamespace w14 =
            "http://schemas.microsoft.com/office/word/2010/wordml";

        public static XName algn = w14 + "algn";
        public static XName alpha = w14 + "alpha";
        public static XName ang = w14 + "ang";
        public static XName b = w14 + "b";
        public static XName bevel = w14 + "bevel";
        public static XName bevelB = w14 + "bevelB";
        public static XName bevelT = w14 + "bevelT";
        public static XName blurRad = w14 + "blurRad";
        public static XName camera = w14 + "camera";
        public static XName cap = w14 + "cap";
        public static XName checkbox = w14 + "checkbox";
        public static XName _checked = w14 + "checked";
        public static XName checkedState = w14 + "checkedState";
        public static XName cmpd = w14 + "cmpd";
        public static XName cntxtAlts = w14 + "cntxtAlts";
        public static XName cNvContentPartPr = w14 + "cNvContentPartPr";
        public static XName conflictMode = w14 + "conflictMode";
        public static XName contentPart = w14 + "contentPart";
        public static XName contourClr = w14 + "contourClr";
        public static XName contourW = w14 + "contourW";
        public static XName defaultImageDpi = w14 + "defaultImageDpi";
        public static XName dir = w14 + "dir";
        public static XName discardImageEditingData = w14 + "discardImageEditingData";
        public static XName dist = w14 + "dist";
        public static XName docId = w14 + "docId";
        public static XName editId = w14 + "editId";
        public static XName enableOpenTypeKerning = w14 + "enableOpenTypeKerning";
        public static XName endA = w14 + "endA";
        public static XName endPos = w14 + "endPos";
        public static XName entityPicker = w14 + "entityPicker";
        public static XName extrusionClr = w14 + "extrusionClr";
        public static XName extrusionH = w14 + "extrusionH";
        public static XName fadeDir = w14 + "fadeDir";
        public static XName fillToRect = w14 + "fillToRect";
        public static XName font = w14 + "font";
        public static XName glow = w14 + "glow";
        public static XName gradFill = w14 + "gradFill";
        public static XName gs = w14 + "gs";
        public static XName gsLst = w14 + "gsLst";
        public static XName h = w14 + "h";
        public static XName hueMod = w14 + "hueMod";
        public static XName id = w14 + "id";
        public static XName kx = w14 + "kx";
        public static XName ky = w14 + "ky";
        public static XName l = w14 + "l";
        public static XName lat = w14 + "lat";
        public static XName ligatures = w14 + "ligatures";
        public static XName lightRig = w14 + "lightRig";
        public static XName lim = w14 + "lim";
        public static XName lin = w14 + "lin";
        public static XName lon = w14 + "lon";
        public static XName lumMod = w14 + "lumMod";
        public static XName lumOff = w14 + "lumOff";
        public static XName miter = w14 + "miter";
        public static XName noFill = w14 + "noFill";
        public static XName numForm = w14 + "numForm";
        public static XName numSpacing = w14 + "numSpacing";
        public static XName nvContentPartPr = w14 + "nvContentPartPr";
        public static XName paraId = w14 + "paraId";
        public static XName path = w14 + "path";
        public static XName pos = w14 + "pos";
        public static XName props3d = w14 + "props3d";
        public static XName prst = w14 + "prst";
        public static XName prstDash = w14 + "prstDash";
        public static XName prstMaterial = w14 + "prstMaterial";
        public static XName r = w14 + "r";
        public static XName rad = w14 + "rad";
        public static XName reflection = w14 + "reflection";
        public static XName rev = w14 + "rev";
        public static XName rig = w14 + "rig";
        public static XName rot = w14 + "rot";
        public static XName round = w14 + "round";
        public static XName sat = w14 + "sat";
        public static XName satMod = w14 + "satMod";
        public static XName satOff = w14 + "satOff";
        public static XName scaled = w14 + "scaled";
        public static XName scene3d = w14 + "scene3d";
        public static XName schemeClr = w14 + "schemeClr";
        public static XName shade = w14 + "shade";
        public static XName shadow = w14 + "shadow";
        public static XName solidFill = w14 + "solidFill";
        public static XName srgbClr = w14 + "srgbClr";
        public static XName stA = w14 + "stA";
        public static XName stPos = w14 + "stPos";
        public static XName styleSet = w14 + "styleSet";
        public static XName stylisticSets = w14 + "stylisticSets";
        public static XName sx = w14 + "sx";
        public static XName sy = w14 + "sy";
        public static XName t = w14 + "t";
        public static XName textFill = w14 + "textFill";
        public static XName textId = w14 + "textId";
        public static XName textOutline = w14 + "textOutline";
        public static XName tint = w14 + "tint";
        public static XName uncheckedState = w14 + "uncheckedState";
        public static XName val = w14 + "val";
        public static XName w = w14 + "w";
        public static XName wProps3d = w14 + "wProps3d";
        public static XName wScene3d = w14 + "wScene3d";
        public static XName wShadow = w14 + "wShadow";
        public static XName wTextFill = w14 + "wTextFill";
        public static XName wTextOutline = w14 + "wTextOutline";
        public static XName xfrm = w14 + "xfrm";
    }

    public static class WNE
    {
        public static XNamespace wne =
            "http://schemas.microsoft.com/office/word/2006/wordml";

        public static XName acd = wne + "acd";
        public static XName acdEntry = wne + "acdEntry";
        public static XName acdManifest = wne + "acdManifest";
        public static XName acdName = wne + "acdName";
        public static XName acds = wne + "acds";
        public static XName active = wne + "active";
        public static XName argValue = wne + "argValue";
        public static XName fci = wne + "fci";
        public static XName fciBasedOn = wne + "fciBasedOn";
        public static XName fciIndexBasedOn = wne + "fciIndexBasedOn";
        public static XName fciName = wne + "fciName";
        public static XName hash = wne + "hash";
        public static XName kcmPrimary = wne + "kcmPrimary";
        public static XName kcmSecondary = wne + "kcmSecondary";
        public static XName keymap = wne + "keymap";
        public static XName keymaps = wne + "keymaps";
        public static XName macro = wne + "macro";
        public static XName macroName = wne + "macroName";
        public static XName mask = wne + "mask";
        public static XName recipientData = wne + "recipientData";
        public static XName recipients = wne + "recipients";
        public static XName swArg = wne + "swArg";
        public static XName tcg = wne + "tcg";
        public static XName toolbarData = wne + "toolbarData";
        public static XName toolbars = wne + "toolbars";
        public static XName val = wne + "val";
        public static XName wch = wne + "wch";
    }

    public static class WP
    {
        public static XNamespace wp =
            "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing";

        public static XName align = wp + "align";
        public static XName anchor = wp + "anchor";
        public static XName cNvGraphicFramePr = wp + "cNvGraphicFramePr";
        public static XName docPr = wp + "docPr";
        public static XName effectExtent = wp + "effectExtent";
        public static XName extent = wp + "extent";
        public static XName inline = wp + "inline";
        public static XName lineTo = wp + "lineTo";
        public static XName positionH = wp + "positionH";
        public static XName positionV = wp + "positionV";
        public static XName posOffset = wp + "posOffset";
        public static XName simplePos = wp + "simplePos";
        public static XName start = wp + "start";
        public static XName wrapNone = wp + "wrapNone";
        public static XName wrapPolygon = wp + "wrapPolygon";
        public static XName wrapSquare = wp + "wrapSquare";
        public static XName wrapThrough = wp + "wrapThrough";
        public static XName wrapTight = wp + "wrapTight";
        public static XName wrapTopAndBottom = wp + "wrapTopAndBottom";
    }

    public static class WP14
    {
        public static XNamespace wp14 =
            "http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing";

        public static XName editId = wp14 + "editId";
        public static XName pctHeight = wp14 + "pctHeight";
        public static XName pctPosVOffset = wp14 + "pctPosVOffset";
        public static XName pctWidth = wp14 + "pctWidth";
        public static XName sizeRelH = wp14 + "sizeRelH";
        public static XName sizeRelV = wp14 + "sizeRelV";
    }

    public static class WPS
    {
        public static XNamespace wps =
            "http://schemas.microsoft.com/office/word/2010/wordprocessingShape";

        public static XName altTxbx = wps + "altTxbx";
        public static XName bodyPr = wps + "bodyPr";
        public static XName cNvSpPr = wps + "cNvSpPr";
        public static XName spPr = wps + "spPr";
        public static XName style = wps + "style";
        public static XName textbox = wps + "textbox";
        public static XName txbx = wps + "txbx";
        public static XName wsp = wps + "wsp";
    }

    public static class WPC
    {
        public static XNamespace wpc =
            "http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas";
    }

    public static class WPG
    {
        public static XNamespace wpg =
            "http://schemas.microsoft.com/office/word/2010/wordprocessingGroup";
    }

    public static class WPI
    {
        public static XNamespace wpi =
            "http://schemas.microsoft.com/office/word/2010/wordprocessingInk";
    }

    public static class PtOpenXml
    {
        public static XNamespace ptOpenXml = "http://powertools.codeplex.com/documentbuilder/2011/insert";
        public static XName Insert = ptOpenXml + "Insert";
        public static XName Id = "Id";
    }

    public static class PtOpenXmlExtensions
    {
        private static readonly EventHandler<XObjectChangeEventArgs> ElementChanged = ElementChangedHandler;

        public static XDocument GetXDocument(this OpenXmlPart part)
        {
            var partXDocument = part.Annotation<XDocument>();
            if (partXDocument != null)
                return partXDocument;
            using (Stream partStream = part.GetStream())
            using (XmlReader partXmlReader = XmlReader.Create(partStream))
                partXDocument = XDocument.Load(partXmlReader);
            part.AddAnnotation(partXDocument);
            return partXDocument;
        }

        public static void PutXDocument(this OpenXmlPart part)
        {
            XDocument partXDocument = part.GetXDocument();
            if (partXDocument != null)
            {
                using (Stream partStream = part.GetStream(FileMode.Create, FileAccess.Write))
                using (XmlWriter partXmlWriter = XmlWriter.Create(partStream))
                    partXDocument.Save(partXmlWriter);
            }
        }

        public static void PutXDocument(this OpenXmlPart part, XDocument document)
        {
            using (Stream partStream = part.GetStream(FileMode.Create, FileAccess.Write))
            using (XmlWriter partXmlWriter = XmlWriter.Create(partStream))
                document.Save(partXmlWriter);
            part.RemoveAnnotations<XDocument>();
            part.AddAnnotation(document);
        }

        public static IEnumerable<XElement> LogicalChildrenContent(this XElement element)
        {
            if (element.Name == W.document)
                return element.Descendants(W.body).Take(1);
            if (element.Name == W.body ||
                element.Name == W.tc ||
                element.Name == W.txbxContent)
                return element
                    .DescendantsTrimmed(e =>
                        e.Name == W.tbl ||
                        e.Name == W.p)
                    .Where(e =>
                        e.Name == W.p ||
                        e.Name == W.tbl);
            if (element.Name == W.tbl)
                return element
                    .DescendantsTrimmed(W.tr)
                    .Where(e => e.Name == W.tr);
            if (element.Name == W.tr)
                return element
                    .DescendantsTrimmed(W.tc)
                    .Where(e => e.Name == W.tc);
            if (element.Name == W.p)
                return element
                    .DescendantsTrimmed(e => e.Name == W.r ||
                                             e.Name == W.pict ||
                                             e.Name == W.drawing)
                    .Where(e => e.Name == W.r ||
                                e.Name == W.pict ||
                                e.Name == W.drawing);
            if (element.Name == W.r)
                return element
                    .DescendantsTrimmed(e => W.SubRunLevelContent.Contains(e.Name))
                    .Where(e => W.SubRunLevelContent.Contains(e.Name));
            if (element.Name == MC.AlternateContent)
                return element
                    .DescendantsTrimmed(e =>
                        e.Name == W.pict ||
                        e.Name == W.drawing ||
                        e.Name == MC.Fallback)
                    .Where(e =>
                        e.Name == W.pict ||
                        e.Name == W.drawing);
            if (element.Name == W.pict || element.Name == W.drawing)
                return element
                    .DescendantsTrimmed(W.txbxContent)
                    .Where(e => e.Name == W.txbxContent);
            return XElement.EmptySequence;
        }

        public static IEnumerable<XElement> LogicalChildrenContent(
            this IEnumerable<XElement> source)
        {
            foreach (XElement e1 in source)
                foreach (XElement e2 in e1.LogicalChildrenContent())
                    yield return e2;
        }

        public static IEnumerable<XElement> LogicalChildrenContent(
            this XElement element, XName name)
        {
            return element.LogicalChildrenContent().Where(e => e.Name == name);
        }

        public static IEnumerable<XElement> LogicalChildrenContent(
            this IEnumerable<XElement> source, XName name)
        {
            foreach (XElement e1 in source)
                foreach (XElement e2 in e1.LogicalChildrenContent(name))
                    yield return e2;
        }

        // Used to track changes to parts

        /// <summary>
        /// Gets the XDocument for a part
        /// </summary>
        public static XDocument GetXDocumentWithTracking(this OpenXmlPart part)
        {
            var xdoc = part.Annotation<XDocument>();
            if (xdoc != null)
                return xdoc;
            try
            {
                using (var sr = new StreamReader(part.GetStream()))
                using (XmlReader xr = XmlReader.Create(sr))
                {
                    xdoc = XDocument.Load(xr);
                    xdoc.Changed += ElementChanged;
                    xdoc.Changing += ElementChanged;
                }
            }
            catch (XmlException)
            {
                var xdec = new XDeclaration("1.0", "UTF-8", "yes");
                xdoc = new XDocument(xdec);
                xdoc.AddAnnotation(new ChangedSemaphore());
            }
            part.AddAnnotation(xdoc);
            return xdoc;
        }

        private static void ElementChangedHandler(object sender, XObjectChangeEventArgs e)
        {
            XDocument xDocument = ((XObject)sender).Document;
            if (xDocument != null)
            {
                xDocument.Changing -= ElementChanged;
                xDocument.Changed -= ElementChanged;
                xDocument.AddAnnotation(new ChangedSemaphore());
            }
        }

        /// <summary>
        /// Writes out all XDocuments
        /// </summary>
        public static void FlushTrackedXDocuments(this OpenXmlPackage doc)
        {
            var visited = new HashSet<OpenXmlPart>();
            foreach (IdPartPair item in doc.Parts)
                FlushPart(item.OpenXmlPart, visited);
        }

        private static void FlushPart(OpenXmlPart part, HashSet<OpenXmlPart> visited)
        {
            visited.Add(part);
            var xdoc = part.Annotation<XDocument>();
            if (xdoc != null && xdoc.Annotation<ChangedSemaphore>() != null)
            {
                using (XmlWriter xw = XmlWriter.Create(part.GetStream(FileMode.Create, FileAccess.Write)))
                {
                    xdoc.Save(xw);
                }
                xdoc.RemoveAnnotations<ChangedSemaphore>();
                xdoc.Changing += ElementChanged;
                xdoc.Changed += ElementChanged;
            }
            foreach (IdPartPair item in part.Parts)
                if (!visited.Contains(item.OpenXmlPart))
                    FlushPart(item.OpenXmlPart, visited);
        }

        #region Nested type: ChangedSemaphore

        private class ChangedSemaphore
        {
        }

        #endregion
    }

    public class FieldInfo
    {
        public string[] Arguments;
        public string FieldType;
        public string[] Switches;
    }

    public static class FieldParser
    {
        private static string[] GetTokens(string field)
        {
            State state = State.InWhiteSpace;
            int tokenStart = 0;
            char quoteStart = char.MinValue;
            var tokens = new List<string>();
            for (int c = 0; c < field.Length; c++)
            {
                if (Char.IsWhiteSpace(field[c]))
                {
                    if (state == State.InToken)
                    {
                        tokens.Add(field.Substring(tokenStart, c - tokenStart));
                        state = State.InWhiteSpace;
                        continue;
                    }
                    if (state == State.OnOpeningQuote)
                    {
                        tokenStart = c;
                        state = State.InQuotedToken;
                    }
                    if (state == State.OnClosingQuote)
                        state = State.InWhiteSpace;
                    continue;
                }
                if (field[c] == '\\')
                {
                    if (state == State.InQuotedToken)
                    {
                        state = State.OnBackslash;
                        continue;
                    }
                }
                if (state == State.OnBackslash)
                {
                    state = State.InQuotedToken;
                    continue;
                }
                if (field[c] == '"' || field[c] == '\'' || field[c] == 0x201d)
                {
                    if (state == State.InWhiteSpace)
                    {
                        quoteStart = field[c];
                        state = State.OnOpeningQuote;
                        continue;
                    }
                    if (state == State.InQuotedToken)
                    {
                        if (field[c] == quoteStart)
                        {
                            tokens.Add(field.Substring(tokenStart, c - tokenStart));
                            state = State.OnClosingQuote;
                            continue;
                        }
                        continue;
                    }
                    if (state == State.OnOpeningQuote)
                    {
                        if (field[c] == quoteStart)
                        {
                            state = State.OnClosingQuote;
                            continue;
                        }
                        else
                        {
                            tokenStart = c;
                            state = State.InQuotedToken;
                            continue;
                        }
                    }
                    continue;
                }
                if (state == State.InWhiteSpace)
                {
                    tokenStart = c;
                    state = State.InToken;
                    continue;
                }
                if (state == State.OnOpeningQuote)
                {
                    tokenStart = c;
                    state = State.InQuotedToken;
                    continue;
                }
                if (state == State.OnClosingQuote)
                {
                    tokenStart = c;
                    state = State.InToken;
                    continue;
                }
            }
            if (state == State.InToken)
                tokens.Add(field.Substring(tokenStart, field.Length - tokenStart));
            return tokens.ToArray();
        }

        public static FieldInfo ParseField(string field)
        {
            var emptyField = new FieldInfo
            {
                FieldType = "",
                Arguments = new string[] { },
                Switches = new string[] { },
            };

            if (field.Length == 0)
                return emptyField;
            string fieldType = field.TrimStart().Split(' ').FirstOrDefault();
            if (fieldType == null || fieldType.ToUpper() != "HYPERLINK")
                return emptyField;
            string[] tokens = GetTokens(field);
            if (tokens.Length == 0)
                return emptyField;
            var fieldInfo = new FieldInfo
            {
                FieldType = tokens[0],
                Switches = tokens.Where(t => t[0] == '\\').ToArray(),
                Arguments = tokens.Skip(1).Where(t => t[0] != '\\').ToArray(),
            };
            return fieldInfo;
        }

        #region Nested type: State

        private enum State
        {
            InToken,
            InWhiteSpace,
            InQuotedToken,
            OnOpeningQuote,
            OnClosingQuote,
            OnBackslash,
        }

        #endregion
    }

    public class InvalidOpenXmlDocumentException : Exception
    {
        public InvalidOpenXmlDocumentException(string message) : base(message)
        {
        }
    }

    public class OpenXmlPowerToolsException : Exception
    {
        public OpenXmlPowerToolsException(string message) : base(message)
        {
        }
    }
}
