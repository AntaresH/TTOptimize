using System;
using System.Collections.Generic;
using System.Text;

namespace TTOptimize.Core.GameAbstractions
{
    public interface IGameRules
    {
        IEnumerable<GameAction> Explore(IGameState currentState);
        object GetFinalStandings(IGameState currentState);
        IGameState Reduce(IGameState baseState, GameAction actionToApply);
        bool Validate(GameAction action);
    }
}
