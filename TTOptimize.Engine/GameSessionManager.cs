using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using TTOptimize.Core;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.Statistics;
using TTOptimize.Core.ViewAbstractions;

namespace TTOptimize.Engine;

public class GameSessionManager
{
    private bool _isInteractive;

    private readonly ISettingsService? _settingsService;
    private readonly IGameContextFactory _gameContextFactory;
    private readonly IMatchRepository? _matchRepository;
    private readonly MatchMetricsCollector _metricsCollector;
    private readonly MatchStatisticsAggregator _statisticsAggregator;

    public GameSessionManager(ISettingsService settingsService, IGameContextFactory gameContextFactory, IMatchRepository? matchRepository, 
        MatchMetricsCollector metricsCollector, MatchStatisticsAggregator statisticsAggregator = null)
    {
        _settingsService = settingsService;
        _gameContextFactory = gameContextFactory;
        _matchRepository = matchRepository;

        _metricsCollector = metricsCollector;
        _statisticsAggregator = statisticsAggregator;

        _isInteractive = _settingsService.IsGameInteractive;
    }
    public SessionResult RunSingleMatch(string gameId, IGameConfiguration configuration, IReadOnlyList<PlayerSlotConfig> slotConfigs)
    {
        var completedMatch = ExecuteMatch(gameId, configuration, slotConfigs, batchRunId: null);

        _matchRepository?.SaveMatch(completedMatch.Record);
        _statisticsAggregator?.ProcessMatches([completedMatch.Record]);

        return completedMatch.SessionResult;
    }

    public BatchExecutionResult RunBatch(string gameId, IGameConfiguration configuration, IReadOnlyList<PlayerSlotConfig> slotConfigs, int matchCount)
    {
        Guid batchRunId = Guid.NewGuid();
        var records = new List<MatchRecord>(matchCount);
        long startTimestamp = Stopwatch.GetTimestamp();

        for (int i = 0; i < matchCount; i++)
        {
            var completedMatch = ExecuteMatch(gameId, configuration, slotConfigs, batchRunId);

            records.Add(completedMatch.Record);
        }

        _matchRepository?.SaveMatchesBulk(records);
        _statisticsAggregator?.ProcessMatches(records);

        return new BatchExecutionResult(batchRunId, matchCount, Stopwatch.GetElapsedTime(startTimestamp));
    }

    private CompletedMatch ExecuteMatch(string gameId, IGameConfiguration configuration, IReadOnlyList<PlayerSlotConfig> slotConfigs, Guid? batchRunId)
    {
        using GameContext context = _gameContextFactory.CreateContext(gameId);

        var agentsBySeat = CreateAgents(slotConfigs, context.AgentFactory, context.Input);
        IGameState initialState = context.StateFactory.Create(configuration);

        var orchestrator = new GameOrchestrator(initialState, agentsBySeat, context.Rules);
        var session = new GameSession(orchestrator, context.Rules, context.View, context.Input);

        SessionResult result = session.Execute();

        MatchRecord record = _metricsCollector.Collect(gameId, batchRunId, configuration, result.Outcome, agentsBySeat, context.MetricsExtractor);

        return new CompletedMatch(result, record);
    }

    private IReadOnlyDictionary<int, IAgent> CreateAgents(IReadOnlyList<PlayerSlotConfig> slotConfigs, IAgentFactory agentFactory, IGameInput? input)
    {
        var agentsBySeat = new Dictionary<int, IAgent>(slotConfigs.Count);

        foreach (PlayerSlotConfig slot in slotConfigs)
        {
            agentsBySeat[slot.Seat] = agentFactory.CreateAgent(slot.AgentTypeId, slot.DisplayName, input);
        }

        return agentsBySeat;
    }
}
