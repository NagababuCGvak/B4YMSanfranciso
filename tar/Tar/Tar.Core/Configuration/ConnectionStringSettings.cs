using System;
using Tar.Core.Cryptography;

namespace Tar.Core.Configuration
{
    public class ConnectionStringsSettings : Settings, IConnectionStringsSettings
    {
        private readonly IHashing _hashing;
        private string SaltKey
        {
            get { return GetSetting<string>("SaltKey"); }
        }

        public ConnectionStringsSettings(IHashing hashing) : 
            base("ConnectionStrings")
        {
            if (hashing == null) throw new ArgumentNullException("hashing");
            _hashing = hashing;
        }

        public string Decrypt(string encryptedConnectionString)
        {
            if (encryptedConnectionString == null) throw new ArgumentNullException("encryptedConnectionString");

            encryptedConnectionString = encryptedConnectionString.Replace(" ", "+");
            string decryptedConnectionString;
            _hashing.DecryptString(encryptedConnectionString, SaltKey, out decryptedConnectionString);
            return decryptedConnectionString;
        }

        public string Encrypt(string connectionString)
        {
            return _hashing.Encrypt(connectionString, SaltKey);
        }
    }
}