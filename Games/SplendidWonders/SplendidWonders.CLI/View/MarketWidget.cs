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
    public class MarketWidget : IConxelWidget
    {
        private string _horizBorderChar = "-";
        private string _vertBorderChar = "|";
        private string _cornerChar = "+";
        private int _cardHeight = 8;
        private int _cardWidth = 24;

        public ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions)
        {
            var swState = (GameState)state;

            var marketCanvas = new ConxelCanvas(3 * _cardHeight + 2, 4 * _cardWidth + 3);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var developmentCard = swState[3 - i][j + 1];
                    var respectivePurchaseAction = legalActions.OfType<PurchaseAction>().FirstOrDefault(pa => pa.Card == developmentCard);

                    marketCanvas.Blit(RenderCard(developmentCard, respectivePurchaseAction), i * _cardHeight + i, j * _cardWidth + j);
                }
            }

            return marketCanvas;
        }

        private ConxelCanvas RenderCard(DevelopmentCard developmentCard, PurchaseAction respectivePurchaseAction)
        {
            var cardColor = GetDarkBonusColor(developmentCard.Bonus);
            var borderColor = respectivePurchaseAction == null
                                ? cardColor
                                : respectivePurchaseAction.RequiredPolymathsCount == 0
                                    ? RgbColor.LightSetYellow
                                    : RgbColor.FullWhite;

            var cardCanvas = new ConxelCanvas(_cardHeight, _cardWidth);

            cardCanvas.WriteText(0, 0, $"{_cornerChar}{string.Concat(Enumerable.Repeat(_horizBorderChar, _cardWidth - 2))}{_cornerChar}", borderColor);

            cardCanvas.Blit(CardHeaderLines(borderColor, cardColor, developmentCard.Name), 1, 0);
            cardCanvas.Blit(StandardLine(borderColor, null), 5, 0);
            cardCanvas.Blit(RequirementsLine(borderColor, developmentCard.Cost, developmentCard.VictoryPoints), 6, 0);

            cardCanvas.WriteText(_cardHeight - 1, 0, $"{_cornerChar}{string.Concat(Enumerable.Repeat(_horizBorderChar, _cardWidth - 2))}{_cornerChar}", borderColor);

            return cardCanvas;
        }

        private ConxelCanvas StandardLine(RgbColor borderColor, RgbColor? cardColor)
        {
            var line = new ConxelCanvas(1, _cardWidth);
            line[0, 0] = new Conxel(_vertBorderChar, borderColor);
            line.WriteText(0, 1, $"{new string(' ', _cardWidth - 2)}", null, cardColor);
            line[0, _cardWidth - 1] = new Conxel(_vertBorderChar, borderColor);

            return line;
        }

        private ConxelCanvas CardHeaderLines(RgbColor borderColor, RgbColor cardColor, string title)
        {
            var header = new ConxelCanvas(4, _cardWidth);

            header.Blit(StandardLine(borderColor, cardColor), 0, 0);

            var (nameLine1, nameLine2) = WidgetRenderingHelper.WrapAndCenterText(title, _cardWidth - 4);

            header[1, 0] = new Conxel(_vertBorderChar, borderColor);
            header[1, 1] = new Conxel(" ", null, cardColor);
            header.WriteText(1, 2, nameLine1, RgbColor.FullWhite, cardColor);
            header[1, _cardWidth - 2] = new Conxel(" ", null, cardColor);
            header[1, _cardWidth - 1] = new Conxel(_vertBorderChar, borderColor);

            if (string.IsNullOrWhiteSpace(nameLine2))
            {
                header.Blit(StandardLine(borderColor, cardColor), 2, 0);
                header.Blit(StandardLine(borderColor, null), 3, 0);
            }
            else
            {
                header[2, 0] = new Conxel(_vertBorderChar, borderColor);
                header[2, 1] = new Conxel(" ", null, cardColor);
                header.WriteText(2, 2, nameLine2, RgbColor.FullWhite, cardColor);
                header[2, _cardWidth - 2] = new Conxel(" ", null, cardColor);
                header[2, _cardWidth - 1] = new Conxel(_vertBorderChar, borderColor);
                header.Blit(StandardLine(borderColor, cardColor), 3, 0);
            }

            return header;
        }

        private ConxelCanvas RequirementsLine(RgbColor borderColor, TokenGroup cost, int victoryPoints)
        {
            var requirements = GetRequirementsEnumerable(cost);

            var reqLine = new ConxelCanvas(1, _cardWidth);
            reqLine[0, 0] = new Conxel(_vertBorderChar, borderColor);

            int i = 2;
            foreach (var requirement in requirements)
            {
                reqLine[0, i] = new Conxel(requirement.Value.ToString(), requirement.Key);
                i += 2;
            }

            reqLine[0, _cardWidth - 5] = new Conxel("{", RgbColor.FullWhite);
            reqLine[0, _cardWidth - 4] = new Conxel(victoryPoints.ToString(), RgbColor.FullWhite);
            reqLine[0, _cardWidth - 3] = new Conxel("}", RgbColor.FullWhite);
            
            reqLine[0, _cardWidth - 1] = new Conxel(_vertBorderChar, borderColor);

            return reqLine;
        }

        private RgbColor GetDarkBonusColor(DevelopmentBonus bonus)
        {
            return bonus switch
            {
                DevelopmentBonus.Yellow => RgbColor.DarkSetYellow,
                DevelopmentBonus.Blue => RgbColor.DarkSetBlue,
                DevelopmentBonus.Green => RgbColor.DarkSetGreen,
                DevelopmentBonus.Brown => RgbColor.DarkSetBrown,
                DevelopmentBonus.Red => RgbColor.DarkSetRed,
                _ => RgbColor.FullWhite
            };
        }

        private RgbColor GetLightBonusColor(DevelopmentBonus bonus)
        {
            return bonus switch
            {
                DevelopmentBonus.Yellow => RgbColor.DarkSetYellow,
                DevelopmentBonus.Blue => RgbColor.DarkSetBlue,
                DevelopmentBonus.Green => RgbColor.DarkSetGreen,
                DevelopmentBonus.Brown => RgbColor.DarkSetBrown,
                DevelopmentBonus.Red => RgbColor.DarkSetRed,
                _ => RgbColor.FullWhite
            };
        }

        public IEnumerable<KeyValuePair<RgbColor, int>> GetRequirementsEnumerable(TokenGroup cost)
        {
            var costs = new Dictionary<RgbColor, int>();

            if (cost.Yellow > 0)
                yield return new KeyValuePair<RgbColor, int>(RgbColor.LightSetYellow, cost.Yellow);

            if (cost.Blue > 0)
                yield return new KeyValuePair<RgbColor, int>(RgbColor.LightSetBlue, cost.Blue);

            if (cost.Green > 0)
                yield return new KeyValuePair<RgbColor, int>(RgbColor.LightSetGreen, cost.Green);

            if (cost.Brown > 0)
                yield return new KeyValuePair<RgbColor, int>(RgbColor.LightSetBrown, cost.Brown);

            if (cost.Red > 0)
                yield return new KeyValuePair<RgbColor, int>(RgbColor.LightSetRed, cost.Red);

            if (cost.Polymath > 0)
                yield return new KeyValuePair<RgbColor, int>(RgbColor.FullWhite, cost.Polymath);

            yield break;
        }
    }
}
