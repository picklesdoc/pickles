    //  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="SpecRunSingleResultLoader.cs" company="PicklesDoc">
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
using System.Xml;
using System.Xml.Linq;

namespace PicklesDoc.Pickles.TestFrameworks.SpecRun
{
    public class SpecRunSingleResultLoader : ISingleResultLoader
    {
        public SingleTestRunBase Load(IFileInfo fileInfo)
        {
            var document = this.ReadResultsFile(fileInfo);
            var features = this.ToFeatures(document);

            return new SpecRunSingleResults(features);
        }

        private List<SpecRunFeature> ToFeatures(XDocument readResultsFile)
        {
            return readResultsFile.Descendants("feature").Select(Factory.ToSpecRunFeature).ToList();
        }

        private XDocument ReadResultsFile(IFileInfo testResultsFile)
        {
            XDocument document;
            //using (var stream = testResultsFile.OpenRead())
            //{
            var stream = testResultsFile.OpenRead();

                using (var streamReader = new System.IO.StreamReader(stream))
                {
                    string content = streamReader.ReadToEnd();

                    int begin = content.IndexOf("<!-- Pickles Begin", StringComparison.Ordinal);

                    content = content.Substring(begin);

                    content = content.Replace("<!-- Pickles Begin", string.Empty);

                    int end = content.IndexOf("Pickles End -->", System.StringComparison.Ordinal);

                    content = content.Substring(0, end);

                    content = content.Replace("&lt;", "<").Replace("&gt;", ">");

                    var xmlReader = XmlReader.Create(new System.IO.StringReader(content));
                    document = XDocument.Load(xmlReader);
                }
            //}

            return document;
        }
    }
}