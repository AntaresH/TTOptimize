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

namespace SplendidWonders.CLI.Input.Wizards
{
    public class ReturnTokensCommandWizard : SplendidWondersBaseWizard
    {
        public override string CommandKey => Strings.ReturnTokensCommandKey;

        public override string Description => Strings.ReturnTokensCommandDescription;

        public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
            => validActions.OfType<ReturnTokensAction>().Any();

        public ReturnTokensCommandWizard(
            IConsolePromptContext promptContext,
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] ISplendidWondersInputPresenter inputPresenter) : base(promptContext, inputPresenter)
        {
            _inputPresenter = inputPresenter;
        }

        public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
        {
            _inputPresenter.DisplaySubCommandBlock(CommandKey, Strings.PromptReturnTokens);

            return PromptForParameter();
        }

        protected override GameAction? ParseInput(string input)
        {
            Match match = Regex.Match(input, @"^[SKZBPskzbp]+( [SKZBPskzbp])*( [SKZBPskzbp])*$");

            if (match.Success)
            {
                return new ReturnTokensAction()
                {
                    Tokens = new TokenGroup
                    {
                        Yellow = input.Count(c => c == 'S' || c == 's'),
                        Blue = input.Count(c => c == 'K' || c == 'k'),
                        Green = input.Count(c => c == 'Z' || c == 'z'),
                        Brown = input.Count(c => c == 'B' || c == 'b'),
                        Red = input.Count(c => c == 'P' || c == 'p')
                    }
                };
            }

            return null;
        }
    }
}
