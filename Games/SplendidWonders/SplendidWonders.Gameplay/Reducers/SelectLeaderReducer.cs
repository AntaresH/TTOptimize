using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;

namespace SplendidWonders.Gameplay.Reducers
{
    public class SelectLeaderReducer : ReducerBase, IStateReducer<SelectLeaderAction>
    {
        public GameState Reduce(GameState sourceState, SelectLeaderAction action)
        {
            return sourceState
                .Transform(AcquireLeader, action.Leader)
                .Transform(TriggerEndgame)
                .Transform(AdvanceTurn, action);
        }
    }
}