using System.Data;

namespace Tar.Core.LoggingOld2
{
    interface ILogDbDataAccess
    {
        void SetParameterValue(IDbCommand command, string parameterName, object value);
        IDbDataParameter AddParameter(IDbCommand command, string parameterName, DbType dbType);

        IDbDataParameter AddParameter(IDbCommand command, string parameterName, DbType dbType,
                                      ParameterDirection direction);
        IDbConnection CreateConnection();
        IDbCommand CreateCommand();
    }
}