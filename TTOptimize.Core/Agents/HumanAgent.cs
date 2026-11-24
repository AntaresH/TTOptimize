using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core.Agents;

public class HumanAgent : IPlayerAgent
{
    private readonly IGameInput _input;

    public string Name { get; }
    public int PlayerId { get; }

    public HumanAgent(string name, int playerId, IGameInput input)
    {
        Name = name;
        PlayerId = playerId;
        _input = input;
    }

    public GameCommand DecideCommand(IGameState gameState, IReadOnlyList<GameAction> validActions)
        => _input.ReadCommand(gameState, validActions);
}