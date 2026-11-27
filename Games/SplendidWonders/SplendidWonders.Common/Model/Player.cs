using System.Collections.Immutable;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Common.Model
{
    public record class Player : IPlayer
    {
        public int Seat { get; set; }

        public string Name { get; set; }

        public TokenGroup Store { get; set; } = new();

        public ImmutableList<DevelopmentCard> Tableau { get; set; } = ImmutableList<DevelopmentCard>.Empty;

        public ImmutableList<DevelopmentCard> ReservedCards { get; set; } = ImmutableList<DevelopmentCard>.Empty;

        public ImmutableList<Leader> AcquiredLeaders { get; set; } = ImmutableList<Leader>.Empty;
    }
}