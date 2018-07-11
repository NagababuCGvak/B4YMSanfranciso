using System;
using System.Configuration;

namespace Tar.Logging.Configuration
{
    public class TypeConfigurationElement : ConfigurationElement
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

        [ConfigurationProperty("constructorParameters", IsDefaultCollection = true, IsKey = false, IsRequired = false)]
        public ConstructorParameterElementCollection ConstructorParameters
        {
            get { return this["constructorParameters"] as ConstructorParameterElementCollection; }
            set { this["constructorParameters"] = value; }
        }
    }
}