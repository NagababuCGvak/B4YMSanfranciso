using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Tar.Core
{
    public class AppSettings
    {
        public static string ServiceLocatorConfigFile
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                    Get("Core.ServiceLocator.ConfigFile", "Windsor.config"));
            }
        }

        public static string Get(string key)
        {
            if (!ConfigurationManager.AppSettings.AllKeys.Contains(key))
                throw new SystemException(string.Format("Cannot found AppSettings key: [{0}]", key));

            return ConfigurationManager.AppSettings[key];
        }

        public static string Get(string key, string defaultValue)
        {
            return !ConfigurationManager.AppSettings.AllKeys.Contains(key)
                       ? defaultValue
                       : ConfigurationManager.AppSettings[key];
        }
    }
}