using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core;

namespace TTOptimize.Platform
{
    public class HumanAgent : IPlayerAgent
    {
        private IGameInput _input;

        public HumanAgent(IGameInput input)
        {
            _input = input;
        }

        public ApplicationCommand DecideCommand(IGameState gameState, IEnumerable<GameAction> validActions)
        {
            return _input.ReadCommand(gameState, validActions);
        }
    }
}
