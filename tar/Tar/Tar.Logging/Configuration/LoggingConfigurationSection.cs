using System.Configuration;

namespace Tar.Logging.Configuration
{
    public class LoggingConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("loggers", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
        public LoggerConfigurationElementCollection Loggers
        {
            get { return base["loggers"] as LoggerConfigurationElementCollection; }
            set { base["loggers"] = value; }
        }

        [ConfigurationProperty("repositories", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
        public RepositoryConfigurationElementCollection Repositories
        {
            get { return base["repositories"] as RepositoryConfigurationElementCollection; }
            set { base["repositories"] = value; }
        }

        [ConfigurationProperty("defaultLogger", IsRequired = false, DefaultValue = "defaultLogger")]
        public string DefaultLogger
        {
            get { return base["defaultLogger"] as string; }
            set { base["defaultLogger"] = value; }
        }

        public LoggingConfigurationSection()
        {
            Loggers = new LoggerConfigurationElementCollection();
        }
    }
}
