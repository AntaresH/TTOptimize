using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;

namespace SplendidWonders.Gameplay.Reducers
{
    public class PurchaseReducer : ReducerBase, IStateReducer<PurchaseAction>
    {
        public GameState Reduce(GameState sourceState, PurchaseAction action)
        {
            return sourceState
                .Transform(ReturnTokens, action)
                .Transform(AcquireCard, action)
                .Transform(ReplaceCard, action)
                .Transform(AcquireLeader)
                .Transform(TriggerEndgame)
                .Transform(AdvanceTurn, action);
        }

        private GameState ReturnTokens(GameState sourceState, PurchaseAction action)
        {
            var cardCostArray = action.Card.Cost.ToArray();
            var playerStoreArray = sourceState.ActivePlayer.Store.ToArray();
            var paymentTokens = new int[6];

            int[] playerBonuses =
            [
                sourceState.ActivePlayer.Tableau.Count(c => c.Bonus == DevelopmentColor.Yellow),
                sourceState.ActivePlayer.Tableau.Count(c => c.Bonus == DevelopmentColor.Blue),
                sourceState.ActivePlayer.Tableau.Count(c => c.Bonus == DevelopmentColor.Green),
                sourceState.ActivePlayer.Tableau.Count(c => c.Bonus == DevelopmentColor.Brown),
                sourceState.ActivePlayer.Tableau.Count(c => c.Bonus == DevelopmentColor.Red),
            ];

            int totalUsedPolymaths = 0;
            for (int i = 0; i < 5; i++)
            {
                int colorUsedPolymaths = Math.Max(0, cardCostArray[i] - playerBonuses[i] - playerStoreArray[i]);

                paymentTokens[i] = Math.Max(0, cardCostArray[i] - playerBonuses[i] - colorUsedPolymaths);

                totalUsedPolymaths += colorUsedPolymaths;
            }

            paymentTokens[5] = totalUsedPolymaths;

            return ReturnTokens(sourceState, paymentTokens);
        }

        private GameState AcquireCard(GameState sourceState, PurchaseAction action)
        {
            var playerTableau = sourceState.ActivePlayer.Tableau.Add(action.Card);
            var playerReserved = sourceState.ActivePlayer.ReservedCards;

            if (sourceState.ActivePlayer.ReservedCards.Contains(action.Card))
            {
                playerReserved = sourceState.ActivePlayer.ReservedCards.Remove(action.Card);
            }

            return sourceState
                .UpdatePlayerData(sourceState.ActivePlayer.Seat, sourceState.ActivePlayer.Store, playerReserved, playerTableau, sourceState.ActivePlayer.AcquiredLeaders);
        }

        private GameState AcquireLeader(GameState sourceState)
        {
            var eligibleLeaders = GetEligibleLeaders(sourceState);

            if (eligibleLeaders.Count() == 1)
            {
                return AcquireLeader(sourceState, eligibleLeaders.First());
            }

            return sourceState;
        }
    }
}