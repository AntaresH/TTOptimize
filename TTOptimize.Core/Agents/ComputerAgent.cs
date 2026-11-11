using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core.Agents
{
    public abstract class ComputerAgent : IPlayerAgent
    {
        public string Name { get; }

        public ComputerAgent(string name)
        {
            Name = name;
        }

        public abstract GameCommand DecideCommand(IGameState gameState, IReadOnlyList<GameAction> validActions);
    }
}
