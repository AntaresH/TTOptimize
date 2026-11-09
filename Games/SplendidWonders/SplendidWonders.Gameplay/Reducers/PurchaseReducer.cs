using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay.Reducers
{
    public class PurchaseReducer : ReducerBase, IStateReducer<PurchaseAction>
    {
        public GameState Reduce(GameState baseState, PurchaseAction action)
        {
            return baseState
                .Transform(ReturnTokens, action)
                .Transform(AcquireCard, action)
                .Transform(ReplaceCard, action)
                .Transform(AcquireLeader)
                .Transform(TriggerEndgame)
                .Transform(AdvanceTurn);
        }

        private GameState ReturnTokens(GameState baseState, PurchaseAction action)
        {
            var cardCostArray = action.Card.Cost.ToArray();
            var playerStoreArray = baseState.ActivePlayer.Store.ToArray();
            var paymentTokens = new int[6];

            int[] playerBonuses =
            [
                baseState.ActivePlayer.Tableau.Count(c => c.Bonus == DevelopmentBonus.Yellow),
                baseState.ActivePlayer.Tableau.Count(c => c.Bonus == DevelopmentBonus.Blue),
                baseState.ActivePlayer.Tableau.Count(c => c.Bonus == DevelopmentBonus.Green),
                baseState.ActivePlayer.Tableau.Count(c => c.Bonus == DevelopmentBonus.Brown),
                baseState.ActivePlayer.Tableau.Count(c => c.Bonus == DevelopmentBonus.Red),
            ];

            int totalUsedPolymaths = 0;
            for (int i = 0; i < 5; i++)
            {
                int colorUsedPolymaths = Math.Max(0, cardCostArray[i] - playerBonuses[i] - playerStoreArray[i]);

                paymentTokens[i] = Math.Max(0, cardCostArray[i] - playerBonuses[i] - colorUsedPolymaths);

                totalUsedPolymaths += colorUsedPolymaths;
            }

            paymentTokens[5] = totalUsedPolymaths;

            return ReturnTokens(baseState, paymentTokens);
        }

        private GameState AcquireCard(GameState baseState, PurchaseAction action)
        {
            var playerTableau = baseState.ActivePlayer.Tableau.Add(action.Card);
            var playerReserved = baseState.ActivePlayer.ReservedCards;

            if (baseState.ActivePlayer.ReservedCards.Contains(action.Card))
            {
                playerReserved = baseState.ActivePlayer.ReservedCards.Remove(action.Card);
            }

            return baseState
                .UpdatePlayerData(baseState.ActivePlayer.Id, baseState.ActivePlayer.Store, playerReserved, playerTableau, baseState.ActivePlayer.AcquiredLeaders);
        }

        private GameState AcquireLeader(GameState baseState)
        {
            var eligibleLeaders = GetEligibleLeaders(baseState);

            if (eligibleLeaders.Count() == 1)
            {
                return AcquireLeader(baseState, eligibleLeaders.First());
            }

            return baseState;
        }
    }
}