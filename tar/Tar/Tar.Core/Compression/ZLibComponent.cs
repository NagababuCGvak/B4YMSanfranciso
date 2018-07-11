using System;
using System.IO;
using Ionic.Zlib;

namespace Tar.Core.Compression
{
    internal class ZLibComponent:IZipComponent
    {
        public int BufferSize = 128;
        public CompressionLevel CompressionLevel = CompressionLevel.Default;

        public void Zip(Stream targetFileStream, string sourcePath, bool recursive)
        {
            if (recursive) throw new ArgumentException("recursive parameter cannot be true!");
            if (Directory.Exists(sourcePath)) throw new ArgumentException("sourcePath parameter cannot be directory!");

            using (var source = File.OpenRead(sourcePath))
            {
                using (var target = new ZlibStream(targetFileStream, CompressionMode.Compress, CompressionLevel))
                {
                    source.CopyTo(target, BufferSize);
                }
            }
        }

        public void UnZip(string zipFilePath, string targetFolderPath, bool recursive)
        {
            if (recursive) throw new ArgumentException("recursive parameter cannot be true!");
            if (Directory.Exists(targetFolderPath)) throw new ArgumentException("sourcePath parameter cannot be directory!");

            using (var compressedFile = File.OpenRead(zipFilePath))
            {
                using (var source = new ZlibStream(compressedFile, CompressionMode.Decompress, CompressionLevel))
                {
                    using (var target= File.OpenWrite(targetFolderPath))
                    {
                        source.CopyTo(target, BufferSize);
                    }
                }
            }
        }
    }
}
