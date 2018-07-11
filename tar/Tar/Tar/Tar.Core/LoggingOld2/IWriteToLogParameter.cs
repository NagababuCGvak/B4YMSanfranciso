using System;

namespace Tar.Core.LoggingOld2
{
    public interface IWriteToLogParameter:ICloneable
    {
        string Process { get; set; }
        string Level { get; set; }
        string ClassName { get; set; }
        string AssemblyName { get; set; }
        string SerializedMessage { get; set; }
        DateTime DateTime { get; set; }
        bool IsWebApplication { get; set; }
        string AppName { get; set; }
        string AppFolder { get; set; }
        string ActiveUserName { get; set; }
        string BuildMode { get; set; }
        string ProcessCode { get; set; }
        int ScopeLevel { get; set; }
        string IpAddress { get; set; }
    }
}