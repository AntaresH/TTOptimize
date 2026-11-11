namespace SplendidWonders.Gameplay.Actions
{
    public abstract class PurchaseAction : CardAction
    {
        public int RequiredPolymathsCount { get; set; }
    }

    public class PurchaseFromBoardAction : PurchaseAction
    {
        public override bool Equals(object? obj)
        {
            if (obj is PurchaseAction other)
            {
                return Card.Equals(other.Card) && RequiredPolymathsCount == other.RequiredPolymathsCount;
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
                return Card.Equals(other.Card) && RequiredPolymathsCount == other.RequiredPolymathsCount;
            }
            return base.Equals(obj);
        }
    }
}