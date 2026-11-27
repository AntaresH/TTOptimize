using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;

namespace SplendidWonders.Gameplay.Reducers
{
    public class ReserveActionReducer : ReducerBase, IStateReducer<ReserveFromBoardAction>, IStateReducer<ReserveFromDeckAction>
    {
        public GameState Reduce(GameState sourceState, ReserveFromBoardAction action)
        {
            return sourceState
                .Transform(DrawPolymath)
                .Transform(ReserveFromBoard, action)
                .Transform(ReplaceCard, action)
                .Transform(AdvanceTurn, action);
        }

        public GameState Reduce(GameState sourceState, ReserveFromDeckAction action)
        {
            return sourceState
                .Transform(DrawPolymath)
                .Transform(ReserveFromDeck, action)
                .Transform(DecreaseDeck, action)
                .Transform(AdvanceTurn, action);
        }

        private GameState DrawPolymath(GameState sourceState)
        {
            return sourceState.TokenSupply.Polymath > 0 
                ? DrawTokens(sourceState, [0, 0, 0, 0, 0, 1]) 
                : sourceState;
        }

        private GameState ReserveFromBoard(GameState sourceState, ReserveFromBoardAction action)
        {
            return ReserveCard(sourceState, action.Card);
        }

        private GameState ReserveFromDeck(GameState sourceState, ReserveFromDeckAction action)
        {
            var card = sourceState[action.Level].Deck.First();

            return ReserveCard(sourceState, card);
        }

        private GameState DecreaseDeck(GameState sourceState, ReserveFromDeckAction action)
        {
            return sourceState.UpdateLevel(action.Level, new CardGroup() { Deck = sourceState[action.Level].Deck.RemoveAt(0), Market = sourceState[action.Level].Market });
        }

        private static GameState ReserveCard(GameState sourceState, DevelopmentCard card)
        {
            var playerStore = sourceState.ActivePlayer.Store;
            var tokenSupply = sourceState.TokenSupply;

            var playerReserved = sourceState.ActivePlayer.ReservedCards;
            playerReserved = playerReserved.Add(card);

            return sourceState.UpdatePlayerData(sourceState.ActivePlayer.Seat, playerStore, playerReserved, sourceState.ActivePlayer.Tableau, sourceState.ActivePlayer.AcquiredLeaders)
                            .UpdateTokenSupply(tokenSupply);
        }
    }
}