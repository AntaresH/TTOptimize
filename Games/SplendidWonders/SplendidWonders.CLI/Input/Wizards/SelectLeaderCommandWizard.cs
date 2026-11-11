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
    public class SelectLeaderCommandWizard : BaseConsoleCommandWizard
    {
        public override string CommandKey => Strings.SelectLeaderCommandKey;

        public override string Description => Strings.SelectLeaderCommandDescriptionFormat;

        private IEnumerable<Leader> _eligibleLeaders;

        public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
            => validActions.OfType<SelectLeaderAction>().Any();

        public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
        {
            _eligibleLeaders = validActions.OfType<SelectLeaderAction>().Select(a => a.Leader);

            Console.WriteLine(Strings.PromptSelectLeader);
            ConsoleInputDisplayHelper.DisplayLeaders(_eligibleLeaders);

            return PromptForParameter();
        }

        protected override GameAction? ParseInput(string input)
        {
            Match match = Regex.Match(input, @"^#?([\d]){1,2}$");

            if (match.Success)
            {
                int cardId = int.Parse(match.Groups[0].Value);

                if (_eligibleLeaders.Any(c => c.Id == cardId))
                {
                    return new SelectLeaderAction()
                    {
                        Leader = _eligibleLeaders.First(c => c.Id == cardId)
                    };
                }
            }

            return null;
        }
    }
}
