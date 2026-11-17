using SplendidWonders.Common.Model;
using TTOptimize.ConsoleUI.View;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.CLI.View;

public class TokenSupplyWidget : WidgetBase, IConxelWidget
{
    public int Rows => 6 * (_pileHeight + 2) + 5;

    public int Columns => _pileWidth + 2;

    private int _pileHeight = 1;

    private int _pileWidth = 3;

    public ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions)
    {
        var swState = (GameState)state;

        var tokenSupplyCanvas = new ConxelCanvas(Rows, Columns);

        tokenSupplyCanvas.Blit(NormalPile(swState.TokenSupply.Yellow, ColorHelper.LightSetYellow), 0, 0);
        tokenSupplyCanvas.Blit(NormalPile(swState.TokenSupply.Blue, ColorHelper.LightSetBlue), 4, 0);
        tokenSupplyCanvas.Blit(NormalPile(swState.TokenSupply.Green, ColorHelper.LightSetGreen), 8, 0);
        tokenSupplyCanvas.Blit(NormalPile(swState.TokenSupply.Brown, ColorHelper.LightSetBrown), 12, 0);
        tokenSupplyCanvas.Blit(NormalPile(swState.TokenSupply.Red, ColorHelper.LightSetRed), 16, 0);
        tokenSupplyCanvas.Blit(PolymathPile(swState.TokenSupply.Polymath), 20, 0);

        return tokenSupplyCanvas;
    }

    private ConxelCanvas NormalPile(int tokens, RgbColor color)
    {
        return Pile(tokens, color).WithSingleBorder(color);
    }

    private ConxelCanvas PolymathPile(int tokens)
    {
        return Pile(tokens, ColorHelper.FullWhite).WithDoubleBorder(ColorHelper.FullWhite);
    }

    private ConxelCanvas Pile(int tokens, RgbColor foreGroundColor)
    {
        var pileBuffer = new ConxelCanvas(1, 3);

        pileBuffer[0, 1] = new Conxel(tokens.ToString(), foreGroundColor);

        return pileBuffer;
    }
}
