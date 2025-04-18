﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MarkdownDocumentationBuilder.cs" company="PicklesDoc">
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

using PicklesDoc.Pickles.DataStructures;
using System.IO;
using System.IO.Abstractions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown
{
    public class MarkdownDocumentationBuilder : IDocumentationBuilder
    {
        private readonly Stylist style = new Stylist();
        private readonly IFileSystem fileSystem;
        private readonly IConfiguration configuration;

        private readonly string namespaceOfResources;

        private readonly string outputFolder;


        public MarkdownDocumentationBuilder(IFileSystem fileSystem, IConfiguration configuration)
        {
            this.fileSystem = fileSystem;
            this.configuration = configuration;
            this.namespaceOfResources = "PicklesDoc.Pickles.DocumentationBuilders.Markdown.Resource.";

            if (configuration.OutputFolder == null)
            {
                configuration.OutputFolder = fileSystem.DirectoryInfo.FromDirectoryName("testing");
            }
            if(!configuration.OutputFolder.Exists)
                configuration.OutputFolder.Create();

            outputFolder = configuration.OutputFolder.FullName;
        }

        public void Build(Tree featureTree)
        {
            var documentation = new Documentation(featureTree);

            this.WriteContentToFile(fileSystem.Path.Combine(this.outputFolder, "features.md"), documentation.CurrentPage);

            WriteImage(outputFolder, "pass_32.png", "pass.png");
            WriteImage(outputFolder, "fail_32.png", "fail.png");
            WriteImage(outputFolder, "inconclusive_32.png", "inconclusive.png");
        }

        // TODO: new class handles file system interaction
        private void WriteContentToFile(string filePath, string content)
        {
            using (var file = fileSystem.FileInfo.FromFileName(filePath).CreateText())
            {
                file.Write(content);

                file.Close();
            }
        }


        private void WriteImage(string folder, string sourcefilename, string targetfilename)
        {
            string path = this.fileSystem.Path.Combine(folder, targetfilename);

            using (Image image = Image.Load(this.GetResourceStream(this.namespaceOfResources + sourcefilename)))
            {
                using (var stream = this.fileSystem.File.Create(path))
                {
                    image.Save(stream, new PngEncoder());
                }
            }
        }

        private Stream GetResourceStream(string nameOfResource)
        {
            return this.GetType().Assembly.GetManifestResourceStream(nameOfResource);
        }
    }
}
