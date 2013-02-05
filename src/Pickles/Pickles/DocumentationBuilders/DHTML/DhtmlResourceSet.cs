using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PicklesDoc.Pickles.DocumentationBuilders.HTML;
using PicklesDoc.Pickles.Extensions;

namespace PicklesDoc.Pickles.DocumentationBuilders.DHTML
{
    public class DhtmlResourceSet
    {
        private readonly Configuration configuration;

        public DhtmlResourceSet(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public Uri ZippedResources
        {
            get { return this.configuration.OutputFolder.ToFileUriCombined("Pickles.BaseDhtmlFiles.zip"); }
        }
    }
}