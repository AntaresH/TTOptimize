using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core
{
    public interface IPlayerAgent
    {
        string Name { get; }
        GameCommand DecideCommand(IGameState gameState, IEnumerable<GameAction> validActions);
    }

    public interface IPlayerResult
    {
        IPlayerAgent Player { get; }
        
        int Result { get; }
    }
}
