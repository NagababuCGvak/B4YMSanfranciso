using System;
using System.IO;
using System.Text;
using Commons.Collections;
using NVelocity.Runtime.Resource;
using NVelocity.Runtime.Resource.Loader;

namespace Tar.Core.Template
{
    public class ReturnTemplateNameAsTemplateStreamResourceLoader : ResourceLoader
    {
        public override void Init(ExtendedProperties configuration)
        {
            if(configuration.GetString("input.encoding")!="UTF-8")
                configuration.AddProperty("templateString.encoding", "UTF-8");
        }

        public override Stream GetResourceStream(string templateName)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(templateName));
        }

        public override bool IsSourceModified(Resource resource)
        {
            return false;
        }

        public override long GetLastModified(Resource resource)
        {
            return DateTime.MinValue.Ticks;
        }
    }
}