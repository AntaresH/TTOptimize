using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay.Reducers;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Gameplay
{
    public class StateReducer : IStateReducer
    {
        private readonly IStateReducer<DrawThreeTokensAction> _drawThreeTokensReducer;
        private readonly IStateReducer<DrawDoubleTokensAction> _drawDoubleTokensReducer;
        private readonly IStateReducer<ReserveFromBoardAction> _reserveFromBoardReducer;
        private readonly IStateReducer<ReserveFromDeckAction> _reserveFromDeckReducer;
        private readonly IStateReducer<PurchaseAction> _purchaseReducer;
        private readonly IStateReducer<SelectLeaderAction> _selectLeaderReducer;
        private readonly IStateReducer<PassAction> _passReducer;
        private readonly IStateReducer<ReturnTokensAction> _returnTokensReducer;

        public StateReducer(
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IStateReducer<DrawThreeTokensAction> drawThreeTokensReducer,
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IStateReducer<DrawDoubleTokensAction> drawDoubleTokensReducer,
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IStateReducer<ReturnTokensAction> returnTokensReducer,
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IStateReducer<ReserveFromBoardAction> reserveFromBoardReducer,
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IStateReducer<ReserveFromDeckAction> reserveFromDeckReducer,
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IStateReducer<PurchaseAction> purchaseReducer,
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IStateReducer<SelectLeaderAction> selectLeaderReducer,
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IStateReducer<PassAction> passReducer

            )
        {
            _drawThreeTokensReducer = drawThreeTokensReducer;
            _drawDoubleTokensReducer = drawDoubleTokensReducer;
            _reserveFromBoardReducer = reserveFromBoardReducer;
            _reserveFromDeckReducer = reserveFromDeckReducer;
            _purchaseReducer = purchaseReducer;
            _selectLeaderReducer = selectLeaderReducer;
            _passReducer = passReducer;
            _returnTokensReducer = returnTokensReducer;
        }

        IGameState IStateReducer.Reduce(IGameState state, GameAction action)
        {
            var swGameState = (GameState)state;

            return action switch
            {
                DrawThreeTokensAction a => _drawThreeTokensReducer.Reduce(swGameState, a),
                DrawDoubleTokensAction a => _drawDoubleTokensReducer.Reduce(swGameState, a),
                ReturnTokensAction a => _returnTokensReducer.Reduce(swGameState, a),
                PurchaseAction a => _purchaseReducer.Reduce(swGameState, a),
                ReserveFromBoardAction a => _reserveFromBoardReducer.Reduce(swGameState, a),
                ReserveFromDeckAction a => _reserveFromDeckReducer.Reduce(swGameState, a),
                SelectLeaderAction a => _selectLeaderReducer.Reduce(swGameState, a),
                PassAction a => _passReducer.Reduce(swGameState, a),
                _ => throw new InvalidOperationException("This action is not supported... yet"),
            };
        }
    }
}
