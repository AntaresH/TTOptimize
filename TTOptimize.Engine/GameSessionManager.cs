using Microsoft.Extensions.DependencyInjection;
using TTOptimize.Core;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.Statistics;
using TTOptimize.Core.ViewAbstractions;

namespace TTOptimize.Engine;

public class GameSessionManager
{
    private readonly IServiceProvider _rootServiceProvider;

    public GameSessionManager(IServiceProvider rootServiceProvider)
    {
        _rootServiceProvider = rootServiceProvider;
    }

    public SessionResult StartNewMatch(
        string gameId,
        IGameConfiguration configuration,
        IReadOnlyList<PlayerSlotConfig> slotConfigs)
    {
        using (IServiceScope scope = _rootServiceProvider.CreateScope())
        {
            IServiceProvider scopeProvider = scope.ServiceProvider;

            var settings = _rootServiceProvider.GetRequiredService<ISettingsService>();

            var stateFactory = scopeProvider.GetRequiredKeyedService<IGameStateFactory>(gameId);
            var rules = scopeProvider.GetRequiredKeyedService<IGameRules>(gameId);
            var input = scopeProvider.GetRequiredKeyedService<IGameInput>(gameId);
            var view = settings.IsInteractive ? scopeProvider.GetRequiredKeyedService<IGameView>(gameId) : null;

            IReadOnlyDictionary<int, IPlayerAgent> agentsBySeat = CreateAgents(slotConfigs, input);
            IGameState initialState = stateFactory.Create(configuration);

            var orchestrator = new GameOrchestrator(initialState, agentsBySeat, rules);
            var session = new GameSession(orchestrator, rules, agentsBySeat, view, input);
            var result = session.Execute();

            var collector = new MatchMetricsCollector(scopeProvider);
            var matchRecord = collector.Collect(gameId, experimentId: null, configuration, result.Outcome, agentsBySeat);

            //if (autoPersist)
            //{
            //    var matchRepository = scopeProvider.GetService<IMatchRepository>();
            //    matchRepository?.SaveMatch(matchRecord);
            //}

            return result;
        }
    }

    private IReadOnlyDictionary<int, IPlayerAgent> CreateAgents(
        IReadOnlyList<PlayerSlotConfig> slotConfigs,
        IGameInput input)
    {
        var agentsBySeat = new Dictionary<int, IPlayerAgent>(slotConfigs.Count);

        for (int i = 0; i < slotConfigs.Count; i++)
        {
            var slot = slotConfigs[i];

            IPlayerAgent agent = slot.Type switch
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
