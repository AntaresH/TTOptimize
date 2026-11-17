namespace SplendidWonders.Common.Actions;

public class ReserveFromBoardAction : CardAction
{
    public override string Description => string.Format(Strings.ActionDesc_ReserveFromBoardFormat, Card.Name);

    public override bool Equals(object? obj)
    {
        if (obj is ReserveFromBoardAction other)
        {
            return Card.Equals(other.Card);
        }

        return base.Equals(obj);
    }
}