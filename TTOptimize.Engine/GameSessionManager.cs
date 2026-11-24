using Microsoft.Extensions.DependencyInjection;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.ViewAbstractions;

namespace TTOptimize.Engine;

public class GameSessionManager
{
    private readonly IServiceProvider _rootServiceProvider;

    public GameSessionManager(IServiceProvider rootServiceProvider)
    {
        _rootServiceProvider = rootServiceProvider;
    }

    public MatchResult StartNewMatch(
        string gameId,
        IGameConfiguration configuration,
        IReadOnlyList<PlayerSlotConfig> slotConfigs)
    {
        using (IServiceScope scope = _rootServiceProvider.CreateScope())
        {
            IServiceProvider scopeProvider = scope.ServiceProvider;

            var stateFactory = scopeProvider.GetRequiredKeyedService<IGameStateFactory>(gameId);
            var rules = scopeProvider.GetRequiredKeyedService<IGameRules>(gameId);
            var input = scopeProvider.GetRequiredKeyedService<IGameInput>(gameId);
            var view = scopeProvider.GetRequiredKeyedService<IGameView>(gameId);

            IReadOnlyList<IPlayerAgent> agents = CreateAgents(slotConfigs, input);
            IGameState initialState = stateFactory.Create(configuration);

            var orchestrator = new GameOrchestrator(initialState, agents, rules);
            var session = new GameSession(orchestrator, rules, agents, view, input);

            return session.Execute();
        }
    }

    private static IReadOnlyList<IPlayerAgent> CreateAgents(
        IReadOnlyList<PlayerSlotConfig> slotConfigs,
        IGameInput input)
    {
        var agents = new List<IPlayerAgent>(slotConfigs.Count);

        foreach (PlayerSlotConfig slot in slotConfigs)
        {
            IPlayerAgent agent = slot.Type switch
            {
                AgentType.Human => new HumanAgent(slot.Name, slot.PlayerId, input),
                AgentType.Random => new RandomAgent(slot.Name, slot.PlayerId),
                _ => throw new ArgumentOutOfRangeException(nameof(slot.Type), $"Ismeretlen ágens típus: {slot.Type}")
            };

            agents.Add(agent);
        }

        return agents;
    }
}
