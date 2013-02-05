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

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace PicklesDoc.Pickles.DocumentationBuilders.DHTML
{
    public class DhtmlResourceProcessor
    {
        private readonly Configuration MyConfiguration;
        public DhtmlResourceSet MyDhtmlResourceSet { get; set; }

        public DhtmlResourceProcessor(Configuration configuration, DhtmlResourceSet dhtmlResourceSet)
        {
            this.MyConfiguration = configuration;
            this.MyDhtmlResourceSet = dhtmlResourceSet;
        }

        public void WriteZippedResources()
        {
            using (var binWriter = new BinaryWriter(File.OpenWrite(MyDhtmlResourceSet.ZippedResources.AbsolutePath)))
            {
                binWriter.Write(Properties.Resources.Pickles_BaseDhtmlFiles);
                binWriter.Flush();
                binWriter.Close();
            }
        }

        public void CleanupZippedResources()
        {
            File.Delete(MyDhtmlResourceSet.ZippedResources.AbsolutePath);
        }
    }
}