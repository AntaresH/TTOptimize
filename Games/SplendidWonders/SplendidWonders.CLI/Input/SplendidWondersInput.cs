using Microsoft.Extensions.DependencyInjection;
using TTOptimize.ConsoleUI.Input;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.Input;

public class SplendidWondersInput : IGameInput
{
    private readonly IEnumerable<ITerminalWizard> _wizards;
    private readonly ICommandTerminal _commandTerminal;
    private readonly ISplendidWondersInputPresenter _inputPresenter;

    public SplendidWondersInput(
        ICommandTerminal commandTerminal,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IEnumerable<ITerminalWizard> wizards,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] ISplendidWondersInputPresenter inputPresenter)
    {
        _wizards = wizards;
        _commandTerminal = commandTerminal;
        _inputPresenter = inputPresenter;
    }

    public void ValidationFailed(string validationFailureString)
    {
        _commandTerminal.BeginZone();
        _inputPresenter.DisplayValidationFailure(string.Format(Strings.InvalidCommandTypeStateFormat, validationFailureString));
        _commandTerminal.Flush();
        Console.ReadKey();
        _commandTerminal.EndZone();
    }

    public GameCommand ReadCommand(IGameState state, IEnumerable<GameAction> validActions)
    {
        _commandTerminal.BeginZone();

        var activeWizards = _wizards
            .Where(w => w.CanExecute(state, validActions))
            .ToDictionary(w => w.CommandKey.ToUpper());

        try
        {
            while (true)
            {
                _commandTerminal.ResetZone();
                _inputPresenter.DisplayCommandBlock(activeWizards.Values, null);
                _commandTerminal.Flush();

                var inputKey = Console.ReadKey(intercept: true).KeyChar.ToString().ToUpper();

                if (inputKey == "X")
                {
                    _commandTerminal.ResetZone();
                    throw new ConsoleUserAbortException();
                }

                if (activeWizards.TryGetValue(inputKey, out var selectedWizard))
                {
                    try
                    {
                        _commandTerminal.ResetZone();
                        _inputPresenter.DisplayCommandBlock(activeWizards.Values, selectedWizard);

                        GameCommand? command = selectedWizard.RunConsoleDialog(state, validActions);

                        if (command != null)
                            return command;
                    }
                    catch (ConsoleUserBackException)
                    {
                        continue;
                    }
                }
            }
        }
        finally
        {
            _commandTerminal.EndZone();
        }
    }
}