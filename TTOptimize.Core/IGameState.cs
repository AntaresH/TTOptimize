using System;
using System.Collections.Generic;
using System.Text;

namespace TTOptimize.Core
{
    public interface IGameState
    {
        GameProgression GameProgression { get; }

        int ActivePlayerId { get; }
    }
}
