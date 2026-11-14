using SplendidWonders.Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View
{
    public class DecksWidget : WidgetBase, IConxelWidget
    {
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

            var decksCanvas = new ConxelCanvas(3 * borderedDeckHeight + 2, borderedDeckWidth);

            decksCanvas.Blit(RenderDeck($"{_level3Icon} {Strings.Level3Title}", swState.Level3.Deck.Count, RgbColor.DarkMagenta), 0, 0);
            decksCanvas.Blit(RenderDeck($"{_level2Icon} {Strings.Level2Title}", swState.Level2.Deck.Count, RgbColor.Turqoise), borderedDeckHeight + 1, 0);
            decksCanvas.Blit(RenderDeck($"{_level1Icon} {Strings.Level1Title}", swState.Level1.Deck.Count, RgbColor.Orange), 2 * borderedDeckHeight + 2, 0);

            return decksCanvas;
        }

        private ConxelCanvas RenderDeck(string deckTitle, int remaining, RgbColor deckColor)
        {
            var deckCanvas = new ConxelCanvas(_innerHeight, _innerWidth);
                        
            deckCanvas.Blit(ColoredBlock(deckColor, _innerWidth), 0, 0);
            deckCanvas.Blit(ColoredBlock(deckColor, _innerWidth), 1, 0);
            deckCanvas.Blit(CenteredTextLine(deckColor, deckTitle), 2, 0);
            deckCanvas.Blit(ColoredBlock(deckColor, _innerWidth), 3, 0);
            deckCanvas.Blit(CenteredTextLine(deckColor, remaining.ToString()), 4, 0);
            deckCanvas.Blit(ColoredBlock(deckColor, _innerWidth), 5, 0);

            return deckCanvas.WithDoubleBorder(deckColor).WithCorners(deckColor);
        }

        private ConxelCanvas CenteredTextLine(RgbColor deckColor, string deckTitle)
        {
            var retBuffer = new ConxelCanvas(1, _innerWidth);
            retBuffer.WriteText(0, 0, WidgetRenderingHelper.CenterText(deckTitle, _innerWidth), RgbColor.FullWhite, deckColor);

            return retBuffer;
        }
    }
}
