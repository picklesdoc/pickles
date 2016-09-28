using System.Text.RegularExpressions;

namespace PicklesDoc.Pickles.TestFrameworks
{
	internal static class SpecFlowNameMapping
	{
		private static readonly Regex PunctuationCharactersRegex = new Regex(@"[\n\.-]+", RegexOptions.Compiled);
		private static readonly Regex NonIdentifierCharacterRegex = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lm}\p{Lo}\p{Nl}\p{Nd}\p{Pc}]", RegexOptions.Compiled);

    public static string Build(string name)
		{
			name = PunctuationCharactersRegex.Replace(name, "_");
			name = NonIdentifierCharacterRegex.Replace(name, string.Empty);
      name = name
        .Replace('ä', 'a')
        .Replace('ö', 'o')
        .Replace('ü', 'u')
        .Replace('Ä', 'A')
        .Replace('Ö', 'O')
        .Replace('Ü', 'U')
        .Replace('ß', 'b');

      return name;
		}
	}
}
