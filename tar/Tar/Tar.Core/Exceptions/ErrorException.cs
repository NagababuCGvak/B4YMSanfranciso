using System;
using System.Runtime.Serialization;

namespace Tar.Core.Exceptions
{
    [Serializable]
    public class ErrorException : SystemException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ErrorException()
        {
        }

        public ErrorException(string message) : base(message)
        {
        }

        public ErrorException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ErrorException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}