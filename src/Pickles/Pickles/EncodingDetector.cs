using System;
using System.IO.Abstractions;
using System.Text;

namespace PicklesDoc.Pickles
{
    public class EncodingDetector
    {
        private readonly IFileSystem fileSystem;

        public EncodingDetector(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public Encoding GetEncoding(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException();
            }

            if (this.fileSystem.File.Exists(filename))
            {
                var bom = new byte[4];
                using (var file = this.fileSystem.FileInfo.FromFileName(filename).OpenRead())
                {
                    file.Read(bom, 0, 4);
                }

                // Analyze the BOM
                if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
                if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
                if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
                if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
                if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            }

            return Encoding.UTF8;
        }
    }
}