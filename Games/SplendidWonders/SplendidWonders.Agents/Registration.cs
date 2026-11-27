using Microsoft.Extensions.DependencyInjection;
using TTOptimize.Core.Agents;
using TTOptimize.Core.Setup;

namespace SplendidWonders.Agents;

public static class Registration
{
    public static IServiceCollection AddSplendidWondersAgents(this IServiceCollection services)
    {
        services.AddKeyedScoped<IAgentFactory, SplendidWondersAgentFactory>(Games.SPLENDID_WONDERS_KEY);

        return services;
    }
}
