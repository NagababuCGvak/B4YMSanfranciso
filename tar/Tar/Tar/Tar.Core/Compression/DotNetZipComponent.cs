using System.IO;
using Ionic.Zip;

namespace Tar.Core.Compression
{
    internal class DotNetZipComponent : IZipComponent
    {
        public void Zip(Stream targetFileStream, string sourcePath, bool recursive)
        {
            using (var zip = new ZipFile())
            {
                if (File.Exists(sourcePath))
                    zip.AddFile(sourcePath);
                else if( Directory.Exists(sourcePath))
                    zip.AddDirectory(sourcePath);
                
                zip.Save(targetFileStream);
            }
        }

        public void UnZip(string zipFilePath, string targetFolderPath, bool recursive)
        {
            using (var zip = new ZipFile(zipFilePath))
            {
                zip.ExtractAll(targetFolderPath);
            }
        }
    }
}
