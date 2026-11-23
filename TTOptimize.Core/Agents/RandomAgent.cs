using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core.Agents;

public class RandomAgent : ComputerAgent
{
    public RandomAgent(string name, int playerId) : base(name, playerId)
    {
    }

    public override GameCommand DecideCommand(IGameState gameState, IReadOnlyList<GameAction> validActions)
    {
        int actionCount = validActions.Count;
        int random = new Random().Next(0, actionCount - 1);

        return new GameCommand() { Action = validActions[random] };
    }
}
