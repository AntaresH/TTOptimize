using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Engine
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

        public GameCommand DecideCommand(IGameState gameState, IEnumerable<GameAction> validActions)
        {
            return _input.ReadCommand(gameState, validActions);
        }
    }
}
