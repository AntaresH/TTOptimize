using SplendidWonders.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View
{
    public class TokenSupplyWidget : WidgetBase, IConxelWidget
    {
        public ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions)
        {
            var swState = (GameState)state;

            var tokenSupplyCanvas = new ConxelCanvas(23, 5);

            tokenSupplyCanvas.Blit(NormalPile(swState.TokenSupply.Yellow, RgbColor.LightSetYellow), 0, 0);
            tokenSupplyCanvas.Blit(NormalPile(swState.TokenSupply.Blue, RgbColor.LightSetBlue), 4, 0);
            tokenSupplyCanvas.Blit(NormalPile(swState.TokenSupply.Green, RgbColor.LightSetGreen), 8, 0);
            tokenSupplyCanvas.Blit(NormalPile(swState.TokenSupply.Brown, RgbColor.LightSetBrown), 12, 0);
            tokenSupplyCanvas.Blit(NormalPile(swState.TokenSupply.Red, RgbColor.LightSetRed), 16, 0);
            tokenSupplyCanvas.Blit(PolymathPile(swState.TokenSupply.Polymath), 20, 0);

            return tokenSupplyCanvas;
        }

        private ConxelCanvas NormalPile(int tokens, RgbColor color)
        {
            return SingleBorder(RenderPile(tokens, color), color);
        }

        private ConxelCanvas PolymathPile(int tokens)
        {
            return DoubleBorder(RenderPile(tokens, RgbColor.FullWhite), RgbColor.FullWhite);
        }

        private ConxelCanvas RenderPile(int tokens, RgbColor foreGroundColor)
        {
            var pileBuffer = new ConxelCanvas(1, 3);

            pileBuffer[0, 1] = new Conxel(tokens.ToString(), foreGroundColor);

            return pileBuffer;
        }
    }
}
