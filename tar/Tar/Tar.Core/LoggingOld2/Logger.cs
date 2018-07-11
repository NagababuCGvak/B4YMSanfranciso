using System;
using System.Threading;
using System.Web;
using Tar.Core.LoggingOld2.Serialization;

namespace Tar.Core.LoggingOld2
{
    class Logger : ILogger
    {
        #region Private Fields
        private string _process;
        private int _scopeLevel;
        private readonly ILogConfiguration _configuration;
        private readonly ILogRepository _logRepository;
        private readonly IMessageSerializer _messageSerializer;
        private string _processCode;
        #endregion Private Fields

        #region Public Properties
        public virtual ILogConfiguration Configuration
        {
            get { return _configuration; }
        }
        public virtual ILogRepository Repository
        {
            get { return _logRepository; }
        }
        #endregion Properties

        #region Get Methods
        public virtual string Process()
        {
            if (string.IsNullOrEmpty(_process) || _process.ToLowerInvariant().Equals("default"))
                return "Default";
            return _process;
        }

        public virtual int ScopeLevel()
        {
            return _scopeLevel;
        }

        public virtual string ProcessCode()
        {
            if (string.IsNullOrEmpty(_processCode))
                _processCode = "null";
            return _processCode;
        }

        #endregion Get Methods

        #region Set Methods
        public virtual ILogger Process(string process)
        {
            _process = process;
            return this;
        }

        public virtual ILogger ScopeLevel(int scopeLevel)
        {
            _scopeLevel = scopeLevel;
            return this;
        }

        public virtual ILogger ProcessCode(string process)
        {
            _processCode = process;
            return this;
        }
        #endregion Set Methods

        #region Write Method
        public virtual ILogger Write(Type source, LogLevel level, object message)
        {

            var processCode = "";
            var process = "";
            var levelString = "";
            var scopeLevel = 0;
            var className = "";
            var assemblyName = "";
            var dateTime = DateTime.Now;
            var isWebApplication = HttpContext.Current != null;
            var appName = "";
            var appFolder = "";
            var activeUserName = "null";
            var serializedMessage = "";

#if (DEBUG)
            const string buildMode = "DEBUG";
#else
            const string buildMode= "RELEASE";
#endif

            if (!_configuration.IsActive(((ILogger) this).Process(), source, level))
                return this;

            if (message == null)
                message = "null";
            else if (message.ToString() == String.Empty)
                message = "String.Empty";



            if (isWebApplication)
            {
                appName = HttpContext.Current.Request.Url.ToString();
                appFolder = HttpContext.Current.Request.MapPath("/");
            }
            else
            {
                appName = AppDomain.CurrentDomain.FriendlyName;
                appFolder = AppDomain.CurrentDomain.BaseDirectory;
            }

            processCode = ((ILogger) this).ProcessCode();
            process = ((ILogger) this).Process();
            levelString = level.ToString();
            scopeLevel = ((ILogger) this).ScopeLevel();
            className = source.FullName;
            assemblyName = source.Assembly.GetName().Name;

            if (Thread.CurrentPrincipal != null)
                activeUserName = Thread.CurrentPrincipal.Identity.Name;

            if (string.IsNullOrEmpty(activeUserName))
                activeUserName = "null";

            serializedMessage = _messageSerializer.Serialize(message);

            var ipAddress = this.IpAddress();

            
                return WriteToLog(processCode, process, levelString, className, assemblyName,
                                  serializedMessage, dateTime, isWebApplication,
                                  appName, appFolder, activeUserName, buildMode, scopeLevel, ipAddress);
            
        }

        #endregion Write Method

        #region Helper Methods
        protected virtual ILogger WriteToLog(string processCode, string process, string level, string className, string assemblyName,
                                              string serializedMessage, DateTime dateTime, bool isWebApplication,
                                              string appName, string appFolder, string activeUserName, string buildMode, int scopeLevel, string ipAddress)
        {
            var parameter = new WriteToLogParameter
                                {
                                    ActiveUserName = activeUserName,
                                    AppFolder = appFolder,
                                    AppName = appName,
                                    AssemblyName = assemblyName,
                                    BuildMode = buildMode,
                                    ClassName = className,
                                    DateTime = dateTime,
                                    IsWebApplication = isWebApplication,
                                    Level = level,
                                    Process = process,
                                    SerializedMessage = serializedMessage,
                                    ProcessCode = processCode,
                                    ScopeLevel = scopeLevel,
                                    IpAddress = ipAddress
                                };
            
                return WriteToLog(parameter);
        }

        protected virtual ILogger WriteToLog(IWriteToLogParameter parameter)
        {
            _logRepository.WriteToLog(parameter);
            return this;
        }

        #endregion Helper Methods

        #region Constructors
        public Logger(ILogConfiguration configuration, ILogRepository logRepository)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (logRepository == null) throw new ArgumentNullException("logRepository");
            _configuration = configuration;
            _logRepository = logRepository;
            _messageSerializer = configuration.Serializer;
        }
        #endregion Constructors
    }
}