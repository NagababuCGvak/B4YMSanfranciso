using System;
using Tar.Logging.Serialization;

namespace Tar.Logging.Repositories
{
    public abstract class LogRepository : ILogRepository
    {
        private readonly IMessageSerializer _serializer;

        protected LogRepository(IMessageSerializer serializer)
        {
            if (serializer == null) throw new ArgumentNullException("serializer");
            _serializer = serializer;
        }
        #region Implementation of ILogRepository

        public ILogRepository NextRepository { get; set; }
        public void WriteToLog(IWriteToLogParameter parameter)
        {
            try
            {
                DoLog(parameter);
            }
            catch (Exception exception)
            {
                if (NextRepository != null)
                {
                    var tmpParameter = (IWriteToLogParameter)parameter.Clone();
                    tmpParameter.SerializedMessage =
                        _serializer.Serialize(
                            new
                            {
                                Message = "Cannot write to log!",
                                RepositoryType = GetType().FullName,
                                Exception = exception.ToString()
                            });
                    NextRepository.WriteToLog(tmpParameter);
                    NextRepository.WriteToLog(parameter);
                    return;
                }
                throw;
            }
        }

        public abstract void DoLog(IWriteToLogParameter parameter);
        #endregion
    }
}
