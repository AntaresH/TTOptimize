namespace TTOptimize.Core
{
    public interface IGameConfiguration
    {
        IList<IPlayerAgent> PlayerAgents { get; set; }
    }
}