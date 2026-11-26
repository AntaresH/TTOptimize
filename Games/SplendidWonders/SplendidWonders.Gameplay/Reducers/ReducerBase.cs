using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Gameplay.Reducers
{
    public class ReducerBase
    {
        protected GameState ReturnTokens(GameState baseState, int[] returnTokens)
        {
            var invertedValues = returnTokens.Select(t => t * -1).ToArray();

            return DrawTokens(baseState, invertedValues);
        }

        protected GameState DrawTokens(GameState baseState, int[] selectedTokens)
        {
            var playerStore = baseState.ActivePlayer.Store.ToArray();
            var tokenSupply = baseState.TokenSupply.ToArray();

            for (int i = 0; i < 6; i++)
            {
                playerStore[i] += selectedTokens[i];
                tokenSupply[i] -= selectedTokens[i];
            }

            return baseState.UpdatePlayerData(baseState.ActivePlayer.Seat, TokenGroup.FromArray(playerStore), baseState.ActivePlayer.ReservedCards, baseState.ActivePlayer.Tableau, baseState.ActivePlayer.AcquiredLeaders)
                            .UpdateTokenSupply(TokenGroup.FromArray(tokenSupply));
        }

        protected GameState ReplaceCard(GameState baseState, CardAction action)
        {
            int level = action.Card.Level;

            var cardGroup = baseState[level];

            if (!cardGroup.Market.Contains(action.Card))
            {
                return baseState;
            }

            int index = cardGroup.Market.IndexOf(action.Card);

            var newMarket = cardGroup.Market.RemoveAt(index);
            var newDeck = cardGroup.Deck;

            if (cardGroup.Deck.Any())
            {
                newMarket = newMarket.Insert(index, cardGroup.Deck[0]);
                newDeck = newDeck.RemoveAt(0);
            }
            else
            {
                newMarket = newMarket.Insert(index, Cards.Empty);
            }

            return baseState.UpdateLevel(level, new CardGroup() { Deck = newDeck, Market = newMarket });
        }

        protected GameState TriggerEndgame(GameState baseState)
        {
            if (!baseState.IsFinalTurn)
            {
                int totalVPs = baseState.ActivePlayer.Tableau.Sum(c => c.VictoryPoints) + baseState.ActivePlayer.AcquiredLeaders.Count * 3;

                if (totalVPs >= 15)
                {
                    return baseState with { IsFinalTurn = true };
                }
            }

            return baseState;
        }

        protected GameState AcquireLeader(GameState baseState, Leader leader)
        {
            var acquiredLeaders = baseState.ActivePlayer.AcquiredLeaders.Add(leader);
            var availableLeaders = baseState.Leaders.Remove(leader);

            return baseState
                    .UpdatePlayerData(baseState.ActivePlayer.Seat, baseState.ActivePlayer.Store,
                                      baseState.ActivePlayer.ReservedCards, baseState.ActivePlayer.Tableau, acquiredLeaders)
                    .UpdateAvailableLeaders(availableLeaders);
        }

        protected GameState AdvanceTurn(GameState baseState, GameAction action)
        {
            int newConsecutivePasses = 0;
            if (action is PassAction)
            {
                if (baseState.ConsecutivePasses == baseState.Players.Count - 1)
                {
                    return baseState with
                    {
                        GameOverReason = GameOverReason.Stalemate,
                        GameProgression = GameProgression.GameOver
                    };
                }
                else
                {
                    newConsecutivePasses = baseState.ConsecutivePasses + 1;
                }
            }

            bool tokenOverflow = CheckTokenOverflow(baseState);
            bool multipleLeadersEligible = CheckMultipleLeadersEligible(baseState);

            bool isLastPlayer = baseState.ActivePlayer.Seat == baseState.Players.Count;
            bool advanceToNextPlayer = !tokenOverflow && !multipleLeadersEligible;

            if (advanceToNextPlayer && isLastPlayer && baseState.IsFinalTurn)
            {
                return baseState with 
                { 
                    GameOverReason = GameOverReason.VictoryConditionMet,
                    GameProgression = GameProgression.GameOver 
                };
            }

            int nextPlayerId = advanceToNextPlayer
                ? baseState.ActivePlayer.Seat % baseState.Players.Count + 1
                : baseState.ActivePlayer.Seat;

            return baseState with 
            { 
                ActivePlayer = baseState.Players[nextPlayerId - 1],
                ConsecutivePasses = action is PassAction ? newConsecutivePasses : 0
            };
        }

        private bool CheckMultipleLeadersEligible(GameState baseState)
        {
            return GetEligibleLeaders(baseState).Count() > 1;
        }

        private bool CheckTokenOverflow(GameState state)
        {
            var playerStore = state.ActivePlayer.Store;
            int allPlayerTokens = playerStore.Yellow + playerStore.Blue + playerStore.Green + playerStore.Brown + playerStore.Red + playerStore.Polymath;

            return allPlayerTokens > 10;
        }

        protected IEnumerable<Leader> GetEligibleLeaders(GameState state)
        {
            return state.Leaders.Where(leader => CheckLeaderEligibility(state, leader));
        }

        private bool CheckLeaderEligibility(GameState state, Leader leader)
        {
            bool passRequirements = true;

            int[] leaderReqsArray = leader.Requirements.ToArray();
            int[] playerTableauArray =
            [
                state.ActivePlayer.Tableau.Count(card => card.Bonus == DevelopmentColor.Yellow),
                    state.ActivePlayer.Tableau.Count(card => card.Bonus == DevelopmentColor.Blue),
                    state.ActivePlayer.Tableau.Count(card => card.Bonus == DevelopmentColor.Green),
                    state.ActivePlayer.Tableau.Count(card => card.Bonus == DevelopmentColor.Brown),
                    state.ActivePlayer.Tableau.Count(card => card.Bonus == DevelopmentColor.Red),
                ];

            for (int i = 0; i < 5; i++)
            {
                if (leaderReqsArray[i] > playerTableauArray[i])
                {
                    passRequirements = false;
                    break;
                }
            }

            return passRequirements;
        }
    }
}
