//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenAddingAFeatureToAWorksheet.cs" company="PicklesDoc">
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
using System.Collections.Generic;

using Autofac;

using ClosedXML.Excel;

using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.Test;

namespace PicklesDoc.Pickles.DocumentationBuilders.Excel.UnitTests
{
    [TestFixture]
    public class WhenAddingAFeatureToAWorksheet : BaseFixture
    {
        [Test]
        public void Then_feature_is_added_successfully()
        {
            var excelFeatureFormatter = Container.Resolve<ExcelFeatureFormatter>();
            var feature = new Feature
            {
                Name = "Test Feature",
                Description =
                    "In order to test this feature,\nAs a developer\nI want to test this feature"
            };

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("SHEET1");
                excelFeatureFormatter.Format(worksheet, feature);

                Check.That(worksheet.Cell("A1").Value).IsEqualTo(feature.Name);
                Check.That(worksheet.Cell("B2").Value).IsEqualTo(feature.Description);
            }
        }

        [Test]
        public void Then_feature_with_background_is_added_successfully()
        {
            var excelFeatureFormatter = Container.Resolve<ExcelFeatureFormatter>();

            var feature = new Feature
            {
                Name = "Test Feature",
                Description =
                    "In order to test this feature,\nAs a developer\nI want to test this feature",
            };
            var background = new Scenario
            {
                Name = "Test Background Scenario",
                Description =
                    "In order to test this background,\nAs a developer\nI want to test this background"
            };
            var given = new Step { NativeKeyword = "Given", Name = "a precondition" };
            background.Steps = new List<Step>(new[] { given });
            feature.AddBackground(background);

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("SHEET1");
                excelFeatureFormatter.Format(worksheet, feature);

                Check.That(worksheet.Cell("B4").Value).IsEqualTo(background.Name);
                Check.That(worksheet.Cell("C5").Value).IsEqualTo(background.Description);
                Check.That(worksheet.Cell("D6").Value).IsEqualTo(given.Name);
            }
        }

        [Test]
        public void Then_feature_without_background_adds_first_scenario_on_correct_row()
        {
            var excelFeatureFormatter = Container.Resolve<ExcelFeatureFormatter>();

            var feature = new Feature
            {
                Name = "Test Feature",
                Description =
                    "In order to test this feature,\nAs a developer\nI want to test this feature",
            };
            var scenario = new Scenario
            {
                Name = "Test Scenario",
                Description =
                    "In order to test this scenario,\nAs a developer\nI want to test this scenario"
            };
            var given = new Step { NativeKeyword = "Given", Name = "a precondition" };
            scenario.Steps = new List<Step>(new[] { given });
            feature.AddFeatureElement(scenario);

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("SHEET1");
                excelFeatureFormatter.Format(worksheet, feature);

                Check.That(worksheet.Cell("B4").Value).IsEqualTo(scenario.Name);
                Check.That(worksheet.Cell("C5").Value).IsEqualTo(scenario.Description);
                Check.That(worksheet.Cell("D6").Value).IsEqualTo(given.Name);
            }
        }
    }
}
