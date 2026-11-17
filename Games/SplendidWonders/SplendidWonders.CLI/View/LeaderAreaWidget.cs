using SplendidWonders.Common.Model;
using TTOptimize.ConsoleUI.View;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.CLI.View;

public class LeaderAreaWidget : WidgetBase, IConxelWidget
{
    public int Rows => _leaderBlockHeight + 2;
    public int Columns => _leaderCount * (_leaderBlockWidth + 2) + _spacing * (_leaderCount - 1);

    private int _leaderCount;

    private int _spacing = 2;

    private int _leaderBlockHeight = 5;
    private int _leaderBlockWidth = 22;

    private string _leaderSymbol = "♛";

    public ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions)
    {
        var swState = (GameState) state;
        _leaderCount = swState.Leaders.Count;

        var leaderAreaCanvas = new ConxelCanvas(Rows, Columns);

        for (int i = 0; i < swState.Leaders.Count; i++)
        {
            var leader = swState.Leaders[i];

            leaderAreaCanvas.Blit(LeaderBlock(leader), 0, i * (_leaderBlockWidth + 2) + _spacing * i);
        }

        return leaderAreaCanvas;
    }

    public void CalculateDynamicWidth(GameState state)
    {
        _leaderCount = state.Leaders.Count;
    }

    private ConxelCanvas LeaderBlock(Leader leader)
    {
        var leaderCanvas = new ConxelCanvas(_leaderBlockHeight, _leaderBlockWidth);

        leaderCanvas.Blit(ColoredBlock(ColorHelper.DarkRed, _leaderBlockWidth), 0, 0);
        leaderCanvas.Blit(CenteredTextLine(ColorHelper.DarkRed, $"{_leaderSymbol} {leader.Name}", _leaderBlockWidth), 1, 0);
        leaderCanvas.Blit(ColoredBlock(ColorHelper.DarkRed, _leaderBlockWidth), 2, 0);
        leaderCanvas.Blit(RequirementsLine(leader), 3, 0);
        leaderCanvas.Blit(ColoredBlock(ColorHelper.DarkRed, _leaderBlockWidth), 4, 0);

        return leaderCanvas.WithDoubleBorder(ColorHelper.DarkRed).WithCorners(ColorHelper.DarkRed);
    }

    private ConxelCanvas RequirementsLine(Leader leader)
    {
        var reqLine = ColoredBlock(ColorHelper.DarkRed, _leaderBlockWidth);

        int i = 2;
        foreach (var bonus in leader.Requirements.AsEnumerable().Where(kvp => kvp.Value > 0))
        {
            var bonusBackgroundColor = ColorHelper.DarkSetColor(bonus.Key);

            reqLine[0, i] = new Conxel(" ", null, bonusBackgroundColor);
            reqLine[0, i + 1] = new Conxel(bonus.Value.ToString(), ColorHelper.FullWhite, bonusBackgroundColor);
            reqLine[0, i + 2] = new Conxel(" ", null, bonusBackgroundColor);

            i += 4;
        }

        reqLine[0, _leaderBlockWidth - 4] = new Conxel("{", ColorHelper.FullWhite, ColorHelper.DarkRed);
        reqLine[0, _leaderBlockWidth - 3] = new Conxel("3", ColorHelper.FullWhite, ColorHelper.DarkRed);
        reqLine[0, _leaderBlockWidth - 2] = new Conxel("}", ColorHelper.FullWhite, ColorHelper.DarkRed);

        return reqLine;
    }
}
