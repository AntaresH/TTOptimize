using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.Agents;

namespace TTOptimize.Core.GameAbstractions
{

    public interface IPlayerResult
    {
        IParticipant Player { get; }

        int Rank { get; }

        int Points { get; }

        int? Tiebreaker { get; }
    }
}
