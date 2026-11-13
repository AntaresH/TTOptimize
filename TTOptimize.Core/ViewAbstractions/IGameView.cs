using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core.ViewAbstractions
{
    public interface IGameView
    {
        void Render(IGameState gameState, IReadOnlyList<GameAction> validActions, string customMessage);
        //void Render(IGameState gameState, IEnumerable<GameAction> validActions);
        void RenderFinalState(IGameState gameState, object finalStandingsData);
    }
}
