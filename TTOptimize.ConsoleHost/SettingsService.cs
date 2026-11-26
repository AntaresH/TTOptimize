using System.Text.RegularExpressions;
using TTOptimize.Core;

namespace TTOptimize.ConsoleHost;

public class SettingsService : ISettingsService
{
    public bool IsInteractive { get; init; }

    public string? GetSetting(string settingId)
    {
        if (_settings.TryGetValue(settingId.ToLower(), out object settingValue))
        {
            return settingValue.ToString();
        }

        return null;
    }

    public string? this[string settingType]
    {
        get
        {
            return _settings.GetValueOrDefault(settingType)?.ToString();
        }
    }

    private Dictionary<string, object> _settings { get; set; } = new Dictionary<string, object>();

    public SettingsService(string[] cmdLineArgs)
    {
        foreach (var argument in cmdLineArgs)
        {
            Match match = Regex.Match(argument, @"^([A-Za-z0-9]+)=(\w+)$");

            if (match.Success)
            {
                string settingId = match.Groups[1].Value;
                string settingValue = match.Groups[2].Value;

                switch (settingId)
                {
                    case TTOptArguments.GAMEID:
                    case TTOptArguments.PLAYER1NAME:
                    case TTOptArguments.PLAYER2NAME:
                    case TTOptArguments.PLAYER3NAME:
                    case TTOptArguments.PLAYER4NAME:
                        _settings.Add(settingId, settingValue);
                        break;
                    case TTOptArguments.PLAYERCOUNT:
                    case TTOptArguments.PLAYER1TYPE:
                    case TTOptArguments.PLAYER2TYPE:
                    case TTOptArguments.PLAYER3TYPE:
                    case TTOptArguments.PLAYER4TYPE:
                    case TTOptArguments.BATCHRUNS:
                    case TTOptArguments.SEED:
                        if (int.TryParse(settingValue, out int parsedNum))
                        {
                            _settings.Add(settingId, parsedNum);
                        }
                        break;
                    case TTOptArguments.INTERACTIVE_MODE:
                    case TTOptArguments.USERANDOMSEED:
                        if (bool.TryParse(settingValue, out bool parsedBool))
                        {
                            _settings.Add(settingId, parsedBool);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        IsInteractive = _settings.TryGetValue(TTOptArguments.INTERACTIVE_MODE, out object? isInteractive) && isInteractive is bool b
        ? b
        : true;
    }
}