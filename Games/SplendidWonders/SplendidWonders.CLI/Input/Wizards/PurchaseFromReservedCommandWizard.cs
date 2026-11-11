using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using static System.Collections.Specialized.BitVector32;

namespace SplendidWonders.CLI.Input.Wizards
{
    public class PurchaseFromReservedCommandWizard : BaseConsoleCommandWizard
    {
        public override string CommandKey => Strings.PurchaseFromReservedCommandKey;

        public override string Description => Strings.PurchaseFromReservedCommandDescription;

        public IEnumerable<PurchaseAction> _purchaseFromReservedActions;

        public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
            => validActions.OfType<PurchaseFromReservedAction>().Any();

        public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
        {
            var swGameState = ((GameState)state);

            var allReservedCards = swGameState.ActivePlayer.ReservedCards;

            _purchaseFromReservedActions = validActions
                .OfType<PurchaseAction>()
                .Where(pa => allReservedCards.Contains(pa.Card));

            Console.WriteLine(Strings.PromptPurchaseFromBoard);
            ConsoleInputDisplayHelper.DisplayCards(_purchaseFromReservedActions);

            return PromptForParameter();
        }

        protected override GameAction? ParseInput(string input)
        {
            Match match = Regex.Match(input, @"^#?([\d]){1,2}$");

            if (match.Success)
            {
                int cardId = int.Parse(match.Groups[0].Value);

                if (_purchaseFromReservedActions.Any(pa => pa.Card.Id == cardId))
                {
                    return new PurchaseFromBoardAction()
                    {
                        Card = _purchaseFromReservedActions.First(pa => pa.Card.Id == cardId).Card
                    };
                }
            }

            return null;
        }
    }
}
