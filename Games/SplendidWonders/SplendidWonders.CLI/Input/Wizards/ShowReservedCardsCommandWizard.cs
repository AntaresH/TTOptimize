using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.Input.Wizards
{
    public class ShowReservedCardsCommandWizard : SplendidWondersPresentationWizard
    {
        public override string CommandKey => Strings.ListReservedCardsCommandKey;

        public override string Description => Strings.ListReservedCardsCommandDescription;

        public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
        {
            return ((GameState)state).ActivePlayer.ReservedCards.Any();
        }

        public ShowReservedCardsCommandWizard(
            ICommandTerminal commandTerminal,
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] ISplendidWondersInputPresenter inputPresenter) : base(commandTerminal, inputPresenter)
        {
            _inputPresenter = inputPresenter;
        }

        public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
        {
            _inputPresenter.DisplayCards(((GameState)state).ActivePlayer.ReservedCards);
            ReadLineOrEscape();

            throw new ConsoleUserBackException();
        }

        protected override GameAction? ParseInput(string input)
        {
            throw new NotImplementedException();
        }
    }
}
