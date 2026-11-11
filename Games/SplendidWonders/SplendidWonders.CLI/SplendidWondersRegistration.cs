using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.CLI.Input;
using SplendidWonders.CLI.Input.Wizards;
using SplendidWonders.CLI.View;
using SplendidWonders.Gameplay;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Explorers;
using SplendidWonders.Gameplay.Reducers;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI
{
    public static class SplendidWondersRegistration
    {
        public static IServiceCollection AddSplendidWonders(this IServiceCollection services)
        {
            services.AddSingleton<IGameManifest, SplendidWondersManifest>();

            services.AddKeyedSingleton<IActionExplorer<DrawThreeTokensAction>, DrawThreeTokensExplorer>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IActionExplorer<DrawDoubleTokensAction>, DrawDoubleTokensExplorer>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IActionExplorer<ReturnTokensAction>, ReturnTokensExplorer>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IActionExplorer<ReserveFromBoardAction>, ReserveFromBoardExplorer>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IActionExplorer<ReserveFromDeckAction>, ReserveFromDeckExplorer>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IActionExplorer<PurchaseAction>, PurchaseExplorer>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IActionExplorer<SelectLeaderAction>, SelectLeaderExplorer>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IActionExplorer, ActionExplorer>(Games.SPLENDID_WONDERS_KEY);

            services.AddKeyedSingleton<TokenActionReducer>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IStateReducer<DrawThreeTokensAction>>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<TokenActionReducer>(key));
            services.AddKeyedSingleton<IStateReducer<DrawDoubleTokensAction>>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<TokenActionReducer>(key));
            services.AddKeyedSingleton<IStateReducer<ReturnTokensAction>>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<TokenActionReducer>(key));

            services.AddKeyedSingleton<ReserveActionReducer>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IStateReducer<ReserveFromBoardAction>>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<ReserveActionReducer>(key));
            services.AddKeyedSingleton<IStateReducer<ReserveFromDeckAction>>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<ReserveActionReducer>(key));

            services.AddKeyedSingleton<IStateReducer<PurchaseAction>, PurchaseReducer>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IStateReducer<SelectLeaderAction>, SelectLeaderReducer>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IStateReducer, StateReducer>(Games.SPLENDID_WONDERS_KEY);

            services.AddKeyedSingleton<IGameRules, Rules>(Games.SPLENDID_WONDERS_KEY);

            services.AddKeyedSingleton<ConsoleGameView>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IGameView>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<ConsoleGameView>(key));
            services.AddKeyedSingleton<ISplendidWondersView>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<ConsoleGameView>(key));

            services.AddKeyedSingleton<IConsoleCommandWizard, DrawDoubleTokensCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IConsoleCommandWizard, DrawThreeTokensCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IConsoleCommandWizard, PurchaseFromBoardCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IConsoleCommandWizard, PurchaseFromReservedCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IConsoleCommandWizard, ReserveFromBoardCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IConsoleCommandWizard, ReserveFromDeckCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IConsoleCommandWizard, ReturnTokensCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IConsoleCommandWizard, SelectLeaderCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IConsoleCommandWizard, ShowReservedCardsCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedSingleton<IGameInput, ConsoleGameInput>(Games.SPLENDID_WONDERS_KEY);

            services.AddKeyedSingleton<IGameStateFactory, StateFactory>(Games.SPLENDID_WONDERS_KEY);

            return services;
        }
    }
}
