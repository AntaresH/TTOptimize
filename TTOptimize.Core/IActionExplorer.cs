using System;
using System.Collections.Generic;
using System.Text;

namespace TTOptimize.Core
{
    public interface IActionExplorer
    {
        IList<GameAction> Explore(IGameState state);
    }
}
