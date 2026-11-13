using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Model;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View;

public class SplendidWondersLayoutManager : ILayoutManager
{
    private const int CanvasWidth = 140;
    private const int CanvasHeight = 42;

    private readonly LeaderAreaWidget _leaderAreaWidget;
    private readonly TokenSupplyWidget _tokenSupplyWidget;
    private readonly DecksWidget _decksWidget;
    private readonly MarketWidget _marketWidget;
    private readonly PlayerAreaWidget _playerAreaWidget;

    public SplendidWondersLayoutManager(
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

    public ConxelCanvas BuildCanvas(IGameState state, IReadOnlyList<GameAction>? legalActions, string? errorMessage)
    {
        var canvas = new ConxelCanvas(CanvasHeight, CanvasWidth);

        canvas.Blit(_tokenSupplyWidget.RenderToBuffer(state, legalActions), startColumn: 0, startRow: 2);
        canvas.Blit(_decksWidget.RenderToBuffer(state, legalActions), startColumn: 7, startRow: 0);

        //canvas.Blit(_leaderAreaWidget.RenderToBuffer(state, legalActions), startX: 85, startY: 0);

        //canvas.Blit(_playerAreaWidget.RenderToBuffer(state, legalActions), startX: 0, startY: 28);

        //if (!string.IsNullOrEmpty(errorMessage))
        //{
        //    canvas.WriteText(0, CanvasHeight - 1, $"[HIBA] {errorMessage}", foreground: new RgbColor(255, 80, 80));
        //}

        return canvas;
    }
}
