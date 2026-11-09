using TTOptimize.Core;

namespace SplendidWonders.Common.Setup
{
    public class GameConfiguration : IGameConfiguration
    {
        public bool UseExtendedLeaders { get; set; }

        public bool UseRandomSeed { get; set; }

        public int Seed { get; set; }

        public IList<IPlayerAgent> PlayerAgents { get; set; }
    }
}