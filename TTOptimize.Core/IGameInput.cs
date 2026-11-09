namespace TTOptimize.Core
{
    public interface IGameInput
    {
        public ApplicationCommand ReadCommand(IGameState gameState, IEnumerable<GameAction> validActions);
    }
}
