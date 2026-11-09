using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay.Actions;

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