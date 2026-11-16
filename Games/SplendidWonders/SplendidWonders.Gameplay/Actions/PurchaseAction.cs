using SplendidWonders.Common.Model;

namespace SplendidWonders.Gameplay.Actions
{
    public abstract class PurchaseAction : CardAction
    {
        public TokenGroup RequiredTokens { get; set; }
    }

    public class PurchaseFromBoardAction : PurchaseAction
    {
        public override bool Equals(object? obj)
        {
            if (obj is PurchaseAction other)
            {
                return Card.Equals(other.Card) && RequiredTokens == other.RequiredTokens;
            }

            return base.Equals(obj);
        }
    }

    public class PurchaseFromReservedAction : PurchaseAction
    {
        public override bool Equals(object? obj)
        {
            if (obj is PurchaseAction other)
            {
                return Card.Equals(other.Card) && RequiredTokens == other.RequiredTokens;
            }
            return base.Equals(obj);
        }
    }
}