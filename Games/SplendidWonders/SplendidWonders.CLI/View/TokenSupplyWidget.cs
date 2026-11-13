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
        private char defaultHorizBorderChar = '-';
        private char defaultVertBorderChar = '|';
        private char polymathHorizBorderChar = '=';
        private char polymathVertBorderChar = (char)0x2551;
        public ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions)
        {
            var swState = (GameState)state;

            var tokenSupplyCanvas = new ConxelCanvas(5, 23);

            tokenSupplyCanvas.Blit(RenderNormalPile(swState.TokenSupply.Yellow, RgbColor.LightSetYellow), 0, 0);
            tokenSupplyCanvas.Blit(RenderNormalPile(swState.TokenSupply.Blue, RgbColor.LightSetBlue), 0, 4);
            tokenSupplyCanvas.Blit(RenderNormalPile(swState.TokenSupply.Green, RgbColor.LightSetGreen), 0, 8);
            tokenSupplyCanvas.Blit(RenderNormalPile(swState.TokenSupply.Brown, RgbColor.LightSetBrown), 0, 12);
            tokenSupplyCanvas.Blit(RenderNormalPile(swState.TokenSupply.Red, RgbColor.LightSetRed), 0, 16);
            tokenSupplyCanvas.Blit(RenderPolymathPile(swState.TokenSupply.Polymath), 0, 20);

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

        private ConxelCanvas RenderPile(int tokens, char vertBorder, char horizBorder, RgbColor foreGroundColor)
        {
            var pileBuffer = new ConxelCanvas(5, 3);

            pileBuffer[0, 1] = new Conxel(horizBorder, foreGroundColor);
            pileBuffer[0, 2] = new Conxel(horizBorder, foreGroundColor);
            pileBuffer[0, 3] = new Conxel(horizBorder, foreGroundColor);

            pileBuffer[1, 0] = new Conxel(vertBorder, foreGroundColor);
            pileBuffer[1, 2] = new Conxel((char)('0' + tokens), foreGroundColor);
            pileBuffer[1, 4] = new Conxel(vertBorder, foreGroundColor);

            pileBuffer[2, 1] = new Conxel(horizBorder, foreGroundColor);
            pileBuffer[2, 2] = new Conxel(horizBorder, foreGroundColor);
            pileBuffer[2, 3] = new Conxel(horizBorder, foreGroundColor);

            return pileBuffer;
        }
    }
}
