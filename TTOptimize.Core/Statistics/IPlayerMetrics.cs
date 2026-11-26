namespace TTOptimize.Core.Statistics;

public interface IPlayerMetrics
{
    int Seat { get; }
    string StatisticsSubjectId { get; }
    int Rank { get; }
    bool IsWinner { get; }
    int VictoryPoints { get; }
    int TurnCount { get; }
    IReadOnlyDictionary<string, int> ActionCounts { get; }
    IReadOnlyDictionary<string, double> ActionRatios { get; }
}
