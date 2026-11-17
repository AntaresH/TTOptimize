using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;

namespace SplendidWonders.Gameplay.Explorers
{
    public class ReserveFromDeckExplorer : IActionExplorer<ReserveFromDeckAction>
    {
        public IReadOnlyList<ReserveFromDeckAction> Explore(GameState gameState)
        {
            List<ReserveFromDeckAction> reserveFromDeckActions = new List<ReserveFromDeckAction>();

            if (gameState.ActivePlayer.ReservedCards.Count < 3)
            {
                for (int i = 1; i < 4; i++)
                {
                    var deck = gameState[i];

                    if (deck.Remaining > 0)
                    {
                        reserveFromDeckActions.Add(new ReserveFromDeckAction() { Level = i });
                    }
                }
            }

            return reserveFromDeckActions;
        }
    }
}
