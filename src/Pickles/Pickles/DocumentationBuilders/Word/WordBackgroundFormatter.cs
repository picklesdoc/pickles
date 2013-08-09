
using DocumentFormat.OpenXml.Wordprocessing;
using PicklesDoc.Pickles.Parser;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word
{
	public class WordBackgroundFormatter
	{
        const string DefaultBackgroundKeyword = "Background";

	    private readonly LanguageServices languageSevices;

	    public WordBackgroundFormatter(Configuration configuration)
	    {
	        this.languageSevices = new LanguageServices(configuration);
	    }

		public void Format(Body body, Scenario background)
		{
			var headerParagraph   = new Paragraph(new ParagraphProperties(new ParagraphStyleId { Val = "Heading2" }));
		    var backgroundKeyword = GetLocalizedBackgroundKeyword();
			headerParagraph.Append(new Run(new RunProperties(new Bold()), new Text(backgroundKeyword)));

			var table = new Table();
			table.Append(GenerateTableProperties());
			var row = new TableRow();
			var cell = new TableCell();
			cell.Append(headerParagraph);

		    foreach (var descriptionSentence in WordDescriptionFormatter.SplitDescription(background.Description))
		    {
		        cell.Append(CreateNormalParagraph(descriptionSentence));
		    }

			foreach (var step in background.Steps)
			{
				cell.Append(WordStepFormatter.GenerateStepParagraph(step));
			}

			cell.Append(CreateNormalParagraph("")); // Is there a better way to generate a new empty line?
			row.Append(cell);
			table.Append(row);

			body.Append(table);
		}

	    private string GetLocalizedBackgroundKeyword()
	    {
	        var keywords = languageSevices.GetLanguage().keywords("background");
	        if (keywords != null && !keywords.isEmpty())
	        {
	            return keywords.get(0).ToString();
	        }
	        return DefaultBackgroundKeyword;
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

		private static Paragraph CreateNormalParagraph(string text)
		{
			var emptyLine = new Paragraph(new ParagraphProperties(new ParagraphStyleId { Val = "Normal" }));
			emptyLine.Append(new Run(new Text(text)));
			return emptyLine;
		}
	}
}
