using SplendidWonders.Common.Model;
using TTOptimize.Core;

namespace SplendidWonders.Gameplay.Actions
{
    public class SelectLeaderAction : GameAction
    {
        public Leader Leader { get; set; }

        override public bool Equals(object? obj)
        {
            if (obj is SelectLeaderAction other)
            {
                return Leader == other.Leader;
            }

            return base.Equals(obj);
        }
    }
}