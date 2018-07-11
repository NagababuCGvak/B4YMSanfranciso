using System;
using System.Net.Mail;
using Tar.Core;
using Tar.Core.Mail;
using NUnit.Framework;

namespace Tar.Tests.Mail
{
    [TestFixture]
    public class MailTests
    {
        [SetUp]
        public void SetUp()
        {
            ServiceLocator.Reset();
        }
        [Test]
        public void TestOne()
        {
            var mailService = ServiceLocator.Current.Get<IMailService>();
            var message = new MailMessage
                              {
                                  Subject = "Mail başlığı - " + Guid.NewGuid().ToString().Substring(0, 10),
                                  Body = DateTime.Now + " Mail içeriği - " + Guid.NewGuid().ToString().Substring(0, 15)
                              };
            message.To.Add("zahirsolak@gmail.com");
            message.To.Add("test@finansanalizi.com");
            //message.To.Add("ozguryamak@gmail.com");
            //message.To.Add("tuncs@hotmail.com");
            //message.From = new MailAddress("support@unitedcountry.com");
            mailService.SendMail(message);
        }
    }
}
