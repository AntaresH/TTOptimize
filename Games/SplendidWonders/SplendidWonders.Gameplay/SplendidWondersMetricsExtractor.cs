using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using SplendidWonders.Common.Statistics;
using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.Statistics;

namespace SplendidWonders.Gameplay
{
    public class SplendidWondersMetricsExtractor : IMatchMetricsExtractor
    {
        public IGameMetrics ExtractGameMetrics(IGameMetrics baseMetrics, IGameState finalState, IReadOnlyList<ActionLogEntry> history)
        {
            var swState = (GameState) finalState;

            var winner = swState.Players
                .OrderByDescending(p => p.Tableau.Sum(c => c.VictoryPoints) + p.AcquiredLeaders.Count * 3)
                .ThenBy(p => p.Tableau.Count)
                .First();

            var unboughtOnMarket = new List<int>();
            for (int level = 1; level <= 3; level++)
            {
                unboughtOnMarket.AddRange(
                    swState[level].Market
                        .Where(c => c != Cards.Empty)
                        .Select(c => c.Id));
            }

            return new SplendidWondersGameMetrics
            {
                PurchasedCardIds = history
                    .Select(h => h.Action)
                    .OfType<PurchaseAction>()
                    .Select(a => a.Card.Id)
                    .ToList(),

                ReservedCardIds = history
                    .Select(h => h.Action)
                    .OfType<ReserveFromBoardAction>()
                    .Select(a => a.Card.Id)
                    .ToList(),

                UnboughtMarketCardIds = unboughtOnMarket,

                WinnerTableauCardIds = winner.Tableau.Select(c => c.Id).ToList(),

                ClaimedLeaderIds = swState.Players
                    .SelectMany(p => p.AcquiredLeaders)
                    .Select(l => l.Id)
                    .ToList(),

                WinnerLeaderIds = winner.AcquiredLeaders.Select(l => l.Id).ToList(),

                FinalTurnNumber = history.Count
            };
        }

        public IPlayerMetrics ExtractPlayerMetrics(IPlayerMetrics baseMetrics, IGameState finalState)
        {
            var swState = (GameState)finalState;
            var player = swState.Players.First(p => p.Seat == baseMetrics.Seat);

            return new SplendidWondersPlayerMetrics(baseMetrics)
            {
                TableauSize = player.Tableau.Count,
                AcquiredLeadersCount = player.AcquiredLeaders.Count,
                RemainingReservedCards = player.ReservedCards.Count,
                RemainingTokens = player.Store.ToArray().Sum()
            };
        }
    }
}
