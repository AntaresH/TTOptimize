using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay.Reducers
{
    public class ReserveActionReducer : ReducerBase, IStateReducer<ReserveFromBoardAction>, IStateReducer<ReserveFromDeckAction>
    {
        public GameState Reduce(GameState baseState, ReserveFromBoardAction action)
        {
            return baseState
                .Transform(DrawPolymath)
                .Transform(ReserveFromBoard, action)
                .Transform(ReplaceCard, action)
                .Transform(AdvanceTurn);
        }

        public GameState Reduce(GameState baseState, ReserveFromDeckAction action)
        {
            return baseState
                .Transform(DrawPolymath)
                .Transform(ReserveFromDeck, action)
                .Transform(DecreaseDeck, action)
                .Transform(AdvanceTurn);
        }

        private GameState DrawPolymath(GameState baseState)
        {
            return baseState.TokenSupply.Polymath > 0 
                ? DrawTokens(baseState, [0, 0, 0, 0, 0, 1]) 
                : baseState;
        }

        private GameState ReserveFromBoard(GameState baseState, ReserveFromBoardAction action)
        {
            return ReserveCard(baseState, action.Card);
        }

        private GameState ReserveFromDeck(GameState baseState, ReserveFromDeckAction action)
        {
            var card = baseState[action.Level].Deck.First();

            return ReserveCard(baseState, card);
        }

        private GameState DecreaseDeck(GameState baseState, ReserveFromDeckAction action)
        {
            return baseState.UpdateLevel(action.Level, new CardGroup() { Deck = baseState[action.Level].Deck.RemoveAt(0), Market = baseState[action.Level].Market });
        }

        private static GameState ReserveCard(GameState baseState, DevelopmentCard card)
        {
            var playerStore = baseState.ActivePlayer.Store;
            var tokenSupply = baseState.TokenSupply;

            var playerReserved = baseState.ActivePlayer.ReservedCards;
            playerReserved = playerReserved.Add(card);

            return baseState.UpdatePlayerData(baseState.ActivePlayer.Id, playerStore, playerReserved, baseState.ActivePlayer.Tableau, baseState.ActivePlayer.AcquiredLeaders)
                            .UpdateTokenSupply(tokenSupply);
        }
    }
}