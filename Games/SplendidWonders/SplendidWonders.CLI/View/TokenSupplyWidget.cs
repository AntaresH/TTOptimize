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

            tokenSupplyCanvas.Blit(NormalPile(swState.TokenSupply.Yellow, WidgetHelper.LightSetYellow), 0, 0);
            tokenSupplyCanvas.Blit(NormalPile(swState.TokenSupply.Blue, WidgetHelper.LightSetBlue), 4, 0);
            tokenSupplyCanvas.Blit(NormalPile(swState.TokenSupply.Green, WidgetHelper.LightSetGreen), 8, 0);
            tokenSupplyCanvas.Blit(NormalPile(swState.TokenSupply.Brown, WidgetHelper.LightSetBrown), 12, 0);
            tokenSupplyCanvas.Blit(NormalPile(swState.TokenSupply.Red, WidgetHelper.LightSetRed), 16, 0);
            tokenSupplyCanvas.Blit(PolymathPile(swState.TokenSupply.Polymath), 20, 0);

            return tokenSupplyCanvas;
        }

        private ConxelCanvas NormalPile(int tokens, RgbColor color)
        {
            return RenderPile(tokens, color).WithSingleBorder(color);
        }

        private ConxelCanvas PolymathPile(int tokens)
        {
            return RenderPile(tokens, RgbColor.FullWhite).WithDoubleBorder(RgbColor.FullWhite);
        }

        private ConxelCanvas RenderPile(int tokens, RgbColor foreGroundColor)
        {
            var pileBuffer = new ConxelCanvas(1, 3);

            pileBuffer[0, 1] = new Conxel(tokens.ToString(), foreGroundColor);

            return pileBuffer;
        }
    }
}
