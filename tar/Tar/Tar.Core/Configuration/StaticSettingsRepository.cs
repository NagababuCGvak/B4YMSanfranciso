using System;
using System.Collections.Generic;

namespace Tar.Core.Configuration
{
    public class StaticSettingsRepository : ISettingsRepository
    {
        private readonly IDictionary<string, IDictionary<string, string>> _source;

        public string GetSetting(string groupKey, string key)
        {
            var settingGroup = _source[groupKey];
            var setting = settingGroup[key];
            return setting;
        }

        public bool Contains(string groupKey, string key)
        {
            return _source[groupKey].ContainsKey(key);
        }

        public void SetSetting(string groupKey, string key, string value)
        {
            _source[groupKey][key] = value;
        }

        public StaticSettingsRepository(IDictionary<string, IDictionary<string, string>> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            _source = source;
        }
    }
}