using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using System.Text.RegularExpressions;
using TTOptimize.ConsoleUI.Input;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.Input.Wizards;

public class ReserveFromBoardWizard : SplendidWondersBaseWizard
{
    public override string CommandKey => Strings.ReserveFromBoardCommandKey;

    public override string Description => Strings.ReserveFromBoardCommandDescription;

    public GameState _gameState;

    public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
        => validActions.OfType<ReserveFromBoardAction>().Any();

    public ReserveFromBoardWizard(
        ICommandTerminal commandTerminal,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] ISplendidWondersInputPresenter inputPresenter) : base(commandTerminal, inputPresenter)
    {
        _inputPresenter = inputPresenter;
    }

    public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
    {
        _gameState = (GameState)state;

        _inputPresenter.DisplaySubCommandBlock(CommandKey, Strings.PromptReserveFromBoard);

        return PromptForParameter();
    }

    protected override GameAction? ParseInput(string input)
    {
        Match match = Regex.Match(input, @"^([1-3]),([1-4])$");

        if (match.Success)
        {
            int level = int.Parse(match.Groups[1].Value);
            int column = int.Parse(match.Groups[2].Value);

            var cardToReserve = _gameState[level][column];

            if (cardToReserve != Cards.Empty)
            {
                return new ReserveFromBoardAction()
                {
                    Card = _gameState[level][column]
                };
            }
        }

        return null;
    }
}
