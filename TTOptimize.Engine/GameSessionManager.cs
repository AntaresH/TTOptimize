using Microsoft.Extensions.DependencyInjection;
using TTOptimize.Core;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Engine;

public class GameSessionManager
{
    private readonly IServiceProvider _rootServiceProvider;

    public GameSessionManager(IServiceProvider rootServiceProvider)
    {
        _rootServiceProvider = rootServiceProvider;
    }

    public void StartNewMatch(string gameId, IGameConfiguration configuration, IReadOnlyList<IPlayerAgent> agents)
    {
        using (IServiceScope scope = _rootServiceProvider.CreateScope())
        {
            IServiceProvider scopeProvider = scope.ServiceProvider;

            var stateFactory = scopeProvider.GetRequiredKeyedService<IGameStateFactory>(gameId);
            var rules = scopeProvider.GetRequiredKeyedService<IGameRules>(gameId);
            var view = scopeProvider.GetRequiredKeyedService<IGameView>(gameId);
            var input = scopeProvider.GetRequiredKeyedService<IGameInput>(gameId);

            foreach(var agent in agents)
            {
                if (agent is HumanAgent hAgent)
                {
                    hAgent.SetInputMethod(input);
                }
            }

            var initialState = stateFactory.Create(configuration);

            var orchestrator = new GameOrchestrator(initialState, agents, rules, view);

            orchestrator.Run();
        }
    }
}