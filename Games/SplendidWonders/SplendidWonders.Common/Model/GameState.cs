using System.Collections.Immutable;
using TTOptimize.Core;

namespace SplendidWonders.Common.Model
{
    public record class GameState : IGameState
    {
        public required List<Player> Players { get; set; }

        public TokenGroup TokenSupply { get; set; }

        public required ImmutableList<Leader> Leaders { get; set; }

        public required CardGroup Level1 { get; set; }

        public required CardGroup Level2 { get; set; }

        public required CardGroup Level3 { get; set; }

        int _activePlayerId = 1;

        public int ActivePlayerId => _activePlayerId;

        public required Player ActivePlayer
        {
            get
            {
                return Players[_activePlayerId - 1];
            }
            set
            {
                _activePlayerId = value.Id;
            }
        }

        public bool IsFinalTurn { get; set; }

        public GameProgression GameProgression { get; set; }

        public CardGroup this[int level]
        {
            get
            {
                return level switch
                {
                    1 => Level1,
                    2 => Level2,
                    3 => Level3,
                    _ => throw new ArgumentException("Level must be between 1 and 3")
                };
            }
        }
    }
}
