using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Model;
using TTOptimize.ConsoleHost.ConsoleView;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.ViewAbstractions;

public class ConxelGameView : IGameRenderer
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

    public void Render(IGameState state, IReadOnlyList<GameAction> legalActions)
    {
        ConxelCanvas canvas = _layoutManager.BuildCanvas(state, legalActions);
        string frame = _compiler.Compile(canvas);

        Console.Clear();
        Console.Write(frame);
    }
}