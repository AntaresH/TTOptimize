using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core.Agents;

public class HumanAgent : IAgent
{
    private readonly IGameInput _input;

    public string Name { get; }

    public string SubjectId => $"Human:{Name}";

    public HumanAgent(string name, IGameInput input)
    {
        Name = name;
        _input = input;
    }

    public GameCommand DecideCommand(IGameState gameState, IReadOnlyList<GameAction> validActions)
        => _input.ReadCommand(gameState, validActions);
}