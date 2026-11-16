using System.Text.RegularExpressions;

namespace TTOptimize.ConsoleHost
{
    public static class TTOptArguments
    {
        public const string INTERACTIVE_MODE = "interactive";
        public const string GAMEID = "gameid";
        public const string PLAYERCOUNT = "playercount";
        public const string PLAYER1NAME = "player1name";
        public const string PLAYER2NAME = "player2name";
        public const string PLAYER1TYPE = "player1type";
        public const string PLAYER2TYPE = "player2type";
        public const string USERANDOMSEED = "userandomseed";
        public const string SEED = "randomseed";
    }

    public class SettingsService
    {
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
                            _settings.Add(settingId, settingValue);
                            break;
                        case TTOptArguments.PLAYERCOUNT:
                        case TTOptArguments.PLAYER1TYPE:
                        case TTOptArguments.PLAYER2TYPE:
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
        }

        public string GetSetting(string settingId)
        {
            if (_settings.TryGetValue(settingId.ToLower(), out object settingValue))
            {
                return settingValue.ToString();
            }

            return null;
        }

        public string QuickRead(string argumentType)
        {
            var argument = GetSetting(argumentType);

            return string.IsNullOrEmpty(argument) ? Console.ReadLine().Trim() : argument;
        }
    }
}