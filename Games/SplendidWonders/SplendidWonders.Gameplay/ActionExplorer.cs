using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay.Explorers;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.Setup;

namespace SplendidWonders.Gameplay
{
    public class ActionExplorer : IActionExplorer
    {
        private readonly IActionExplorer<DrawThreeTokensAction> _drawThreeTokensExplorer;
        private readonly IActionExplorer<DrawDoubleTokensAction> _drawDoubleTokensExplorer;
        private readonly IActionExplorer<ReserveFromBoardAction> _reserveFromBoardExplorer;
        private readonly IActionExplorer<ReserveFromDeckAction> _reserveFromDeckExplorer;
        private readonly IActionExplorer<PurchaseAction> _purchaseExplorer;
        private readonly IActionExplorer<SelectLeaderAction> _selectLeaderExplorer;
        private readonly IActionExplorer<ReturnTokensAction> _returnTokensExplorer;

        public ActionExplorer(
            IActionExplorer<DrawThreeTokensAction> drawThreeTokensExplorer,
            IActionExplorer<DrawDoubleTokensAction> drawDoubleTokensExplorer,
            IActionExplorer<ReturnTokensAction> returnTokensExplorer,
            IActionExplorer<ReserveFromBoardAction> reserveFromBoardExplorer,
            IActionExplorer<ReserveFromDeckAction> reserveFromDeckExplorer,
            IActionExplorer<PurchaseAction> purchaseExplorer,
            IActionExplorer<SelectLeaderAction> selectLeaderExplorer
            )
        {
            _drawThreeTokensExplorer = drawThreeTokensExplorer;
            _drawDoubleTokensExplorer = drawDoubleTokensExplorer;
            _reserveFromBoardExplorer = reserveFromBoardExplorer;
            _reserveFromDeckExplorer = reserveFromDeckExplorer;
            _purchaseExplorer = purchaseExplorer;
            _selectLeaderExplorer = selectLeaderExplorer;
            _returnTokensExplorer = returnTokensExplorer;
        }

        public IList<GameAction> Explore(IGameState state)
        {
            var swGameState = (GameState)state;

            var validActions = new List<GameAction>();

            var returnTokensActions = _returnTokensExplorer.Explore(swGameState);
            var selectLeaderActions = _selectLeaderExplorer.Explore(swGameState);

            if (returnTokensActions.Any())
            {
                validActions.AddRange(returnTokensActions);
                return validActions;
            }
            else if (selectLeaderActions.Count > 1)
            {
                validActions.AddRange(selectLeaderActions);
                return validActions;
            }
            else
            {
                validActions.AddRange(_drawThreeTokensExplorer.Explore(swGameState));
                validActions.AddRange(_drawDoubleTokensExplorer.Explore(swGameState));
                validActions.AddRange(_reserveFromBoardExplorer.Explore(swGameState));
                validActions.AddRange(_reserveFromDeckExplorer.Explore(swGameState));
                validActions.AddRange(_purchaseExplorer.Explore(swGameState));
            }

            if (!validActions.Any())
            {
                validActions.Add(new PassAction());
            }

            return validActions;
        }
    }
}