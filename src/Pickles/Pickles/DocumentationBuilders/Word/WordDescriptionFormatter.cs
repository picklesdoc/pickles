using System;
using DocumentFormat.OpenXml.Wordprocessing;
using PicklesDoc.Pickles.Extensions;

namespace PicklesDoc.Pickles.DocumentationBuilders.Word
{
    public class WordDescriptionFormatter
    {
        public void Format(Body body, string description)
        {
            foreach (var paragraph in SplitDescription(description))
            {
                body.GenerateParagraph(paragraph, "Normal");
            }
        }

        public static string[] SplitDescription(string description)
        {
            return description.Split(new string[] {"\n", "\r"}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
