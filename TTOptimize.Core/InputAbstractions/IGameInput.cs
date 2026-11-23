using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.InputAbstractions
{
    public interface IGameInput
    {
        public void ValidationFailed(string validationFailureMessage);
        public GameCommand ReadCommand(IGameState gameState, IEnumerable<GameAction> validActions);
    }
}
