namespace TTOptimize.Core.GameAbstractions
{
    public interface IGameConfiguration
    {
        int PlayerCount { get; set; }
        List<string> PlayerNames { get; set; }
    }
}