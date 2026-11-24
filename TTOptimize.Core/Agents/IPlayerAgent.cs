using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core.Agents;

public interface IPlayerAgent
{
    string Name { get; }
    int PlayerId { get; }
    GameCommand DecideCommand(IGameState gameState, IReadOnlyList<GameAction> validActions);
}
