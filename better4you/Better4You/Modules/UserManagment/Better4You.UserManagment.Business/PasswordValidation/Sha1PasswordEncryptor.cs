using System.Security.Cryptography;

namespace Better4You.UserManagment.Business.PasswordValidation
{
    public class Sha1PasswordEncryptor : HashAlgorithmPasswordEncryptor
    {
        public Sha1PasswordEncryptor()
            : base(new SHA1CryptoServiceProvider())
        {
        }
    }
}