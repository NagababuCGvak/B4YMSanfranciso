using System;
using System.Configuration;

namespace Tar.Logging.Configuration
{
    public class RepositoryConfigurationElement : TypeConfigurationElement
    {

        [ConfigurationProperty("nextRepository", IsRequired = false)]
        public string NextRepository
        {
            get { return base["nextRepository"] as string; }
            set { base["nextRepository"] = value; }
        }
    }
}