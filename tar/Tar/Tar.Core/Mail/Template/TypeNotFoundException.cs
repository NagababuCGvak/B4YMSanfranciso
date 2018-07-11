using System;

namespace Tar.Core.Mail.Template
{
    public class TypeNotFoundException : Exception
    {
        public TypeNotFoundException(string typeName)
            : base(string.Format("Type not found. TypeName: {0}", typeName))
        {
        }

        public TypeNotFoundException(string typeName, Exception innerException)
            : base(string.Format("Type not found. TypeName: {0}", typeName), innerException)
        {
        }
    }
}