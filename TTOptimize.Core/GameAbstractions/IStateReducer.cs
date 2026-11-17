namespace TTOptimize.Core.GameAbstractions;

public interface IStateReducer
{
    IGameState Reduce(IGameState state, GameAction action);
}
