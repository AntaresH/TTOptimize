using Microsoft.Extensions.DependencyInjection;

namespace TTOptimize.Engine;

public static class EngineRegistration
{
    public static IServiceCollection AddTTOptimizeEngine(this IServiceCollection services)
    {
        services.AddTransient<GameSessionManager>();
        services.AddSingleton<MatchMetricsCollector>();
        services.AddSingleton<IGameContextFactory, GameContextFactory>();

        return services;
    }
}