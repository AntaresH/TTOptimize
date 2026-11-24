using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Actions;
using System.Text.RegularExpressions;
using TTOptimize.ConsoleUI.Input;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.Setup;

namespace SplendidWonders.CLI.Input.Wizards;

public class PurchaseFromBoardWizard : SplendidWondersBaseWizard
{
    public override string CommandKey => Strings.PurchaseFromBoardCommandKey;

    public override string Description => Strings.PurchaseFromBoardCommandDescription;

    private IEnumerable<PurchaseAction> _availablePurchases;

    public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
        => validActions.OfType<PurchaseFromBoardAction>().Any();

    public PurchaseFromBoardWizard(
        ICommandTerminal commandTerminal,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] ISplendidWondersInputPresenter inputPresenter) : base(commandTerminal, inputPresenter)
    {
        _inputPresenter = inputPresenter;
    }

    public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
    {
        _availablePurchases = validActions.OfType<PurchaseFromBoardAction>();

        _inputPresenter.DisplaySubCommandBlock(CommandKey, Strings.PromptPurchaseFromBoard);
        _inputPresenter.DisplayPurchases(_availablePurchases);

        return PromptForParameter();
    }

    protected override GameAction? ParseInput(string input)
    {
        Match match = Regex.Match(input, @"^#?([\d]){1,2}$");

        if (match.Success)
        {
            int key = int.Parse(match.Groups[1].Value);

            var selectedPurchase = _availablePurchases.ElementAtOrDefault(key - 1);

            if (selectedPurchase != null)
            {
                return selectedPurchase;
            }
        }

        return null;
    }
}
