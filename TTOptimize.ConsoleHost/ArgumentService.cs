using System.Text.RegularExpressions;

namespace TTOptimize.ConsoleHost
{
    public static class TTOptArguments
    {
        public const string GAMEID = "gameid";
        public const string PLAYERCOUNT = "playercount";
        public const string PLAYER1NAME = "player1name";
        public const string PLAYER2NAME = "player2name";
        public const string PLAYER1TYPE = "player1type";
        public const string PLAYER2TYPE = "player2type";
        public const string USERANDOMSEED = "userandomseed";
        public const string SEED = "randomseed";
    }

    public class ArgumentService
    {
        private Dictionary<string, object> _commandArguments { get; set; } = new Dictionary<string, object>();

        public ArgumentService(string[] cmdLineArgs)
        {
            foreach (var argument in cmdLineArgs)
            {
                Match match = Regex.Match(argument, @"^([A-Za-z0-9]+)=(\w+)$");

                if (match.Success)
                {
                    string argumentType = match.Groups[1].Value;
                    string argumentValue = match.Groups[2].Value;

                    switch (argumentType)
                    {
                        case TTOptArguments.GAMEID:
                        case TTOptArguments.PLAYER1NAME:
                        case TTOptArguments.PLAYER2NAME:
                            _commandArguments.Add(argumentType, argumentValue);
                            break;
                        case TTOptArguments.PLAYERCOUNT:
                        case TTOptArguments.PLAYER1TYPE:
                        case TTOptArguments.PLAYER2TYPE:
                        case TTOptArguments.SEED:
                            if (int.TryParse(argumentValue, out int parsedNum))
                            {
                                _commandArguments.Add(argumentType, parsedNum);
                            }
                            break;
                        case TTOptArguments.USERANDOMSEED:
                            if (bool.TryParse(argumentValue, out bool parsedBool))
                            {
                                _commandArguments.Add(argumentType, parsedBool);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public string GetArgument(string argumentType)
        {
            if (_commandArguments.TryGetValue(argumentType.ToLower(), out object argumentValue))
            {
                return argumentValue.ToString();
            }

            return null;
        }

        public string QuickRead(string argumentType)
        {
            var argument = GetArgument(argumentType);

            return string.IsNullOrEmpty(argument) ? Console.ReadLine().Trim() : argument;
        }
    }
}