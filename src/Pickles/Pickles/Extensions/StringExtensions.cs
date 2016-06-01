//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StringExtensions.cs" company="PicklesDoc">
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
using System.Text;

namespace PicklesDoc.Pickles.Extensions
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string ExpandWikiWord(this string word)
        {
            var sb = new StringBuilder();
            char previous = char.MinValue;
            foreach (char current in word.Where(x => char.IsLetterOrDigit(x)))
            {
                if (previous != char.MinValue && sb.Length > 1
                    && ((char.IsUpper(current) || char.IsDigit(current)) && char.IsLower(previous)))
                {
                    sb.Append(' ');
                }
                else if (char.IsNumber(previous) && char.IsUpper(current))
                {
                    sb.Append(' ');
                }

                sb.Append(current);
                previous = current;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Takes a string and lowercases it, removing newline characters and replacing tabs with spaces
        /// </summary>
        /// <param name="text">The string that will be normalized for comparison.</param>
        /// <returns>The normalized string.</returns>
        public static string ComparisonNormalize(this string text)
        {
            return
                text.Trim()
                    .ToLowerInvariant()
                    .Replace("\r", string.Empty)
                    .Replace("\n", Environment.NewLine)
                    .Replace("\t", "    ");
        }

        /// <summary>
        /// Takes a string and returns a url-friendly version of the string (slug) with all special characters
        /// and extra spaces stripped out, with words seperated by dashes.
        /// </summary>
        /// <param name="text">The string that will be used to create the slug.</param>
        /// <returns>A slug generated from the given string.</returns>
        public static string ToSlug(this string text)
        {
            // remove any accent characters
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(text);
            var str = Encoding.ASCII.GetString(bytes);

            // modify string to slug format
            str = str.ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }
    }
}
