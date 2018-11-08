//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Lines.cs" company="PicklesDoc">
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

using System;
using System.Collections;
using System.Collections.Generic;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown
{
    class Lines : IEnumerable<string> 
    {
        private List<string> lines;

        public int Count { get { return lines.Count;  } }

        public Lines()
        {
            lines = new List<string>();
        }

        public void Add(string line)
        {
            lines.Add(line);
        }

        public void Add(Lines lines)
        {
            foreach(var line in lines)
            {
                this.lines.Add(line);
            }
        }

        public void Add(List<string> lines)
        {
            foreach (string line in lines)
            {
                this.lines.Add(line);
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return lines.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            string result = string.Empty;

            foreach (var line in lines)
            {
                result = string.Concat(result, line, Environment.NewLine);
            }

            return result;
        }
    }
}
