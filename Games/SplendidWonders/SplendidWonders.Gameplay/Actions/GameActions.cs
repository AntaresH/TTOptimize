
using SplendidWonders.Common.Model;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Gameplay.Actions
{
    public abstract class TokensAction : GameAction
    {
        public TokenGroup Tokens { get; set; }
    }

    public abstract class CardAction : GameAction
    {
        public DevelopmentCard Card { get; set; }
    }
}