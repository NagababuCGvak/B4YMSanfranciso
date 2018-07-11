namespace Tar.Core.Mail
{
    public class MailServiceFactory
    {
        private static IMailService _mailService;

        public static void InitializeEmailServiceFactory(IMailService mailService)
        {
            _mailService = mailService;
        }

        public static IMailService GetEmailService()
        {
            return _mailService;
        }
    }
}
