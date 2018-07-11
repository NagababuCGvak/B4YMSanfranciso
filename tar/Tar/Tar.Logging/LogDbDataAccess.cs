using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace Tar.Logging
{
    class LogDbDataAccess : ILogDbDataAccess
    {
        #region Private Fields
        private readonly ConnectionStringSettings _connectionStringSettings;
        private readonly DbProviderFactory _factory;
        private readonly List<DbConnection> _connections;
        #endregion Private Fields

        #region Implementation of ILogDbDataAccess
        public void SetParameterValue(IDbCommand command, string parameterName, object value)
        {
            var parameter = (DbParameter)command.Parameters[parameterName];
            parameter.Value = value;
        }

        public IDbDataParameter AddParameter(IDbCommand command, string parameterName, DbType dbType)
        {
            var parameter = _factory.CreateParameter();
            Debug.Assert(parameter != null, "parameter != null");
            parameter.ParameterName = parameterName;
            parameter.DbType = dbType;
            command.Parameters.Add(parameter);
            return parameter;
        }

        public IDbDataParameter AddParameter(IDbCommand command, string parameterName, DbType dbType, ParameterDirection direction)
        {
            var prm = AddParameter(command, parameterName, dbType);
            prm.Direction = direction;
            return prm;
        }

        public IDbConnection CreateConnection()
        {
            var connection = _factory.CreateConnection();
            Debug.Assert(connection != null, "connection != null");
            _connections.Add(connection);
            connection.ConnectionString = _connectionStringSettings.ConnectionString;
            return connection;
        }

        public IDbCommand CreateCommand()
        {
            return _factory.CreateCommand();
        }
        #endregion

        #region Constructors
        public LogDbDataAccess(string connectionStringName)
        {
            if (connectionStringName == null) throw new ArgumentNullException("connectionStringName");
            _connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
            _factory = DbProviderFactories.GetFactory(_connectionStringSettings.ProviderName);
            _connections = new List<DbConnection>();
        }
        #endregion Constructors
    }
}