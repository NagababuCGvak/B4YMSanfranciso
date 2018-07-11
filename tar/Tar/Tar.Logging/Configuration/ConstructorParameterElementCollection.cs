using System.Configuration;

namespace Tar.Logging.Configuration
{
    public class ConstructorParameterElementCollection : ConfigurationElementCollectionBase<ConstructorParameterElement>
    {
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConstructorParameterElement) element).Name;
        }

        protected override string ElementName
        {
            get
            {
                return "constructorParameter";
            }
        }
    }
}