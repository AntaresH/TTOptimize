using Microsoft.Extensions.DependencyInjection;

namespace TTOptimize.Engine;

public static class EngineRegistration
{
    public static IServiceCollection AddTTOptimizeEngine(this IServiceCollection services)
    {
        services.AddTransient<GameSessionManager>();

        return services;
    }
}