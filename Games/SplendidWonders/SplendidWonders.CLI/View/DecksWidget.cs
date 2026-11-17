using SplendidWonders.Common.Model;
using TTOptimize.ConsoleUI.View;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.CLI.View;

public class DecksWidget : WidgetBase, IConxelWidget
{
    public int Rows => 3 * (_innerHeight + 2) + 3;
    public int Columns => _innerWidth + 2;

    private int _innerHeight = 6;
    private int _innerWidth = 18;

    private string _level3Icon = "\uD83C\uDFDB "; // the extra space is VERY necessary as the 🏛 is a UTF16 surrogate-pair which is condensed into one column
    private string _level2Icon = "⛯";
    private string _level1Icon = "▓"; // try the double-length 🧱 (the counter problem of the level 3 icon)

    public ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions)
    {
        var swState = (GameState)state;

        var borderedDeckHeight = _innerHeight + 2;
        var borderedDeckWidth = _innerWidth + 2;

        var decksCanvas = new ConxelCanvas(Rows, Columns);

        decksCanvas.Blit(Deck($"{_level3Icon} {Strings.Level3Title}", swState.Level3.Deck.Count, ColorHelper.DarkMagenta), 0, 0);
        decksCanvas.Blit(Deck($"{_level2Icon} {Strings.Level2Title}", swState.Level2.Deck.Count, ColorHelper.Turqoise), _innerHeight + 3, 0);
        decksCanvas.Blit(Deck($"{_level1Icon} {Strings.Level1Title}", swState.Level1.Deck.Count, ColorHelper.Orange), 2 * _innerHeight + 6, 0);

        return decksCanvas;
    }

    private ConxelCanvas Deck(string deckTitle, int remaining, RgbColor deckColor)
    {
        var deckCanvas = new ConxelCanvas(_innerHeight, _innerWidth);
                    
        deckCanvas.Blit(ColoredBlock(deckColor, _innerWidth), 0, 0);
        deckCanvas.Blit(ColoredBlock(deckColor, _innerWidth), 1, 0);
        deckCanvas.Blit(CenteredTextLine(deckColor, deckTitle, _innerWidth), 2, 0);
        deckCanvas.Blit(ColoredBlock(deckColor, _innerWidth), 3, 0);
        deckCanvas.Blit(CenteredTextLine(deckColor, remaining.ToString(), _innerWidth), 4, 0);
        deckCanvas.Blit(ColoredBlock(deckColor, _innerWidth), 5, 0);

        return deckCanvas.WithDoubleBorder(deckColor).WithCorners(deckColor);
    }
}
