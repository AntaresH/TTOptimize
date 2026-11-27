using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core.Agents;

public sealed record class PlayerSlotConfig(int Seat, string DisplayName, string AgentTypeId);

public interface IAgent
{
    string Name { get; }
    string SubjectId { get; }
    GameCommand DecideCommand(IGameState gameState, IReadOnlyList<GameAction> validActions);
}
