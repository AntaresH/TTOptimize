using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core.Agents;

public enum AgentType
{
    Human = 1,
    Random = 2
}

public record PlayerSlotConfig(string Name, int PlayerId, AgentType Type);

public interface IAgent
{
    string Name { get; }
    string StatisticsSubjectId { get; }
    GameCommand DecideCommand(IGameState gameState, IReadOnlyList<GameAction> validActions);
}
