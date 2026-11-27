using Microsoft.Extensions.DependencyInjection;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.Statistics;

namespace TTOptimize.Engine;

public class MatchMetricsCollector
{
    public MatchRecord Collect(string gameId, Guid? batchRunId, IGameConfiguration configuration, MatchOutcome outcome,
        IReadOnlyDictionary<int, IAgent> agentsBySeat, IMatchMetricsExtractor metricsExtractor)
    {
        var playerMetricsList = new List<IPlayerMetrics>(outcome.Standings.Count);

        foreach (IPlayerResult standing in outcome.Standings)
        {
            IPlayer player = standing.Player;

            var playerActions = outcome.ActionHistory
                .Where(entry => entry.Player.Seat == player.Seat)
                .ToList();

            int turnCount = playerActions.Count;

            var executedActionCounts = playerActions
                .GroupBy(entry => entry.Action.GetType().Name)
                .ToDictionary(group => group.Key, group => group.Count());
            
            var allActionTypes = metricsExtractor?.KnownActionTypes != null
                ? metricsExtractor.KnownActionTypes.Union(executedActionCounts.Keys)
                : executedActionCounts.Keys;

            var actionCounts = allActionTypes.ToDictionary(
                actionName => actionName,
                actionName => executedActionCounts.GetValueOrDefault(actionName, 0));

            var actionRatios = executedActionCounts.ToDictionary(
                kvp => kvp.Key,
                kvp => turnCount > 0 ? (double)kvp.Value / turnCount : 0.0);

            var baseMetrics = new BasePlayerMetrics
            {
                Seat = player.Seat,
                SubjectId = agentsBySeat[player.Seat].StatisticsSubjectId, 
                Rank = standing.Rank,
                IsWinner = standing.Rank == 1,
                VictoryPoints = standing.Points,
                TurnCount = turnCount,
                ActionCounts = executedActionCounts,
                ActionRatios = actionRatios
            };

            IPlayerMetrics finalPlayerMetrics = metricsExtractor != null
                ? metricsExtractor.ExtractPlayerMetrics(baseMetrics, outcome.FinalState)
                : baseMetrics;

            playerMetricsList.Add(finalPlayerMetrics);
        }

        var baseGameMetrics = new BaseGameMetrics
        {
            TotalTurns = outcome.ActionHistory.Count
        };

        var finalGameMetrics = metricsExtractor != null
            ? metricsExtractor.ExtractGameMetrics(baseGameMetrics, outcome.FinalState, outcome.ActionHistory)
            : baseGameMetrics;

        return new MatchRecord
        {
            GameId = gameId,
            BatchRunId = batchRunId,
            GameOverReason = outcome.FinalState.GameOverReason.ToString(),
            GameConfiguration = configuration,
            PlayerMetrics = playerMetricsList,
            GameMetrics = finalGameMetrics
        };
    }
}
