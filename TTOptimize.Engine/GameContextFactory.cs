using Microsoft.Extensions.DependencyInjection;
using TTOptimize.Core;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.Statistics;
using TTOptimize.Core.ViewAbstractions;

namespace TTOptimize.Engine;

public class GameContextFactory : IGameContextFactory
{
    private readonly IServiceProvider _rootServiceProvider;
    private readonly bool _isInteractive;

    public GameContextFactory(IServiceProvider rootServiceProvider, ISettingsService settingsService)
    {
        _rootServiceProvider = rootServiceProvider;
        _isInteractive = settingsService.IsGameInteractive;
    }

    public GameContext CreateContext(string gameId)
    {
        IServiceScope scope = _rootServiceProvider.CreateScope();
        IServiceProvider sp = scope.ServiceProvider;

        var stateFactory = sp.GetRequiredKeyedService<IGameStateFactory>(gameId);
        var rules = sp.GetRequiredKeyedService<IGameRules>(gameId);
        var agentFactory = sp.GetRequiredKeyedService<IAgentFactory>(gameId);
        var metricsExtractor = sp.GetKeyedService<IMatchMetricsExtractor>(gameId);

        IGameView? view = _isInteractive ? sp.GetRequiredKeyedService<IGameView>(gameId) : null;
        IGameInput? input = _isInteractive ? sp.GetRequiredKeyedService<IGameInput>(gameId) : null;

        return new GameContext(stateFactory, rules, agentFactory, metricsExtractor, view, input, scope);
    }
}
