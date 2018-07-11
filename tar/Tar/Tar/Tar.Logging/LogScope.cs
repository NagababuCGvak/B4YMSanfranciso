using System;

namespace Tar.Logging
{
    public class LogScope : ILogScope
    {
        public string ProcessName { get; private set; }
        private readonly string _oldProcessName;
        private readonly ILogger _logger;
        private readonly bool _mainScope;
        private static int _scopeLevel;
        
        public LogScope(string processName)
            : this(processName, LogMan.GetLogger())
        {
        }

        public LogScope(string processName,ILogger logger)
        {
            ProcessName = processName;
            _logger = logger;
            _oldProcessName = _logger.Process();

            _scopeLevel++;
            _logger.ScopeLevel(_scopeLevel);

            _logger.Process(processName);

            if (_logger.ProcessCode() == "null")
                _mainScope = true;

            if (_mainScope)
                _logger.ProcessCode(Guid.NewGuid().ToString());

            _logger.Write(typeof (LogScope), LogLevel.Scope, string.Format("{0} - {1}", ProcessName, "Start"));
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            _logger.Write(typeof(LogScope), LogLevel.Scope, string.Format("{0} - {1}", ProcessName, "End"));
            _scopeLevel--;
            _logger.ScopeLevel(_scopeLevel);
            _logger.Process(_oldProcessName);
            if (_mainScope)
            {
                _logger.ProcessCode(null);
            }
        }
        #endregion
    }
}