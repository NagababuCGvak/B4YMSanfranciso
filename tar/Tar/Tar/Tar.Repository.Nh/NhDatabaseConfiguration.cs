using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tar.Repository.Nh
{
    public class NhDatabaseConfiguration : INhDatabaseConfiguration
    {
        private readonly IDictionary<MappingType, IEnumerable<string>> _mappings;

        public string ScriptFolder { get; private set; }

        public bool AllowChangeSchema { get; private set; }

        public bool AllowInstall
        {
            get { return AllowChangeSchema && !IsDatabaseInitilized(); }
        }

        public NhDatabaseConfiguration(
            IDictionary<MappingType, IEnumerable<string>> mappings,
            string scriptFolder,
            bool allowChangeSchema)
        {
            if (scriptFolder == null) throw new ArgumentNullException("scriptFolder");
            _mappings = mappings;
            AllowChangeSchema = allowChangeSchema;

            ScriptFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, scriptFolder);
        }

        public IList<string> GetMappings(MappingType mappingType)
        {
            return _mappings.ContainsKey(mappingType) ? _mappings[mappingType].ToList() : new List<string>();
        }

        public IList<string> GetScriptFiles()
        {
            return Directory.GetFiles(ScriptFolder, "*.sql").ToList();
        }

        public bool IsDatabaseInitilized()
        {
            return File.Exists(ScriptFilePath);
        }
        public string ScriptFilePath { get { return Path.Combine(ScriptFolder, "installed.sql"); } }
    }
}
