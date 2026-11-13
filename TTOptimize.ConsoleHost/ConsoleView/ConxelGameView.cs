using Microsoft.Extensions.DependencyInjection;
using TTOptimize.ConsoleHost.ConsoleView;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.ViewAbstractions;

public class ConxelGameView : IGameView
{
    private readonly ILayoutManager _layoutManager;
    private readonly ConxelAnsiCompiler _compiler;

    public ConxelGameView(
        ILayoutManager layoutManager,
        ConxelAnsiCompiler compiler)
    {
        _layoutManager = layoutManager;
        _compiler = compiler;
    }

    public void DisplayGameTitle()
    {
        throw new NotImplementedException();
    }

    public void Render(IGameState state, IReadOnlyList<GameAction> legalActions, string errorMessage)
    {
        ConxelCanvas canvas = _layoutManager.BuildCanvas(state, legalActions, errorMessage);
        string frame = _compiler.Compile(canvas);

        Console.SetCursorPosition(0, 0);
        Console.Write(frame);
    }

    public void RenderFinalState(IGameState gameState, object finalStandingsData)
    {
        throw new NotImplementedException();
    }
}