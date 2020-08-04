using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.TestFrameworks.NUnit.NUnit3
{
    public enum Keywords
    {
        QcTestPlanPath,
        QcTestLabPath,
        QcTestSet,
        TestCaseName,
        Objective
    }

    public enum WordsCount
    {
        FirstSix,
        LastThree,
        SeventyFive
    }

    public static class SFPHelpers
    {
        private static short ScenarioNameLength => 75;

        public static string ExtractSingleLineKeywordValue(Keywords keywordName, string description)
        {
            // The following line is needed otherwise the regular expression doesn't
            // work when the scenario description only contains the keywordName
            // keyword (string description must always include at least 1 "\n").
            description += "\n";
            var pattern = keywordName + @":\s+(.+?)\r?\n";
            var regex = new Regex(pattern);
            var match = regex.Match(description);

            if (!match.Success) return null;

            var firstMatch = match.Groups[1].Value;

            //Find if there's another keyword in the extracted Value
            var lstKeywords = Enum.GetValues(typeof(Keywords)).Cast<Keywords>().Where(k => k != keywordName);

            //Add : to the last value so that it's recognized as a keyword
            var sOtherKeywords = string.Join(":|", lstKeywords) + ":";

            var pattern2 = @"(" + sOtherKeywords + @")(\s+)";
            regex = new Regex(pattern2);
            match = regex.Match(firstMatch);

            //If there's no other keyword in the same line then return the expression that is extracted from the first regex
            if (!match.Success) return firstMatch;

            var secondMatch = match.Groups[1].Value;

            var pattern3 = keywordName + @":\s+(.+)(\r?\n?)" + secondMatch;
            regex = new Regex(pattern3);
            match = regex.Match(description);

            return match.Success ? match.Groups[1].Value.TrimEnd() : null;
        }

        public static string GetTestCaseName(ScenarioOutline scenarioOutline, string[] row)
        {
            var customTestCaseName = ExtractSingleLineKeywordValue(Keywords.TestCaseName, scenarioOutline.Description);
            var testCaseName = FixVeryLongTestCaseName(scenarioOutline.Name);

            var newTestCaseName = string.IsNullOrEmpty(customTestCaseName) ? testCaseName : customTestCaseName;

            var testCaseId = GetTestCaseID(scenarioOutline);
            if (!string.IsNullOrEmpty(testCaseId)) return newTestCaseName + "_" + testCaseId;

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(newTestCaseName).Append("_");
            foreach (var value in row)
            {
                stringBuilder.Append(Regex.Escape(value));
                stringBuilder.Append("_");
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            return stringBuilder.ToString();
        }

        private static string GetTestCaseID(ScenarioOutline scenarioOutline)
        {
            foreach (var example in scenarioOutline.Examples)
            {
                if(example.TableArgument.HeaderRow.Cells.First().Equals("TestCaseID", StringComparison.InvariantCulture))
                    return example.TableArgument.DataRows.First().Cells.First().ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Agreed length: 75 characters or the first 6 words of the scenario (use the one which is shorter)
        /// Try to use sequence numbers for normal scenarios.If that is not feasible than use the agreed length followed by a "~" followed by the last 3 words of the scenario.
        /// In case of scenarios with @ID tag
        ///     Shortened name will be: &lt;Feature&gt;_&lt;IDX&gt;~ (for scenario)
        ///         BagTagValidation_ID01 ~
        ///     Shortened name will be: &lt;Feature&gt;_&lt;IDX&gt; ~_Example&lt;Row number examples table&gt; (for scenario outline)
        ///         BagTagValidation_ID01 ~_Example1 (where IDX represents the @ID tag, ~the indication that the path is shortened and Row number the number of the example in the examples table
        /// In case of normal scenarios(check if using sequence numbers is feasible, otherwise use the last 3 words of the scenario)
        ///     Shortened name will be&lt;feature&gt;&lt;sequence number&gt;&lt;75 chars/first 6 words of the scenario&gt;~(_ExampleX)
        ///         BagTagValidation_001_TheHHT_GUIShallClear~
        ///         BagTagValidation_001_TheHHT_GUIShallClear~_Example1
        ///     Shortened name will be&lt;feature&gt; _&lt;last 3 words of the scenario&gt;~(_ExampleX )
        ///         BagTagValidation_TheHHT_GUIShallClear~NotifyTheOperator
        ///         BagTagValidation_TheHHT_GUIShallClear~NotifyTheOperator_Example1
        /// </summary>
        /// <param name="currentTestCaseName">the long test case name</param>
        /// <returns>A string that is shortened</returns>
        private static string FixVeryLongTestCaseName(string currentTestCaseName)
        {
            string fixedTestCaseName;

            if (currentTestCaseName.Length <= ScenarioNameLength) return currentTestCaseName;

            var firstSixChars = GetChars(currentTestCaseName, WordsCount.FirstSix);
            var lastThreeChars = GetChars(currentTestCaseName, WordsCount.LastThree);

            fixedTestCaseName = string.Concat(firstSixChars, "~", lastThreeChars);
            if (fixedTestCaseName.Length > ScenarioNameLength)
                fixedTestCaseName = GetChars(currentTestCaseName, WordsCount.SeventyFive);

            return fixedTestCaseName;
        }

        public static string GetChars(string fullTestName, WordsCount place)
        {
            var arrAllWords = fullTestName.Split(' ');

            var newText = string.Empty;

            switch (place)
            {
                case WordsCount.FirstSix:
                    var firstSix = Slice(arrAllWords, 0, 6).ToArray();
                    newText = string.Join(" ", firstSix);
                    break;
                case WordsCount.LastThree:
                    var lastThree = arrAllWords.Skip(arrAllWords.Length - 3).ToArray();
                    newText = string.Join(" ", lastThree);
                    break;
                case WordsCount.SeventyFive:
                    var seventyFiveChars = new StringBuilder();
                    for (var i = 0; i < arrAllWords.Length; i++)
                    {
                        var currentWord = string.Join(" ", Slice(arrAllWords, i, i + 1).ToArray());
                        if (!(seventyFiveChars.ToString().Length > 75))
                        {
                            seventyFiveChars = seventyFiveChars.Append(currentWord).Append(" ");
                        }
                    }

                    newText = seventyFiveChars.ToString().Trim();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Given " + place + " not yet handled.");
            }

            return newText;
        }

        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            // Handles negative ends.
            if (end < 0)
            {
                end = source.Length + end;
            }
            var len = end - start;

            // Return new array.
            var res = new T[len];
            for (var i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }
    }
}
