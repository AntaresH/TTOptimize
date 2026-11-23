using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.Agents;

namespace SplendidWonders.Gameplay
{
    public record class PlayerResult : IPlayerResult
    {
        public IPlayerAgent Player { get; set; }
        public int Rank { get; set; }
        public int Points { get; set; }
        public int? Tiebreaker { get; set; }
    }
}
