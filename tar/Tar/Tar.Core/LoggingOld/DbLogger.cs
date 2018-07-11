using System;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Tar.Core.LoggingOld
{
    /// <summary>
    /// Database'e loglar. Tablo yapısı aşağıdaki gibi olmalıdır.
    /// CREATE  TABLE [dbo].[GeneralLog](
    /// 	[GeneralLogId] [int] IDENTITY(1,1) NOT NULL,
    /// 	[LogCategory] [nvarchar](100) NOT NULL,
    /// 	[LogSource] [nvarchar](300) NOT NULL,
    /// 	[LogType] [nvarchar](20) NOT NULL,
    /// 	[LogDate] [datetime] NOT NULL,
    /// 	[LogMessage] [nvarchar](max) NOT NULL,
    /// CONSTRAINT PK_Log PRIMARY KEY ([LogId] DESC))
    /// go
    /// CREATE INDEX IX_Log_Source_Category_Type ON dbo.Log(LogSource,LogCategory,LogType)
    /// go
    /// </summary>
    public class DbLogger : Logger
    {
        private readonly DbProviderFactory _factory;
        private readonly IDbCommand _command;
        private readonly IDataParameter _prmLogCategory;
        private readonly IDataParameter _prmLogSource;
        private readonly IDataParameter _prmLogDate;
        private readonly IDataParameter _prmLogMessage;
        private readonly IDataParameter _prmLogType;
        
        private readonly string _connectionString;

        public DbLogger(string connectionStringName)
        {
            var settings = ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>()
                .ToList().FirstOrDefault(c => c.Name == connectionStringName);

            if (settings == null)
                throw new SystemException(string.Format("{0} isimli connection string bulunamadı!", connectionStringName));

            if (string.IsNullOrEmpty(settings.ProviderName))
                throw new SystemException(
                    string.Format("{0} isimli connection string tanımınızda ProviderName belirtilmemiş!",
                                  connectionStringName));

            _connectionString = settings.ConnectionString;

            _factory = DbProviderFactories.GetFactory(settings.ProviderName);

            _command = CreateConnection().CreateCommand();
            Debug.Assert(_command != null, "_command != null");

            _command.CommandType = CommandType.Text;
            _command.CommandText = "Insert Into Log(LogCategory,LogSource,LogType,LogDate,LogMessage) Values(@LogCategory,@LogSource,@LogType,@LogDate,@LogMessage);";


            _prmLogCategory = _command.CreateParameter();
            _prmLogCategory.ParameterName = "@LogCategory";
            _prmLogCategory.DbType = DbType.String;

            _prmLogSource = _command.CreateParameter();
            _prmLogSource.ParameterName = "@LogSource";
            _prmLogSource.DbType = DbType.String;

            _prmLogType = _command.CreateParameter();
            _prmLogType.ParameterName = "@LogType";
            _prmLogType.DbType = DbType.String;

            _prmLogDate = _command.CreateParameter();
            _prmLogDate.ParameterName = "@LogDate";
            _prmLogDate.DbType = DbType.DateTime;

            _prmLogMessage = _command.CreateParameter();
            _prmLogMessage.DbType = DbType.String;
            _prmLogMessage.ParameterName = "@LogMessage";

            _command.Parameters.Add(_prmLogCategory);
            _command.Parameters.Add(_prmLogSource);
            _command.Parameters.Add(_prmLogType); 
            _command.Parameters.Add(_prmLogDate);
            _command.Parameters.Add(_prmLogMessage);
        }

        private IDbConnection CreateConnection()
        {
            var connection = _factory.CreateConnection();
            Debug.Assert(connection != null, "connection != null");

            connection.ConnectionString = _connectionString;
            connection.Open();
            return connection;
        }

        protected override void SendToLog(string source, LogType logType, string message)
        {
            _prmLogDate.Value = DateTime.Now;
            _prmLogCategory.Value = Category();
            _prmLogSource.Value = source;
            _prmLogType.Value = logType.ToString();
            _prmLogMessage.Value = message;

            _command.ExecuteNonQuery();
        }
    }
}