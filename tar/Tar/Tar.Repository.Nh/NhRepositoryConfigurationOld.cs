using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Tar.Core;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace Tar.Repository.Nh
{
    public class NhRepositoryConfigurationOld : INhRepositoryConfiguration
    {
        private ISessionFactory _sessionFactory;
        private readonly INhDatabaseConfiguration _databaseConfiguration;
        public Configuration Configuration { get; private set; }        
        public bool Configured { get; private set; }

        public NhRepositoryConfigurationOld(INhDatabaseConfiguration databaseConfiguration)
        {
            if (databaseConfiguration == null) throw new ArgumentNullException("databaseConfiguration");
            _databaseConfiguration = databaseConfiguration;
        }
        
        public void Configure()
        {
            Configuration = new Configuration().DataBaseIntegration(
                x =>
                {
                    #if DEBUG
                    x.LogSqlInConsole = true;
                    x.LogFormattedSql = true;
                    #endif
                });

            var filePath = ConfigFile;
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

            var cfg = Configuration.Configure(filePath);

            Action<IEnumerable<string>> funcCreateInitialScripts =
                scriptFileNames =>
                {
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
        }
        
        public void CreateSchema()
        {
            new SchemaExport(Configuration).Execute(true, true, false);
        }
        
        public ISessionFactory CreateSessionFactory()
        {
            if(!Configured)
                Configure();
            return Configuration.BuildSessionFactory();
        }

        public ISessionFactory GetSessionFactory()
        {
            return _sessionFactory ?? (_sessionFactory = CreateSessionFactory());
        }

        protected virtual string ConfigFile
        {
            get { return AppSettings.Get("Repository.Nh.ConfigFile", "Repository.Nh.config"); }
        }
    }
}
