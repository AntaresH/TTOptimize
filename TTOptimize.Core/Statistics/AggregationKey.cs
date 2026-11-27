namespace TTOptimize.Core.Statistics;

public record class AggregationKey
{
    public string GameId { get; init; } = string.Empty;
    public string SubjectId { get; init; } = string.Empty; // Pl. "RandomAgent", "Heuristic_Gen50", vagy "Máté"
    public int PlayerCount { get; init; }
    public bool WinnersOnly { get; init; }

    public string ToStatisticsGroupId()
    {
        return $"{GameId}|{PlayerCount}|{SubjectId}|{(WinnersOnly ? "Winners" : "All")}";
    }

    public AggregationKey() { }

    public AggregationKey(string gameId, int playerCount, string subjectId, bool winnersOnly)
    {
        GameId = gameId;
        PlayerCount = playerCount;
        SubjectId = subjectId;
        WinnersOnly = winnersOnly;
    }
}