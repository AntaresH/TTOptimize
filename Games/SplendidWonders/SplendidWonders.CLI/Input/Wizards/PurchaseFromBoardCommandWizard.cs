using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
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
    public class PurchaseFromBoardCommandWizard : SplendidWondersBaseWizard
    {
        public override string CommandKey => Strings.PurchaseFromBoardCommandKey;

        public override string Description => Strings.PurchaseFromBoardCommandDescription;

        private IEnumerable<PurchaseAction> _availablePurchases;

        public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
            => validActions.OfType<PurchaseFromBoardAction>().Any();

        public PurchaseFromBoardCommandWizard(
            IConsolePromptContext promptContext,
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] ISplendidWondersInputPresenter inputPresenter) : base(promptContext, inputPresenter)
        {
            _inputPresenter = inputPresenter;
        }

        public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
        {
            _availablePurchases = validActions.OfType<PurchaseFromBoardAction>();

            _inputPresenter.DisplaySubCommandBlock(CommandKey, Strings.PromptPurchaseFromBoard);
            _inputPresenter.DisplayPurchases(_availablePurchases);

            return PromptForParameter();
        }

        protected override GameAction? ParseInput(string input)
        {
            Match match = Regex.Match(input, @"^#?([\d]){1,2}$");

            if (match.Success)
            {
                int key = int.Parse(match.Groups[0].Value);

                var selectedPurchase = _availablePurchases.ElementAtOrDefault(key - 1);

                if (selectedPurchase != null)
                {
                    return selectedPurchase;
                }
            }

            return null;
        }
    }
}
