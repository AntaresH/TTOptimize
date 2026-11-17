namespace TTOptimize.Core.GameAbstractions;

public interface IGameStateFactory
{
    IGameState Create(IGameConfiguration gameConfiguration);
}
