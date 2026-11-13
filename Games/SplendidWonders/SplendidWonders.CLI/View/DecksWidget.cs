using SplendidWonders.Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View
{
    public class DecksWidget : IConxelWidget
    {
        private string _horizBorderChar = "=";
        private string _vertBorderChar = "\u2551";
        private string _cornerChar = "+";
        private int _deckHeight = 8;
        private int _deckWidth = 18;
        private string _level3Icon = "\uD83C\uDFDB "; // the extra space is VERY necessary as the 🏛 is a UTF16 surrogate-pair which is condensed into one column
        private string _level2Icon = "⛯";
        private string _level1Icon = "▓"; // try the double-length 🧱 (the counter problem of the level 3 icon)


        public ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions)
        {
            var swState = (GameState)state;

            var decksCanvas = new ConxelCanvas(3 * _deckHeight + 2, _deckWidth);

            decksCanvas.Blit(RenderDeck($"{_level3Icon} {Strings.Level3Title}", swState.Level3.Deck.Count, RgbColor.DarkMagenta), 0, 0);
            decksCanvas.Blit(RenderDeck($"{_level2Icon} {Strings.Level2Title}", swState.Level2.Deck.Count, RgbColor.Turqoise), _deckHeight + 1, 0);
            decksCanvas.Blit(RenderDeck($"{_level1Icon} {Strings.Level1Title}", swState.Level1.Deck.Count, RgbColor.Orange), 2 * _deckHeight + 2, 0);

            return decksCanvas;
        }

        private ConxelCanvas RenderDeck(string deckTitle, int remaining, RgbColor deckColor)
        {
            var deckCanvas = new ConxelCanvas(_deckHeight, _deckWidth);

            deckCanvas.WriteText(0, 0, $"{_cornerChar}{string.Concat(Enumerable.Repeat(_horizBorderChar, _deckWidth-2))}{_cornerChar}", deckColor);
            
            deckCanvas.Blit(StandardLine(deckColor), 1, 0);
            deckCanvas.Blit(StandardLine(deckColor), 2, 0);
            deckCanvas.Blit(CenteredTextLine(deckColor, deckTitle), 3, 0);
            deckCanvas.Blit(CenteredTextLine(deckColor, remaining.ToString()), 4, 0);
            deckCanvas.Blit(StandardLine(deckColor), 5, 0);
            deckCanvas.Blit(StandardLine(deckColor), 6, 0);

            deckCanvas.WriteText(_deckHeight -1, 0, $"{_cornerChar}{string.Concat(Enumerable.Repeat(_horizBorderChar, _deckWidth - 2))}{_cornerChar}", deckColor);

            return deckCanvas;
        }

        private ConxelCanvas CenteredTextLine(RgbColor deckColor, string deckTitle)
        {
            var retBuffer = new ConxelCanvas(1, _deckWidth);
            retBuffer[0, 0] = new Conxel(_vertBorderChar, deckColor);
            retBuffer.WriteText(0, 1, WidgetRenderingHelper.CenterText(deckTitle, _deckWidth - 2), RgbColor.FullWhite, deckColor);
            retBuffer[0, _deckWidth - 1] = new Conxel(_vertBorderChar, deckColor);

            return retBuffer;
        }

        private ConxelCanvas StandardLine(RgbColor deckColor)
        {
            var retBuffer = new ConxelCanvas(1, _deckWidth);
            retBuffer[0, 0] = new Conxel(_vertBorderChar, deckColor);
            retBuffer.WriteText(0, 1, $"{new string(' ', _deckWidth - 2)}", null, deckColor);
            retBuffer[0, _deckWidth - 1] = new Conxel(_vertBorderChar, deckColor);

            return retBuffer;
        }
    }
}
