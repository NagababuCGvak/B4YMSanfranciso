using System.Data;

namespace Tar.Logging
{
    public interface ILogDbDataAccess
    {
        void SetParameterValue(IDbCommand command, string parameterName, object value);
        IDbDataParameter AddParameter(IDbCommand command, string parameterName, DbType dbType);

        IDbDataParameter AddParameter(IDbCommand command, string parameterName, DbType dbType,
                                      ParameterDirection direction);
        IDbConnection CreateConnection();
        IDbCommand CreateCommand();
    }
}