using SplendidWonders.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.Input.Wizards
{
    public class ShowReservedCardsCommandWizard : PresentationWizard
    {
        public override string CommandKey => Strings.ListReservedCardsCommandKey;

        public override string Description => Strings.ListReservedCardsCommandDescription;

        public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
        {
            return ((GameState)state).ActivePlayer.ReservedCards.Any();
        }

        public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
        {
            ConsoleInputDisplayHelper.DisplayCards(((GameState)state).ActivePlayer.ReservedCards);
            Console.ReadLine();

            return null;
        }

        protected override GameAction? ParseInput(string input)
        {
            throw new NotImplementedException();
        }
    }
}
