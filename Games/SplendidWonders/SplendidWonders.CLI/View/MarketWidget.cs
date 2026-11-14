using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View
{
    public class MarketWidget : WidgetBase, IConxelWidget
    {
        private int _cardInnerHeight = 6;
        private int _cardInnerWidth = 24;

        public ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions)
        {
            var swState = (GameState)state;

            var borderedCardHeight = _cardInnerHeight + 2;
            var borderedCardWidth = _cardInnerWidth + 2;

            var marketCanvas = new ConxelCanvas(3 * borderedCardHeight + 2, 4 * borderedCardWidth + 3);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var developmentCard = swState[3 - i][j + 1];
                    var purchaseAction = legalActions.OfType<PurchaseAction>().FirstOrDefault(pa => pa.Card == developmentCard);

                    marketCanvas.Blit(RenderCard(developmentCard, purchaseAction), i * borderedCardHeight + i, j * borderedCardWidth + j);
                }
            }

            return marketCanvas;
        }

        private ConxelCanvas RenderCard(DevelopmentCard developmentCard, PurchaseAction respectivePurchaseAction)
        {
            var cardColor = GetDarkDevelopmentColor(developmentCard.Bonus);
            var borderColor = respectivePurchaseAction == null
                                ? cardColor
                                : respectivePurchaseAction.RequiredPolymathsCount == 0
                                    ? WidgetHelper.LightSetYellow
                                    : RgbColor.WarmWhite;

            var needsDoubleBorder = respectivePurchaseAction != null;

            var cardCanvas = new ConxelCanvas(_cardInnerHeight, _cardInnerWidth);

            cardCanvas.Blit(CardHeaderLines(borderColor, cardColor, developmentCard.Name), 0, 0);
            cardCanvas.Blit(EmptyBlock(_cardInnerWidth), 4, 0);
            cardCanvas.Blit(RequirementsLine(borderColor, developmentCard.Cost, developmentCard.VictoryPoints), 5, 0);

            return needsDoubleBorder 
                        ? cardCanvas.WithDoubleBorder(borderColor).WithCorners(borderColor)
                        : cardCanvas.WithSingleBorder(borderColor).WithCorners(borderColor);
        }

        private ConxelCanvas CardHeaderLines(RgbColor borderColor, RgbColor cardColor, string title)
        {
            var header = new ConxelCanvas(4, _cardInnerWidth);

            var (nameLine1, nameLine2) = WidgetRenderingHelper.WrapAndCenterText(title, _cardInnerWidth - 2);

            header.Blit(ColoredBlock(cardColor, _cardInnerWidth), 0, 0);
            header.WriteText(1, 0, $" " + nameLine1 + " ", RgbColor.FullWhite, cardColor);

            if (string.IsNullOrWhiteSpace(nameLine2))
            {
                header.Blit(ColoredBlock(cardColor, _cardInnerWidth), 2, 0);
                header.Blit(EmptyBlock(_cardInnerWidth), 3, 0);
            }
            else
            {
                header.WriteText(2, 0, $" " + nameLine2 + " ", RgbColor.FullWhite, cardColor);
                header.Blit(ColoredBlock(cardColor, _cardInnerWidth), 3, 0);
            }

            return header;
        }

        private ConxelCanvas RequirementsLine(RgbColor borderColor, TokenGroup cost, int victoryPoints)
        {
            var reqLine = new ConxelCanvas(1, _cardInnerWidth);

            int i = 1;
            foreach (var requirement in cost.AsEnumerable().Where(kvp => kvp.Value > 0))
            {
                reqLine[0, i] = new Conxel(requirement.Value.ToString(), GetLightDevelopmentColor(requirement.Key));
                i += 2;
            }

            if (victoryPoints > 0)
            {
                reqLine[0, _cardInnerWidth - 4] = new Conxel("{", RgbColor.FullWhite);
                reqLine[0, _cardInnerWidth - 3] = new Conxel(victoryPoints.ToString(), RgbColor.FullWhite);
                reqLine[0, _cardInnerWidth - 2] = new Conxel("}", RgbColor.FullWhite);
            }

            return reqLine;
        }
    }
}
