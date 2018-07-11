using System;

namespace Tar.Core.LoggingOld2
{
    class LoggerSyntax : ILoggerSyntax
    {
        private ILogConfiguration _config;
        private ILogRepository _repository;
        #region Implementation of ILoggerSyntax

        public ILoggerSyntax Configuration(ILogConfiguration config)
        {
            if (config == null) throw new ArgumentNullException("config");
            _config = config;
            return this;
        }

        public ILogConfiguration Configuration()
        {
            return _config;
        }

        public ILoggerSyntax Repository(ILogRepository repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            _repository = repository;
            return this;
        }

        public ILogRepository Repository()
        {
            return _repository;
        }

        public ILogger Build()
        {
            return new Logger(_config, _repository);
        }
        #endregion
    }
}