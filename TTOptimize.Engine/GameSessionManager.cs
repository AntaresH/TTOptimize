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

    private readonly IGameContextFactory _gameContextFactory;
    private readonly MatchMetricsCollector _metricsCollector;
    private readonly ISettingsService? _settingsService;
    private readonly IMatchRepository? _matchRepository;

    public GameSessionManager(IGameContextFactory gameContextFactory, MatchMetricsCollector metricsCollector, 
        ISettingsService settingsService, IMatchRepository? matchRepository)
    {
        _gameContextFactory = gameContextFactory;
        _metricsCollector = metricsCollector;
        _settingsService = settingsService;
        _matchRepository = matchRepository;

        _isInteractive = _settingsService.IsInteractive;
    }
    public SessionResult RunSingleMatch(string gameId, IGameConfiguration configuration, IReadOnlyList<PlayerSlotConfig> slotConfigs)
    {
        var completedMatch = ExecuteMatch(gameId, configuration, slotConfigs, batchRunId: null);

        _matchRepository?.SaveMatch(completedMatch.Record);

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

        return new BatchExecutionResult(batchRunId, matchCount, Stopwatch.GetElapsedTime(startTimestamp));
    }

    private CompletedMatch ExecuteMatch(string gameId, IGameConfiguration configuration, IReadOnlyList<PlayerSlotConfig> slotConfigs, Guid? batchRunId)
    {
        using GameContext context = _gameContextFactory.CreateContext(gameId);

        var agentsBySeat = CreateAgents(slotConfigs, context.Input);
        IGameState initialState = context.StateFactory.Create(configuration);

        var orchestrator = new GameOrchestrator(initialState, agentsBySeat, context.Rules);
        var session = new GameSession(orchestrator, context.Rules, context.View, context.Input);

        SessionResult result = session.Execute();

        MatchRecord record = _metricsCollector.Collect(gameId, batchRunId, configuration, result.Outcome, agentsBySeat, context.MetricsExtractor);

        return new CompletedMatch(result, record);
    }

    private IReadOnlyDictionary<int, IAgent> CreateAgents(
        IReadOnlyList<PlayerSlotConfig> slotConfigs,
        IGameInput input)
    {
        var agentsBySeat = new Dictionary<int, IAgent>(slotConfigs.Count);

        for (int i = 0; i < slotConfigs.Count; i++)
        {
            var slot = slotConfigs[i];

            IAgent agent = slot.Type switch
            {
                AgentType.Human => new HumanAgent(slot.Name, input),
                AgentType.Random => new RandomAgent(slot.Name),
                _ => throw new ArgumentOutOfRangeException(nameof(slot.Type), $"Unknown agent type: {slot.Type}")
            };

            agentsBySeat.Add(i + 1, agent);

        }

        foreach (PlayerSlotConfig slot in slotConfigs)
        {
        }

        return agentsBySeat;
    }
}
