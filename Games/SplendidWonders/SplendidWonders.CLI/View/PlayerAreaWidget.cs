using SplendidWonders.Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View
{
    public class PlayerAreaWidget : WidgetBase, IConxelWidget
    {
        private string _cardSymbol = "🂠";
        private int _playerBlockHeight = 5;
        private int _playerBlockWidth = 21;

        public ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions)
        {
            var swState = (GameState)state;

            var borderedPlayerBlockWidth = _playerBlockWidth + 2;
            var borderedPlayerBlockHeight = _playerBlockHeight + 2;

            var playerAreaCanvas = new ConxelCanvas(borderedPlayerBlockHeight, swState.Players.Count * borderedPlayerBlockWidth + swState.Players.Count - 1);

            for (int i = 0; i < swState.Players.Count; i++)
            {
                var player = swState.Players[i];
                var isActive = swState.ActivePlayerId == player.Id;
                var blockColor = isActive ? WidgetHelper.LightSetDefault : WidgetHelper.DarkSetDefault;

                playerAreaCanvas.Blit(RenderPlayerBlock(player, blockColor), 0, i * borderedPlayerBlockWidth + i);
            }

            return playerAreaCanvas;
        }

        private ConxelCanvas RenderPlayerBlock(Player player, RgbColor blockColor)
        {
            var playerBlockCanvas = new ConxelCanvas(_playerBlockHeight, _playerBlockWidth);

            int victoryPoints = player.Tableau.Sum(dc => dc.VictoryPoints) + player.AcquiredLeaders.Count * 3;

            playerBlockCanvas.Blit(NameLine(player.Name, player.Tableau, victoryPoints, blockColor), 0, 0);
            playerBlockCanvas.Blit(StoreLine(player.Store, player.ReservedCards, blockColor), 2, 0);
            playerBlockCanvas.Blit(TableauLine(player.Tableau), 4, 0);

            return playerBlockCanvas.WithSingleBorder(blockColor).WithCorners(blockColor);
        }

        private ConxelCanvas NameLine(string name, ImmutableList<DevelopmentCard> tableau, int victoryPoints, RgbColor blockColor)
        {
            var nameLine = new ConxelCanvas(1, _playerBlockWidth);

            nameLine.WriteText(0, 1, name, blockColor);

            if (victoryPoints > 0)
            {
                nameLine[0, 17] = new Conxel("{", blockColor);
                nameLine[0, 18] = new Conxel(victoryPoints.ToString(), blockColor);
                nameLine[0, 19] = new Conxel("}", blockColor);
            }

            return nameLine;
        }

        private ConxelCanvas StoreLine(TokenGroup store, ImmutableList<DevelopmentCard> reservedCards, RgbColor blockColor)
        {
            var storeLine = new ConxelCanvas(1, _playerBlockWidth);

            var supply = store.AsEnumerable();

            int i = 1;
            foreach (var requirement in store.AsEnumerable().Where(kvp => kvp.Key != DevelopmentColor.Polymath))
            {
                storeLine[0, i] = new Conxel(requirement.Value.ToString(), GetLightDevelopmentColor(requirement.Key));
                i += 2;
            }

            storeLine[0, 12] = new Conxel(store.Polymath.ToString(), RgbColor.FullWhite);

            storeLine[0, 15] = new Conxel(_cardSymbol, blockColor);
            storeLine[0, 17] = new Conxel(":", blockColor);
            storeLine[0, 19] = new Conxel(reservedCards.Count.ToString(), blockColor);

            return storeLine;
        }

        private ConxelCanvas TableauLine(ImmutableList<DevelopmentCard> tableau)
        {
            var tableauLine = new ConxelCanvas(1, _playerBlockWidth);
            var tableauBonuses = GetTableauAsEnumerable(tableau);

            int i = 1;
            foreach (var bonus in tableauBonuses)
            {
                tableauLine[0, i] = new Conxel(" ", null, GetDarkDevelopmentColor(bonus.Key));
                tableauLine[0, i + 1] = new Conxel(bonus.Value.ToString(), RgbColor.FullWhite, GetDarkDevelopmentColor(bonus.Key));
                tableauLine[0, i + 2] = new Conxel(" ", null, GetDarkDevelopmentColor(bonus.Key));

                i += 4;
            }

            return tableauLine;
        }

        private IEnumerable<KeyValuePair<DevelopmentColor, int>> GetTableauAsEnumerable(ImmutableList<DevelopmentCard> tableau)
        {
            yield return new KeyValuePair<DevelopmentColor, int>(DevelopmentColor.Yellow, tableau.Count(c => c.Bonus == DevelopmentColor.Yellow));
            yield return new KeyValuePair<DevelopmentColor, int>(DevelopmentColor.Blue, tableau.Count(c => c.Bonus == DevelopmentColor.Blue));
            yield return new KeyValuePair<DevelopmentColor, int>(DevelopmentColor.Green, tableau.Count(c => c.Bonus == DevelopmentColor.Green));
            yield return new KeyValuePair<DevelopmentColor, int>(DevelopmentColor.Brown, tableau.Count(c => c.Bonus == DevelopmentColor.Brown));
            yield return new KeyValuePair<DevelopmentColor, int>(DevelopmentColor.Red, tableau.Count(c => c.Bonus == DevelopmentColor.Red));

            yield break;
        }
    }
}
