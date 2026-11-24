using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Common.Actions
{
    public record class PassAction : GameAction
    {
        public override string Description => Strings.ActionDesc_Pass;
    }
}
