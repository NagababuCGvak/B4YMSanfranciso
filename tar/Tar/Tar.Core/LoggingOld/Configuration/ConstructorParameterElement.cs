using System.Configuration;

namespace Tar.Core.LoggingOld.Configuration
{
    public class ConstructorParameterElement:ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("value")]
        public string Value
        {
            get { return this["value"] as string; }
            set { this["value"] = value; }
        }
    }
}