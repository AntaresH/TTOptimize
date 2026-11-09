using System;
using System.Collections.Generic;
using System.Text;

namespace TTOptimize.Core
{
    public interface IPlayerAgent
    {
        ApplicationCommand DecideCommand(IGameState gameState, IEnumerable<GameAction> legalActions);
    }
}
