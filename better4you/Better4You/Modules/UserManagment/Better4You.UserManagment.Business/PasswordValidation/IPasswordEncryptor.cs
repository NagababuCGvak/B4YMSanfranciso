namespace Better4You.UserManagment.Business.PasswordValidation
{
    public interface IPasswordEncryptor
    {
        string Encrypt(string input);
    }
}