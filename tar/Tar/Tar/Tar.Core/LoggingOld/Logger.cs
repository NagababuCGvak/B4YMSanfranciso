using System;

namespace Tar.Core.LoggingOld
{
    public abstract class Logger : ILogger
    {
        private string _category;
        private ILogger _nextLogger;

        public virtual void Dispose()
        {
            Log(_category, LogType.Info, "{0}.Dispose()", GetType().Name);
        }

        protected Logger()
            : this("Default")
        {
        }

        protected Logger(string category)
        {
            if (category == null) throw new ArgumentNullException("category");
            Category(category);
        }

        protected abstract void SendToLog(string source, LogType logType, string message);

        public ILogger Log(string source, LogType logType, string message, params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
                message = string.Format(message, parameters);
            Log(source, logType, message);
            return this;
        }

        public string Category()
        {
            return _category;
        }

        public ILogger Category(string categoryName)
        {
            _category = categoryName;
            return this;
        }

        public ILogger Log(string source, LogType logType, string message)
        {
            try
            {
                SendToLog(source, logType, message);
            }
            catch (Exception exception)
            {
                if (NextLogger() != null)
                {
                    NextLogger().Error<Logger>(
                        "Loglama işlemi sırasında hata oluştu! Excetion.Message:{0}, Exception.StackTrace:{1}",
                        exception.Message, exception.StackTrace);
                    NextLogger().Log(source, logType, message);
                }
                else
                    throw;
            }
            return this;
        }

        public ILogger NextLogger()
        {
            return _nextLogger;
        }
        public ILogger NextLogger(ILogger logger)
        {
            _nextLogger = logger;
            return this;
        }
    }
}
