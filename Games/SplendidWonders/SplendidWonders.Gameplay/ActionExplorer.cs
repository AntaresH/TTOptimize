using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Explorers;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;

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
            [FromKeyedServices(Games.SPLENDID_WONDERS)] IActionExplorer<DrawThreeTokensAction> drawThreeTokensExplorer,
            [FromKeyedServices(Games.SPLENDID_WONDERS)] IActionExplorer<DrawDoubleTokensAction> drawDoubleTokensExplorer,
            [FromKeyedServices(Games.SPLENDID_WONDERS)] IActionExplorer<ReturnTokensAction> returnTokensExplorer,
            [FromKeyedServices(Games.SPLENDID_WONDERS)] IActionExplorer<ReserveFromBoardAction> reserveFromBoardExplorer,
            [FromKeyedServices(Games.SPLENDID_WONDERS)] IActionExplorer<ReserveFromDeckAction> reserveFromDeckExplorer,
            [FromKeyedServices(Games.SPLENDID_WONDERS)] IActionExplorer<PurchaseAction> purchaseExplorer,
            [FromKeyedServices(Games.SPLENDID_WONDERS)] IActionExplorer<SelectLeaderAction> selectLeaderExplorer
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
            else if (selectLeaderActions.Any())
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

            return validActions;
        }
    }
}