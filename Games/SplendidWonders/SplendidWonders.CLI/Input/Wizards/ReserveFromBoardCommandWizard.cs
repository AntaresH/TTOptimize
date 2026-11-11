using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
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
    public class ReserveFromBoardCommandWizard : BaseConsoleCommandWizard
    {
        public override string CommandKey => Strings.ReserveFromBoardCommandKey;

        public override string Description => Strings.ReserveFromBoardCommandDescription;

        public GameState _gameState;

        public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
            => validActions.OfType<ReserveFromBoardAction>().Any();

        public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
        {
            _gameState = (GameState)state;

            Console.WriteLine(Strings.PromptReserveFromBoard);

            return PromptForParameter();
        }

        protected override GameAction? ParseInput(string input)
        {
            Match match = Regex.Match(input, @"^([1-3]),([1-4])$");

            if (match.Success)
            {
                int level = int.Parse(match.Groups[1].Value);
                int column = int.Parse(match.Groups[2].Value);

                var cardToReserve = _gameState[level][column];

                if (cardToReserve != Cards.Empty)
                {
                    return new ReserveFromBoardAction()
                    {
                        Card = _gameState[level][column]
                    };
                }
            }

            return null;
        }
    }
}
