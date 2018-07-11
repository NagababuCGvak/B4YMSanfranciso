using System;
using System.Data;
using Tar.Logging.Serialization;

namespace Tar.Logging.Repositories
{
    public class DbLogRepository : LogRepository
    {
        #region Private Fields
        public ILogDbDataAccess DataAccess { get; private set; }

        private IDbConnection _connection;
        private IDbCommand _command;
        #endregion Private Fields

        #region Constructors
        private DbLogRepository(IMessageSerializer serializer, ILogDbDataAccess dataAccess)
            :base(serializer)
        {
            if (dataAccess == null) throw new ArgumentNullException("dataAccess");
            DataAccess = dataAccess;
        }
        public DbLogRepository(string connectionStringName)
            : this(new DefaultMessageSerializer(), new LogDbDataAccess(connectionStringName))
        {
        }
        #endregion Constructors

        #region Implementation of ILogRepository
        public override void DoLog(IWriteToLogParameter parameter)
        {
            OpenConnection();
            DataAccess.SetParameterValue(_command, "Process", parameter.Process);
            DataAccess.SetParameterValue(_command, "Level", parameter.Level);
            DataAccess.SetParameterValue(_command, "Class", parameter.ClassName);
            DataAccess.SetParameterValue(_command, "Assembly", parameter.AssemblyName);
            DataAccess.SetParameterValue(_command, "Message", parameter.SerializedMessage);
            DataAccess.SetParameterValue(_command, "DateTime", parameter.DateTime);
            DataAccess.SetParameterValue(_command, "IsWebApplication", parameter.IsWebApplication);
            DataAccess.SetParameterValue(_command, "ApplicationName", parameter.AppName);
            DataAccess.SetParameterValue(_command, "ApplicationFolder", parameter.AppFolder);
            DataAccess.SetParameterValue(_command, "ActiveUserName", parameter.ActiveUserName);
            DataAccess.SetParameterValue(_command, "BuildMode", parameter.BuildMode);
            DataAccess.SetParameterValue(_command, "ProcessCode", parameter.ProcessCode);
            DataAccess.SetParameterValue(_command, "ScopeLevel", parameter.ScopeLevel);
            DataAccess.SetParameterValue(_command, "IpAddress", parameter.IpAddress);

            _command.ExecuteNonQuery();
            CloseConnection();
        }
        #endregion


        #region Helper Methods
        private void CloseConnection()
        {
            if (_connection != null && _connection.State != ConnectionState.Closed)
                _connection.Close();
        }
        private void OpenConnection()
        {
            if (_connection == null)
            {
                _connection = DataAccess.CreateConnection();

                _command = DataAccess.CreateCommand();
                _command.Connection = _connection;
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "GeneralLogInsert";
                DataAccess.AddParameter(_command, "GeneralLogId", DbType.Int32, ParameterDirection.Output);
                DataAccess.AddParameter(_command, "Process", DbType.String);
                DataAccess.AddParameter(_command, "BuildMode", DbType.String);
                DataAccess.AddParameter(_command, "ActiveUserName", DbType.String);
                DataAccess.AddParameter(_command, "Level", DbType.String);
                DataAccess.AddParameter(_command, "Class", DbType.String);
                DataAccess.AddParameter(_command, "Assembly", DbType.String);
                DataAccess.AddParameter(_command, "DateTime", DbType.Date);
                DataAccess.AddParameter(_command, "Message", DbType.String);
                DataAccess.AddParameter(_command, "IsWebApplication", DbType.Boolean);
                DataAccess.AddParameter(_command, "ApplicationName", DbType.String);
                DataAccess.AddParameter(_command, "ApplicationFolder", DbType.String);
                DataAccess.AddParameter(_command, "ProcessCode", DbType.String);
                DataAccess.AddParameter(_command, "ScopeLevel", DbType.Int32);
                DataAccess.AddParameter(_command, "IpAddress", DbType.String);
            }
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }
        #endregion
    }
}