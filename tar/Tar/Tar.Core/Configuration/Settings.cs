using System.ComponentModel;

namespace Tar.Core.Configuration
{
    public class Settings : ISettings
    {
        public ISettingsRepository SettingsRepository { get; set; }

        public virtual T GetSetting<T>(string settingKey)
        {
            SettingsRepository = ServiceLocator.Current.Get<ISettingsRepository>();
            var value = SettingsRepository.GetSetting(SettingGroupKey, settingKey);
            var returnValue = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
            return returnValue;
        }

        public virtual void SetSetting<T>(string key, T value)
        {
            SettingsRepository.SetSetting(SettingGroupKey, key,
                ((object) value) == null
                    ? null
                    : value.ToString());
        }

        public bool Contains(string settingKey)
        {
            return SettingsRepository.Contains(SettingGroupKey, settingKey);
        }

        public string SettingGroupKey { get; set; }

        public Settings(string settingGroupKey)
        {
            SettingGroupKey = settingGroupKey;
        }
    }
}