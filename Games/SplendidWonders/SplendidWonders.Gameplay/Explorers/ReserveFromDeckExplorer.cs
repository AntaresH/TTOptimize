using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay.Explorers
{
    public class ReserveFromDeckExplorer : IActionExplorer<ReserveFromDeckAction>
    {
        public IEnumerable<ReserveFromDeckAction> Explore(GameState gameState)
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
