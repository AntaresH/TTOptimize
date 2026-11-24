using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using System.Text.RegularExpressions;
using TTOptimize.ConsoleUI.Input;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.Setup;

namespace SplendidWonders.CLI.Input.Wizards;

public class PurchaseFromReservedWizard : SplendidWondersBaseWizard
{
    public override string CommandKey => Strings.PurchaseFromReservedCommandKey;

    public override string Description => Strings.PurchaseFromReservedCommandDescription;

    public IEnumerable<PurchaseAction> _availablePurchases;

    public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
        => validActions.OfType<PurchaseFromReservedAction>().Any();

    public PurchaseFromReservedWizard(
        ICommandTerminal commandTerminal,
        ISplendidWondersInputPresenter inputPresenter) : base(commandTerminal, inputPresenter)
    {
        _inputPresenter = inputPresenter;
    }

    public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
    {
        var swGameState = ((GameState)state);

        var allReservedCards = swGameState.ActivePlayer.ReservedCards;

        _availablePurchases = validActions
            .OfType<PurchaseFromReservedAction>()
            .Where(pa => allReservedCards.Contains(pa.Card));

        _inputPresenter.DisplaySubCommandBlock(CommandKey, Strings.PromptPurchaseFromReserved);
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
