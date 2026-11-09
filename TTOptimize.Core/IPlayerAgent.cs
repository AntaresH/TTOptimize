using System;
using System.Collections.Generic;
using System.Text;

namespace TTOptimize.Core
{
    public interface IPlayerAgent
    {
        string Name { get; }
        ApplicationCommand DecideCommand(IGameState gameState, IEnumerable<GameAction> legalActions);
    }

    public interface IPlayerResult
    {
        IPlayerAgent Player { get; }
        
        int Result { get; }
    }
}
