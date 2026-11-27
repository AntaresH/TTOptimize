using TTOptimize.Core.Agents;

namespace TTOptimize.Core.GameAbstractions;

public interface IGameRules
{
    IReadOnlyList<GameAction> Explore(IGameState currentState);
    IReadOnlyList<IPlayerResult> GetFinalStandings(IGameState finalState);
    IGameState Reduce(IGameState sourceState, GameAction actionToApply);
    bool Validate(GameAction action);
}
