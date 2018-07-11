using System;
using System.Security.Cryptography;

namespace Tar.Core.Cryptography
{
    public class EncryptionTripleDes : IEncryption
    {
        private readonly string _key;

        public string Encrypt(string input)
        {
            byte[] results;
            var utf8 = new System.Text.UTF8Encoding();
            var hashProvider = new MD5CryptoServiceProvider();
            var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(_key));
            var tdesAlgorithm = new TripleDESCryptoServiceProvider
                                    {
                                        Key = tdesKey,
                                        Mode = CipherMode.ECB,
                                        Padding = PaddingMode.PKCS7
                                    };
            var dataToEncrypt = utf8.GetBytes(input);
            try
            {
                var encryptor = tdesAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }
            return Convert.ToBase64String(results);
        }

        public string Decrypt(string input)
        {
            byte[] results;
            var utf8 = new System.Text.UTF8Encoding();
            var hashProvider = new MD5CryptoServiceProvider();
            var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(_key));
            var tdesAlgorithm = new TripleDESCryptoServiceProvider { Key = tdesKey, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };
            var dataToDecrypt = Convert.FromBase64String(input);
            try
            {
                ICryptoTransform decryptor = tdesAlgorithm.CreateDecryptor();
                results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            finally
            {
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }
            return utf8.GetString(results);
        }

        public EncryptionTripleDes(string key)
        {
            _key = key;
        }
    }
}