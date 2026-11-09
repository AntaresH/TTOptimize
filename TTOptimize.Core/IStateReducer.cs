using System;
using System.Collections.Generic;
using System.Text;

namespace TTOptimize.Core
{
    public interface IStateReducer
    {
        IGameState Reduce(IGameState state, GameAction action);
    }
}
