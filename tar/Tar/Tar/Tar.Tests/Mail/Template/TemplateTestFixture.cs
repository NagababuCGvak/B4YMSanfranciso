using System;
using Tar.Core;
using Tar.Core.Extensions;
using Tar.Core.Mail.Template;
using Tar.Repository;
using NUnit.Framework;

namespace Tar.Tests.Mail.Template
{
    [TestFixture]
    public class TemplateTestFixture : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            ServiceLocator.Reset();
        }
        [Test]
        public void SaveMailTemplateDefinition()
        {
            var mailTemplateBuilder = Locator.Get<IMailTemplateBuilder>()
                .Subject(
                    "Test.test - Hello $source.FirstName $source.LastName")
                .Body("FirstName:$source.FirstName<br/>LastName: $source.LastName")
                .Signature(
                    "<br/>------------------------<br/>$source.FirstName $source.LastName.ToUpper()")
                .AddDynamicSource("source",
                                  "Tar.Tests.Mail.Template.TestTemplateClass, Tar.Tests",
                                  "TestTemplateMethod");

            var mailTemplate = mailTemplateBuilder.Build();

            using (Locator.Get<IUnitOfWork>())
            {
                var repository = Locator.Get<IMailTemplateRepository>();
                repository.Save("Test.test", mailTemplate);
            }

            using (var uow = Locator.Get<IUnitOfWork>())
            {
                var repository = Locator.Get<IMailTemplateRepository>();
                repository.Save("Test.test", mailTemplate);
                uow.Commit();
            }
        }

        [Test]
        public void SaveSampleMailTemplateDefinition()
        {
            var repository = Locator.Get<IMailTemplateRepository>();
            var template = new MailTemplate();
            template.To.Add("to1@hizmet.web.tr");
            template.To.Add("to2@hizmet.web.tr");
            template.Cc.Add("cc1@hizmet.web.tr");
            template.Cc.Add("cc2@hizmet.web.tr");
            template.Bcc.Add("bcc1@hizmet.web.tr");
            template.Bcc.Add("bcc2@hizmet.web.tr");

            template.Subject = "Test.MailTemplate Mail başlığı";
            template.Body = "Mail başlığı";
            template.Signature = "Mail başlığı";

            template.DynamicSourcesDefinition.Add(new DynamicSourceDefinition
            {
                Key = "listing",
                TypeName = "Tar.Tests.Mail.TestTemplateClass",
                MethodName = "TestTemplateMethod"
            });
            template.DynamicSourcesDefinition.Add(new DynamicSourceDefinition
            {
                Key = "listing2",
                TypeName = "Tar.Tests.Mail.TestTemplateClass",
                MethodName = "TestTemplateMethod2"
            });
            repository.Save("Test.MailTemplate", template);
        }

        [Test]
        public void SendMailWithStaticSource()
        {
            var mailTemplateManager = Locator.Get<IMailTemplateManager>();
            var parameter = new SendMailParameter("Test.test");
            parameter.To.Add("zahirsolak@gmail.com");
            parameter.Parameters.Add("source", new { FirstName = "M. Zahir", LastName = "Solak" });
            parameter.UseDynamicSource = false;

            var message = mailTemplateManager.GenerateMessage(parameter);
            var messageJson = message.ToJson();
            Console.WriteLine(messageJson);

            mailTemplateManager.SendMail(parameter);
        }

        [Test]
        public void SendMailWithDynamicSource()
        {
            var mailTemplateManager = Locator.Get<IMailTemplateManager>();

            var parameter = new SendMailParameter("Test.test");
            parameter.To.Add("zahirsolak@gmail.com");
            parameter.Parameters.Add("a", "a");
            parameter.Parameters.Add("b", "b");
            parameter.UseDynamicSource = true;

            var message = mailTemplateManager.GenerateMessage(parameter);
            var messageJson = message.ToJson();
            Console.WriteLine(messageJson);

            mailTemplateManager.SendMail(parameter);
        }
    }

    public class TestBase
    {
        protected IServiceLocator Locator { get; private set; }
        public TestBase()
        {
            Locator = ServiceLocator.Current;
            Locator.Get<IUnitOfWork>().Dispose();
        }

    }
}
