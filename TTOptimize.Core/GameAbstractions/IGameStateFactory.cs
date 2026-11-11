using System;
using System.Collections.Generic;
using System.Text;

namespace TTOptimize.Core.GameAbstractions
{
    public interface IGameStateFactory
    {
        IGameState Create(IGameConfiguration gameConfiguration);
    }
}
