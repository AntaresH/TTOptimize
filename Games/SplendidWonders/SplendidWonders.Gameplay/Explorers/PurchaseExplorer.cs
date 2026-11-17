using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using System.Collections.Immutable;

namespace SplendidWonders.Gameplay.Explorers
{
    public class PurchaseExplorer : IActionExplorer<PurchaseAction>
    {
        public IReadOnlyList<PurchaseAction> Explore(GameState gameState)
        {
            List<PurchaseAction> purchaseActions = new List<PurchaseAction>();

            for (int i = 1; i < 4; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    var card = gameState[i][j];

                    if (card == null || card == Cards.Empty)
                        continue;

                    var requiredTokens = CalculateRequiredTokens(card.Cost, gameState.ActivePlayer.Store, gameState.ActivePlayer.Tableau);
                    int availablePolymaths = gameState.ActivePlayer.Store.Polymath - card.Cost.Polymath;

                    if (availablePolymaths - requiredTokens.Polymath >= 0)
                    {
                        purchaseActions.Add(new PurchaseFromBoardAction() { Card = card, RequiredTokens = requiredTokens });
                    }
                }
            }

            foreach (var card in gameState.ActivePlayer.ReservedCards)
            {
                var requiredTokens = CalculateRequiredTokens(card.Cost, gameState.ActivePlayer.Store, gameState.ActivePlayer.Tableau);
                int availablePolymaths = gameState.ActivePlayer.Store.Polymath - card.Cost.Polymath;

                if (availablePolymaths - requiredTokens.Polymath >= 0)
                {
                    purchaseActions.Add(new PurchaseFromReservedAction() { Card = card, RequiredTokens = requiredTokens });
                }
            }

            return purchaseActions;
        }

        private TokenGroup CalculateRequiredTokens(TokenGroup cardCosts, TokenGroup playerStore, ImmutableList<DevelopmentCard> playerTableau)
        {
            int yellowBonuses = playerTableau.Count(c => c.Bonus == DevelopmentColor.Yellow);
            int blueBonuses = playerTableau.Count(c => c.Bonus == DevelopmentColor.Blue);
            int greenBonuses = playerTableau.Count(c => c.Bonus == DevelopmentColor.Green);
            int brownBonuses = playerTableau.Count(c => c.Bonus == DevelopmentColor.Brown);
            int redBonuses = playerTableau.Count(c => c.Bonus == DevelopmentColor.Red);

            int yellowToSpend = Math.Max(cardCosts.Yellow - yellowBonuses, 0);
            int blueToSpend = Math.Max(cardCosts.Blue - blueBonuses, 0);
            int greenToSpend = Math.Max(cardCosts.Green - greenBonuses, 0);
            int brownToSpend = Math.Max(cardCosts.Brown - brownBonuses, 0);
            int redToSpend = Math.Max(cardCosts.Red - redBonuses, 0);

            int deficit = Math.Max(0, yellowToSpend - playerStore.Yellow) +
                          Math.Max(0, blueToSpend - playerStore.Blue) +
                          Math.Max(0, greenToSpend - playerStore.Green) +
                          Math.Max(0, brownToSpend - playerStore.Brown) +
                          Math.Max(0, redToSpend - playerStore.Red);

            return new TokenGroup()
            {
                Yellow = Math.Min(playerStore.Yellow, yellowToSpend),
                Blue = Math.Min(playerStore.Blue, blueToSpend),
                Green = Math.Min(playerStore.Green, greenToSpend),
                Brown = Math.Min(playerStore.Brown, brownToSpend),
                Red = Math.Min(playerStore.Red, redToSpend),
                Polymath = deficit
            };
        }
    }
}
