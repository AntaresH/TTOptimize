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
    public class DrawDoubleTokensCommandWizard : SplendidWondersBaseWizard
    {
        public override string CommandKey => Strings.DrawDoubleTokensCommandKey;

        public override string Description => Strings.DrawDoubleTokensCommandDescription;

        public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
            => validActions.OfType<DrawDoubleTokensAction>().Any();

        public DrawDoubleTokensCommandWizard(
            IConsolePromptContext promptContext,
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] ISplendidWondersInputPresenter inputPresenter) : base(promptContext, inputPresenter)
        {
            _inputPresenter = inputPresenter;
        }

        public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
        {
            _inputPresenter.DisplaySubCommandBlock(CommandKey, Strings.PromptDoubleTokens);

            return PromptForParameter();
        }

        protected override GameAction? ParseInput(string input)
        {
            Match match = Regex.Match(input, @"^[SsKkZzBbPp]{1}$");

            if (match.Success)
            {
                return new DrawDoubleTokensAction()
                {
                    Tokens = new TokenGroup
                    {
                        Yellow = input.Contains('S', StringComparison.OrdinalIgnoreCase) ? 2 : 0,
                        Blue = input.Contains('K', StringComparison.OrdinalIgnoreCase) ? 2 : 0,
                        Green = input.Contains('Z', StringComparison.OrdinalIgnoreCase) ? 2 : 0,
                        Brown = input.Contains('B', StringComparison.OrdinalIgnoreCase) ? 2 : 0,
                        Red = input.Contains('P', StringComparison.OrdinalIgnoreCase) ? 2 : 0
                    }
                };
            }

            return null;
        }
    }
}
