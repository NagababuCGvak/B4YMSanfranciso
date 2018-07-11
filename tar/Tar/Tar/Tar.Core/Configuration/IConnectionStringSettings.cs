namespace Tar.Core.Configuration
{
    public interface IConnectionStringsSettings : ISettings
    {
        string Decrypt(string encryptedConnectionString);
        string Encrypt(string connectionString);
    }
}