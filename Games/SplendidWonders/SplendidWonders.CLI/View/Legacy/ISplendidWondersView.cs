using SplendidWonders.Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View.Legacy
{
    public interface ISplendidWondersView
    {
        void DisplayGameOver(List<Player> players);
        void DisplayGame(GameState gameState, IEnumerable<GameAction> validActions, string previousInvalidCommand);
        void DisplayBoard(GameState gameState);
    }
}
