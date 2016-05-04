//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DhtmlResourceWriter.cs" company="PicklesDoc">
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

using PicklesDoc.Pickles.DocumentationBuilders.HTML;

namespace PicklesDoc.Pickles.DocumentationBuilders.DHTML
{
    public class DhtmlResourceWriter : ResourceWriter
    {
        private readonly IConfiguration configuration;

        public DhtmlResourceWriter(IFileSystem fileSystem, IConfiguration configuration)
            : base(fileSystem, "PicklesDoc.Pickles.Resources.Dhtml.")
        {
            this.configuration = configuration;
        }

        public void WriteTo(string folder)
        {
            if (this.configuration.ShouldIncludeExperimentalFeatures)
            {
                string mathScript = @"    <script type=""text/x-mathjax-config"">
        MathJax.Hub.Config({ tex2jax: { inlineMath: [['$', '$'], ['\\(','\\)']]}
});
    </script>
    <script type=""text/javascript"" src=""https://cdn.mathjax.org/mathjax/latest/MathJax.js?config=TeX-MML-AM_CHTML"">
    </script>
";

                this.WriteTextFile(folder, "Index.html", "#### EMBED EXPERIMENTALS ####", mathScript);
            }
            else
            {
                this.WriteTextFile(folder, "Index.html", "#### EMBED EXPERIMENTALS ####", "");
            }
            this.WriteTextFile(folder, "pickledFeatures.js");

            string cssFolder = this.FileSystem.Path.Combine(folder, "css");
            this.EnsureFolder(cssFolder);
            this.WriteStyleSheet(cssFolder, "bootstrap.min.css");
            this.WriteStyleSheet(cssFolder, "print.css");
            this.WriteStyleSheet(cssFolder, "styles.css");

            string imagesFolder = this.FileSystem.Path.Combine(folder, "img");
            this.EnsureFolder(imagesFolder);
            this.WriteImage(imagesFolder, "glyphicons-halflings-white.png");
            this.WriteImage(imagesFolder, "glyphicons-halflings.png");

            string scriptsFolder = this.FileSystem.Path.Combine(folder, "js");
            this.EnsureFolder(scriptsFolder);
            this.WriteScript(scriptsFolder, "bootstrap.min.js");
            this.WriteScript(scriptsFolder, "featureSearch.js");
            this.WriteScript(scriptsFolder, "featuresModel.js");
            this.WriteScript(scriptsFolder, "heirarchyBuilder.js");
            this.WriteScript(scriptsFolder, "html5.js");
            this.WriteScript(scriptsFolder, "jquery-1.8.3.min.js");
            this.WriteScript(scriptsFolder, "jquery.highlight-4.closure.js");
            this.WriteScript(scriptsFolder, "knockout-3.4.0.js");
            this.WriteScript(scriptsFolder, "knockout.mapping-latest.js");
            this.WriteScript(scriptsFolder, "logger.js");
            this.WriteScript(scriptsFolder, "Markdown.Converter.js");
            this.WriteScript(scriptsFolder, "Markdown.Extra.js");
            this.WriteScript(scriptsFolder, "stringFormatting.js");
            this.WriteScript(scriptsFolder, "typeaheadList.js");
            this.WriteScript(scriptsFolder, "underscore-min.js");
            this.WriteScript(scriptsFolder, "Chart.min.js");
            this.WriteScript(scriptsFolder, "Chart.StackedBar.js");
            this.WriteScript(scriptsFolder, "picklesOverview.js");
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
