using System;

namespace Better4You.UserManagment.Business.PasswordValidation
{
    public class PasswordInfo
    {
        public string Password { get; private set; }

        public string EncryptedPassword { get; private set; }

        public string Format { get; private set; }

        public PasswordInfo(string format, string password, string encryptedPassword)
        {
            if (format == null) throw new ArgumentNullException("format");
            if (password == null) throw new ArgumentNullException("password");
            if (encryptedPassword == null) throw new ArgumentNullException("encryptedPassword");
            EncryptedPassword = encryptedPassword;
            Password = password;
            Format = format;
        }
    }
}