using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Tar.Core.Cryptography
{
    public class HashCryptography : BaseCryptography
    {
        public HashCryptography(HashAlgorithm hashAlgorithm, int algorithmKeyLength)
            : base(hashAlgorithm, algorithmKeyLength)
        {
        }

        #region Overrides of BaseCryptography

        public override IList<KeyValuePair<CryptographyEnum, string>> Encrypt(IList<KeyValuePair<CryptographyEnum, string>> data)
        {
            throw new NotImplementedException();
        }

        public override void Decrypt()
        {
            throw new NotImplementedException();
        }

        #endregion Overrides of BaseCryptography
    }
}