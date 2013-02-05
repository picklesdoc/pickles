using ikvm.extensions;
using java.io;
using java.util.zip;
using System.IO;
using IOException = System.IO.IOException;

namespace PicklesDoc.Pickles.DocumentationBuilders.DHTML
{
    public class UnZipper
    {
        public void UnZip(string zipFileLocation, string destinationRootFolder, string zipRootToRemove)
        {
            try
            {
                var zipFile = new ZipFile(zipFileLocation);
                var zipFileEntries = zipFile.entries();
                
                while (zipFileEntries.hasMoreElements())
                {
                    var zipEntry = (ZipEntry)zipFileEntries.nextElement();

                    var name = zipEntry.getName().Replace(zipRootToRemove, "").Replace("/", "\\").TrimStart('/').TrimStart('\\');
                    var p = Path.Combine(destinationRootFolder, name);

                    if (zipEntry.isDirectory())
                    {
                        if (!Directory.Exists(p))
                        {
                            Directory.CreateDirectory(p);
                        };
                    }
                    else
                    {
                        using (var bis = new BufferedInputStream(zipFile.getInputStream(zipEntry)))
                        {
                            var buffer = new byte[2048];
                            var count = buffer.GetLength(0);
                            using (var fos = new FileOutputStream(p))
                            {
                                using (var bos = new BufferedOutputStream(fos, count))
                                {
                                    int size;
                                    while ((size = bis.read(buffer, 0, count)) != -1)
                                    {
                                        bos.write(buffer, 0, size);
                                    }

                                    bos.flush();
                                    bos.close();
                                }
                            }

                            bis.close();
                        }
                    }
                }

                zipFile.close();
            }
            catch (IOException e)
            {
                e.printStackTrace();
            }
            catch(System.Exception e)
            {
                var t = e.ToString();
            }
        }
    }
}
