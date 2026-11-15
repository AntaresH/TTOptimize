using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.ViewAbstractions
{
    public interface IGameView
    {
        void DisplayGameTitle();
        void DisplayGameOver(IGameState gameState, IGameRules finalStandings);
        void DisplayValidationFailure(string lastValidationFailure);
    }
}
