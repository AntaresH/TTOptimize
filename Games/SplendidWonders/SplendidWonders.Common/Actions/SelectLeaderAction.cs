using SplendidWonders.Common.Model;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Common.Actions;

public record class SelectLeaderAction : GameAction
{
    public Leader Leader { get; set; }

    public override string Description => string.Format(Strings.ActionDesc_SelectLeaderFormat, Leader.Name);
}