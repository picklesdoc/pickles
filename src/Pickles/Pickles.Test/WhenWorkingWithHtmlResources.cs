//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenWorkingWithHtmlResources.cs" company="PicklesDoc">
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
using NFluent;
using NUnit.Framework;

using PicklesDoc.Pickles.DocumentationBuilders.Html;

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    public class WhenWorkingWithHtmlResources : BaseFixture
    {
        [Test]
        public void ThenCanDetectAllImagesSuccessfully()
        {
            var htmlResources = this.CreateHtmlResourceSet();

            HtmlResource[] images = htmlResources.Images.ToArray();

            Check.That(images.Length).IsEqualTo(3);
            Check.That(images.Select(image => image.File == "success.png")).Not.IsEmpty();
            Check.That(images.Select(image => image.File == "failure.png")).Not.IsEmpty();
            Check.That(images.Select(image => image.File == "inconclusive.png")).Not.IsEmpty();
        }

        private HtmlResourceSet CreateHtmlResourceSet()
        {
            var configuration = new Configuration { OutputFolder = FileSystem.DirectoryInfo.FromDirectoryName(@"c:\") };
            var htmlResources = new HtmlResourceSet(configuration, FileSystem);
            return htmlResources;
        }

        [Test]
        public void ThenCanDetectAllResourcesSuccessfully()
        {
            var htmlResources = this.CreateHtmlResourceSet();

            HtmlResource[] resources = htmlResources.All.ToArray();

            Check.That(resources.Length).IsEqualTo(9);
            Check.That(resources.Select(resource => resource.File == "success.png")).Not.IsEmpty();
            Check.That(resources.Select(resource => resource.File == "failure.png")).Not.IsEmpty();
            Check.That(resources.Select(resource => resource.File == "inconclusive.png")).Not.IsEmpty();
            Check.That(resources.Select(resource => resource.File == "global.css")).Not.IsEmpty();
            Check.That(resources.Select(resource => resource.File == "master.css")).Not.IsEmpty();
            Check.That(resources.Select(resource => resource.File == "reset.css")).Not.IsEmpty();
            Check.That(resources.Select(resource => resource.File == "structure.css")).Not.IsEmpty();
            Check.That(resources.Select(resource => resource.File == "print.css")).Not.IsEmpty();
            Check.That(resources.Select(resource => resource.File == "font-awesome.css")).Not.IsEmpty();
        }

        [Test]
        public void ThenCanDetectAllScriptsSuccessfully()
        {
            var htmlResources = this.CreateHtmlResourceSet();

            HtmlResource[] scripts = htmlResources.Scripts.ToArray();

            Check.That(scripts.Length).IsEqualTo(2);
            Check.That(scripts.Select(script => script.File == "jquery.js")).Not.IsEmpty();
            Check.That(scripts.Select(script => script.File == "scripts.js")).Not.IsEmpty();
        }

        [Test]
        public void ThenCanDetectAllStylesheetsSuccessfully()
        {
            var htmlResources = this.CreateHtmlResourceSet();

            HtmlResource[] stylesheets = htmlResources.Stylesheets.ToArray();

            Check.That(stylesheets.Length).IsEqualTo(6);
            Check.That(stylesheets.Select(stylesheet => stylesheet.File == "global.css")).Not.IsEmpty();
            Check.That(stylesheets.Select(stylesheet => stylesheet.File == "master.css")).Not.IsEmpty();
            Check.That(stylesheets.Select(stylesheet => stylesheet.File == "reset.css")).Not.IsEmpty();
            Check.That(stylesheets.Select(stylesheet => stylesheet.File == "structure.css")).Not.IsEmpty();
            Check.That(stylesheets.Select(stylesheet => stylesheet.File == "print.css")).Not.IsEmpty();
            Check.That(stylesheets.Select(resource => resource.File == "font-awesome.css")).Not.IsEmpty();
        }

        [Test]
        public void ThenSavesCssFilesToCorrectLocation()
        {
            FileSystem.AddDirectory(@"c:\output\");
            var htmlResourceWriter = new HtmlResourceWriter(FileSystem);

            htmlResourceWriter.WriteTo(@"c:\output\");

            var filesOnFileSystem = FileSystem.AllFiles.AsEnumerable<string>().ToArray();

            Check.That(filesOnFileSystem).Contains(@"c:\output\css\master.css");
            Check.That(filesOnFileSystem).Contains(@"c:\output\css\reset.css");
            Check.That(filesOnFileSystem).Contains(@"c:\output\css\global.css");
            Check.That(filesOnFileSystem).Contains(@"c:\output\css\global.css");
            Check.That(filesOnFileSystem).Contains(@"c:\output\css\structure.css");
            Check.That(filesOnFileSystem).Contains(@"c:\output\css\print.css");
            Check.That(filesOnFileSystem).Contains(@"c:\output\css\font-awesome.css");
        }
    }
}
