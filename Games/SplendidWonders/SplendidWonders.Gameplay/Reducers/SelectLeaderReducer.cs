using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;

namespace SplendidWonders.Gameplay.Reducers
{
    public class SelectLeaderReducer : ReducerBase, IStateReducer<SelectLeaderAction>
    {
        public GameState Reduce(GameState baseState, SelectLeaderAction action)
        {
            return baseState
                .Transform(AcquireLeader, action.Leader)
                .Transform(TriggerEndgame)
                .Transform(AdvanceTurn);
        }
    }
}