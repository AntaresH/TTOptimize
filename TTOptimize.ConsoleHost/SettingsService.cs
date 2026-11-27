using Microsoft.Extensions.Configuration;
using TTOptimize.Core;

public class SettingsService : ISettingsService
{
    private readonly IConfigurationSection _simulationSection;

    public bool IsSetupInteractive { get; }
    public bool IsGameInteractive { get; }

    public int BatchRuns { get; }

    public string? this[string settingKey]
    {
        get { return _simulationSection[settingKey]; }
    }

    public SettingsService(IConfiguration configuration)
    {
        _simulationSection = configuration.GetSection("Simulation");

        IsSetupInteractive = _simulationSection.GetValue<bool?>("SetupInteractive") ?? true;
        IsGameInteractive = _simulationSection.GetValue<bool?>("GameInteractive") ?? true;

        BatchRuns = _simulationSection.GetValue<int?>("BatchRuns") ?? 1;
    }
}