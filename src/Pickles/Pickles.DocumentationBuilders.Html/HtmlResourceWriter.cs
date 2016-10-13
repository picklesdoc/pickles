//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlResourceWriter.cs" company="PicklesDoc">
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
using System.IO.Abstractions;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html
{
    public class HtmlResourceWriter : ResourceWriter
    {
        public HtmlResourceWriter(IFileSystem fileSystem)
            : base(fileSystem, "PicklesDoc.Pickles.DocumentationBuilders.Html.Resources.")
        {
        }

        public void WriteTo(string folder)
        {
            string cssFolder = this.FileSystem.Path.Combine(folder, "css");
            this.EnsureFolder(cssFolder);
            this.WriteStyleSheet(cssFolder, "master.css");
            this.WriteStyleSheet(cssFolder, "reset.css");
            this.WriteStyleSheet(cssFolder, "global.css");
            this.WriteStyleSheet(cssFolder, "structure.css");
            this.WriteStyleSheet(cssFolder, "print.css");
            this.WriteStyleSheet(cssFolder, "font-awesome.css");

            string imagesFolder = this.FileSystem.Path.Combine(folder, "img");
            this.EnsureFolder(imagesFolder);
            this.WriteImage(imagesFolder, "success.png");
            this.WriteImage(imagesFolder, "failure.png");
            this.WriteImage(imagesFolder, "inconclusive.png");

            string scriptsFolder = this.FileSystem.Path.Combine(folder, "js");
            this.EnsureFolder(scriptsFolder);
            this.WriteScript(scriptsFolder, "jquery.js");
            this.WriteScript(scriptsFolder, "scripts.js");

            string fontsFolder = this.FileSystem.Path.Combine(cssFolder, "fonts");
            this.EnsureFolder(fontsFolder);
            this.WriteFont(fontsFolder, "FontAwesome.ttf");
            this.WriteFont(fontsFolder, "fontawesome-webfont.eot");
            this.WriteFont(fontsFolder, "fontawesome-webfont.svg");
            this.WriteFont(fontsFolder, "fontawesome-webfont.ttf");
            this.WriteFont(fontsFolder, "fontawesome-webfont.woff");
        }

        private void EnsureFolder(string cssFolder)
        {
            if (!this.FileSystem.Directory.Exists(cssFolder))
            {
                this.FileSystem.Directory.CreateDirectory(cssFolder);
            }
        }
    }
}
