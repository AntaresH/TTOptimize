using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.InputAbstractions
{
    public interface IGameInput
    {
        public GameCommand ReadCommand(IGameState gameState, IEnumerable<GameAction> validActions);
    }
}
