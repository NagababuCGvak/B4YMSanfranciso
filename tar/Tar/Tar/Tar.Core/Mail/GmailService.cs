namespace Tar.Core.Mail
{
    public class GmailService : StandartMailService
    {
        public GmailService(string mailAddress, string password)
            : base("smtp.gmail.com", 587, true, mailAddress, password)
        {
        }
    }
}
