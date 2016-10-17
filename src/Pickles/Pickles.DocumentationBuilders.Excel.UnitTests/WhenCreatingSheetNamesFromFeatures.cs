//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenCreatingSheetNamesFromFeatures.cs" company="PicklesDoc">
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
using ClosedXML.Excel;
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.DocumentationBuilders.Excel;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.Test.DocumentationBuilders.Excel
{
    [TestFixture]
    public class WhenCreatingSheetNamesFromFeatures : BaseFixture
    {
        [Test]
        public void ThenCanCreateSimpleNameSuccessfully()
        {
            var excelSheetNameGenerator = Container.Resolve<ExcelSheetNameGenerator>();
            var feature = new Feature { Name = "A short feature name" };

            string name;
            using (var wb = new XLWorkbook())
            {
                name = excelSheetNameGenerator.GenerateSheetName(wb, feature);
            }

            Check.That(name).IsEqualTo("ASHORTFEATURENAME");
        }

        [Test]
        public void ThenCanShortenLongDuplicatedNameSuccessfully()
        {
            var excelSheetNameGenerator = Container.Resolve<ExcelSheetNameGenerator>();
            var feature1 = new Feature { Name = "This is a really really long feature name that needs to be shortened A" };
            var feature2 = new Feature { Name = "This is a really really long feature name that needs to be shortened B" };

            string name1;
            string name2;
            using (var wb = new XLWorkbook())
            {
                name1 = excelSheetNameGenerator.GenerateSheetName(wb, feature1);
                wb.AddWorksheet(name1);
                name2 = excelSheetNameGenerator.GenerateSheetName(wb, feature2);
            }

            Check.That(name2).IsEqualTo("THISISAREALLYREALLYLONGFEATU(1)");
        }

        [Test]
        public void ThenCanShortenLongMultipleDuplicatedNameSuccessfully()
        {
            var excelSheetNameGenerator = Container.Resolve<ExcelSheetNameGenerator>();
            var feature1 = new Feature { Name = "This is a really really long feature name that needs to be shortened A" };
            var feature2 = new Feature { Name = "This is a really really long feature name that needs to be shortened B" };
            var feature3 = new Feature { Name = "This is a really really long feature name that needs to be shortened C" };

            string name1;
            string name2;
            string name3;
            using (var wb = new XLWorkbook())
            {
                name1 = excelSheetNameGenerator.GenerateSheetName(wb, feature1);
                wb.AddWorksheet(name1);
                name2 = excelSheetNameGenerator.GenerateSheetName(wb, feature2);
                wb.AddWorksheet(name2);
                name3 = excelSheetNameGenerator.GenerateSheetName(wb, feature3);
            }

            Check.That(name3).IsEqualTo("THISISAREALLYREALLYLONGFEATU(2)");
        }

        [Test]
        public void ThenCanShortenLongNameSuccessfully()
        {
            var excelSheetNameGenerator = Container.Resolve<ExcelSheetNameGenerator>();
            var feature = new Feature { Name = "This is a really really long feature name that needs to be shortened" };

            string name;
            using (var wb = new XLWorkbook())
            {
                name = excelSheetNameGenerator.GenerateSheetName(wb, feature);
            }

            Check.That(name).IsEqualTo("THISISAREALLYREALLYLONGFEATUREN");
        }

        [Test]
        public void ThenItWillRemoveUnnecessaryAndInvalidCharacters()
        {
            var excelSheetNameGenerator = Container.Resolve<ExcelSheetNameGenerator>();
            var feature = new Feature { Name = @"This Had invalid characters: :\/?*[]" };

            string name;
            using (var wb = new XLWorkbook())
            {
                name = excelSheetNameGenerator.GenerateSheetName(wb, feature);
            }

            Check.That(name).IsEqualTo("THISHADINVALIDCHARACTERS");
        }
    }
}
