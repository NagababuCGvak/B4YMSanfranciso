using System;
using System.Security.Cryptography;
using System.Text;

namespace Better4You.UserManagment.Business.PasswordValidation
{
    public abstract class HashAlgorithmPasswordEncryptor : IPasswordEncryptor
    {
        private readonly HashAlgorithm _hashAlgorithm;

        protected HashAlgorithmPasswordEncryptor(HashAlgorithm hashAlgorithm)
        {
            if (hashAlgorithm == null) throw new ArgumentNullException("hashAlgorithm");
            _hashAlgorithm = hashAlgorithm;
        }

        public string Encrypt(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hash = _hashAlgorithm.ComputeHash(inputBytes);
            var result = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            return result;
        }
    }
}