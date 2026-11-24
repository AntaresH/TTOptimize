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
using TTOptimize.Core.Setup;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI;

public static class SplendidWondersRegistration
{
    public static IServiceCollection AddSplendidWonders(this IServiceCollection services)
    {
        services.AddSingleton<IGameManifest, SplendidWondersManifest>();

        services.AddKeyedScoped<IActionExplorer<DrawThreeTokensAction>, DrawThreeTokensExplorer>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IActionExplorer<DrawDoubleTokensAction>, DrawDoubleTokensExplorer>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IActionExplorer<ReturnTokensAction>, ReturnTokensExplorer>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IActionExplorer<ReserveFromBoardAction>, ReserveFromBoardExplorer>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IActionExplorer<ReserveFromDeckAction>, ReserveFromDeckExplorer>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IActionExplorer<PurchaseAction>, PurchaseExplorer>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IActionExplorer<SelectLeaderAction>, SelectLeaderExplorer>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IActionExplorer, ActionExplorer>(Games.SPLENDID_WONDERS_KEY);

        services.AddKeyedScoped<TokenActionReducer>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IStateReducer<DrawThreeTokensAction>>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<TokenActionReducer>(key));
        services.AddKeyedScoped<IStateReducer<DrawDoubleTokensAction>>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<TokenActionReducer>(key));
        services.AddKeyedScoped<IStateReducer<ReturnTokensAction>>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<TokenActionReducer>(key));

        services.AddKeyedScoped<ReserveActionReducer>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IStateReducer<ReserveFromBoardAction>>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<ReserveActionReducer>(key));
        services.AddKeyedScoped<IStateReducer<ReserveFromDeckAction>>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<ReserveActionReducer>(key));

        services.AddKeyedScoped<IStateReducer<PurchaseAction>, PurchaseReducer>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IStateReducer<SelectLeaderAction>, SelectLeaderReducer>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IStateReducer<PassAction>, PassActionReducer>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IStateReducer, StateReducer>(Games.SPLENDID_WONDERS_KEY);

        services.AddKeyedScoped<IGameRules, Rules>(Games.SPLENDID_WONDERS_KEY);

        //services.AddKeyedScoped<ConsoleGameView>(Games.SPLENDID_WONDERS_KEY);
        //services.AddKeyedScoped<IGameView>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<ConsoleGameView>(key));
        //services.AddKeyedScoped<ISplendidWondersView>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<ConsoleGameView>(key));

        services.AddKeyedScoped<LeaderAreaWidget>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<TokenSupplyWidget>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<DecksWidget>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<MarketWidget>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<PlayerAreaWidget>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IConxelMasterLayout, MasterLayout>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IConxelPaneBuilder<BoardRenderData>, BoardBuilder>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IConxelPaneBuilder<EventLogRenderData>, EventLogBuilder>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IConxelPaneBuilder<ResultsRenderData>, ResultsBuilder>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IConxelPaneBuilder<CommandRenderData>, CommandPaneBuilder>(Games.SPLENDID_WONDERS_KEY,
            (_, _) => new CommandPaneBuilder(LayoutConstants.COMMANDSPANEHEIGHT, LayoutConstants.BOARDWIDTH + LayoutConstants.PANEGAP + LayoutConstants.EVENTSWIDTH));

        services.AddKeyedScoped<ITerminalWizard, DrawDoubleTokensWizard>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<ITerminalWizard, DrawThreeTokensWizard>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<ITerminalWizard, PurchaseFromBoardWizard>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<ITerminalWizard, PurchaseFromReservedWizard>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<ITerminalWizard, ReserveFromBoardWizard>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<ITerminalWizard, ReserveFromDeckWizard>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<ITerminalWizard, ReturnTokensWizard>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<ITerminalWizard, PassWizard>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<ITerminalWizard, SelectLeaderWizard>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<ITerminalWizard, ShowReservedCardsWizard>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<ISplendidWondersInputPresenter, SplendidWondersInputPresenter>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IGameInput, SplendidWondersInput>(Games.SPLENDID_WONDERS_KEY);

        services.AddKeyedScoped<IGameStateFactory, StateFactory>(Games.SPLENDID_WONDERS_KEY);

        return services;
    }
}
