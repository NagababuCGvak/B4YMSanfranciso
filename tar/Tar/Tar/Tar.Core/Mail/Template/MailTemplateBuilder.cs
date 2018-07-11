namespace Tar.Core.Mail.Template
{
    class MailTemplateBuilder : IMailTemplateBuilder
    {
        private MailTemplate _mailTemplate;

        public MailTemplateBuilder()
        {
            CreateMailTemplate();
        }

        private void CreateMailTemplate()
        {
            _mailTemplate = new MailTemplate();
        }

        public IMailTemplateBuilder From(string mailAddress)
        {
            _mailTemplate.From = mailAddress;
            return this;
        }

        public IMailTemplateBuilder To(params string[] mailAddresses)
        {
            _mailTemplate.To.AddRange(mailAddresses);
            return this;
        }

        public IMailTemplateBuilder Cc(params string[] mailAddresses)
        {
            _mailTemplate.Cc.AddRange(mailAddresses);
            return this;
        }

        public IMailTemplateBuilder Bcc(params string[] mailAddresses)
        {
            _mailTemplate.Bcc.AddRange(mailAddresses);
            return this;
        }

        public IMailTemplateBuilder Subject(string subject)
        {
            _mailTemplate.Subject = subject;
            return this;
        }

        public IMailTemplateBuilder Body(string body)
        {
            _mailTemplate.Body = body;
            return this;
        }

        public IMailTemplateBuilder Signature(string signature)
        {
            _mailTemplate.Signature = signature;
            return this;
        }

        public MailTemplate Build()
        {
            var tmp = _mailTemplate;
            CreateMailTemplate();
            return tmp;
        }
        
        public IMailTemplateBuilder AddStaticSource(string key, object value)
        {
            _mailTemplate.StaticSources.Add(key, value);
            return this;
        }

        public IMailTemplateBuilder AddDynamicSource(string key, string typeName, string methodName)
        {
            _mailTemplate.DynamicSourcesDefinition.Add(new DynamicSourceDefinition {Key = key, TypeName = typeName, MethodName = methodName});
            return this;
        }

        //public IMailTemplateBuilder UseDynamicSource(string typeName, string methodName, params object[] parameters)
        //{
        //    if (typeName == null) throw new ArgumentNullException("typeName");

        //    var type = Type.GetType(typeName);

        //    if (type == null) throw new TypeNotFoundException(typeName);

        //    var methodInfo = type.GetMethod(methodName);
        //    var instance = Activator.CreateInstance(type);
        //    var source = methodInfo.Invoke(instance, parameters);
        //    _source.Add("source", source);
        //    return this;
        //}

        //public IMailTemplateBuilder UseDynamicSource(string typeName, string methodName, object parameters)
        //{
        //    if (typeName == null) throw new ArgumentNullException("typeName");

        //    var type = Type.GetType(typeName);

        //    if (type == null) throw new TypeNotFoundException(typeName);

        //    var methodInfo = type.GetMethod(methodName);
        //    var instance = Activator.CreateInstance(type);
        //    var methodParameters = methodInfo.GetParameters();

        //    if (methodParameters.Length > 0 && parameters != null)
        //    {
        //        var methodParameter = methodParameters.First();
        //        var prm = Activator.CreateInstance(methodParameter.ParameterType);

        //        var properties = methodParameter.ParameterType.GetProperties().Where(
        //            p => parameters.GetType().GetProperty(p.Name) != null)
        //            .ToList();

        //        properties.ForEach(
        //            p => p.SetValue(prm, parameters.GetType().GetProperty(p.Name).GetValue(parameters, null), null));

        //        var source = methodInfo.Invoke(instance, new[] {prm});
        //        _source.Add("source", source);
        //    }
        //    else
        //    {
        //        var source = methodInfo.Invoke(instance, null);
        //        _source.Add("source", source);
        //    }

        //    return this;
        //}
    }
}