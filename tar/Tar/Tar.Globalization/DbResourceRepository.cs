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
                var isOracle = dbConnection.GetType().Name.Contains("Oracle");
                
                dbConnection.ConnectionString = _connectionString.ConnectionString;
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandType = CommandType.Text;

                    dbCommand.CommandText = string.Format(
                        "SELECT RESOURCEVALUE from V_RESOURCE WHERE RESOURCEKEY='{0}' AND RESOURCETYPE='{1}' AND (RESOURCELANGUAGE='{2}' OR RESOURCELANGUAGE IS NULL ) {3}",
                        resourceKey, resourceType, resourceLanguage,
                        isOracle ? " ORDER BY RESOURCELANGUAGE " : " ORDER BY RESOURCELANGUAGE DESC");

                    var reader = dbCommand.ExecuteReader();
                    string result = null;
                    if (reader.Read())
                        result =reader.GetString(0);
                    return result ?? NullResourceRepository.Instance
                               .GetResource(resourceLanguage, resourceType, resourceKey);
                }
            }
        }
    }
}