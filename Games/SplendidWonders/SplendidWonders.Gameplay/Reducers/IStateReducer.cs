using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay.Actions;
using TTOptimize.Core;

namespace SplendidWonders.Gameplay.Reducers
{
    public interface IStateReducer<T> where T : GameAction
    {
        GameState Reduce(GameState state, T action);
    }
}