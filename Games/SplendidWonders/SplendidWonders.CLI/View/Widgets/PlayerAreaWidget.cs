using SplendidWonders.Common.Model;
using System.Collections.Immutable;
using TTOptimize.ConsoleUI.View;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.CLI.View.Widgets;

public class PlayerAreaWidget : WidgetBase, IConxelWidget
{
    public int Height => _playerBlockHeight + 2 * _defaultBorderWidth;
    public int Width => _playerCount * (_playerBlockWidth + 2 * _defaultBorderWidth) + _playerSpacing * (_playerCount - 1);

    private int _playerSpacing = 4;
    private int _playerCount;

    private int _playerBlockHeight = LayoutConstants.PLAYERHEIGHT;
    private int _playerBlockWidth = LayoutConstants.PLAYERWIDTH;

    private string _cardSymbol = "🂠";

    public ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions)
    {
        var swState = (GameState)state;
        _playerCount = swState.Players.Count;

        var playerAreaCanvas = new ConxelCanvas(Height, Width);

        for (int i = 0; i < swState.Players.Count; i++)
        {
            var playerBlock = PlayerBlock(swState, i);

            playerAreaCanvas.Blit(playerBlock, 0, i * (_playerBlockWidth + 2) + _playerSpacing * i);
        }

        return playerAreaCanvas;
    }

    private ConxelCanvas PlayerBlock(GameState swState, int i)
    {
        var player = swState.Players[i];
        var isActive = swState.ActivePlayerId == player.Id;

        var blockColor = isActive ? ColorHelper.WarmWhite : ColorHelper.LightSetDefault;
        var playerColor = GetPlayerSpecificForeground(i + 1, isActive);

        var playerBlock = InnerBlock(player, playerColor, blockColor);

        var borderedBlock = isActive
            ? playerBlock.WithDoubleBorder(playerColor)
            : playerBlock.WithSingleBorder(playerColor);

        return borderedBlock.WithCorners(playerColor);
    }

    public void CalculateDynamicWidth(GameState state)
    {
        _playerCount = state.Players.Count;
    }

    private ConxelCanvas InnerBlock(Player player, RgbColor playerColor, RgbColor blockColor)
    {
        var playerBlockCanvas = new ConxelCanvas(_playerBlockHeight, _playerBlockWidth);

        int victoryPoints = player.Tableau.Sum(dc => dc.VictoryPoints) + player.AcquiredLeaders.Count * 3;

        playerBlockCanvas.Blit(NameLine(player.Name, player.Tableau, victoryPoints, playerColor, blockColor), 0, 0);
        playerBlockCanvas.Blit(StoreLine(player.Store, player.ReservedCards, blockColor), 2, 0);
        playerBlockCanvas.Blit(TableauLine(player.Tableau), 4, 0);

        return playerBlockCanvas;
    }

    private ConxelCanvas NameLine(string name, ImmutableList<DevelopmentCard> tableau, int victoryPoints, RgbColor playerColor, RgbColor blockColor)
    {
        var nameLine = new ConxelCanvas(1, _playerBlockWidth);

        nameLine.WriteText(0, 1, name, playerColor);

        if (victoryPoints > 0)
        {
            int startIndex = victoryPoints < 10 ? 17 : 16;
            nameLine[0, startIndex] = new Conxel("{", blockColor);
            nameLine.WriteText(0, startIndex + 1, victoryPoints.ToString(), blockColor);
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
            storeLine[0, i] = new Conxel(requirement.Value.ToString(), ColorHelper.LightSetColor(requirement.Key));
            i += 2;
        }

        storeLine[0, 12] = new Conxel(store.Polymath.ToString(), ColorHelper.FullWhite);

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
            tableauLine[0, i] = new Conxel(" ", null, ColorHelper.DarkSetColor(bonus.Key));
            tableauLine[0, i + 1] = new Conxel(bonus.Value.ToString(), ColorHelper.FullWhite, ColorHelper.DarkSetColor(bonus.Key));
            tableauLine[0, i + 2] = new Conxel(" ", null, ColorHelper.DarkSetColor(bonus.Key));

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

    private RgbColor GetPlayerSpecificForeground(int playerId, bool isActive)
    {
        return playerId switch
        {
            1 => isActive ? ColorHelper.PaleSetBlue : ColorHelper.PaleSetDarkBlue,
            2 => isActive ? ColorHelper.PaleSetYellow : ColorHelper.PaleSetDarkYellow,
            3 => isActive ? ColorHelper.PaleSetGreen : ColorHelper.PaleSetDarkGreen,
            4 => isActive ? ColorHelper.PaleSetRed : ColorHelper.PaleSetDarkRed,
            _ => throw new InvalidOperationException("Player count mzst be between 1 and 4!")
        };
    }
}
