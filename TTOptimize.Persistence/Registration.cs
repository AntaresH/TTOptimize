using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using TTOptimize.Core.Statistics;

namespace TTOptimize.Persistence;

public static class Registration
{
    public static IServiceCollection AddTTOptimizePersistence(
        this IServiceCollection services,
        string databaseFilePath)
    {
        services.AddSingleton(_ => new LiteDatabase(databaseFilePath));
        services.AddSingleton<IMatchRepository, LiteDbMatchRepository>();
        services.AddSingleton<IStatisticsRepository, LiteDbStatisticsRepository>();

        return services;
    }
}
