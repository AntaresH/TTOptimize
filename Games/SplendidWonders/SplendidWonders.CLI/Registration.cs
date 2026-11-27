using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.CLI.Input;
using SplendidWonders.CLI.Input.Wizards;
using SplendidWonders.CLI.View;
using SplendidWonders.CLI.View.Widgets;
using TTOptimize.ConsoleUI.View;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.Setup;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI;

public static class Registration
{
    public static IServiceCollection AddSplendidWondersCLI(this IServiceCollection services)
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

        services.AddKeyedScoped<IGameInput, SplendidWondersInput>(Games.SPLENDID_WONDERS_KEY);

        services.AddKeyedScoped<IConxelMasterLayout, MasterLayout>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IConxelPaneBuilder<BoardRenderData>, BoardBuilder>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IConxelPaneBuilder<EventLogRenderData>, EventLogBuilder>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IConxelPaneBuilder<ResultsRenderData>, ResultsBuilder>(Games.SPLENDID_WONDERS_KEY);
        services.AddKeyedScoped<IConxelPaneBuilder<CommandRenderData>, CommandPaneBuilder>(Games.SPLENDID_WONDERS_KEY,
            (_, _) => new CommandPaneBuilder(LayoutConstants.COMMANDSPANEHEIGHT, LayoutConstants.BOARDWIDTH + LayoutConstants.PANEGAP + LayoutConstants.EVENTSWIDTH));

        return services;
    }
}
