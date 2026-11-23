using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core.Agents;

public interface IPlayerAgent
{
    string Name { get; }
    int PlayerId { get; }
    GameCommand DecideCommand(IGameState gameState, IReadOnlyList<GameAction> validActions);
}

public interface IPlayerResult
{
    IPlayerAgent Player { get; }
    
    int Rank { get; }

    int Points { get; }

    int? Tiebreaker { get; }
}
