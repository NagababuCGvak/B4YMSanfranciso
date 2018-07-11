namespace Tar.Core.Mail
{
    public class LocalhostMailService : StandartMailService
    {
        public LocalhostMailService(string mailAddress, string password, int port, bool enableSsl)
            : base("localhost", port, enableSsl, mailAddress, password)
        {
        }
    }
}