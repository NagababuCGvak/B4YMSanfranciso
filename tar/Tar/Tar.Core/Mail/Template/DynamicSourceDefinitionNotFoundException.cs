using System;

namespace Tar.Core.Mail.Template
{
    public class DynamicSourceDefinitionNotFoundException : Exception
    {
        public DynamicSourceDefinitionNotFoundException() :
            base("Not found dynamic source definition.")
        {
        }
    }
}