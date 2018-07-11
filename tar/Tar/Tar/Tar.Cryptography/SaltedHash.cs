using System;
using System.Security.Cryptography;
using System.Text;

namespace Tar.Cryptography
{
    public class SaltedHash : IHashing, IDisposable
    {
        private readonly HashAlgorithm _hashAlgorithm;
        private readonly SymmetricAlgorithm _symmetricAlgorithm;
        private readonly int _salthLength;

        /// <summary>
        /// The constructor takes a HashAlgorithm as a parameter.
        /// </summary>
        /// <param name="hashAlgorithm">
        /// A <see cref="System.String" /> HashAlgorihm which is derived from HashAlgorithm. C# provides
        /// the following classes: SHA1Managed,SHA256Managed, SHA384Managed, SHA512Managed and MD5CryptoServiceProvider
        /// http://msdn.microsoft.com/en-us/library/wet69s13.aspx
        /// </param>
        /// <param name="symmetricAlgorithm">
        /// A <see cref="System.String" /> The classes that derive from the SymmetricAlgorithm class use a chaining mode
        /// called cipher block chaining (CBC), which requires a key (Key) and an initialization vector (IV) to perform
        /// cryptographic transformations on data. To decrypt data that was encrypted using one of the SymmetricAlgorithm
        /// classes, you must set the Key property and the IV property to the same values that were used for encryption.
        /// For a symmetric algorithm to be useful, the secret key must be known only to the sender and the receiver.
        /// RijndaelManaged , DESCryptoServiceProvider, RC2CryptoServiceProvider, and TripleDESCryptoServiceProvider are
        /// implementations of symmetric algorithms.
        /// http://msdn.microsoft.com/en-us/library/system.security.cryptography.symmetricalgorithm.aspx
        /// http://msdn.microsoft.com/en-us/library/k74a682y.aspx
        /// </param>
        /// <param name="saltLength">salt length</param>
        public SaltedHash(string hashAlgorithm, string symmetricAlgorithm)
        {
            //_hashProvider = hashAlgorithm;
            _hashAlgorithm = HashAlgorithm.Create(hashAlgorithm);
            _symmetricAlgorithm = SymmetricAlgorithm.Create(symmetricAlgorithm);
            _symmetricAlgorithm.Mode = CipherMode.ECB;
            _symmetricAlgorithm.Padding = PaddingMode.PKCS7;
            //_salthLength = saltLength;
            _salthLength = _symmetricAlgorithm.KeySize / 8;// Get Byte Count

            //_hashProvider = new SHA256Managed().;
        }

        /// <summary>
        /// Default constructor which initialises the SaltedHash with the SHA256Managed algorithm
        /// and a Salt of 4 bytes ( or 4*8 = 32 bits)
        /// </summary>

        public SaltedHash()
            : this("SHA256", "TripleDES")
        {
        }

        private byte[] GenerateSalt()
        {
            // Allocate memory for the salt
            var salt = new byte[_salthLength];
            // Strong runtime pseudo-random number generator, on Windows uses CryptAPI
            // on Unix /dev/urandom
            var random = new RNGCryptoServiceProvider();

            // Create a random salt
            random.GetNonZeroBytes(salt);
            return salt;
        }

        /// <summary>
        /// The actual hash calculation is shared by both GetHashAndSalt and the VerifyHash functions
        /// </summary>
        /// <param name="data">A byte array of the Data to Hash</param>
        /// <param name="salt">A byte array of the Salt to add to the Hash</param>
        /// <returns>A byte array with the calculated hash</returns>

        private byte[] ComputeHash(byte[] data, byte[] salt)
        {
            // Allocate memory to store both the Data and Salt together
            var dataAndSalt = new byte[data.Length + _salthLength];

            // Copy both the data and salt into the new array
            Array.Copy(data, dataAndSalt, data.Length);
            Array.Copy(salt, 0, dataAndSalt, data.Length, _salthLength);

            // Calculate the hash
            // Compute hash value of our plain text with appended salt.
            return _hashAlgorithm.ComputeHash(dataAndSalt);
        }

        /// <summary>
        /// Given a data block this routine returns both a Hash and a Salt
        /// </summary>
        /// <param name="data">
        /// A <see cref="System.Byte"/>byte array containing the data from which to derive the salt
        /// </param>
        /// <param name="hash">
        /// A <see cref="System.Byte"/>byte array which will contain the hash calculated
        /// </param>
        /// <param name="salt">
        /// A <see cref="System.Byte"/>byte array which will contain the salt generated
        /// </param>

        public void GetHashAndSalt(byte[] data, out byte[] hash, out byte[] salt)
        {
            salt = GenerateSalt();
            // Compute hash value of our data with the salt.
            hash = ComputeHash(data, salt);
        }

        /// <summary>
        /// The routine provides a wrapper around the GetHashAndSalt function providing conversion
        /// from the required byte arrays to strings. Both the Hash and Salt are returned as Base-64 encoded strings.
        /// </summary>
        /// <param name="data">
        /// A <see cref="System.String"/> string containing the data to hash
        /// </param>
        /// <param name="hash">
        /// A <see cref="System.String"/> base64 encoded string containing the generated hash
        /// </param>
        /// <param name="salt">
        /// A <see cref="System.String"/> base64 encoded string containing the generated salt
        /// </param>

        public void GetHashAndSaltString(string data, out string hash, out string salt)
        {
            byte[] hashOut;
            byte[] saltOut;

            // Obtain the Hash and Salt for the given string
            GetHashAndSalt(Encoding.UTF8.GetBytes(data), out hashOut, out saltOut);

            // Transform the byte[] to Base-64 encoded strings
            hash = Convert.ToBase64String(hashOut);
            salt = Convert.ToBase64String(saltOut);
        }

        /// <summary>
        /// This routine verifies whether the data generates the same hash as we had stored previously
        /// </summary>
        /// <param name="data">The data to verify </param>
        /// <param name="hash">The hash we had stored previously</param>
        /// <param name="salt">The salt we had stored previously</param>
        /// <returns>True on a succesfull match</returns>

        public bool VerifyHash(byte[] data, byte[] hash, byte[] salt)
        {
            byte[] newHash = ComputeHash(data, salt);

            //  No easy array comparison in C# -- we do the legwork
            if (newHash.Length != hash.Length) return false;

            for (int lp = 0; lp < hash.Length; lp++)
                if (!hash[lp].Equals(newHash[lp]))
                    return false;

            return true;
        }

        /// <summary>
        /// This routine provides a wrapper around VerifyHash converting the strings containing the
        /// data, hash and salt into byte arrays before calling VerifyHash.
        /// </summary>
        /// <param name="data">A UTF-8 encoded string containing the data to verify</param>
        /// <param name="hash">A base-64 encoded string containing the previously stored hash</param>
        /// <param name="salt">A base-64 encoded string containing the previously stored salt</param>
        /// <returns></returns>

        public bool VerifyHashString(string data, string hash, string salt)
        {
            byte[] hashToVerify = Convert.FromBase64String(hash);
            byte[] saltToVerify = Convert.FromBase64String(salt);
            byte[] dataToVerify = Encoding.UTF8.GetBytes(data);
            return VerifyHash(dataToVerify, hashToVerify, saltToVerify);
        }

        /// <summary>
        /// Given a data block this routine returns both a Encrpted and a Salt Key
        /// </summary>
        /// <param name="data">
        /// A <see cref="System.Byte"/>byte array containing the data from which to derive the encypted
        /// </param>
        /// <param name="encrypted">
        /// A <see cref="System.Byte"/>byte array which will contain the encrypted data calculated
        /// </param>
        /// <param name="saltKey">
        /// A <see cref="System.Byte"/> byte array which will contain the salt generated for key
        /// </param>

        public void Encrypt(byte[] data, out byte[] encrypted, out byte[] saltKey)
        {
            // Allocate memory for the salt
            saltKey = GenerateSalt();
            var hash = _hashAlgorithm.ComputeHash(saltKey);
            try
            {
                _symmetricAlgorithm.Key = saltKey;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
            }
            encrypted = _symmetricAlgorithm.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
        }

        /// <summary>
        /// This routine provides a wrapper around Encrypt the strings
        /// </summary>
        /// <param name="data">string containing the data to encrpyt</param>
        /// <param name="encrypted">
        /// A <see cref="System.String"/> base64 encoded string containing the generated encryted data
        /// </param>
        /// <param name="salt">
        /// A <see cref="System.String"/> base64 encoded string containing the generated encryted key
        /// </param>
        public void EncryptString(string data, out string encrypted, out string saltKey)
        {
            byte[] encryptedOut;
            byte[] saltOut;

            Encrypt(Encoding.UTF8.GetBytes(data), out encryptedOut, out saltOut);
            // Transform the byte[] to Base-64 encoded strings
            encrypted = Convert.ToBase64String(encryptedOut);
            saltKey = Convert.ToBase64String(saltOut);
        }

        /// <summary>
        /// Given a enrypted and slatKey block this routine returns a decrypted
        /// </summary>
        /// <param name="encrypted">
        /// A <see cref="System.Byte"/>byte array containing the enrypted data
        /// </param>
        /// <param name="saltKey">
        /// A <see cref="System.Byte"/>byte array which will contain the salt key
        /// </param>
        /// <param name="data">
        /// A <see cref="System.Byte"/> byte array which will contain the decrypted generated for enrypted and key
        /// </param>
        public void Decrypt(byte[] encrypted, byte[] saltKey, out byte[] data)
        {
            _symmetricAlgorithm.Key = saltKey;
            data = _symmetricAlgorithm.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
        }

        /// <summary>
        /// Given a enrypted and slatKey block this routine returns a decrypted
        /// </summary>
        /// <param name="encrypted">
        /// A <see cref="System.Byte"/>byte array containing the enrypted data
        /// </param>
        /// <param name="saltKey">
        /// A <see cref="System.Byte"/>byte array which will contain the salt key
        /// </param>
        /// <param name="data">
        /// A <see cref="System.Byte"/> byte array which will contain the decrypted generated for enrypted and key
        /// </param>
        public void DecryptString(string encrypted, string saltKey, out string data)
        {
            byte[] dataOut;
            Decrypt(Convert.FromBase64String(encrypted), Convert.FromBase64String(saltKey), out dataOut);
            //data = Convert.ToB(dataOut);
            data = Encoding.UTF8.GetString(dataOut);
        }

        public bool VerifyEncryption(byte[] data, byte[] saltKey, byte[] encrypted)
        {
            string dataToVerify = Encoding.UTF8.GetString(data);
            string encryptedToVerify = Convert.ToBase64String(encrypted);
            string saltKeyToVerify = Convert.ToBase64String(saltKey);
            return VerifyEncryptionString(dataToVerify, saltKeyToVerify, encryptedToVerify);
        }

        public bool VerifyEncryptionString(string data, string saltKey, string encrypted)
        {
            byte[] encryptedToVerify = Convert.FromBase64String(encrypted);
            byte[] saltKeyToVerify = Convert.FromBase64String(saltKey);
            string decryptedData;
            DecryptString(encrypted, saltKey, out decryptedData);
            return data.Equals(decryptedData) ? true : false;
        }

        #region Implementation of IDisposable

        public void Release()
        {
            IsReleased = true;
            //_hashAlgorithm.Clear();
            //_symmetricAlgorithm.Clear();
            _hashAlgorithm.Clear();
            _symmetricAlgorithm.Dispose();
        }

        public string Encrypt(string plainText, string saltKey)
        {
            var dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
            var saltKeyToEncrypt = Convert.FromBase64String(saltKey);
            _symmetricAlgorithm.Key = saltKeyToEncrypt;
            var encrypted = _symmetricAlgorithm.CreateEncryptor().TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            var encryptedData = Convert.ToBase64String(encrypted);
            return encryptedData;
        }

        void IDisposable.Dispose()
        {
            Release();
        }

        public bool IsReleased { get; private set; }

        #endregion Implementation of IDisposable
    }
}