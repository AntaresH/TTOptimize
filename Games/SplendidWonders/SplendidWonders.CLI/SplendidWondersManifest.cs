using SplendidWonders.Common.Setup;
using TTOptimize.Core;

namespace SplendidWonders.CLI;

public class SplendidWondersManifest : IGameManifest
{
    public string AsciiLogo => _logo;
    public string GameId => Games.SPLENDID_WONDERS_KEY;
    public string DisplayName => Games.SPLENDID_WONDERS;
    public string Description => Strings.GameDescription;
    public Type ConfigurationType => typeof(SplendidWondersConfiguration);

    private string _logo =
        "   _____       _                _ _     _  __          __             _               \\r\\n" +
        "  / ____|     | |              | (_)   | | \\ \\        / /            | |              \\r\\n" +
        " | (___  _ __ | | ___ _ __   __| |_  __| |  \\ \\  /\\  / /__  _ __   __| | ___ _ __ ___ \\r\\n" +
        "  \\___ \\| '_ \\| |/ _ \\ '_ \\ / _` | |/ _` |   \\ \\/  \\/ / _ \\| '_ \\ / _` |/ _ \\ '__/ __|\\r\\n" +
        "  ____) | |_) | |  __/ | | | (_| | | (_| |    \\  /\\  / (_) | | | | (_| |  __/ |  \\__ \\\\r\\n" +
        " |_____/| .__/|_|\\___|_| |_|\\__,_|_|\\__,_|     \\/  \\/ \\___/|_| |_|\\__,_|\\___|_|  |___/\\r\\n" +
        "        | |                                                                           \\r\\n" +
        "        |_|                                                                           \\r\\n";
}
