namespace Better4You.UserManagment.Business.PasswordValidation
{
    public class PlainTextPasswordEncryptor : IPasswordEncryptor
    {
        public string Encrypt(string input)
        {
            return input;
        }
    }
}