using System.Diagnostics;
using Tar.Core;
using Tar.Core.Configuration;
using NUnit.Framework;
using Tar.Core.Cryptography;

namespace Tar.Tests.Cryptography
{
    [TestFixture]
    public class HashingTestFixture
    {
        private IHashing _hashing;

        [SetUp]
        public void SetUp()
        {
            _hashing = ServiceLocator.Current.Get<IHashing>();
        }

        [Test, Category("Cryptography")]
        public void GetHashAndSaltStringToVerifySuccess()
        {
            const string data = "Reşad";
            string hash;
            string salt;
            _hashing.GetHashAndSaltString(data, out hash, out salt);
            bool verified = _hashing.VerifyHashString(data, hash, salt);

            Assert.AreEqual(verified, true);
        }

        [Test, Category("Hashing")]
        public void GetHashAndSaltStringToVerifyFailed()
        {
            const string data = "Reşad";
            string hash;
            string salt;
            _hashing.GetHashAndSaltString(data, out hash, out salt);
            bool verified = _hashing.VerifyHashString("Reşad1", hash, salt);
            Assert.AreEqual(verified, false);
        }

        [Test, Category("Cryptography")]
        public void EncryptStringToVerifySuccess()
        {
            const string data = "Reşad";
            string encrypted;
            string saltKey;
            _hashing.EncryptString(data, out encrypted, out saltKey);

            Debug.WriteLine("Data : {0}, Encrypted : {1}, SaltKey : {2}", data, encrypted, saltKey);
            bool verified = _hashing.VerifyEncryptionString(data, saltKey, encrypted);
            Assert.AreEqual(verified, true);
        }

        [Test, Category("Cryptography")]
        public void EncryptStringToVerifyFailed()
        {
            const string data = "Reşad";
            string encrypted;
            string saltKey;
            _hashing.EncryptString(data, out encrypted, out saltKey);
            Debug.WriteLine("Data : {0}, Encrypted : {1}, SaltKey : {2}", data, encrypted, saltKey);
            bool verified = _hashing.VerifyEncryptionString("Reşad1", saltKey, encrypted);
            Assert.AreEqual(verified, false);
        }

        [Test, Category("Cryptography")]
        public void DecryptStringToVerifySuccess()
        {
            string data;
            const string encrypted = "ORCbifIIPKs=";
            const string saltKey = "3GOx2WCt4tZefybWLJNcIH2vTbQWFJAF";
            _hashing.DecryptString(encrypted, saltKey, out data);

            Debug.WriteLine("Data : {0}, Encrypted : {1}, SaltKey : {2}", data, encrypted, saltKey);
            bool verified = _hashing.VerifyEncryptionString(data, saltKey, encrypted);
            Assert.AreEqual(verified, true);
        }

        [Test, Category("Cryptography")]
        public void DecryptStringToVerifyFailed()
        {
            string data;
            const string encrypted = "ORCbifIIPKs=";
            const string saltKey = "3GOx2WCt4tZefybWLJNcIH2vTbQWFJAF";
            _hashing.DecryptString(encrypted, saltKey, out data);

            Debug.WriteLine("Data : {0}, Encrypted : {1}, SaltKey : {2}", data, encrypted, saltKey);
            bool verified = _hashing.VerifyEncryptionString("Reşad1", saltKey, encrypted);
            Assert.AreEqual(verified, false);
        }

        [Test]
        public void EncryptDecrypt()
        {
            var connectionSettings = ServiceLocator.Current.Get<IConnectionStringsSettings>();
            const string valueToEncryt = "Test-1234.4321*tseT - Reşad Askeroğlu";
            var encryptedValue = connectionSettings.Encrypt(valueToEncryt);
            Assert.AreNotEqual(valueToEncryt, encryptedValue);

            var decryptedValue = connectionSettings.Decrypt(encryptedValue);
            Assert.AreNotEqual(encryptedValue, decryptedValue);
            Assert.AreEqual(decryptedValue, valueToEncryt);
        }
    }
}