namespace TTOptimize.Core;

public interface ISettingsService
{
    string GetSetting(string settingId);

    string this[string settingType] { get; }
}
