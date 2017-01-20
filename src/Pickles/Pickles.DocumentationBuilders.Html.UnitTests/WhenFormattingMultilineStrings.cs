//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenFormattingMultilineStrings.cs" company="PicklesDoc">
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

using Autofac;

using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.Test;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html.UnitTests
{
    [TestFixture]
    public class WhenFormattingMultilineStrings : BaseFixture
    {
        [Test]
        public void ThenCanFormatNormalMultilineStringSuccessfully()
        {
            var multilineString = @"This is a
multiline string
that has been put into a
gherkin-style spec";

            var multilineStringFormatter = Container.Resolve<HtmlMultilineStringFormatter>();
            var output = multilineStringFormatter.Format(multilineString);

            Check.That(output).IsNotNull();
        }

        [Test]
        public void ThenCanFormatNullMultilineStringSuccessfully()
        {
            var multilineStringFormatter = Container.Resolve<HtmlMultilineStringFormatter>();
            var output = multilineStringFormatter.Format(null);

            Check.That(output).IsNull();
        }
    }
}
