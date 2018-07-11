using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Tar.Core.Compression;
using Tar.Core.Extensions;

namespace Tar.Tests.Core
{
    [TestFixture]
    public class ZlibTestCase
    {
        private IZipComponent _zlib;
        private string _compressedFilePath;
        private string _unCompressedFilePath;
        private string _clearFilePath;

        [SetUp]
        public void SetUp()
        {
            _zlib = ZipComponentFactory.CreateZipComponent(ZipComponentType.Zlib);
            _clearFilePath = "c:\\ZlibTestCase_clear.txt";
            _compressedFilePath = "c:\\ZlibTestCase_compressed.txt";
            _unCompressedFilePath = "c:\\ZlibTestCase_unCompressed.txt";

            var lines = Enumerable.Range(0, 100).ToList()
                .Select(x => "Mehmet Zahir Solak").ToArray();
            File.WriteAllLines(_clearFilePath, lines);
        }

        [Test]
        public void ZipFile()
        {
            var targetStream = File.OpenWrite(_compressedFilePath);
            _zlib.Zip(targetStream, _clearFilePath, false);
        }

        [Test]
        public void UnZipFile()
        {
            ZipFile();
            _zlib.UnZip(_compressedFilePath, _unCompressedFilePath, false);
        }

        [Test]
        public void CompressDeCompressZip()
        {
            const string clearText = "Mehmet Zahir Solak";
            Console.WriteLine("clearText for Zip: {0}", clearText);
            Console.WriteLine("compressedBase64: {0}", clearText.Compress().ToBase64String());
            Console.WriteLine("decompressed: {0}", clearText.Compress().UnCompress());
        }
        [Test]
        public void CompressDeCompressZlib()
        {
            const string clearText = "Mehmet Zahir Solak";
            Console.WriteLine("clearText for Zlib: {0}", clearText);
            Console.WriteLine("compressedBase64: {0}", clearText.CompressZlib().ToBase64String());
            Console.WriteLine("decompressed: {0}", clearText.Compress().UnCompress());
        }
        [Test]
        public void ToAscii()
        {
            const string chars = "abcçdefğhıijklmnoöprsştuüvyzABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";
            Console.WriteLine(chars);
            Console.WriteLine(chars.ToAscii());
        }

        [Test]
        public void ToUtf8()
        {
            const string chars = "abcçdefğhıijklmnoöprsştuüvyzABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";
            Console.WriteLine(chars);
            Console.WriteLine(chars.ToUtf8());
        }

    }
}
