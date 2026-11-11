using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay.Explorers
{
    public class ReserveFromBoardExplorer : IActionExplorer<ReserveFromBoardAction>
    {
        public IReadOnlyList<ReserveFromBoardAction> Explore(GameState gameState)
        {
            List<ReserveFromBoardAction> reserveFromBoardActions = new List<ReserveFromBoardAction>();

            if (gameState.ActivePlayer.ReservedCards.Count < 3)
            {
                for (int i = 1; i < 4; i++)
                {
                    var cardList = gameState[i];

                    for (int j = 1; j < 5; j++)
                    {
                        var card = cardList[j];

                        if (card != Cards.Empty)
                        {
                            reserveFromBoardActions.Add(new ReserveFromBoardAction() { Card = card });
                        }
                    }
                }
            }

            return reserveFromBoardActions;
        }
    }
}
