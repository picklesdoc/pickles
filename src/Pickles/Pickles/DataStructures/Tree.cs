//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Tree.cs" company="PicklesDoc">
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
using System.Collections;
using System.Collections.Generic;

using PicklesDoc.Pickles.DirectoryCrawler;

namespace PicklesDoc.Pickles.DataStructures
{
    public class Tree : IEnumerable<INode>
    {
        private readonly INode currentNode;

        public Tree(INode currentNode)
        {
            this.currentNode = currentNode;
            this.ChildNodes = new List<Tree>();
        }

        public IList<Tree> ChildNodes { get; }

        public INode Data
        {
            get
            {
                return this.currentNode;
            }
        }

        public IEnumerator<INode> GetEnumerator()
        {
            List<INode> result = new List<INode>();
            result.Add(this.currentNode);

            foreach(var childNode in this.ChildNodes)
            {
                using (var enumerator = childNode.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        result.Add(enumerator.Current);
                    }
                }
            }

            return result.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(Tree node)
        {
            this.ChildNodes.Add(node);
        }

        public void Add(INode node)
        {
            this.Add(new Tree(node));
        }
    }
}