using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core.Agents;

public abstract class ComputerAgent : IPlayerAgent
{
    public string Name { get; }

    public int PlayerId { get; }

    public ComputerAgent(string name, int playerId)
    {
        Name = name;
        PlayerId = playerId;
    }

    public abstract GameCommand DecideCommand(IGameState gameState, IReadOnlyList<GameAction> validActions);
}
