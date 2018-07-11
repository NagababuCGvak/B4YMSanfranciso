using System.Collections.Generic;

namespace Tar.Repository.Nh
{
    public interface INhDatabaseConfiguration
    {
        string ScriptFolder { get; }
        bool AllowChangeSchema { get; }
        bool AllowInstall { get; }
        string ScriptFilePath { get; }
        IList<string> GetMappings(MappingType mappingType);
        IList<string> GetScriptFiles();
        bool IsDatabaseInitilized();
    }
}