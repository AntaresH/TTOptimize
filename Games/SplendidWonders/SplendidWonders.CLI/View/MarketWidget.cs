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
        public int Rows => 3 * (_cardHeight + 2) + 2;
        public int Columns => 4 * (_cardWidth + 2) + 3;

        private int _cardHeight = 6;
        private int _cardWidth = 24;

        public ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions)
        {
            var swState = (GameState)state;

            var marketCanvas = new ConxelCanvas(Rows, Columns);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var developmentCard = swState[3 - i][j + 1];
                    var purchaseAction = legalActions?.OfType<PurchaseAction>()?.FirstOrDefault(pa => pa.Card == developmentCard);

                    var cardCanvas = developmentCard == Cards.Empty
                                        ? EmptyCard(WidgetHelper.DarkSetDefault)
                                        : Card(developmentCard, purchaseAction);

                    marketCanvas.Blit(cardCanvas, i * (_cardHeight + 2) + i, j * (_cardWidth + 2) + j);
                }
            }

            return marketCanvas;
        }

        private ConxelCanvas EmptyCard(RgbColor cardColor)
        {
            var cardCanvas = new ConxelCanvas(_cardHeight, _cardWidth);

            return new ConxelCanvas(_cardHeight, _cardWidth).WithSingleBorder(cardColor).WithCorners(cardColor);
        }

        private ConxelCanvas Card(DevelopmentCard developmentCard, PurchaseAction respectivePurchaseAction)
        {
            var cardColor = GetDarkDevelopmentColor(developmentCard.Bonus);
            var borderColor = respectivePurchaseAction == null
                                ? cardColor
                                : respectivePurchaseAction.RequiredPolymathsCount == 0
                                    ? WidgetHelper.LightSetYellow
                                    : RgbColor.WarmWhite;

            var needsDoubleBorder = respectivePurchaseAction != null;

            var cardCanvas = new ConxelCanvas(_cardHeight, _cardWidth);

            cardCanvas.Blit(CardHeaderLines(borderColor, cardColor, developmentCard.Name), 0, 0);
            cardCanvas.Blit(EmptyBlock(_cardWidth), 4, 0);
            cardCanvas.Blit(RequirementsLine(borderColor, developmentCard.Cost, developmentCard.VictoryPoints), 5, 0);

            return needsDoubleBorder
                        ? cardCanvas.WithDoubleBorder(borderColor).WithCorners(borderColor)
                        : cardCanvas.WithSingleBorder(borderColor).WithCorners(borderColor);
        }

        private ConxelCanvas CardHeaderLines(RgbColor borderColor, RgbColor cardColor, string title)
        {
            var header = new ConxelCanvas(4, _cardWidth);

            var (nameLine1, nameLine2) = WidgetHelper.WrapAndCenterText(title, _cardWidth - 2);

            header.Blit(ColoredBlock(cardColor, _cardWidth), 0, 0);
            header.WriteText(1, 0, $" " + nameLine1 + " ", RgbColor.FullWhite, cardColor);

            if (string.IsNullOrWhiteSpace(nameLine2))
            {
                header.Blit(ColoredBlock(cardColor, _cardWidth), 2, 0);
                header.Blit(EmptyBlock(_cardWidth), 3, 0);
            }
            else
            {
                header.WriteText(2, 0, $" " + nameLine2 + " ", RgbColor.FullWhite, cardColor);
                header.Blit(ColoredBlock(cardColor, _cardWidth), 3, 0);
            }

            return header;
        }

        private ConxelCanvas RequirementsLine(RgbColor borderColor, TokenGroup cost, int victoryPoints)
        {
            var reqLine = new ConxelCanvas(1, _cardWidth);

            int i = 1;
            foreach (var requirement in cost.AsEnumerable().Where(kvp => kvp.Value > 0))
            {
                reqLine[0, i] = new Conxel(requirement.Value.ToString(), GetLightDevelopmentColor(requirement.Key));
                i += 2;
            }

            if (victoryPoints > 0)
            {
                reqLine[0, _cardWidth - 4] = new Conxel("{", RgbColor.FullWhite);
                reqLine[0, _cardWidth - 3] = new Conxel(victoryPoints.ToString(), RgbColor.FullWhite);
                reqLine[0, _cardWidth - 2] = new Conxel("}", RgbColor.FullWhite);
            }

            return reqLine;
        }
    }
}
