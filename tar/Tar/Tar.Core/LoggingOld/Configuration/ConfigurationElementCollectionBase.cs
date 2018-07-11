using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Tar.Core.LoggingOld.Configuration
{
    public abstract class ConfigurationElementCollectionBase<TElement>:ConfigurationElementCollection
        where TElement :ConfigurationElement,new ()
    {
        public List<TElement> ToList()
        {
            return this.Cast<TElement>().ToList();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TElement();
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }
    }
}