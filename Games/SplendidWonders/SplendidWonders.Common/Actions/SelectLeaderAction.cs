using SplendidWonders.Common;
using SplendidWonders.Common.Model;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Common.Actions;

public class SelectLeaderAction : GameAction
{
    public Leader Leader { get; set; }

    public override string Description => string.Format(Strings.ActionDesc_SelectLeaderFormat, Leader.Name);


    override public bool Equals(object? obj)
    {
        if (obj is SelectLeaderAction other)
        {
            return Leader == other.Leader;
        }

        return base.Equals(obj);
    }
}