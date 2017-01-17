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
using System.IO;
using System.IO.Abstractions;

using PicklesDoc.Pickles.DocumentationBuilders.Html;

namespace PicklesDoc.Pickles.DocumentationBuilders.Dhtml
{
    public class DhtmlResourceWriter : ResourceWriter
    {
        private const string ExperimentalPlaceholder = "<!-- #### EMBED EXPERIMENTALS #### -->";

        private readonly string baseDirectory;
        private readonly IConfiguration configuration;

        public DhtmlResourceWriter(IFileSystem fileSystem, IConfiguration configuration)
            : base(fileSystem, "PicklesDoc.Pickles.DocumentationBuilders.Dhtml.Resources.")
        {
            this.configuration = configuration;
            this.baseDirectory = this.FileSystem.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "basedhtmlfiles");
        }

        public void WriteTo(string folder)
        {
            if (!this.FileSystem.Directory.Exists(folder))
            {
                this.FileSystem.Directory.CreateDirectory(folder);
            }

            var indexText = this.FileSystem.File.ReadAllText(this.FileSystem.Path.Combine(this.baseDirectory, "index.html"));

            if (this.configuration.ShouldIncludeExperimentalFeatures)
            {
                string mathScript = @"    
                    <script type=""text/x-mathjax-config"">
                        MathJax.Hub.Config({ tex2jax: { inlineMath: [['$', '$'], ['\\(','\\)']]} });
                    </script>
                    <script type=""text/javascript"" src=""https://cdn.mathjax.org/mathjax/latest/MathJax.js?config=TeX-MML-AM_CHTML""></script>
                    ";

                indexText = indexText.Replace(ExperimentalPlaceholder, mathScript);
            } 
            else
            {
                indexText = indexText.Replace(ExperimentalPlaceholder, string.Empty);
            }

            this.FileSystem.File.WriteAllText(this.FileSystem.Path.Combine(folder, "index.html"), indexText);

            string cssFolder = this.FileSystem.Path.Combine(folder, "css");
            this.EnsureFolder(cssFolder);
            this.CopyFolder(this.FileSystem.Path.Combine(this.baseDirectory, "css"), cssFolder);

            string imagesFolder = this.FileSystem.Path.Combine(folder, "img");
            this.EnsureFolder(imagesFolder);
            this.CopyFolder(this.FileSystem.Path.Combine(this.baseDirectory, "img"), imagesFolder);

            string scriptsFolder = this.FileSystem.Path.Combine(folder, "js");
            this.EnsureFolder(scriptsFolder);
            this.CopyFolder(this.FileSystem.Path.Combine(this.baseDirectory, "js"), scriptsFolder);
        }

        private void CopyFolder(string sourceFolder, string destFolder)
        {
            foreach (var file in Directory.EnumerateFiles(sourceFolder, "*.*", SearchOption.TopDirectoryOnly))
            {
                var fileInfo = new FileInfo(file);
                File.Copy(file, this.FileSystem.Path.Combine(destFolder, fileInfo.Name), true);
            }
        }

        private void EnsureFolder(string folder)
        {
            if (!this.FileSystem.Directory.Exists(folder))
            {
                this.FileSystem.Directory.CreateDirectory(folder);
            }
        }
    }
}
