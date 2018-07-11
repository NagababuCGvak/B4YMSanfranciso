using System.Configuration;

namespace Tar.Logging.Configuration
{
    public class LoggerConfigurationElement : TypeConfigurationElement
    {
        [ConfigurationProperty("repository", IsRequired = false, DefaultValue = "repository")]
        public string Repository
        {
            get { return base["repository"] as string; }
            set { base["repository"] = value; }
        }
    }
}