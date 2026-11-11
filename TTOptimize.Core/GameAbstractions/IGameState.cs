using System;
using System.Collections.Generic;
using System.Text;

namespace TTOptimize.Core.GameAbstractions
{
    public interface IGameState
    {
        GameProgression GameProgression { get; }

        int ActivePlayerId { get; }
    }
}
