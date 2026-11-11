namespace TTOptimize.Core.GameAbstractions
{
    public interface IGameConfiguration
    {
        IList<IPlayerAgent> PlayerAgents { get; set; }
    }
}