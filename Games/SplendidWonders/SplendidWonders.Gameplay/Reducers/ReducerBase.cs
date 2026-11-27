using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Gameplay.Reducers
{
    public class ReducerBase
    {
        protected GameState ReturnTokens(GameState sourceState, int[] returnTokens)
        {
            var invertedValues = returnTokens.Select(t => t * -1).ToArray();

            return DrawTokens(sourceState, invertedValues);
        }

        protected GameState DrawTokens(GameState sourceState, int[] selectedTokens)
        {
            var playerStore = sourceState.ActivePlayer.Store.ToArray();
            var tokenSupply = sourceState.TokenSupply.ToArray();

            for (int i = 0; i < 6; i++)
            {
                playerStore[i] += selectedTokens[i];
                tokenSupply[i] -= selectedTokens[i];
            }

            return sourceState.UpdatePlayerData(sourceState.ActivePlayer.Seat, TokenGroup.FromArray(playerStore), sourceState.ActivePlayer.ReservedCards, sourceState.ActivePlayer.Tableau, sourceState.ActivePlayer.AcquiredLeaders)
                            .UpdateTokenSupply(TokenGroup.FromArray(tokenSupply));
        }

        protected GameState ReplaceCard(GameState sourceState, CardAction action)
        {
            int level = action.Card.Level;

            var cardGroup = sourceState[level];

            if (!cardGroup.Market.Contains(action.Card))
            {
                return sourceState;
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

            return sourceState.UpdateLevel(level, new CardGroup() { Deck = newDeck, Market = newMarket });
        }

        protected GameState TriggerEndgame(GameState sourceState)
        {
            if (!sourceState.IsFinalTurn)
            {
                int totalVPs = sourceState.ActivePlayer.Tableau.Sum(c => c.VictoryPoints) + sourceState.ActivePlayer.AcquiredLeaders.Count * 3;

                if (totalVPs >= 15)
                {
                    return sourceState with { IsFinalTurn = true };
                }
            }

            return sourceState;
        }

        protected GameState AcquireLeader(GameState sourceState, Leader leader)
        {
            var acquiredLeaders = sourceState.ActivePlayer.AcquiredLeaders.Add(leader);
            var availableLeaders = sourceState.Leaders.Remove(leader);

            return sourceState
                    .UpdatePlayerData(sourceState.ActivePlayer.Seat, sourceState.ActivePlayer.Store,
                                      sourceState.ActivePlayer.ReservedCards, sourceState.ActivePlayer.Tableau, acquiredLeaders)
                    .UpdateAvailableLeaders(availableLeaders);
        }

        protected GameState AdvanceTurn(GameState sourceState, GameAction action)
        {
            int newConsecutivePasses = 0;
            if (action is PassAction)
            {
                if (sourceState.ConsecutivePasses == sourceState.Players.Count - 1)
                {
                    return sourceState with
                    {
                        GameOverReason = GameOverReason.Stalemate,
                        GameProgression = GameProgression.GameOver
                    };
                }
                else
                {
                    newConsecutivePasses = sourceState.ConsecutivePasses + 1;
                }
            }

            bool tokenOverflow = CheckTokenOverflow(sourceState);
            bool multipleLeadersEligible = CheckMultipleLeadersEligible(sourceState);

            bool isLastPlayer = sourceState.ActivePlayer.Seat == sourceState.Players.Count;
            bool advanceToNextPlayer = !tokenOverflow && !multipleLeadersEligible;

            if (advanceToNextPlayer && isLastPlayer && sourceState.IsFinalTurn)
            {
                return sourceState with 
                { 
                    GameOverReason = GameOverReason.VictoryConditionMet,
                    GameProgression = GameProgression.GameOver 
                };
            }

            int nextPlayerId = advanceToNextPlayer
                ? sourceState.ActivePlayer.Seat % sourceState.Players.Count + 1
                : sourceState.ActivePlayer.Seat;

            return sourceState with 
            { 
                ActivePlayer = sourceState.Players[nextPlayerId - 1],
                ConsecutivePasses = action is PassAction ? newConsecutivePasses : 0
            };
        }

        private bool CheckMultipleLeadersEligible(GameState sourceState)
        {
            return GetEligibleLeaders(sourceState).Count() > 1;
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
