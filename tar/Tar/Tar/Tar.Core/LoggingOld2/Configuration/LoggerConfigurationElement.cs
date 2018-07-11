using System;
using System.Configuration;

namespace Tar.Core.LoggingOld2.Configuration
{
    public class LoggerConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return Convert.ToString(this["name"]); }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return Convert.ToString(this["type"]); }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("nextLogger", IsRequired = false)]
        public string NextLogger
        {
            get { return Convert.ToString(this["nextLogger"]); }
            set { this["nextLogger"] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
        public ConstructorParameterElementCollection ConstructorParameters
        {
            get { return this[""] as ConstructorParameterElementCollection; }
            set { this[""] = value; }
        }
    }
}