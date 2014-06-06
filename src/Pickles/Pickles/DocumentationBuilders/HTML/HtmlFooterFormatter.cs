#region License

/*
    Copyright [2011] [Jeffrey Cameron]

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

#endregion

using System;
using System.Reflection;
using System.Xml.Linq;

namespace PicklesDoc.Pickles.DocumentationBuilders.HTML
{
    public class HtmlFooterFormatter
    {
        private readonly Configuration configuration;
        private readonly XNamespace xmlns;

        public HtmlFooterFormatter(Configuration configuration)
        {
            this.configuration = configuration;
            this.xmlns = HtmlNamespace.Xhtml;
        }

        private XElement BuildVersionString()
        {
            if (!string.IsNullOrEmpty(this.configuration.SystemUnderTestName) &&
                !string.IsNullOrEmpty(this.configuration.SystemUnderTestVersion))
            {
                return new XElement(this.xmlns + "p",
                                    string.Format("Test results generated for: {0}, version {1}",
                                                  this.configuration.SystemUnderTestName,
                                                  this.configuration.SystemUnderTestVersion));
            }
            else if (!string.IsNullOrEmpty(this.configuration.SystemUnderTestName))
            {
                return new XElement(this.xmlns + "p",
                                    string.Format("Test results generated for: {0}", this.configuration.SystemUnderTestName));
            }
            else if (!string.IsNullOrEmpty(this.configuration.SystemUnderTestVersion))
            {
                return new XElement(this.xmlns + "p",
                                    string.Format("Test results generated for: version {1}",
                                                  this.configuration.SystemUnderTestVersion));
            }

            return null;
        }

        public XElement Format()
        {
            return new XElement(this.xmlns + "div",
                                new XAttribute("id", "footer"),
                                this.BuildVersionString(),
                                new XElement(this.xmlns + "p", "Pickled on: " + DateTime.Now.ToString("d MMMM yyyy HH:mm:ss")),
                                new XElement(this.xmlns + "p",
                                             "Produced by Pickles, version " +
                                             Assembly.GetExecutingAssembly().GetName().Version)
                );
        }
    }
}