//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PathExtensions.cs" company="PicklesDoc">
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
using System.Linq;

namespace PicklesDoc.Pickles.Extensions
{
    public static class PathExtensions
    {
        public static string MakeRelativePath(string from, string to, IFileSystem fileSystem)
        {
            if (string.IsNullOrEmpty(from))
            {
                throw new ArgumentNullException("from");
            }

            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentNullException("to");
            }

            var fromUri = fileSystem.GetUri(from);
            var toUri = fileSystem.GetUri(to);

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath.Replace(Uri.SchemeDelimiter, fileSystem.Path.DirectorySeparatorChar.ToString());
        }


        public static string MakeRelativePath(IFileSystemInfo from, IFileSystemInfo to, IFileSystem fileSystem)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }

            if (to == null)
            {
                throw new ArgumentNullException("to");
            }

            return MakeRelativePath(from.FullName, to.FullName, fileSystem);
        }

        private static string[] GetAllFilesFromPathAndFileNameWithOptionalWildCards(string fileFilePath, IFileSystem fileSystem)
        {
            var path = fileSystem.Path.GetDirectoryName(fileFilePath);
            var wildcardFileName = fileSystem.Path.GetFileName(fileFilePath);
            if (string.IsNullOrWhiteSpace(path))
                path = fileSystem.Directory.GetCurrentDirectory();
            // GetFiles returns an array with 1 empty string when wildcard match is not found.
            return fileSystem.Directory.GetFiles(path, wildcardFileName).Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }

        public static IEnumerable<IFileInfo> GetAllFilesFromPathAndFileNameWithOptionalSemicolonsAndWildCards(string fileFullName, IFileSystem fileSystem)
        {
            var files = fileFullName.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            return files.SelectMany(f => GetAllFilesFromPathAndFileNameWithOptionalWildCards(f, fileSystem))
                    .Distinct()
                    .Select(f => fileSystem.FileInfo.FromFileName(f));
        }

    }
}
