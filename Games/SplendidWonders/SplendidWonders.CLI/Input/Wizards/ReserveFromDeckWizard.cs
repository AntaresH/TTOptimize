using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using System.Text.RegularExpressions;
using TTOptimize.ConsoleUI.Input;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.Input.Wizards;

public class ReserveFromDeckWizard : SplendidWondersBaseWizard
{
    public override string CommandKey => Strings.ReserveFromDeckCommandKey;

    public override string Description => Strings.ReserveFromDeckCommandDescription;

    public GameState _gameState;

    public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
        => validActions.OfType<ReserveFromDeckAction>().Any();

    public ReserveFromDeckWizard(
        ICommandTerminal commandTerminal,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] ISplendidWondersInputPresenter inputPresenter) : base(commandTerminal, inputPresenter)
    {
        _inputPresenter = inputPresenter;
    }

    public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
    {
        _gameState = (GameState)state;

        _inputPresenter.DisplaySubCommandBlock(CommandKey, Strings.PromptReserveFromDeck);

        return PromptForParameter();
    }

    protected override GameAction? ParseInput(string input)
    {
        Match match = Regex.Match(input, @"^([1-3])$");

        if (match.Success)
        {
            int level = int.Parse(match.Groups[1].Value);

            return new ReserveFromDeckAction()
            {
                Level = level
            };
        }

        return null;
    }
}
