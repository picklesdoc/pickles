//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DescriptionProcessorTests.cs" company="PicklesDoc">
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

using NFluent;

using NUnit.Framework;

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    public class DescriptionProcessorTests
    {
        [Test]
        public void Process_NullInput_ReturnsEmptyString()
        {
            var preProcessor = CreatePreProcessor();

            string result = preProcessor.Process((string)null);

            Check.That(result).IsEqualTo(string.Empty);
        }

        private static DescriptionProcessor CreatePreProcessor()
        {
            return new DescriptionProcessor();
        }

        [Test]
        public void Process_EmptySpaces_ReturnsEmptyString()
        {
            var preProcessor = CreatePreProcessor();

            string result = preProcessor.Process("     ");

            Check.That(result).IsEqualTo(string.Empty);
        }

        [Test]
        public void Process_BlankThenText_ReturnsText()
        {
            var preProcessor = CreatePreProcessor();

            string result = preProcessor.Process(" abc");

            Check.That(result).IsEqualTo("abc");
        }

        [Test]
        public void Process_TwoLines_ReturnsText()
        {
            var preProcessor = CreatePreProcessor();

            string result = preProcessor.Process(" abc" + Environment.NewLine + " def");

            Check.That(result).IsEqualTo("abc" + Environment.NewLine + "def");
        }

        [Test]
        public void Specification_Scenario1()
        {
            var preProcessor = CreatePreProcessor();

            string result = preProcessor.Process(
                "  As a math idiot" + Environment.NewLine +
                "  In order to avoid mistakes" + Environment.NewLine +
                "  I want to be told the sum of two numbers");

            Check.That(result).IsEqualTo(
                "As a math idiot" + Environment.NewLine +
                "In order to avoid mistakes" + Environment.NewLine +
                "I want to be told the sum of two numbers");
        }

        [Test]
        public void Specification_Scenario2()
        {
            var preProcessor = CreatePreProcessor();

            string result = preProcessor.Process(
                "  As a math idiot" + Environment.NewLine +
                "  In order to avoid mistakes" + Environment.NewLine +
                "  I want to be told the sum of two numbers" + Environment.NewLine +
                Environment.NewLine +
                "      This is a real code block");

            Check.That(result).IsEqualTo(
                "As a math idiot" + Environment.NewLine +
                "In order to avoid mistakes" + Environment.NewLine +
                "I want to be told the sum of two numbers" + Environment.NewLine +
                Environment.NewLine +
                "    This is a real code block");
        }

        [Test]
        public void Specification_Scenario3()
        {
            var preProcessor = CreatePreProcessor();

            string result = preProcessor.Process(
                "  As a math idiot" + Environment.NewLine +
                "  In order to avoid mistakes" + Environment.NewLine +
                "  I want to be told the sum of two numbers" + Environment.NewLine +
                Environment.NewLine +
                "  Another line after a blank line");

            Check.That(result).IsEqualTo(
                "As a math idiot" + Environment.NewLine +
                "In order to avoid mistakes" + Environment.NewLine +
                "I want to be told the sum of two numbers" + Environment.NewLine +
                Environment.NewLine +
                "Another line after a blank line");
        }

        [Test]
        public void Process_OnlyLinesWithEmptySpaces_ReturnsAsManyEmptyLines()
        {
            var preProcessor = CreatePreProcessor();

            string result = preProcessor.Process(
                "     " + Environment.NewLine +
                "     " + Environment.NewLine +
                "     " + Environment.NewLine +
                "     ");

            Check.That(result).IsEqualTo(
                Environment.NewLine +
                Environment.NewLine +
                Environment.NewLine);
        }

        [Test]
        public void Specification_Scenario4_WithTabs()
        {
            var preProcessor = CreatePreProcessor();

            string result = preProcessor.Process(
                "" + '\t' + "As a math idiot" + Environment.NewLine +
                "" + '\t' + "In order to avoid mistakes" + Environment.NewLine +
                "" + '\t' + "I want to be told the sum of two numbers");

            Check.That(result).IsEqualTo(
                "As a math idiot" + Environment.NewLine +
                "In order to avoid mistakes" + Environment.NewLine +
                "I want to be told the sum of two numbers");
        }

        [Test]
        public void LineThatStartsWithEscapedHash_Header1_ShouldRemoveEscapeSign()
        {
            var preProcessor = CreatePreProcessor();

            string result = preProcessor.Process("    \\# Header 1");

            Check.That(result).IsEqualTo("# Header 1");
        }

        [Test]
        public void LineThatStartsWithEscapedHash_Header2_ShouldRemoveEscapeSign()
        {
            var preProcessor = CreatePreProcessor();

            string result = preProcessor.Process("    \\#\\# Header 2");

            Check.That(result).IsEqualTo("## Header 2");
        }

        [Test]
        public void LineThatStartsWithEscapedHash_Header3_ShouldRemoveEscapeSign()
        {
            var preProcessor = CreatePreProcessor();

            string result = preProcessor.Process("    \\#\\#\\# Header 3");

            Check.That(result).IsEqualTo("### Header 3");
        }

        [Test]
        public void LineThatStartsWithEscapedHash_Header4_ShouldRemoveEscapeSign()
        {
            var preProcessor = CreatePreProcessor();

            string result = preProcessor.Process("    \\#\\#\\#\\# Header 4");

            Check.That(result).IsEqualTo("#### Header 4");
        }

        [Test]
        public void LineThatStartsWithEscapedHash_Header5_ShouldRemoveEscapeSign()
        {
            var preProcessor = CreatePreProcessor();

            string result = preProcessor.Process("    \\#\\#\\#\\#\\# Header 5");

            Check.That(result).IsEqualTo("##### Header 5");
        }

        [Test]
        public void LineThatStartsWithEscapedHash_Header6_ShouldRemoveEscapeSign()
        {
            var preProcessor = CreatePreProcessor();

            string result = preProcessor.Process("    \\#\\#\\#\\#\\#\\# Header 6");

            Check.That(result).IsEqualTo("###### Header 6");
        }
    }
}