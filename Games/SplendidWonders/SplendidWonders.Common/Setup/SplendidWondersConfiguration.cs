using SplendidWonders.Common.Model;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Common.Setup
{
    public class SplendidWondersConfiguration : IGameConfiguration
    {
        [GameConfigParam("Játékosok száma", min: 2, max: 4)]
        public int PlayerCount { get; set; } = 2;

        [GameConfigParam("Véletlenszerű keverés (Seed) használata")]
        public bool UseRandomSeed { get; set; } = true;

        [GameConfigParam("Egyedi Seed szám", min: 0, max: 100000)]
        [DependsOn(nameof(UseRandomSeed), false)]
        public int Seed { get; set; }

        //[GameConfigParam("Győzelmi pont határ", min: 10, max: 30)]
        public int VictoryPointsThreshold { get; set; } = 15;

        //[GameConfigParam("Bővített vezérlista használata")]
        public bool UseExtendedLeaders { get; set; } = true;

        public List<string> PlayerNames { get; set; }
    }
}