using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Tar.Core.Template;

namespace Tar.Core.Mail.Template
{
    public class MailTemplateManager : IMailTemplateManager
    {
        private readonly ITemplateEngine _templateEngine;
        private readonly IMailTemplateRepository _mailTemplateRepository;
        private readonly IMailService _mailService;
        public IServiceLocator ServiceLocator { get; set; }

        public MailTemplate CreateMailTemplate(Func<IMailTemplateBuilder, IMailTemplateBuilder> func)
        {
            return func(ServiceLocator.Get<IMailTemplateBuilder>()).Build();
        }

        public MailTemplateManager(ITemplateEngine templateEngine, IMailTemplateRepository mailTemplateRepository, IMailService mailService)
        {
            _templateEngine = templateEngine;
            _mailTemplateRepository = mailTemplateRepository;
            _mailService = mailService;
        }

        public MailTemplate GetMailTemplate(string key)
        {
            return _mailTemplateRepository.GetMailTemplate(key);
        }

        public void SendMail(string key, IDictionary<string, object> parameters, bool useDynamicSource, string[] to, string[] cc, string[] bcc)
        {
            var message = GenerateMessage(key, parameters, useDynamicSource, to, cc, bcc);
            _mailService.SendMail(message);
        }

        public MailMessage GenerateMessage(string key, IDictionary<string, object> parameters, bool useDynamicSource, string[] to, string[] cc, string[] bcc)
        {
            var template = GetMailTemplate(key);
            var source = new Dictionary<string, object>();

            if (useDynamicSource)
            {
                if (template.DynamicSourcesDefinition.Count == 0) throw new DynamicSourceDefinitionNotFoundException();

                template.DynamicSourcesDefinition.ToList()
                    .ForEach(i =>
                    {
                        var type = Type.GetType(i.TypeName);
                        if (type == null) throw new TypeNotFoundException(i.TypeName);
                        object instance = null;
                        try
                        {
                            instance = ServiceLocator.Get(type);
                        }
                        catch
                        {
                            instance = Activator.CreateInstance(type);
                        }
                        if (instance==null)
                            throw new Exception("Not initialized instance. TypeName: " + i.TypeName);

                        var method = type.GetMethod(i.MethodName);

                        var inputParameters = method.GetParameters();

                        if (inputParameters.Length == 0)
                        {
                            var methodResult = method.Invoke(instance, new object[] { });
                            template.StaticSources.Add(i.Key, methodResult);
                        }
                        else if (inputParameters.Length == 1 &&
                                 inputParameters.First().ParameterType.
                                     Name.Contains("Request"))
                        {
                            var methodParameter = inputParameters.First();

                            var request = Activator.CreateInstance(methodParameter.ParameterType);

                            var properties = methodParameter.ParameterType.GetProperties()
                                .Where(p => parameters.ContainsKey(p.Name))
                                .ToList();

                            properties.ForEach(p => p.SetValue(request, parameters[p.Name], null));

                            var methodResult = method.Invoke(instance, new[] { request });
                            source.Add(i.Key, methodResult);
                        }
                        else
                        {
                            var methodResult = method.Invoke(instance, parameters.Values.ToArray());
                            source.Add(i.Key, methodResult);
                        }
                    });
            }
            else
            {
                parameters.ToList().ForEach(item => source.Add(item.Key, item.Value));
            }

            var subject = _templateEngine.Merge(template.Subject, source);

            var body = _templateEngine.Merge(template.Body, source);

            if (!string.IsNullOrEmpty(template.Signature))
                body += "<br/>" + _templateEngine.Merge(template.Signature, source);


            if (string.IsNullOrEmpty(body))
                body = string.Format("TemplateKey:{0}", key);

            if (string.IsNullOrEmpty(subject))
                subject = string.Format("TemplateKey:{0}", key);

            var message = new MailMessage
                              {
                                  Body = body,
                                  Subject = subject,
                                  IsBodyHtml = true,
                                  BodyEncoding = Encoding.UTF8,
                                  SubjectEncoding = Encoding.UTF8
                              };

            if (to != null && to.Length > 0) to.ToList().ForEach(message.To.Add);
            if (cc != null && cc.Length > 0) cc.ToList().ForEach(message.CC.Add);
            if (bcc != null && bcc.Length > 0) bcc.ToList().ForEach(message.Bcc.Add);

            if (template.To.Count > 0) template.To.ForEach(message.To.Add);
            if (template.Cc.Count > 0) template.Cc.ForEach(message.CC.Add);
            if (template.Bcc.Count > 0) template.Bcc.ForEach(message.Bcc.Add);
            return message;
        }

        public void SendMail(SendMailParameter parameter)
        {
            SendMail(parameter.Key,
                     parameter.Parameters,
                     parameter.UseDynamicSource,
                     parameter.To.ToArray(),
                     parameter.Cc.ToArray(),
                     parameter.Bcc.ToArray());
        }

        public MailMessage GenerateMessage(SendMailParameter parameter)
        {
            return GenerateMessage(parameter.Key,
                                 parameter.Parameters,
                                 parameter.UseDynamicSource,
                                 parameter.To.ToArray(),
                                 parameter.Cc.ToArray(),
                                 parameter.Bcc.ToArray());
        }

        //public void SendMailOld(string key, IDictionary<string, object> parameters, bool useDynamicSource, string[] to, string[] cc, string[] bcc)
        //{
        //    var template = GetMailTemplate(key);
        //    string subject;
        //    string body;

        //    if (useDynamicSource)
        //    {
        //        if (template.DynamicSourcesDefinition.Count == 0) throw new DynamicSourceDefinitionNotFoundException();

        //        template.DynamicSourcesDefinition.ToList()
        //            .ForEach(i =>
        //                         {
        //                             var type = Type.GetType(i.TypeName);
        //                             if (type == null) throw new TypeNotFoundException(i.TypeName);

        //                             var instance = Activator.CreateInstance(type);

        //                             var method = type.GetMethod(i.MethodName);

        //                             var inputParameters = method.GetParameters();

        //                             if (inputParameters.Length == 0)
        //                             {
        //                                 var methodResult = method.Invoke(instance, new object[] {});
        //                                 template.StaticSources.Add(i.Key, methodResult);
        //                             }
        //                             else if (inputParameters.Length == 1 &&
        //                                      inputParameters.First().ParameterType.
        //                                          Name.Contains("Request"))
        //                             {
        //                                 var methodParameter = inputParameters.First();

        //                                 var request = Activator.CreateInstance(methodParameter.ParameterType);

        //                                 var properties = methodParameter.ParameterType.GetProperties()
        //                                     .Where(p => parameters.ContainsKey(p.Name))
        //                                     .ToList();

        //                                 properties.ForEach(p => p.SetValue(request, parameters[p.Name], null));

        //                                 var methodResult = method.Invoke(instance, new[] {request});
        //                                 template.StaticSources.Add(i.Key, methodResult);
        //                             }
        //                             else
        //                             {
        //                                 var methodResult = method.Invoke(instance, parameters.Values.ToArray());
        //                                 template.StaticSources.Add(i.Key, methodResult);
        //                             }

        //                         });

        //        subject = _templateEngine.Merge(template.Subject, template.StaticSources);

        //        body = _templateEngine.Merge(template.Body, template.StaticSources);

        //        if (!string.IsNullOrEmpty(template.Signature))
        //            body += _templateEngine.Merge(template.Signature, template.StaticSources);
        //    }
        //    else
        //    {
        //        subject = _templateEngine.Merge(template.Subject, parameters);

        //        body = _templateEngine.Merge(template.Body, parameters);

        //        if (!string.IsNullOrEmpty(template.Signature))
        //            body += _templateEngine.Merge(template.Signature, parameters);
        //    }

        //    if (string.IsNullOrEmpty(body))
        //        body = string.Format("TemplateKey:{0}", key);

        //    if (string.IsNullOrEmpty(subject))
        //        subject = string.Format("TemplateKey:{0}", key);

        //    var message = new MailMessage { Body = body, Subject = subject };

        //    if (to != null && to.Length > 0) to.ToList().ForEach(message.To.Add);
        //    if (cc != null && cc.Length > 0) cc.ToList().ForEach(message.CC.Add);
        //    if (bcc != null && bcc.Length > 0) bcc.ToList().ForEach(message.Bcc.Add);

        //    if (template.To.Count > 0) template.Bcc.ForEach(message.To.Add);
        //    if (template.Cc.Count > 0) template.Cc.ForEach(message.CC.Add);
        //    if (template.Bcc.Count > 0) template.Bcc.ForEach(message.CC.Add);

        //    _mailService.SendMail(message);
        //}
    }
}