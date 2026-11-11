using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Gameplay
{
    public class StateFactory : IGameStateFactory
    {
        public IGameState Create(IGameConfiguration gameConfiguration)
        {
            var swGameConfiguration = (GameConfiguration) gameConfiguration;

            return GameFactory.GetNewGameState(swGameConfiguration);
        }
    }
}
