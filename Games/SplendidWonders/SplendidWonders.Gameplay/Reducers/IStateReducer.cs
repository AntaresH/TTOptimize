using SplendidWonders.Common.Model;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Gameplay.Reducers
{
    public interface IStateReducer<T> where T : GameAction
    {
        GameState Reduce(GameState state, T action);
    }
}