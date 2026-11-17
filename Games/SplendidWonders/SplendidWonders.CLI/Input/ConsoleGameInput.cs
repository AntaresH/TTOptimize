using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.CLI.View;
using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using System.Text.RegularExpressions;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.Input
{
    public class ConsoleGameInput : IGameInput
    {
        private readonly IEnumerable<IConsoleCommandWizard> _wizards;
        private readonly IConsolePromptContext _promptContext;
        private readonly ISplendidWondersInputPresenter _inputPresenter;

        public ConsoleGameInput(
            IConsolePromptContext promptContext,
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IEnumerable<IConsoleCommandWizard> wizards,
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] ISplendidWondersInputPresenter inputPresenter)
        {
            _wizards = wizards;
            _promptContext = promptContext;
            _inputPresenter = inputPresenter;
        }

        public GameCommand ReadCommand(IGameState state, IEnumerable<GameAction> validActions)
        {
            _promptContext.BeginZone();

            var activeWizards = _wizards
                .Where(w => w.CanExecute(state, validActions))
                .ToDictionary(w => w.CommandKey.ToUpper());

            try
            {
                while (true)
                {
                    _promptContext.ResetZone();
                    _inputPresenter.DisplayCommandBlock(activeWizards.Values, null);

                    var inputKey = Console.ReadKey(intercept: true).KeyChar.ToString().ToUpper();

                    if (inputKey == "X")
                    {
                        _promptContext.ResetZone();
                        throw new ConsoleUserAbortException();
                    }

                    if (activeWizards.TryGetValue(inputKey, out var selectedWizard))
                    {
                        try
                        {
                            _promptContext.ResetZone();
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
                _promptContext.EndZone();
            }
        }
    }
}