using Microsoft.Extensions.DependencyInjection;
using TTOptimize.Core.Agents;
using TTOptimize.Core.Setup;

namespace TTOptimize.Engine;

public static class Registration
{
    public static IServiceCollection AddTTOptimizeEngine(this IServiceCollection services)
    {
        services.AddTransient<GameSessionManager>();
        services.AddSingleton<MatchMetricsCollector>();
        services.AddSingleton<MatchStatisticsAggregator>();
        services.AddSingleton<IGameContextFactory, GameContextFactory>();

        return services;
    }
}