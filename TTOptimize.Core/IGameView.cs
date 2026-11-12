using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core
{
    public interface IGameView
    {
        void DisplayGameTitle();
        void Render(IGameState gameState, IEnumerable<GameAction> validActions, string customMessage);
        void Render(IGameState gameState, IEnumerable<GameAction> validActions);
        void RenderFinalState(IGameState gameState, object finalStandingsData);
    }
}
