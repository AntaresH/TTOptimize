using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core.Agents
{
    public class HumanAgent : IPlayerAgent
    {
        public string Name { get; }
        private IGameInput _input;

        public HumanAgent(string name)
        {
            Name = name;
        }

        public void SetInputMethod(IGameInput input)
        {
            _input = input;
        }

        public GameCommand DecideCommand(IGameState gameState, IReadOnlyList<GameAction> validActions)
        {
            return _input.ReadCommand(gameState, validActions);
        }
    }
}