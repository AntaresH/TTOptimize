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
            services.AddKeyedScoped<IStateReducer, StateReducer>(Games.SPLENDID_WONDERS_KEY);

            services.AddKeyedScoped<IGameRules, Rules>(Games.SPLENDID_WONDERS_KEY);

            services.AddKeyedScoped<ConsoleGameView>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedScoped<IGameView>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<ConsoleGameView>(key));
            services.AddKeyedScoped<ISplendidWondersView>(Games.SPLENDID_WONDERS_KEY, (sp, key) => sp.GetRequiredKeyedService<ConsoleGameView>(key));

            services.AddKeyedScoped<IConsoleCommandWizard, DrawDoubleTokensCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedScoped<IConsoleCommandWizard, DrawThreeTokensCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedScoped<IConsoleCommandWizard, PurchaseFromBoardCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedScoped<IConsoleCommandWizard, PurchaseFromReservedCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedScoped<IConsoleCommandWizard, ReserveFromBoardCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedScoped<IConsoleCommandWizard, ReserveFromDeckCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedScoped<IConsoleCommandWizard, ReturnTokensCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedScoped<IConsoleCommandWizard, SelectLeaderCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedScoped<IConsoleCommandWizard, ShowReservedCardsCommandWizard>(Games.SPLENDID_WONDERS_KEY);
            services.AddKeyedScoped<IGameInput, ConsoleGameInput>(Games.SPLENDID_WONDERS_KEY);

            services.AddKeyedScoped<IGameStateFactory, StateFactory>(Games.SPLENDID_WONDERS_KEY);

            return services;
        }
    }
}
