using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Core;
using Ionic.Zip;
using Ionic.Zlib;
namespace Tar.Core.Extensions
{
    public static class CompressExtensions
    {
        public const string InputFile = "c:\\CompressExtensions.input.log";
        public const string OutputFile = "c:\\CompressExtensions.output.log";

        public static byte[] Compress(this string source)
        {
            using (var memStreamIn = new MemoryStream(Encoding.UTF8.GetBytes(source)))
            {
                using (var outputMemStream = new MemoryStream())
                {
                    using (var zipStream = new ZipOutputStream(outputMemStream))
                    {
                        zipStream.CompressionLevel = CompressionLevel.Default;
                        zipStream.PutNextEntry("tar");
                        StreamUtils.Copy(memStreamIn, zipStream, new byte[4096]);
                    }
                    return outputMemStream.ToArray();
                }
            }
        }
        public static string UnCompress(this byte[] source)
        {
            using (var memStreamIn = new MemoryStream(source))
            {
                using (var outputMemStream = new MemoryStream())
                {
                    using (var zipStream = new ZipInputStream(memStreamIn))
                    {
                        zipStream.GetNextEntry();
                        StreamUtils.Copy(zipStream, outputMemStream, new byte[4096]);
                    }
                    return Encoding.UTF8.GetString(outputMemStream.ToArray());
                }
            }
        }

        public static byte[] CompressZlib(this string source)
        {
            return ZlibStream.CompressString(source);
        }

        public static string UnCompressZlib(this byte[] source)
        {
            return ZlibStream.UncompressString(source);
        }

    }
}
