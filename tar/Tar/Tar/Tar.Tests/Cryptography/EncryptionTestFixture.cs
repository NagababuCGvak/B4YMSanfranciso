using Tar.Core;
using Tar.Cryptography;
using NUnit.Framework;

namespace Tar.Tests.Cryptography
{
    [TestFixture]
    public class EncryptionTestFixture
    {
        const string EncryptedPassword = "wS55At/Ys7o=";
        const string Password = "zahir";
        IEncryption _encryption;

        public EncryptionTestFixture()
        {
            ServiceLocator.Reset();
            _encryption = ServiceLocator.Current.Get<IEncryption>();
        }

        [Test, Category("Encryption")]
        public void Encrypt()
        {
            var encryptedText = _encryption.Encrypt(Password);
            Assert.AreEqual(encryptedText, EncryptedPassword);
        }

        [Test, Category("Encryption")]
        public void Decrypt()
        {
            var decryptedText = _encryption.Decrypt(EncryptedPassword);
            Assert.AreEqual(decryptedText, Password);
        }
    }
}
