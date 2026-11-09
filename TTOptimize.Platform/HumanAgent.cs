using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core;

namespace TTOptimize.Platform
{
    public class HumanAgent : IPlayerAgent
    {
        public string Name { get; }
        private IGameInput _input;

        public HumanAgent(IGameInput input, string name)
        {
            _input = input;

            Name = name;
        }

        public ApplicationCommand DecideCommand(IGameState gameState, IEnumerable<GameAction> validActions)
        {
            return _input.ReadCommand(gameState, validActions);
        }
    }
}
