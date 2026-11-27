using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Actions;
using SplendidWonders.Gameplay.Explorers;
using SplendidWonders.Gameplay.Reducers;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.Statistics;
using TTOptimize.Core.Setup;

namespace SplendidWonders.Gameplay;

public static class Registration
{
    public static IServiceCollection AddSplendidWondersGameplay(this IServiceCollection services)
    {
        services.AddScoped<IActionExplorer<DrawThreeTokensAction>, DrawThreeTokensExplorer>();
        services.AddScoped<IActionExplorer<DrawDoubleTokensAction>, DrawDoubleTokensExplorer>();
        services.AddScoped<IActionExplorer<ReturnTokensAction>, ReturnTokensExplorer>();
        services.AddScoped<IActionExplorer<ReserveFromBoardAction>, ReserveFromBoardExplorer>();
        services.AddScoped<IActionExplorer<ReserveFromDeckAction>, ReserveFromDeckExplorer>();
        services.AddScoped<IActionExplorer<PurchaseAction>, PurchaseExplorer>();
        services.AddScoped<IActionExplorer<SelectLeaderAction>, SelectLeaderExplorer>();

        services.AddScoped<TokenActionReducer>();
        services.AddScoped<IStateReducer<DrawThreeTokensAction>>((sp) => sp.GetRequiredService<TokenActionReducer>());
        services.AddScoped<IStateReducer<DrawDoubleTokensAction>>((sp) => sp.GetRequiredService<TokenActionReducer>());
        services.AddScoped<IStateReducer<ReturnTokensAction>>((sp) => sp.GetRequiredService<TokenActionReducer>());

        services.AddScoped<ReserveActionReducer>();
        services.AddScoped<IStateReducer<ReserveFromBoardAction>>((sp) => sp.GetRequiredService<ReserveActionReducer>());
        services.AddScoped<IStateReducer<ReserveFromDeckAction>>((sp) => sp.GetRequiredService<ReserveActionReducer>());
        services.AddScoped<IStateReducer<PurchaseAction>, PurchaseReducer>();
        services.AddScoped<IStateReducer<SelectLeaderAction>, SelectLeaderReducer>();
        services.AddScoped<IStateReducer<PassAction>, PassActionReducer>();

        services.AddKeyedScoped<IGameStateFactory, StateFactory>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IActionExplorer, ActionExplorer>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IStateReducer, StateReducer>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IGameRules, Rules>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IMatchMetricsExtractor, SplendidWondersMetricsExtractor>(Games.SPLENDID_WONDERS_KEY);

        return services;
    }
}
