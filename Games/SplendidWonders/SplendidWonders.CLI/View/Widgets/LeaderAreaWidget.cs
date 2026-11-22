using SplendidWonders.Common.Model;
using TTOptimize.ConsoleUI.View;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.CLI.View.Widgets;

public class LeaderAreaWidget : WidgetBase, IConxelWidget
{
    public int Height => _innerHeight + _defaultBorderWidth * 2;
    public int Width => _leaderCount * (_innerWidth + _defaultBorderWidth * 2) + _leaderSpacing * (_leaderCount - 1);

    private int _leaderSpacing = LayoutConstants.DOUBLESPACING;
    private int _innerHeight = LayoutConstants.LEADERHEIGHT;
    private int _innerWidth = LayoutConstants.LEADERWIDTH;

    private string _leaderSymbol = "♛";
    private int _leaderCount;

    public ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions)
    {
        var swState = (GameState) state;
        _leaderCount = swState.Leaders.Count;

        var leaderAreaCanvas = new ConxelCanvas(Height, Width);

        for (int i = 0; i < swState.Leaders.Count; i++)
        {
            var leader = swState.Leaders[i];

            leaderAreaCanvas.Blit(LeaderBlock(leader), 0, i * (_innerWidth + 2) + _leaderSpacing * i);
        }

        return leaderAreaCanvas;
    }

    public void CalculateDynamicWidth(GameState state)
    {
        _leaderCount = state.Leaders.Count;
    }

    private ConxelCanvas LeaderBlock(Leader leader)
    {
        var leaderCanvas = new ConxelCanvas(_innerHeight, _innerWidth);

        leaderCanvas.Blit(ColoredBlock(ColorHelper.DarkRed, _innerWidth), 0, 0);
        leaderCanvas.Blit(CenteredTextLine(ColorHelper.DarkRed, $"{_leaderSymbol} {leader.Name}", _innerWidth), 1, 0);
        leaderCanvas.Blit(ColoredBlock(ColorHelper.DarkRed, _innerWidth), 2, 0);
        leaderCanvas.Blit(RequirementsLine(leader), 3, 0);
        leaderCanvas.Blit(ColoredBlock(ColorHelper.DarkRed, _innerWidth), 4, 0);

        return leaderCanvas.WithDoubleBorder(ColorHelper.DarkRed).WithCorners(ColorHelper.DarkRed);
    }

    private ConxelCanvas RequirementsLine(Leader leader)
    {
        var reqLine = ColoredBlock(ColorHelper.DarkRed, _innerWidth);

        int i = 2;
        foreach (var bonus in leader.Requirements.AsEnumerable().Where(kvp => kvp.Value > 0))
        {
            var bonusBackgroundColor = ColorHelper.DarkSetColor(bonus.Key);

            reqLine[0, i] = new Conxel(" ", null, bonusBackgroundColor);
            reqLine[0, i + 1] = new Conxel(bonus.Value.ToString(), ColorHelper.FullWhite, bonusBackgroundColor);
            reqLine[0, i + 2] = new Conxel(" ", null, bonusBackgroundColor);

            i += 4;
        }

        reqLine[0, _innerWidth - 4] = new Conxel("{", ColorHelper.FullWhite, ColorHelper.DarkRed);
        reqLine[0, _innerWidth - 3] = new Conxel("3", ColorHelper.FullWhite, ColorHelper.DarkRed);
        reqLine[0, _innerWidth - 2] = new Conxel("}", ColorHelper.FullWhite, ColorHelper.DarkRed);

        return reqLine;
    }
}
