using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.Agents
{
    public class HeuristicAgent : ComputerAgent
    {
        public HeuristicAgent(string name) : base(name)
        {
        }

        public override string SubjectId => nameof(HeuristicAgent);

        public override GameCommand DecideCommand(IGameState gameState, IReadOnlyList<GameAction> validActions)
        {
            throw new NotImplementedException();
        }
    }
}
