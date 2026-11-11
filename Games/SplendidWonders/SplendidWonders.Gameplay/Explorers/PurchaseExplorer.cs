using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using SplendidWonders.Gameplay.Actions;
using System.Collections.Immutable;

namespace SplendidWonders.Gameplay.Explorers
{
    public class PurchaseExplorer : IActionExplorer<PurchaseAction>
    {
        public IEnumerable<PurchaseAction> Explore(GameState gameState)
        {
            List<PurchaseAction> purchaseActions = new List<PurchaseAction>();

            for (int i = 1; i < 4; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    var card = gameState[i][j];

                    if (card == null || card == Cards.Empty)
                        continue;

                    int deficit = CalculateDeficit(card.Cost, gameState.ActivePlayer.Store, gameState.ActivePlayer.Tableau);
                    int availablePolymaths = gameState.ActivePlayer.Store.Polymath - card.Cost.Polymath;

                    if (deficit == 0 || availablePolymaths + deficit >= 0)
                    {
                        purchaseActions.Add(new PurchaseFromBoardAction() { Card = card, RequiredPolymathsCount = -deficit });
                    }
                }
            }

            foreach (var card in gameState.ActivePlayer.ReservedCards)
            {
                int deficit = CalculateDeficit(card.Cost, gameState.ActivePlayer.Store, gameState.ActivePlayer.Tableau);
                int availablePolymaths = gameState.ActivePlayer.Store.Polymath - card.Cost.Polymath;

                if (deficit == 0 || availablePolymaths + deficit >= 0)
                {
                    purchaseActions.Add(new PurchaseFromBoardAction() { Card = card, RequiredPolymathsCount = -deficit });
                }
            }

            return purchaseActions;
        }

        private int CalculateDeficit(TokenGroup cardCosts, TokenGroup playerStore, ImmutableList<DevelopmentCard> playerTableau)
        {
            int playerYellowBonuses = playerTableau.Count(c => c.Bonus == DevelopmentBonus.Yellow);
            int playerBlueBonuses = playerTableau.Count(c => c.Bonus == DevelopmentBonus.Blue);
            int playerGreenBonuses = playerTableau.Count(c => c.Bonus == DevelopmentBonus.Green);
            int playerBrownBonuses = playerTableau.Count(c => c.Bonus == DevelopmentBonus.Brown);
            int playerRedBonuses = playerTableau.Count(c => c.Bonus == DevelopmentBonus.Red);

            return 
                Math.Min(0, playerYellowBonuses + playerStore.Yellow - cardCosts.Yellow) +
                Math.Min(0, playerBlueBonuses + playerStore.Blue - cardCosts.Blue) +
                Math.Min(0, playerGreenBonuses + playerStore.Green - cardCosts.Green) +
                Math.Min(0, playerBrownBonuses + playerStore.Brown - cardCosts.Brown) +
                Math.Min(0, playerRedBonuses + playerStore.Red - cardCosts.Red);
        }
    }
}
