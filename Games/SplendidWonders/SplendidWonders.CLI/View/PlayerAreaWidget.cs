using SplendidWonders.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View
{
    public class PlayerAreaWidget : WidgetBase, IConxelWidget
    {
        private string _horizBorderChar = "-";
        private string _vertBorderChar = "|";
        private string _cornerChar = "+";

        private int _playerBlockHeight = 6;
        private int _playerBlockWidth = 20;

        public ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions)
        {
            var swState = (GameState)state;

            var playerAreaCanvas = new ConxelCanvas(_playerBlockHeight, swState.Players.Count * _playerBlockWidth + swState.Players.Count - 1);

            for (int i = 0; i < swState.Players.Count; i++)
            {
                var player = swState.Players[i];
                var isActive = swState.ActivePlayerId == player.Id;
                var blockColor = isActive ? RgbColor.LightSetDefault : RgbColor.DarkSetDefault;

                playerAreaCanvas.Blit(RenderPlayerBlock(player, blockColor), 0, i * swState.Players.Count + i);
            }

            return playerAreaCanvas;
        }

        private ConxelCanvas RenderPlayerBlock(Player player, RgbColor blockColor)
        {
            var playerBlockCanvas = new ConxelCanvas(_playerBlockHeight, _playerBlockWidth);


            //playerBlockCanvas.WriteText(1, 1, player.Name, blockColor);
            //playerBlockCanvas.Blit(StoreLine(player.Store, player.ReservedCards), 2, 1);
            //playerBlockCanvas.Blit(TableauLine(player.Tableau), 4, 1);

            return playerBlockCanvas;
        }
    }
}
