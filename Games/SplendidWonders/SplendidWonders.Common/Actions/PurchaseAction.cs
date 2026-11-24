using SplendidWonders.Common.Model;

namespace SplendidWonders.Common.Actions;

public abstract record class PurchaseAction : CardAction
{
    public TokenGroup RequiredTokens { get; set; }
}

public record class PurchaseFromBoardAction : PurchaseAction
{
    public override string Description => string.Format(Strings.ActionDesc_PurchaseFromBoardFormat, Card.Name);
}

public record class PurchaseFromReservedAction : PurchaseAction
{
    public override string Description => string.Format(Strings.ActionDesc_PurchaseFromReservedFormat, Card.Name);
}