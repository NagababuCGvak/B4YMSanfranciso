using System.IO;

namespace Tar.Core.Compression
{
    /// <summary>
    /// ZipComponent
    /// </summary>
    public interface IZipComponent
    {
        /// <summary>
        /// Zip
        /// </summary>
        /// <param name="targetFileStream"></param>
        /// <param name="sourcePath">
        /// File or Folder Path For DotNetZipComponent , otherwise Folder Path
        /// </param>
        /// <param name="recursive"></param>
        void Zip(Stream targetFileStream, string sourcePath, bool recursive);

        /// <summary>
        /// UnZip
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <param name="targetFolderPath"></param>
        /// <param name="recursive"></param>
        void UnZip(string zipFilePath, string targetFolderPath, bool recursive);
    }
}