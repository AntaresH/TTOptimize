using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using static System.Collections.Specialized.BitVector32;

namespace SplendidWonders.CLI.Input.Wizards
{
    public class ReserveFromDeckCommandWizard : SplendidWondersBaseWizard
    {
        public override string CommandKey => Strings.ReserveFromDeckCommandKey;

        public override string Description => Strings.ReserveFromDeckCommandDescription;

        public GameState _gameState;

        public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
            => validActions.OfType<ReserveFromDeckAction>().Any();

        public ReserveFromDeckCommandWizard(
            ICommandTerminal commandTerminal,
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] ISplendidWondersInputPresenter inputPresenter) : base(commandTerminal, inputPresenter)
        {
            _inputPresenter = inputPresenter;
        }

        public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
        {
            _gameState = (GameState)state;

            _inputPresenter.DisplaySubCommandBlock(CommandKey, Strings.PromptReserveFromDeck);

            return PromptForParameter();
        }

        protected override GameAction? ParseInput(string input)
        {
            Match match = Regex.Match(input, @"^([1-3])$");

            if (match.Success)
            {
                int level = int.Parse(match.Groups[1].Value);

                return new ReserveFromDeckAction()
                {
                    Level = level
                };
            }

            return null;
        }
    }
}
