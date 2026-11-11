using SplendidWonders.CLI;
using SplendidWonders.Common.Setup;
using TTOptimize.Core;

namespace SplendidWonders.Gameplay;

public class SplendidWondersManifest : IGameManifest
{
    public string GameId => Games.SPLENDID_WONDERS_KEY;
    public string DisplayName => Games.SPLENDID_WONDERS;
    public string Description => Strings.GameDescription;
    public Type ConfigurationType => typeof(SplendidWondersConfiguration);
}
