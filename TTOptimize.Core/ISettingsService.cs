namespace TTOptimize.Core;

public interface ISettingsService
{
    bool IsInteractive { get; }

    string? this[string settingType] { get; }

    string? GetSetting(string settingId);
}
