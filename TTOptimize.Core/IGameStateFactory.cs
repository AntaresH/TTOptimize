using System;
using System.Collections.Generic;
using System.Text;

namespace TTOptimize.Core
{
    public interface IGameStateFactory
    {
        IGameState Create(IGameConfiguration gameConfiguration);
    }
}
