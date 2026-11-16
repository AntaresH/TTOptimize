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
            int blockTop = Console.CursorTop;

            var activeWizards = _wizards.Where(w => w.CanExecute(state, validActions));

            ConsoleInputDisplayHelper.PrintCommandsBlock(activeWizards, null);

            int promptStartTop = Console.CursorTop;

            while (true)
            {
                ConsoleInputDisplayHelper.ClearFromLine(promptStartTop);

                try
                {
                    var inputKey = Console.ReadKey(intercept: true).KeyChar.ToString().ToUpper();

                    ConsoleInputDisplayHelper.ClearFromLine(promptStartTop);

                    if (inputKey == "X")
                    {
                        throw new ConsoleUserAbortException();
                    }

                    var wizard = activeWizards.FirstOrDefault(wiz => wiz.CommandKey == inputKey);

                    if (wizard != null)
                    {
                        ConsoleInputDisplayHelper.ClearFromLine(blockTop);
                        ConsoleInputDisplayHelper.PrintCommandsBlock(activeWizards, wizard);

                        GameCommand? command = wizard.RunConsoleDialog(state, validActions, promptStartTop);

                        if (command != null) 
                            return command;

                        continue;
                    }
                }
                catch (ConsoleUserBackException)
                {
                    ConsoleInputDisplayHelper.ClearFromLine(blockTop);
                    ConsoleInputDisplayHelper.PrintCommandsBlock(activeWizards, null);
                    continue;
                }
            }
        }
    }
}
