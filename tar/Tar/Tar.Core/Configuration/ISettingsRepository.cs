namespace Tar.Core.Configuration
{
    public interface ISettingsRepository
    {
        string GetSetting(string groupKey, string key);
        bool Contains(string groupKey, string key);
        void SetSetting(string groupKey, string key, string value);
    }
}