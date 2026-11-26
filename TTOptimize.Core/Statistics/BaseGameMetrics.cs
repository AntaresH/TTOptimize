namespace TTOptimize.Core.Statistics;

public record class BaseGameMetrics : IGameMetrics
{
    public int TotalTurns { get; init; }
}
