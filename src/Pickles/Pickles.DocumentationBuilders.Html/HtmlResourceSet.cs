//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlResourceSet.cs" company="PicklesDoc">
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
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using PicklesDoc.Pickles.Extensions;

namespace PicklesDoc.Pickles.DocumentationBuilders.HTML
{
    public class HtmlResourceSet
    {
        private readonly IConfiguration configuration;

        private readonly IFileSystem fileSystem;

        public HtmlResourceSet(IConfiguration configuration, IFileSystem fileSystem)
        {
            this.configuration = configuration;
            this.fileSystem = fileSystem;
        }

        public Uri MasterStylesheet
        {
            get { return this.configuration.OutputFolder.ToFileUriCombined("css/master.css", this.fileSystem); }
        }

        public Uri PrintStylesheet
        {
            get { return this.configuration.OutputFolder.ToFileUriCombined("css/print.css", this.fileSystem); }
        }

        public Uri JQueryScript
        {
            get { return this.configuration.OutputFolder.ToFileUriCombined("js/jquery.js", this.fileSystem); }
        }

        public Uri AdditionalScripts
        {
            get { return this.configuration.OutputFolder.ToFileUriCombined("js/scripts.js", this.fileSystem); }
        }

        public Uri SuccessImage
        {
            get { return new Uri(this.fileSystem.Path.Combine(this.ImagesFolder, "success.png")); }
        }

        public string ImagesFolder
        {
            get { return this.fileSystem.Path.Combine(this.configuration.OutputFolder.FullName, "img"); }
        }

        public string ScriptsFolder
        {
            get { return this.fileSystem.Path.Combine(this.configuration.OutputFolder.FullName, "js"); }
        }

        public Uri FailureImage
        {
            get { return new Uri(this.fileSystem.Path.Combine(this.ImagesFolder, "failure.png")); }
        }

        public IEnumerable<HtmlResource> All
        {
            get { return this.Stylesheets.Concat(this.Images); }
        }

        public IEnumerable<HtmlResource> Stylesheets
        {
            get
            {
                string[] resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();

                foreach (string resource in resources.Where(resource => resource.EndsWith(".css") && resource.Contains(".Html.")))
                {
                    string fileName = this.GetNameFromResourceName(resource);
                    yield return new HtmlResource
                    {
                        File = fileName,
                        Uri = this.configuration.OutputFolder.ToFileUriCombined(fileName, this.fileSystem)
                    };
                }
            }
        }

        public IEnumerable<HtmlResource> Images
        {
            get
            {
                string[] resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();

                foreach (string resource in resources.Where(resource => resource.EndsWith(".png") && resource.Contains(".Html.")))
                {
                    string fileName = this.GetNameFromResourceName(resource);
                    yield return new HtmlResource
                    {
                        File = fileName,
                        Uri = new Uri(this.fileSystem.Path.Combine(this.ImagesFolder, fileName))
                    };
                }
            }
        }

        public IEnumerable<HtmlResource> Scripts
        {
            get
            {
                string[] resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();

                foreach (string resource in resources.Where(resource => resource.EndsWith(".js") && resource.Contains(".Html.")))
                {
                    string fileName = this.GetNameFromResourceName(resource);
                    yield return new HtmlResource
                    {
                        File = fileName,
                        Uri = new Uri(this.fileSystem.Path.Combine(this.ScriptsFolder, fileName))
                    };
                }
            }
        }

        public Uri InconclusiveImage
        {
            get { return new Uri(this.fileSystem.Path.Combine(this.ImagesFolder, "inconclusive.png")); }
        }

        private string GetNameFromResourceName(string resourceName)
        {
            if (resourceName.StartsWith("PicklesDoc.Pickles.Resources.Html.img"))
            {
                return resourceName.Replace("PicklesDoc.Pickles.Resources.Html.img.", string.Empty);
            }
            else if (resourceName.StartsWith("PicklesDoc.Pickles.Resources.Html.js"))
            {
                return resourceName.Replace("PicklesDoc.Pickles.Resources.Html.js.", string.Empty);
            }
            else
            {
                return resourceName.Replace("PicklesDoc.Pickles.Resources.Html.css.", string.Empty);
            }
        }
    }
}
