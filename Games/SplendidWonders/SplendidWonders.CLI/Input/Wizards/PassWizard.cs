using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.ConsoleUI.Input;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.Setup;

namespace SplendidWonders.CLI.Input.Wizards
{
    public class PassWizard : SplendidWondersBaseWizard
    {
        public PassWizard(ICommandTerminal commandTerminal,
            ISplendidWondersInputPresenter inputPresenter) : base(commandTerminal, inputPresenter)
        {
        }

        public override string CommandKey => Strings.PassCommandKey;

        public override string Description => Strings.PassCommandDescription;

        public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
        {
            return validActions.OfType<PassAction>().Any();
        }

        public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
        {
            return new GameCommand()
            {
                Action = validActions.OfType<PassAction>().FirstOrDefault(),
                CommandText = string.Empty
            };
        }

        protected override GameAction? ParseInput(string input)
        {
            throw new NotImplementedException();
        }
    }
}
