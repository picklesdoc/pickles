//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ResourceWriter.cs" company="PicklesDoc">
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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Abstractions;

namespace PicklesDoc.Pickles.DocumentationBuilders.Html
{
    public class ResourceWriter
    {
        private readonly IFileSystem fileSystem;

        private readonly string namespaceOfResources;

        public ResourceWriter(IFileSystem fileSystem, string namespaceOfResources)
        {
            this.fileSystem = fileSystem;
            this.namespaceOfResources = namespaceOfResources;
        }

        protected IFileSystem FileSystem
        {
            get { return this.fileSystem; }
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    return;
                }

                output.Write(buffer, 0, read);
            }
        }

        protected void WriteStyleSheet(string folder, string filename)
        {
            string path = this.fileSystem.Path.Combine(folder, filename);

            using (var reader = this.GetResourceStreamReader(this.namespaceOfResources + "css." + filename))
            {
                this.fileSystem.File.WriteAllText(path, reader.ReadToEnd());
            }
        }

        protected void WriteTextFile(string folder, string filename)
        {
            this.WriteTextFile(folder, filename, null, null);
        }

        protected void WriteTextFile(string folder, string filename, string toBeReplaced, string replacement)
        {
            string path = this.fileSystem.Path.Combine(folder, filename);

            using (var reader = this.GetResourceStreamReader(this.namespaceOfResources + filename))
            {
                var contents = reader.ReadToEnd();

                if (!string.IsNullOrWhiteSpace(toBeReplaced))
                {
                    contents = contents.Replace(toBeReplaced, replacement ?? string.Empty);
                }

                this.fileSystem.File.WriteAllText(path, contents);
            }
        }

        private StreamReader GetResourceStreamReader(string nameOfResource)
        {
            return new StreamReader(this.GetResourceStream(nameOfResource));
        }

        private Stream GetResourceStream(string nameOfResource)
        {
            return this.GetType().Assembly.GetManifestResourceStream(nameOfResource);
        }

        protected void WriteImage(string folder, string filename)
        {
            string path = this.fileSystem.Path.Combine(folder, filename);

            using (Image image = Image.FromStream(this.GetResourceStream(this.namespaceOfResources + "img." + filename)))
            {
                using (var stream = this.fileSystem.File.Create(path))
                {
                    image.Save(stream, ImageFormat.Png);
                }
            }
        }

        protected void WriteScript(string folder, string filename)
        {
            string path = this.fileSystem.Path.Combine(folder, filename);

            using (var reader = this.GetResourceStreamReader(this.namespaceOfResources + "js." + filename))
            {
                this.fileSystem.File.WriteAllText(path, reader.ReadToEnd());
            }
        }

        protected void WriteFont(string folder, string filename)
        {
            using (var input = this.GetResourceStream(this.namespaceOfResources + "css.fonts." + filename))
            {
                using (var output = this.fileSystem.File.Create(this.fileSystem.Path.Combine(folder, filename)))
                {
                    CopyStream(input, output);
                }
            }
        }
    }
}
