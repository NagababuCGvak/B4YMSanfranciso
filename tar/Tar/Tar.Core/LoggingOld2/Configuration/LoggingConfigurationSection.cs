using System.Configuration;

namespace Tar.Core.LoggingOld2.Configuration
{
    public class LoggingConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("default", IsRequired = false, DefaultValue = "default")]
        public string Default
        {
            get { return base["default"] as string; }
            set { base["default"] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
        public LoggerConfigurationElementCollection Loggers
        {
            get { return base[""] as LoggerConfigurationElementCollection; }
            set { base[""] = value; }
         }

        public LoggingConfigurationSection()
        {
            Loggers = new LoggerConfigurationElementCollection();
        }
    }
}
