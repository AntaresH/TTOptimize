using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.ViewAbstractions
{
    public interface IGameRenderer
    {
        void Render(IGameState state, IReadOnlyList<GameAction> legalActions);
    }
}
