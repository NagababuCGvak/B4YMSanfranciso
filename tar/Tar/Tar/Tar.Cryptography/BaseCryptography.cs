using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Tar.Cryptography
{
    public abstract class BaseCryptography : ICryptography, IDisposable
    {
        #region Implementation of ICryptography

        private readonly int _algorithmKeyLength;
        private readonly HashAlgorithm _hashAlgorithm;

        //public abstract void Encrypt();
        public abstract IList<KeyValuePair<CryptographyEnum, string>> Encrypt(IList<KeyValuePair<CryptographyEnum, string>> data);

        public abstract void Decrypt();

        protected BaseCryptography(HashAlgorithm hashAlgorithm, int algorithmKeyLength)
        {
            _hashAlgorithm = hashAlgorithm;
            _algorithmKeyLength = algorithmKeyLength;
        }

        private byte[] GenerateSalt()
        {
            // Allocate memory for the salt
            var salt = new byte[_algorithmKeyLength];
            // Strong runtime pseudo-random number generator, on Windows uses CryptAPI
            // on Unix /dev/urandom
            var random = new RNGCryptoServiceProvider();

            // Create a random salt
            random.GetNonZeroBytes(salt);
            return salt;
        }

        private byte[] ComputeHash(byte[] data, byte[] salt)
        {
            // Allocate memory to store both the Data and Salt together
            var dataAndSalt = new byte[data.Length + _algorithmKeyLength];

            // Copy both the data and salt into the new array
            Array.Copy(data, dataAndSalt, data.Length);
            Array.Copy(salt, 0, dataAndSalt, data.Length, _algorithmKeyLength);

            // Calculate the hash
            // Compute hash value of our plain text with appended salt.
            return _hashAlgorithm.ComputeHash(dataAndSalt);
        }

        private IList<KeyValuePair<CryptographyEnum, byte[]>> Hash(IList<KeyValuePair<CryptographyEnum, byte[]>> data)
        {
            var clearKey = data.Where(d => d.Key == CryptographyEnum.Clear).Select(d => d.Value).Single();
            var saltKey = data.Where(d => d.Key == CryptographyEnum.Salt).Select(d => d.Value).Single();
            if (saltKey == null)
            {
                saltKey = GenerateSalt();
                data.Add(new KeyValuePair<CryptographyEnum, byte[]>(CryptographyEnum.Salt, saltKey));
            }

            // Compute hash value of our data with the salt.
            var hashKey = ComputeHash(clearKey, saltKey);
            data.Add(new KeyValuePair<CryptographyEnum, byte[]>(CryptographyEnum.Hash, hashKey));
            return data;
        }

        #endregion Implementation of ICryptography

        #region Implementation of IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BaseCryptography()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _hashAlgorithm.Clear();
                _hashAlgorithm.Dispose();
            }
        }

        #endregion Implementation of IDisposable
    }
}