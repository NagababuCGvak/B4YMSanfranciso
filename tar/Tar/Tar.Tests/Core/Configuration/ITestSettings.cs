using System;
using Tar.Core.Configuration;

namespace Tar.Tests.Core.Configuration
{
    public interface ITestSettings : ISettings
    {
        bool IsOnline{ get;}
        int Integer { get; }
        Guid Guid { get; }
    }
}