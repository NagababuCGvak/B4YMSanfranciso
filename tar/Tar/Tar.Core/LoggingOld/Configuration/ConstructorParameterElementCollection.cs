using System.Configuration;

namespace Tar.Core.LoggingOld.Configuration
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