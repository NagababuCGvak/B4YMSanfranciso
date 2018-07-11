namespace Tar.Security
{
    public interface IAuthenticationProvider
    {
        CurrentUser GetUser(string name, string password);
        CurrentUser GetUser(string name);
    }
}