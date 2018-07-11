using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using Tar.Core;
using Tar.Core.Configuration;
using Configuration = NHibernate.Cfg.Configuration;

namespace Tar.Repository.Nh
{
    public class NhRepositoryConfiguration : INhRepositoryConfiguration
    {
        public const string ConnectionStringKey = "connection.connection_string";
        public const string ConnectionStringNameKey = "connection.connection_string_name";
        public const string EncryptedKeyFormat = "{0}.Encrypted";
        private readonly IConnectionStringsSettings _connectionStrings;

        private string GenerateEncryptedKey(string key)
        {
            return string.Format(EncryptedKeyFormat, key);
        }

        private ISessionFactory _sessionFactory;
        private readonly INhDatabaseConfiguration _databaseConfiguration;
        private readonly Dictionary<string, string> _parameters;
        public Configuration Configuration { get; private set; }
        public bool Configured { get; private set; }

        public NhRepositoryConfiguration(INhDatabaseConfiguration databaseConfiguration, Dictionary<string,string> parameters,
            IConnectionStringsSettings connectionStrings)
        {
            if (databaseConfiguration == null) throw new ArgumentNullException("databaseConfiguration");
            if (parameters == null) throw new ArgumentNullException("parameters");
            if (connectionStrings == null) throw new ArgumentNullException("connectionStrings");
            _databaseConfiguration = databaseConfiguration;
            _parameters = parameters;
            _connectionStrings = connectionStrings;
        }

        public void Configure()
        {
            if (Configured) return;
            lock (this)
            {
                if (Configured) return;
                OnConfiguring();
                Configuration = new Configuration().DataBaseIntegration(
                    x =>
                    {
#if DEBUG
                        x.LogSqlInConsole = true;
                        x.LogFormattedSql = true;
#endif
                    });

                //var filePath = ConfigFile;
                //filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
                //var cfg = Configuration.Configure(filePath);

                var cfg = Configuration.SetProperties(_parameters);
                Console.WriteLine("NhRepositoryConfiguration Parameters");
                _parameters.ToList().ForEach(x => Console.WriteLine("{0}:{1}", x.Key, x.Value));
                Action<IEnumerable<string>> funcCreateInitialScripts =
                    scriptFileNames =>
                    {
                        if (!scriptFileNames.Any()) return;
                        var scriptBuilder = new StringBuilder();
                        scriptFileNames
                            .ToList().ForEach(
                                scriptFile =>
                                {
                                    using (var streamReader = new StreamReader(scriptFile, Encoding.GetEncoding("iso-8859-9")))
                                    {
                                        scriptBuilder.AppendLine(streamReader.ReadToEnd());
                                    }
                                }
                            );
                        cfg.AddAuxiliaryDatabaseObject(new SimpleAuxiliaryDatabaseObject(scriptBuilder.ToString(), null));
                    };

                Action<IEnumerable<string>> actCodeMappings =
                    assemblyFileNames =>
                    {
                        var modelMapper = new ModelMapper();
                        assemblyFileNames.ToList().ForEach(
                            assebmlyFile => modelMapper.AddMappings(Assembly.Load(assebmlyFile).GetExportedTypes()));

                        var mp = modelMapper.CompileMappingForAllExplicitlyAddedEntities();
                        // For Duplicate mapping
                        mp.autoimport = false;
                        cfg.AddDeserializedMapping(mp, null);
                    };

                Action<IEnumerable<string>> actXmlMappings = assemblyFileNames => assemblyFileNames.ToList().ForEach(
                    assebmlyFile => cfg.AddAssembly(assebmlyFile));

                if (_databaseConfiguration.AllowInstall)
                    funcCreateInitialScripts(_databaseConfiguration.GetScriptFiles());

                var codeMappings = _databaseConfiguration.GetMappings(MappingType.Code);

                if (codeMappings.Any()) actCodeMappings(codeMappings);

                var xmlMappings = _databaseConfiguration.GetMappings(MappingType.Xml);
                if (xmlMappings.Any()) actXmlMappings(xmlMappings);

                SchemaMetadataUpdater.QuoteTableAndColumns(cfg);

                if (_databaseConfiguration.AllowInstall)
                    new SchemaExport(cfg).SetOutputFile(_databaseConfiguration.ScriptFilePath).Create(false, true);
                Configured = true;

                OnConfigured();
            }
        }

        protected virtual void OnConfiguring()
        {
            if (!_parameters.ContainsKey(ConnectionStringNameKey)) return;
            
            var connectionStringName = _parameters[ConnectionStringNameKey];
            _parameters.Remove(ConnectionStringNameKey);

            var encryptedConnectionStringName = GenerateEncryptedKey(connectionStringName);

            if (ConfigurationManager.ConnectionStrings[connectionStringName] != null)
            {
                _parameters[ConnectionStringKey] = ConfigurationManager.ConnectionStrings
                    [connectionStringName].ConnectionString;
            }
            else if (ConfigurationManager.ConnectionStrings[encryptedConnectionStringName] != null)
            {
                _parameters[ConnectionStringKey] = _connectionStrings.Decrypt(ConfigurationManager.ConnectionStrings
                    [encryptedConnectionStringName].ConnectionString);
            }
            else if (_connectionStrings.Contains(connectionStringName))
            {
                _parameters[ConnectionStringKey] = _connectionStrings.GetSetting<string>(connectionStringName);
            }
            else if (_connectionStrings.Contains(encryptedConnectionStringName))
            {
                _parameters[ConnectionStringKey] =
                    _connectionStrings.Decrypt(_connectionStrings.GetSetting<string>(encryptedConnectionStringName));
            }
            else
            {
                throw new Exception(string.Format("{0} called connection string not found!", connectionStringName));
            }
        }

        protected virtual void OnConfigured()
        {
        }
        public void CreateSchema()
        {
            new SchemaExport(Configuration).Execute(true, true, false);
        }

        public ISessionFactory CreateSessionFactory()
        {
            Configure();
            return Configuration.BuildSessionFactory();
        }

        public ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory == null)
            {
                lock (this)
                {
                    if (_sessionFactory == null)
                    {
                        _sessionFactory = CreateSessionFactory();
                    }
                }
            }
            return _sessionFactory;
        }

        protected virtual string ConfigFile
        {
            get { return AppSettings.Get("Repository.Nh.ConfigFile", "Repository.Nh.config"); }
        }
    }
}