using System.Security.Cryptography;

namespace Better4You.UserManagment.Business.PasswordValidation
{
    public class Md5PasswordEncryptor : HashAlgorithmPasswordEncryptor
    {
        public Md5PasswordEncryptor()
            : base(new MD5CryptoServiceProvider())
        {
        }
    }
}