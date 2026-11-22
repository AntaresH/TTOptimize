using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.ConsoleUI.View;

public interface IConxelWidget
{
    public int Height { get; }
    public int Width { get; }
    ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions);
}
