using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.CLI.View;
using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using SplendidWonders.Gameplay.Actions;
using System.Text.RegularExpressions;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.Input
{
    public class ConsoleGameInput : IGameInput
    {
        private readonly IEnumerable<IConsoleCommandWizard> _wizards;

        public ConsoleGameInput(
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IEnumerable<IConsoleCommandWizard> wizards)
        {
            _wizards = wizards;
        }

        public GameCommand ReadCommand(IGameState state, IEnumerable<GameAction> validActions)
        {
            var activeWizards = _wizards.Where(w => w.CanExecute(state, validActions));

            ConsoleInputDisplayHelper.DisplayActions(activeWizards);
            int promptStartTop = Console.CursorTop;

            while (true)
            {
                ConsoleInputDisplayHelper.ClearFromLine(promptStartTop);

                try
                {
                    string commandInput = (Console.ReadLine() ?? "").Trim().ToUpper();

                    if (commandInput == "X")
                    {
                        throw new ConsoleUserAbortException();
                    }

                    var wizard = activeWizards.FirstOrDefault(wiz => wiz.CommandKey == commandInput);

                    if (wizard != null)
                    {
                        GameCommand? command = wizard.RunConsoleDialog(state, validActions);

                        if (command != null) 
                            return command;

                        continue;
                    }
                }
                catch (ConsoleUserBackException)
                {
                    continue;
                }
            }
        }
    }
}
