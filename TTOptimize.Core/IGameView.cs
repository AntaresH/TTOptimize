namespace TTOptimize.Core
{
    public interface IGameView
    {
        void Render(IGameState gameState, IEnumerable<GameAction> legalActions, string customMessage);
        void Render(IGameState gameState, IEnumerable<GameAction> legalActions);
        void RenderFinalState(IGameState gameState, object finalStandingsData);
        void DisplayPresentationCommand(IGameState gameState, PresentationCommand presentationCommand);
    }
}
