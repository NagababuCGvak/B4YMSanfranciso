using System;
using Tar.Core;
using Tar.Core.Configuration;
using Tar.Core.DataAnnotations;
using Tar.Repository;
using Tar.Service.Messages;

namespace Tar.Service
{
    public abstract class Service<TServiceInterface, TImplementation>
        where TImplementation : TServiceInterface
    {
        public IServiceLocator Locator { get; set; }
        public event EventHandler<TarServiceOnErrorEventArgs> Error;

        protected virtual void OnError(TarServiceOnErrorEventArgs e)
        {
            var handler = Error;
            if (handler != null) handler(this, e);
        }

        public virtual TResponse Execute<TResponse>(Action<TResponse> func)
            where TResponse : Response, new()
        {
            return Execute(new NullRequest(), func);
        }

        public virtual TResponse Execute<TRequest, TResponse>(TRequest request, Action<TResponse> func, string memberName = "")
            where TRequest : Request, new()
            where TResponse : Response, new()
        {
            var response = new TResponse();
            try
            {
                new DataAnnotationsValidatorManager
                {
                    Settings = Locator.Get<IApplicationSettings>()
                }.Validate(request);
                request.Validate();
                using (var uow = Locator.Get<IUnitOfWork>())
                {
                    func(response);
                    uow.Commit();
                }
                response.Result = Result.Success;
                if (string.IsNullOrEmpty(response.Message))
                    response.Message = "OK";
            }
            catch (Exception exception)
            {
                response.Message = exception.ToString();
                //var exceptionTypeName = exception.GetType().Name;
                //response.Message = exceptionTypeName == "AutoMapperMappingException"
                //    ? "Record not found!"
                //    : exception.ToString();// FullException(exception);
                response.Result = Result.Failed;
                OnError(new TarServiceOnErrorEventArgs(request, response, exception));
            }
            return response;
        }


        private string FullException(Exception exception)
        {
            var text = exception.ToString();
            if (exception.InnerException != null)
                text = text + Environment.NewLine
                       + "InnerException: " + FullException(exception.InnerException);
            return text;
        }
    }
}