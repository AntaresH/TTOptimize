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
    public class DrawDoubleTokensCommandWizard : BaseConsoleCommandWizard
    {
        public override string CommandKey => Strings.DrawDoubleTokensCommandKey;

        public override string Description => Strings.DrawDoubleTokensCommandDescription;

        public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
            => validActions.OfType<DrawDoubleTokensAction>().Any();

        public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
        {
            Console.WriteLine(Strings.PromptDoubleTokens);

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
