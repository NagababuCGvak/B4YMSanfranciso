namespace Tar.Core.Configuration
{
    public interface ISettings
    {
        T GetSetting<T>(string key);
        void SetSetting<T>(string key, T value);

        bool Contains(string settingKey);
        string SettingGroupKey { get; set; }
    }
}