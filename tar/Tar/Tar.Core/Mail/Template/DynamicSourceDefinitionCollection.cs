using System.Collections.ObjectModel;

namespace Tar.Core.Mail.Template
{
    public class DynamicSourceDefinitionCollection : KeyedCollection<string, DynamicSourceDefinition>
    {
        protected override string GetKeyForItem(DynamicSourceDefinition item)
        {
            return item.Key;
        }
    }
 }
