using SplendidWonders.Common.Model;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Common.Setup
{
    public class SplendidWondersConfiguration : IGameConfiguration
    {
        [GameConfigParam("Játékosok száma", min: 2, max: 4)]
        public int PlayerCount { get; set; } = 2;

        [GameConfigParam("Győzelmi pont határ", min: 10, max: 30)]
        public int VictoryPointsThreshold { get; set; } = 15;

        [GameConfigParam("Bővített vezérlista használata")]
        public bool UseExtendedLeaders { get; set; }

        [GameConfigParam("Random seed", min: 0, max: 100000)]
        public int Seed { get; set; }

        public List<string> PlayerNames { get; set; }

        public bool UseRandomSeed { get; set; }
    }
}