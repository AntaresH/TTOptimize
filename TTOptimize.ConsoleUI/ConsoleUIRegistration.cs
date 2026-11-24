using Microsoft.Extensions.DependencyInjection;
using TTOptimize.ConsoleUI.Input;
using TTOptimize.ConsoleUI.View;
using TTOptimize.Core.ViewAbstractions;

public static class ConsoleUIRegistration
{
    public static IServiceCollection AddTTOptimizeConsoleUI(this IServiceCollection services)
    {
        services.AddSingleton<ConxelCompiler>();
        services.AddScoped<ICommandTerminal, ConxelTerminal>();
        return services;
    }

    public static IServiceCollection AddConxelViewForGame(this IServiceCollection services, string gameKey)
    {
        services.AddKeyedScoped<IGameView>(gameKey, (sp, key) => new ConxelView(
            sp.GetRequiredService<ICommandTerminal>(),
            sp.GetRequiredKeyedService<IConxelMasterLayout>(key),
            sp.GetRequiredService<ConxelCompiler>(),
            sp.GetRequiredKeyedService<IConxelPaneBuilder<BoardRenderData>>(key),
            sp.GetRequiredKeyedService<IConxelPaneBuilder<EventLogRenderData>>(key),
            sp.GetRequiredKeyedService<IConxelPaneBuilder<ResultsRenderData>>(key),
            sp.GetRequiredKeyedService<IConxelPaneBuilder<CommandRenderData>>(key)
        ));

        return services;
    }
}