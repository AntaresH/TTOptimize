using TTOptimize.Core.Agents;

namespace TTOptimize.Core.GameAbstractions;

public interface IGameRules
{
    IReadOnlyList<GameAction> Explore(IGameState currentState);
    IReadOnlyList<IPlayerResult> GetFinalStandings(IGameState finalState, IReadOnlyList<IPlayerAgent> playerAgents);
    IGameState Reduce(IGameState baseState, GameAction actionToApply);
    bool Validate(GameAction action);
}
