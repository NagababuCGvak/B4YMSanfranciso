using System.Configuration;

namespace Tar.Logging.Configuration
{
    public class RepositoryConfigurationElementCollection : ConfigurationElementCollectionBase<RepositoryConfigurationElement>
    {
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RepositoryConfigurationElement)element).Name;
        }

        protected override string ElementName
        {
            get
            {
                return "repository";
            }
        }
    }
}