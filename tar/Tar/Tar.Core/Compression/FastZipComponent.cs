using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Tar.Core.Compression
{
    /// <summary>
    /// FastZipComponent
    /// </summary>
    internal class FastZipComponent : IZipComponent
    {
        private readonly FastZip _fastZip = new FastZip();

        /// <summary>
        /// Zip
        /// </summary>
        /// <param name="targetFileStream"></param>
        /// <param name="sourceFolderPath"></param>
        /// <param name="recursive"></param>
        public void Zip(Stream targetFileStream, string sourceFolderPath, bool recursive)
        {
            _fastZip.CreateZip(targetFileStream, sourceFolderPath, recursive, null, null);
        }

        /// <summary>
        /// UnZip
        /// </summary>
        /// <param name="sourceZipFilePath"></param>
        /// <param name="targetFolderPath"></param>
        /// <param name="recursive"></param>
        public void UnZip(string sourceZipFilePath, string targetFolderPath, bool recursive)
        {
            _fastZip.ExtractZip(sourceZipFilePath, targetFolderPath, null);
        }
    }
}
