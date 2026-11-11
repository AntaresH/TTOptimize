using SplendidWonders.Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.View
{
    public interface ISplendidWondersView : IGameView
    {
        void DisplayGameOver(List<Player> players);
        void DisplayGame(GameState gameState, IEnumerable<GameAction> validActions, string previousInvalidCommand);
        void DisplayValidActions(IEnumerable<GameAction> validActions);
        void DisplayInvalidCommandMessage();
        void DisplayInvalidCommand(GameAction command);
        void DisplayGameTitle();
        void DisplayBoard(GameState gameState);
    }
}
