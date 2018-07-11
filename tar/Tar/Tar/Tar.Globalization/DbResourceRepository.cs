using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace Tar.Globalization
{
    public class DbResourceRepository : IResourceRepository
    {
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly ConnectionStringSettings _connectionString;

        public DbResourceRepository(string connectionStringName)
        {
            if (connectionStringName == null) throw new ArgumentNullException("connectionStringName");
            _connectionString = ConfigurationManager.ConnectionStrings[connectionStringName];
            if(_connectionString==null)
                throw new SystemException("Not found connection string. ConnectionStringName:" + connectionStringName);
            _dbProviderFactory = DbProviderFactories.GetFactory(_connectionString.ProviderName);
        }

        public string GetResource(string resourceLanguage, string resourceType, string resourceKey)
        {
            using (var dbConnection = _dbProviderFactory.CreateConnection())
            {
                Debug.Assert(dbConnection != null, "dbConnection != null"); 
                
                dbConnection.ConnectionString = _connectionString.ConnectionString;
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandType = CommandType.Text;

                    dbCommand.CommandText = " SELECT TOP 1 RESOURCEVALUE from V_RESOURCE " +
                                            " WHERE RESOURCEKEY=@RESOURCEKEY " +
                                            " AND RESOURCETYPE=@RESOURCETYPE " +
                                            " AND (RESOURCELANGUAGE=@RESOURCELANGUAGE " +
                                            "       OR RESOURCELANGUAGE IS NULL )" +
                                            " ORDER BY RESOURCELANGUAGE DESC ";

                    var prmLanguage = dbCommand.CreateParameter();
                    prmLanguage.ParameterName = "@RESOURCELANGUAGE";
                    prmLanguage.Value = resourceLanguage;
                    dbCommand.Parameters.Add(prmLanguage);

                    var prmType = dbCommand.CreateParameter();
                    prmType.ParameterName = "@RESOURCETYPE";
                    prmType.Value = resourceType;
                    dbCommand.Parameters.Add(prmType);

                    var prmKey = dbCommand.CreateParameter();
                    prmKey.ParameterName = "@RESOURCEKEY";
                    prmKey.Value = resourceKey;
                    dbCommand.Parameters.Add(prmKey);

                    var value = dbCommand.ExecuteScalar();

                    return value != null
                               ? value.ToString()
                               : NullResourceRepository.Instance
                                     .GetResource(resourceLanguage, resourceType, resourceKey);
                }
            }
        }
    }
}