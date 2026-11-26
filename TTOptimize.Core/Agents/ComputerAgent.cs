using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core.Agents;

public abstract class ComputerAgent : IAgent
{
    public string Name { get; }

    public string StatisticsSubjectId => nameof(ComputerAgent);

    public ComputerAgent(string name)
    {
        Name = name;
    }

    public abstract GameCommand DecideCommand(IGameState gameState, IReadOnlyList<GameAction> validActions);
}
