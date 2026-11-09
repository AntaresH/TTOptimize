using Microsoft.Extensions.DependencyInjection;

namespace TTOptimize.Platform;

public static class PlatformEngineRegistration
{
    public static IServiceCollection AddTTOptimizeEngine(this IServiceCollection services)
    {
        services.AddSingleton<GameSessionManager>();
        //services.AddSingleton<ConsoleConfigurationEngine>();

        return services;
    }
}