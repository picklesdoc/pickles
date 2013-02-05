using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PicklesDoc.Pickles.DocumentationBuilders.DHTML
{
    public class JsonTweaker
    {
        public void AddJsonPWrapperTo(string filePath)
        {
            var existingContent = File.ReadAllText(filePath);

            using (StreamWriter s = new StreamWriter(filePath))
            {
                s.WriteLine("jsonPWrapper (");
                s.WriteLine(existingContent);
                s.Write(");");

                s.Flush();
                s.Close();
            }

        }

        public void RenameFileTo(string oldFilePath, string newFilePath)
        {
            File.Delete(newFilePath);
            File.Move(oldFilePath, newFilePath);
        }
    }
}
