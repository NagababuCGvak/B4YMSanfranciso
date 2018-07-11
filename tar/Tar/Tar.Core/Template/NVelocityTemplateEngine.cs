using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime.Resource.Loader;

namespace Tar.Core.Template
{
    public class NVelocityTemplateEngine : ITemplateEngine
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly VelocityEngine _velocity;

        public NVelocityTemplateEngine(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
            _velocity = new VelocityEngine();
            var configuration = new ExtendedProperties();
            configuration.AddProperty("resource.loader", "custom");
            var type = _serviceLocator.Get<ResourceLoader>().GetType();
            var typeName = type.FullName;
            var assemblyName = type.Assembly.GetName().Name;

            configuration.AddProperty("custom.resource.loader.class", string.Format("{0};{1}", typeName, assemblyName));
            configuration.AddProperty("input.encoding", "UTF-8");
            configuration.AddProperty("output.encoding", "UTF-8");
            _velocity.Init(configuration);
        }

        public string Merge(string templateString, IDictionary<string, object> parameters)
        {
            if (templateString == null) throw new ArgumentNullException("templateString");
            if (parameters == null) throw new ArgumentNullException("parameters");

            if (!parameters.ContainsKey("now")) parameters.Add("now", DateTime.Now);
            if (!parameters.ContainsKey("formatter")) parameters.Add("formatter", new NVelocityTemplateEngineFormatter());

            var template = _velocity.GetTemplate(templateString);
            var context = new VelocityContext();
            parameters.ToList()
                .ForEach(item => context.Put(item.Key, item.Value));

            using (var writer = new StringWriter())
            {
                template.Merge(context, writer);
                return writer.ToString();
            }
        }
    }
}