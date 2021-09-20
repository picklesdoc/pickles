//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="XmlDeserializer.cs" company="PicklesDoc">
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

using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Xml;

namespace PicklesDoc.Pickles.TestFrameworks
{
    /// <summary>
    /// A generic xml deserializer class.
    /// </summary>
    /// <typeparam name="TItem">The type of the items that will be deserialized.</typeparam>
    public class XmlDeserializer<TItem>
        where TItem : class
    {
        public TItem Load(IFileInfo fileInfo)
        {
            TItem document;

            using (var stream = fileInfo.OpenRead())
            {
                document = this.Load(stream);
            }

            return document;
        }

        public TItem Load(Stream stream)
        {
            TItem result;

            using (XmlReader reader = XmlReader.Create(stream))
            {
                result = (TItem)new System.Xml.Serialization.XmlSerializer(typeof(TItem)).Deserialize(reader);
            }

            return result;
        }
    }
}