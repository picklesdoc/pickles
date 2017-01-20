//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FolderNode.cs" company="PicklesDoc">
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
using PicklesDoc.Pickles.Extensions;

namespace PicklesDoc.Pickles.DirectoryCrawler
{
    public class FolderNode : INode
    {
        public FolderNode(FileSystemInfoBase location, string relativePathFromRoot)
        {
            this.OriginalLocation = location;
            this.OriginalLocationUrl = location.ToUri();
            this.RelativePathFromRoot = relativePathFromRoot;
        }

        public NodeType NodeType
        {
            get { return NodeType.Structure; }
        }

        public string Name
        {
            get { return this.OriginalLocation.Name.ExpandWikiWord(); }
        }

        public FileSystemInfoBase OriginalLocation { get; }

        public Uri OriginalLocationUrl { get; }

        public string RelativePathFromRoot { get; }

        public string GetRelativeUriTo(Uri other, string newExtension)
        {
            bool areSameLocation = this.OriginalLocation.FullName == other.LocalPath;

            if (areSameLocation)
            {
                return "#";
            }

            string result = other.MakeRelativeUri(this.OriginalLocationUrl).ToString();

            string oldExtension = this.OriginalLocation.Extension;

            if (!string.IsNullOrEmpty(oldExtension))
            {
                result = result.Replace(oldExtension, newExtension);
            }

            return result;
        }

        public string GetRelativeUriTo(Uri other)
        {
            return this.GetRelativeUriTo(other, ".html");
        }
    }
}
