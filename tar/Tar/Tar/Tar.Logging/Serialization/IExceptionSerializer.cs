using System;

namespace Tar.Logging.Serialization
{
    /// <summary/>
    public interface IExceptionSerializer
    {
        /// <summary/>
        string Serialize<T>(T exception) where T : Exception;
    }
}