namespace SplendidWonders.Common.Actions;

public record class ReserveFromBoardAction : CardAction
{
    public override string Description => string.Format(Strings.ActionDesc_ReserveFromBoardFormat, Card.Name);
}