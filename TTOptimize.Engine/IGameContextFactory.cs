namespace TTOptimize.Engine;

public interface IGameContextFactory
{
    GameContext CreateContext(string gameId);
}