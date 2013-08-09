
using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;
using PicklesDoc.Pickles.Extensions;
using PicklesDoc.Pickles.Parser;
using PicklesDoc.Pickles.TestFrameworks;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word
{
	public class WordBackgroundFormatter
	{

		public void Format(Body body, Scenario background)
		{
			var headerParagraph = new Paragraph(new ParagraphProperties(new ParagraphStyleId { Val = "Heading2" }));
			headerParagraph.Append(new Run(new RunProperties(new Bold()), new Text(background.Name)));

			var table = new Table();
			table.Append(GenerateTableProperties());
			var row = new TableRow();
			var cell = new TableCell();
			cell.Append(headerParagraph);

			foreach (var step in background.Steps)
			{
				cell.Append(WordStepFormatter.GenerateStepParagraph(step));
			}

			cell.Append(CreateEmptyLine());
			row.Append(cell);
			table.Append(row);

			body.Append(table);
		}

		private static TableProperties GenerateTableProperties()
		{
			var tableProperties1 = new TableProperties();
			var tableStyle1 = new TableStyle { Val = "TableGrid" };
			var tableWidth1 = new TableWidth { Width = "4900", Type = TableWidthUnitValues.Pct };
			var tableLook1 = new TableLook { Val = "04A0" };
			var tableJustification = new TableJustification { Val = TableRowAlignmentValues.Center };

			tableProperties1.Append(tableStyle1);
			tableProperties1.Append(tableWidth1);
			tableProperties1.Append(tableLook1);
			tableProperties1.Append(tableJustification);
			return tableProperties1;
		}

		private static Paragraph CreateEmptyLine()
		{
			// Is there a better way to do this?
			var emptyLine = new Paragraph(new ParagraphProperties(new ParagraphStyleId { Val = "Normal" }));
			emptyLine.Append(new Run(new Text("")));
			return emptyLine;
		}
	}
}
