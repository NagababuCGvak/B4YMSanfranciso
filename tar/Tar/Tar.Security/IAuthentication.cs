namespace Tar.Security
{
    public interface IAuthentication
    {
        CurrentUser SignIn(string userName, string password);
    }
}