namespace TTOptimize.Core.Statistics;

public record class BasePlayerMetrics : IPlayerMetrics
{
    public int Seat { get; init; }
    public string StatisticsSubjectId { get; init; } = string.Empty;
    public int Rank { get; init; }
    public bool IsWinner { get; init; }
    public int VictoryPoints { get; init; }
    public int TurnCount { get; init; }
    public Dictionary<string, int> ActionCounts { get; init; } = new();
    public Dictionary<string, double> ActionRatios { get; init; } = new();

    IReadOnlyDictionary<string, int> IPlayerMetrics.ActionCounts => ActionCounts;
    IReadOnlyDictionary<string, double> IPlayerMetrics.ActionRatios => ActionRatios;

    public BasePlayerMetrics() { }

    public BasePlayerMetrics(IPlayerMetrics source)
    {
        Seat = source.Seat;
        StatisticsSubjectId = source.StatisticsSubjectId;
        Rank = source.Rank;
        IsWinner = source.IsWinner;
        VictoryPoints = source.VictoryPoints;
        TurnCount = source.TurnCount;
        ActionCounts = new Dictionary<string, int>(source.ActionCounts);
        ActionRatios = new Dictionary<string, double>(source.ActionRatios);
    }
}
