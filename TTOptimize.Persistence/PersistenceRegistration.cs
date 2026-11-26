using Microsoft.Extensions.DependencyInjection;
using TTOptimize.Core.Statistics;

namespace TTOptimize.Persistence;

public static class PersistenceRegistration
{
    public static IServiceCollection AddTTOptimizePersistence(
        this IServiceCollection services,
        string databaseFilePath)
    {
        services.AddSingleton<IMatchRepository>(_ => new LiteDbMatchRepository(databaseFilePath));
        return services;
    }
}
