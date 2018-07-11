using System;
using System.Runtime.Serialization;

namespace Tar.Core.Exceptions
{
    [Serializable]
    public class TarException : ApplicationException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public TarException ()
        {
        }

        public TarException (string message)
            : base(message)
        {
        }

        public TarException (string message, Exception inner)
            : base(message, inner)
        {
        }

        protected TarException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}