using SplendidWonders.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View
{
    public class TokenSupplyWidget : IConxelWidget
    {
        private string defaultHorizBorderChar = "-";
        private string defaultVertBorderChar = "|";
        private string polymathHorizBorderChar = "=";
        private string polymathVertBorderChar = "\u2551";
        public ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions)
        {
            var swState = (GameState)state;

            var tokenSupplyCanvas = new ConxelCanvas(23, 5);

            tokenSupplyCanvas.Blit(RenderNormalPile(swState.TokenSupply.Yellow, RgbColor.LightSetYellow), 0, 0);
            tokenSupplyCanvas.Blit(RenderNormalPile(swState.TokenSupply.Blue, RgbColor.LightSetBlue), 4, 0);
            tokenSupplyCanvas.Blit(RenderNormalPile(swState.TokenSupply.Green, RgbColor.LightSetGreen), 8, 0);
            tokenSupplyCanvas.Blit(RenderNormalPile(swState.TokenSupply.Brown, RgbColor.LightSetBrown), 12, 0);
            tokenSupplyCanvas.Blit(RenderNormalPile(swState.TokenSupply.Red, RgbColor.LightSetRed), 16, 0);
            tokenSupplyCanvas.Blit(RenderPolymathPile(swState.TokenSupply.Polymath), 20, 0);

            return tokenSupplyCanvas;
        }

        private ConxelCanvas RenderNormalPile(int tokens, RgbColor color)
        {
            return RenderPile(tokens, defaultVertBorderChar, defaultHorizBorderChar, color);
        }

        private ConxelCanvas RenderPolymathPile(int tokens)
        {
            return RenderPile(tokens, polymathVertBorderChar, polymathHorizBorderChar, RgbColor.FullWhite);
        }

        private ConxelCanvas RenderPile(int tokens, string vertBorder, string horizBorder, RgbColor foreGroundColor)
        {
            var pileBuffer = new ConxelCanvas(3, 5);

            pileBuffer[0, 1] = new Conxel(horizBorder, foreGroundColor);
            pileBuffer[0, 2] = new Conxel(horizBorder, foreGroundColor);
            pileBuffer[0, 3] = new Conxel(horizBorder, foreGroundColor);

            pileBuffer[1, 0] = new Conxel(vertBorder, foreGroundColor);
            pileBuffer[1, 2] = new Conxel(tokens.ToString(), foreGroundColor);
            pileBuffer[1, 4] = new Conxel(vertBorder, foreGroundColor);

            pileBuffer[2, 1] = new Conxel(horizBorder, foreGroundColor);
            pileBuffer[2, 2] = new Conxel(horizBorder, foreGroundColor);
            pileBuffer[2, 3] = new Conxel(horizBorder, foreGroundColor);

            return pileBuffer;
        }
    }
}
