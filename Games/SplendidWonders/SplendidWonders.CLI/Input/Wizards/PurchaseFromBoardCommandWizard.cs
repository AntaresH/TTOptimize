using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using static System.Collections.Specialized.BitVector32;

namespace SplendidWonders.CLI.Input.Wizards
{
    public class PurchaseFromBoardCommandWizard : BaseConsoleCommandWizard
    {
        public override string CommandKey => Strings.PurchaseFromBoardCommandKey;

        public override string Description => Strings.PurchaseFromBoardCommandDescription;

        private IEnumerable<PurchaseAction> _availablePurchases;

        public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
            => validActions.OfType<PurchaseFromBoardAction>().Any();

        public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
        {
            _availablePurchases = validActions.OfType<PurchaseFromBoardAction>();

            Console.WriteLine(Strings.PromptPurchaseFromBoard);
            ConsoleInputDisplayHelper.DisplayCards(_availablePurchases);

            return PromptForParameter();
        }

        protected override GameAction? ParseInput(string input)
        {
            Match match = Regex.Match(input, @"^#?([\d]){1,2}$");

            if (match.Success)
            {
                int cardId = int.Parse(match.Groups[0].Value);

                var selectedPurchase = _availablePurchases.First(pa => pa.Card.Id == cardId);

                if (selectedPurchase != null)
                {
                    return selectedPurchase;
                }
            }

            return null;
        }
    }
}
