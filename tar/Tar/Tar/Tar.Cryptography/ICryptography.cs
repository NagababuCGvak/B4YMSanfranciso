using System.Collections.Generic;

namespace Tar.Cryptography
{
    public interface ICryptography
    {
        IList<KeyValuePair<CryptographyEnum, string>> Encrypt(IList<KeyValuePair<CryptographyEnum, string>> data);

        void Decrypt();
    }
}