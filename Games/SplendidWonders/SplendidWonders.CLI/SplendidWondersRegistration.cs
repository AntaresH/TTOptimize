using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.CLI.Input;
using SplendidWonders.CLI.Input.Wizards;
using SplendidWonders.CLI.View;
using SplendidWonders.CLI.View.Legacy;
using SplendidWonders.CLI.View.Widgets;
using SplendidWonders.Common.Actions;
using SplendidWonders.Gameplay;
using SplendidWonders.Gameplay.Explorers;
using SplendidWonders.Gameplay.Reducers;
using TTOptimize.ConsoleUI.View;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.Statistics;
using TTOptimize.Core.Setup;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI;

public static class SplendidWondersRegistration
{
    public static IServiceCollection AddSplendidWonders(this IServiceCollection services)
    {
        services.AddSingleton<IGameManifest, SplendidWondersManifest>();

        services.AddScoped<LeaderAreaWidget>();
        services.AddScoped<TokenSupplyWidget>();
        services.AddScoped<DecksWidget>();
        services.AddScoped<MarketWidget>();
        services.AddScoped<PlayerAreaWidget>();

        services.AddScoped<ITerminalWizard, DrawDoubleTokensWizard>();
        services.AddScoped<ITerminalWizard, DrawThreeTokensWizard>();
        services.AddScoped<ITerminalWizard, PurchaseFromBoardWizard>();
        services.AddScoped<ITerminalWizard, PurchaseFromReservedWizard>();
        services.AddScoped<ITerminalWizard, ReserveFromBoardWizard>();
        services.AddScoped<ITerminalWizard, ReserveFromDeckWizard>();
        services.AddScoped<ITerminalWizard, ReturnTokensWizard>();
        services.AddScoped<ITerminalWizard, PassWizard>();
        services.AddScoped<ITerminalWizard, SelectLeaderWizard>();
        services.AddScoped<ITerminalWizard, ShowReservedCardsWizard>();
        services.AddScoped<ISplendidWondersInputPresenter, SplendidWondersInputPresenter>();

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
        services.AddKeyedScoped<IGameInput, SplendidWondersInput>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IMatchMetricsExtractor, SplendidWondersMetricsExtractor>(Games.SPLENDID_WONDERS_KEY);


        services.AddKeyedScoped<IConxelMasterLayout, MasterLayout>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IConxelPaneBuilder<BoardRenderData>, BoardBuilder>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IConxelPaneBuilder<EventLogRenderData>, EventLogBuilder>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IConxelPaneBuilder<ResultsRenderData>, ResultsBuilder>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IConxelPaneBuilder<CommandRenderData>, CommandPaneBuilder>(Games.SPLENDID_WONDERS_KEY,
            (_, _) => new CommandPaneBuilder(LayoutConstants.COMMANDSPANEHEIGHT, LayoutConstants.BOARDWIDTH + LayoutConstants.PANEGAP + LayoutConstants.EVENTSWIDTH));

        return services;
    }
}
