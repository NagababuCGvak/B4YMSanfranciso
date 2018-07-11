using System.Collections.Generic;

namespace Better4You.UserManagment.Business.PasswordValidation
{
    public class PasswordManager
    {
        private static readonly IDictionary<string, IPasswordEncryptor> ValidatorList = new Dictionary<string, IPasswordEncryptor>();

        public static void Clear()
        {
            ValidatorList.Clear();
        }

        public static void AddEncryptor(string format, IPasswordEncryptor passwordEncryptor)
        {
            ValidatorList.Add(format, passwordEncryptor);
        }

        public static bool IsValid(PasswordInfo info)
        {
            var encryptor = ValidatorList[info.Format];
            var encryptedPassword = encryptor.Encrypt(info.Password);
            return encryptedPassword == info.EncryptedPassword;
        }

        public static string Encrypt(string format, string password)
        {
            return ValidatorList[format].Encrypt(password);
        }
    }
}