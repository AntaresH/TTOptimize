using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Model;
using TTOptimize.ConsoleUI.View;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View;

public class SplendidWondersBoardBuilder : IConxelPaneBuilder<BoardRenderData>
{
    private const int MinCanvasRows = 34;
    private const int MinCanvasColumns = 140;

    private readonly LeaderAreaWidget _leaderAreaWidget;
    private readonly TokenSupplyWidget _tokenSupplyWidget;
    private readonly DecksWidget _decksWidget;
    private readonly MarketWidget _marketWidget;
    private readonly PlayerAreaWidget _playerAreaWidget;

    public SplendidWondersBoardBuilder(
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] TokenSupplyWidget tokenSupplyWidget,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] DecksWidget decksWidget,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] LeaderAreaWidget leaderAreaWidget,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] MarketWidget marketWidget,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] PlayerAreaWidget playerAreaWidget)
    {
        _tokenSupplyWidget = tokenSupplyWidget;
        _decksWidget = decksWidget;
        _marketWidget = marketWidget;
        _leaderAreaWidget = leaderAreaWidget;
        _playerAreaWidget = playerAreaWidget;
    }

    public ConxelCanvas Build(BoardRenderData paneData)
    {
        var swState = (GameState)paneData.State;
        var validActions = paneData.ValidActions;

        return swState.Leaders.Any()
            ? BoardWithLeaders(swState, validActions)
            : BoardWithoutLeaders(swState, validActions);
    }

    private ConxelCanvas BoardWithoutLeaders(GameState state, IReadOnlyList<GameAction> legalActions)
    {
        var canvas = new ConxelCanvas(MinCanvasRows, MinCanvasColumns);

        canvas.Blit(_tokenSupplyWidget.RenderToBuffer(state, legalActions), startRow: 2, startColumn: 1);
        
        canvas.Blit(_decksWidget.RenderToBuffer(state, legalActions), startRow: 0, startColumn: 8);
        
        canvas.Blit(_marketWidget.RenderToBuffer(state, legalActions), startRow: 0, startColumn: 29);

        _playerAreaWidget.CalculateDynamicWidth(state);
        var playerStartColumn = _tokenSupplyWidget.Columns + 3 + (_decksWidget.Columns + _marketWidget.Columns + 1 - _playerAreaWidget.Columns) / 2;

        canvas.Blit(_playerAreaWidget.RenderToBuffer(state, legalActions), startRow: 27, startColumn: playerStartColumn);

        return canvas;
    }

    private ConxelCanvas BoardWithLeaders(GameState state, IReadOnlyList<GameAction> legalActions)
    {
        var canvas = new ConxelCanvas(MinCanvasRows + _leaderAreaWidget.Rows + 1, MinCanvasColumns);

        _leaderAreaWidget.CalculateDynamicWidth(state);

        var leaderStartColumn = _tokenSupplyWidget.Columns + 3 + (_decksWidget.Columns +_marketWidget.Columns + 1 - _leaderAreaWidget.Columns) / 2;

        canvas.Blit(_leaderAreaWidget.RenderToBuffer(state, legalActions), startRow: 0, startColumn: leaderStartColumn);
        canvas.Blit(BoardWithoutLeaders(state, legalActions), startRow: _leaderAreaWidget.Rows + 1, startColumn: 0);

        return canvas;
    }
}
