using System;

namespace Tar.Core.LoggingOld2.Serialization
{
    /// <summary/>
    public interface IExceptionSerializer
    {
        /// <summary/>
        string Serialize<T>(T exception) where T : Exception;
    }
}