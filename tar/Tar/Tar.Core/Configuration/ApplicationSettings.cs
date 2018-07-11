using System;

namespace Tar.Core.Configuration
{
    public class ApplicationSettings : Settings, IApplicationSettings
    {
        public ApplicationSettings() : 
            base("Application")
        {
        }
        public int CompanyId { get { return GetSetting<int>("CompanyId"); } }
    }
}