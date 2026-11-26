using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Gameplay
{
    public record class PlayerResult : IPlayerResult
    {
        public required IPlayerState Player { get; set; }
        public int Rank { get; set; }
        public int Points { get; set; }
        public int? Tiebreaker { get; set; }
    }
}
