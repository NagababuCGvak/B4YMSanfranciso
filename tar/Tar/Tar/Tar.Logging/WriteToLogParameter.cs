using System;

namespace Tar.Logging
{
    class WriteToLogParameter : IWriteToLogParameter
    {
        #region Implementation of IWriteToLogParameter
        public string Process { get; set; }
        public string Level { get; set; }
        public string ClassName { get; set; }
        public string AssemblyName { get; set; }
        public string SerializedMessage { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsWebApplication { get; set; }
        public string AppName { get; set; }
        public string AppFolder { get; set; }
        public string ActiveUserName { get; set; }
        public string BuildMode { get; set; }
        public string ProcessCode { get; set; }
        public int ScopeLevel { get; set; }
        public string IpAddress { get; set; }

        #endregion

        #region Implementation of ICloneable
        public object Clone()
        {
            return MemberwiseClone();
        }
        #endregion
    }
}