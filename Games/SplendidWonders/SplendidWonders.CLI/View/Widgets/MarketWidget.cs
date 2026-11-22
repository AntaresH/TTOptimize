using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using TTOptimize.ConsoleUI.View;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.CLI.View.Widgets;

public class MarketWidget : WidgetBase, IConxelWidget
{
    public int Height => 3 * (_cardHeight + 2 * _defaultBorderWidth) + 2 * _spacing;
    public int Width => 4 * (_cardWidth + 2 * _defaultBorderWidth) + 3 * _spacing;

    private int _spacing = LayoutConstants.SINGLESPACING;
    private int _cardHeight = LayoutConstants.CARDHEIGHT;
    private int _cardWidth = LayoutConstants.CARDWIDTH;

    public ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions)
    {
        var swState = (GameState)state;

        var marketCanvas = new ConxelCanvas(Height, Width);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                var developmentCard = swState[3 - i][j + 1];
                var purchaseAction = legalActions?.OfType<PurchaseAction>()?.FirstOrDefault(pa => pa.Card == developmentCard);

                var cardCanvas = developmentCard == Cards.Empty
                                    ? EmptyCard(ColorHelper.DarkSetDefault)
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
        var cardColor = ColorHelper.DarkSetColor(developmentCard.Bonus);
        var borderColor = respectivePurchaseAction == null
                            ? cardColor
                            : respectivePurchaseAction.RequiredTokens.Polymath == 0
                                ? ColorHelper.LightSetYellow
                                : ColorHelper.WarmWhite;

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

        var (nameLine1, nameLine2) = WrapAndCenterText(title, _cardWidth - 2);

        header.Blit(ColoredBlock(cardColor, _cardWidth), 0, 0);
        header.WriteText(1, 0, $" " + nameLine1 + " ", ColorHelper.FullWhite, cardColor);

        if (string.IsNullOrWhiteSpace(nameLine2))
        {
            header.Blit(ColoredBlock(cardColor, _cardWidth), 2, 0);
            header.Blit(EmptyBlock(_cardWidth), 3, 0);
        }
        else
        {
            header.WriteText(2, 0, $" " + nameLine2 + " ", ColorHelper.FullWhite, cardColor);
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
            reqLine[0, i] = new Conxel(requirement.Value.ToString(), ColorHelper.LightSetColor(requirement.Key));
            i += 2;
        }

        if (victoryPoints > 0)
        {
            reqLine[0, _cardWidth - 4] = new Conxel("{", ColorHelper.FullWhite);
            reqLine[0, _cardWidth - 3] = new Conxel(victoryPoints.ToString(), ColorHelper.FullWhite);
            reqLine[0, _cardWidth - 2] = new Conxel("}", ColorHelper.FullWhite);
        }

        return reqLine;
    }
}
