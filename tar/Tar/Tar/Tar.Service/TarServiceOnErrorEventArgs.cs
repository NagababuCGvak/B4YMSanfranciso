using System;
using Tar.Service.Messages;

namespace Tar.Service
{
    public class TarServiceOnErrorEventArgs: EventArgs
    {
        public Request Request { get; set; }
        public Response Response { get; set; }
        public Exception Exception { get; set; }

        public TarServiceOnErrorEventArgs(Request request, Response response, Exception exception)
        {
            Request = request;
            Response = response;
            Exception = exception;
            if (exception == null) throw new ArgumentNullException("exception");
        }
    }
}