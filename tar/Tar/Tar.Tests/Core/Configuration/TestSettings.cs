using System;
using Tar.Core.Configuration;

namespace Tar.Tests.Core.Configuration
{
    public class TestSettings : Settings, ITestSettings
    {
        public TestSettings() : 
            base("Test")
        {
        }

        public bool IsOnline
        {
            get { return GetSetting<bool>("IsOnline"); }
        }
        public int Integer
        {
            get { return GetSetting<int>("Integer"); }
        }

        public Guid Guid
        {
            get { return GetSetting<Guid>("Guid"); }
        }

    }
}