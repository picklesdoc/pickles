//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TreeTests.cs" company="PicklesDoc">
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
using System.IO.Abstractions;
using System.Linq;

using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;

namespace PicklesDoc.Pickles.Test.DataStructures
{
    [TestFixture]
    public class TreeTests : BaseFixture
    {
        private const string RootPath = FileSystemPrefix + @"OrderingTests";

        [Test]
        public void ShouldIterateInRightOrder()
        {
            var tree = CreateTree("root");

            Tree treeB = tree.Add(new MyNode("B"));
            treeB.Add(new MyNode("b-b"));
            treeB.Add(new MyNode("b-a"));

            Tree treeA = tree.Add(new MyNode("A"));
            treeA.Add(new MyNode("a-b"));
            treeA.Add(new MyNode("a-a"));

            var actualSequence = tree.Select(n => n.Name).ToList();

            Check.That(actualSequence).ContainsExactly("root", "A", "a-a", "a-b", "B", "b-a", "b-b");
        }

        [Test]
        public void Constructor_NullArgument_ShouldThrowArgumentNullException()
        {
            Check.ThatCode(() => new Tree(null)).Throws<ArgumentNullException>();
        }

        [Test]
        public void Iterator_NodeWithNullName_ShouldNotThrowException()
        {
            var tree = CreateTree("node");
            tree.Add(new MyNode(null));
            tree.Add(new MyNode("name"));

            Check.ThatCode(() => tree.ToList()).DoesNotThrow();
        }

        private static Tree CreateTree(string name)
        {
            return new Tree(new MyNode(name));
        }

        [Test]
        public void Add_NullTree_ThrowArgumentNullException()
        {
            var tree = CreateTree("root");

            Check.ThatCode(() => tree.Add((Tree)null)).Throws<ArgumentNullException>();
        }

        [Test]
        public void Add_NullNode_ThrowArgumentNullException()
        {
            var tree = CreateTree("root");

            Check.ThatCode(() => tree.Add((INode)null)).Throws<ArgumentNullException>();
        }

        private class MyNode : INode
        {
            public MyNode(string name)
            {
                this.Name = name;
            }

            public NodeType NodeType { get; }

            public string Name { get; }

            public FileSystemInfoBase OriginalLocation { get; }

            public Uri OriginalLocationUrl { get; }

            public string RelativePathFromRoot { get; }

            public string GetRelativeUriTo(Uri other)
            {
                throw new NotImplementedException();
            }

            public string GetRelativeUriTo(Uri other, string newExtension)
            {
                throw new NotImplementedException();
            }
        }
    }
}