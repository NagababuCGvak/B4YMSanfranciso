using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Tar.Core.LoggingOld2.Serialization;

namespace Tar.Core.LoggingOld2
{
    class DbLogConfiguration : ILogConfiguration
    {
        
        #region Private Fields
        public string ConnectionStringName { get; private set; }
        private readonly Dictionary<string, string> _settings;
        //private const string LevelConfigKey = "Rhapsody.Communication.Logging.DbLogConfiguration.LevelConfigs";

        private List<LevelConfig> _configs;
        private DateTime LastReadConfigTime { get; set; }
        private TimeSpan CacheTime = TimeSpan.FromMinutes(5);
        private IEnumerable<LevelConfig> LevelConfigs
        {
            get
            {
                if (LastReadConfigTime != DateTime.MinValue)
                {
                    if (DateTime.Now - LastReadConfigTime < CacheTime)
                        return _configs;
                }
                using (var connection = DataAccess.CreateConnection())
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        var list = new List<LevelConfig>();
                        command.CommandText = "GeneralLogLevelSelect";
                        command.CommandType = CommandType.StoredProcedure;
                        DataAccess.AddParameter(command, "GeneralLogLevelId", DbType.Int32).Value = DBNull.Value;
                        DataAccess.AddParameter(command, "Level", DbType.String).Value = DBNull.Value;
                        DataAccess.AddParameter(command, "IsActive", DbType.Boolean).Value = DBNull.Value;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(LevelConfigFrom(reader));
                            }
                        }
                        _configs = list;
                        LastReadConfigTime = DateTime.Now;
                        return list;
                    }
                }
            }
        }

        protected LevelConfig LevelConfigFrom(IDataReader record)
        {
            if (record == null) throw new ArgumentNullException("record");
            return new LevelConfig
                       {
                           Id = Convert.ToInt32(record["GeneralLogLevelId"]),
                           Level = record["Level"].ToString(),
                           IsActive = Convert.ToBoolean(record["IsActive"])
                       };
        }

        #endregion

        #region Public Properties
        public Dictionary<string, string> Settings
        {
            get { return _settings; }
        }
        public ILogDbDataAccess DataAccess { get; private set; }
        public IMessageSerializer Serializer { get; private set; }
        #endregion Public Properties

        #region Public Methods
        public bool IsActive(string process, Type source, LogLevel level)
        {
            return LevelConfigs.Count(x => x.Level == level.ToString() && x.IsActive) > 0;
        }
        #endregion Public Methods

        #region Constructors
        public DbLogConfiguration(string connectionStringName, IMessageSerializer serializer, ILogDbDataAccess dataAccess)
        {
            if (connectionStringName == null) throw new ArgumentNullException("connectionStringName");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (dataAccess == null) throw new ArgumentNullException("dataAccess");

            DataAccess = dataAccess;
            
            ConnectionStringName = connectionStringName;
            Serializer = serializer;

            _settings = new Dictionary<string, string>();
            _settings["ConnectionStringName"] = connectionStringName;
        }
        #endregion Constructors
    }
}