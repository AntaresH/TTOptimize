using Microsoft.Extensions.DependencyInjection;
using TTOptimize.Core;

namespace TTOptimize.Platform;

public class GameSessionManager
{
    private readonly IServiceProvider _rootServiceProvider;

    public GameSessionManager(IServiceProvider rootServiceProvider)
    {
        _rootServiceProvider = rootServiceProvider;
    }

    public void StartNewMatch(string gameId, IGameConfiguration configuration)
    {
        using (IServiceScope scope = _rootServiceProvider.CreateScope())
        {
            IServiceProvider scopeProvider = scope.ServiceProvider;

            var stateFactory = scopeProvider.GetRequiredKeyedService<IGameStateFactory>(gameId);
            var rules = scopeProvider.GetRequiredKeyedService<IGameRules>(gameId);
            var view = scopeProvider.GetRequiredKeyedService<IGameView>(gameId);
            var input = scopeProvider.GetRequiredKeyedService<IGameInput>(gameId);

            var agents = new List<IPlayerAgent>()
            {
                new HumanAgent(input, "Atanáziusz"),
                new HumanAgent(input, "Brünhilda")
            };

            configuration.PlayerAgents = agents;

            var orchestrator = new GameOrchestrator(configuration, stateFactory, rules, view);

            orchestrator.Run();

            Console.ReadLine();
        }
    }
}