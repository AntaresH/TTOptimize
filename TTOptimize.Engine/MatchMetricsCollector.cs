using Microsoft.Extensions.DependencyInjection;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.Statistics;

namespace TTOptimize.Engine;

public class MatchMetricsCollector
{
    private readonly IServiceProvider _serviceProvider;

    public MatchMetricsCollector(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public MatchRecord Collect(
        string gameId,
        Guid? experimentId,
        IGameConfiguration configuration,
        MatchOutcome outcome,
        IReadOnlyDictionary<int, IPlayerAgent> agentsBySeat)
    {
        var gameExtractor = _serviceProvider.GetKeyedService<IMatchMetricsExtractor>(gameId);

        var playerMetricsList = new List<IPlayerMetrics>(outcome.Standings.Count);

        foreach (IPlayerResult standing in outcome.Standings)
        {
            IPlayerState player = standing.Player;

            var playerActions = outcome.ActionHistory
                .Where(entry => entry.Player.Seat == player.Seat)
                .ToList();

            int turnCount = playerActions.Count;

            var actionCounts = playerActions
                .GroupBy(entry => entry.Action.GetType().Name)
                .ToDictionary(group => group.Key, group => group.Count());

            var actionRatios = actionCounts.ToDictionary(
                kvp => kvp.Key,
                kvp => turnCount > 0 ? (double)kvp.Value / turnCount : 0.0);

            var baseMetrics = new BasePlayerMetrics
            {
                Seat = player.Seat,
                StatisticsSubjectId = agentsBySeat[player.Seat].StatisticsSubjectId, 
                Rank = standing.Rank,
                IsWinner = standing.Rank == 1,
                VictoryPoints = standing.Points,
                TurnCount = turnCount,
                ActionCounts = actionCounts,
                ActionRatios = actionRatios
            };

            IPlayerMetrics finalPlayerMetrics = gameExtractor != null
                ? gameExtractor.ExtractPlayerMetrics(baseMetrics, outcome.FinalState)
                : baseMetrics;

            playerMetricsList.Add(finalPlayerMetrics);
        }

        var baseGameMetrics = new BaseGameMetrics
        {
            TotalTurns = outcome.ActionHistory.Count
        };

        var finalGameMetrics = gameExtractor != null
            ? gameExtractor.ExtractGameMetrics(baseGameMetrics, outcome.FinalState, outcome.ActionHistory)
            : baseGameMetrics;

        return new MatchRecord
        {
            GameId = gameId,
            ExperimentId = experimentId,
            GameOverReason = outcome.FinalState.GameOverReason.ToString(),
            GameConfiguration = configuration,
            PlayerMetrics = playerMetricsList,
            GameMetrics = finalGameMetrics
        };
    }
}
