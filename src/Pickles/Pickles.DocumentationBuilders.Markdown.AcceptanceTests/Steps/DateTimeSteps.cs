//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DateTimeSteps.cs" company="PicklesDoc">
//  Copyright 2018 Darren Comeau
//  Copyright 2018-present PicklesDoc team and community contributors
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
using TechTalk.SpecFlow;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.AcceptanceTests.Steps
{
    [Binding]
    public sealed class DateTimeSteps
    {
        [Given(@"the date is (.*) and the time is (.*)")]
        public void GivenTheDateIsAndTheTimeIs(string date, string time)
        {
            var datetime = DateTime.Parse(date + " " + time);

            ScenarioContext.Current["DateTime.Now"] = datetime;
        }

    }
}
