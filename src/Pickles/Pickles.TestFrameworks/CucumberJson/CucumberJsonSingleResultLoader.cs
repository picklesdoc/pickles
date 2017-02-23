//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CucumberJsonSingleResultLoader.cs" company="PicklesDoc">
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
using System.IO;
using System.IO.Abstractions;

using Newtonsoft.Json;

namespace PicklesDoc.Pickles.TestFrameworks.CucumberJson
{
    public class CucumberJsonSingleResultLoader : ISingleResultLoader
    {
        public SingleTestRunBase Load(FileInfoBase fileInfo)
        {
            List<Feature> features = this.ReadResultsFile(fileInfo);
            if (features == null)
            {
                Console.WriteLine("There was an issue with the results format for " + fileInfo.Name);
                features = new List<Feature>();
            }

            return new CucumberJsonSingleResults(features);
        }

        private List<Feature> ReadResultsFile(FileInfoBase testResultsFile)
        {
            List<Feature> result;
            using (var stream = testResultsFile.OpenRead())
            {
                using (var reader = new StreamReader(stream))
                {
                    result = JsonConvert.DeserializeObject<List<Feature>>(reader.ReadToEnd());
                }
            }

            return result;
        }
    }
}