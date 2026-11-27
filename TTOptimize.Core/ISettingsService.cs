namespace TTOptimize.Core;

public interface ISettingsService
{
    bool IsSetupInteractive { get; }
    bool IsGameInteractive { get; }

    int BatchRuns { get; }

    string? this[string settingType] { get; }
}
