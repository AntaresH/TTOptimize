using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Engine
{
    public record class ActionLogEntry
    {
        public int TurnNumber { get; set; }
        public IPlayerAgent Agent { get; set; }

        public GameAction Action { get; set; }
    }
}
