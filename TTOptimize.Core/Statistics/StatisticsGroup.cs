namespace TTOptimize.Core.Statistics;

public record class AggregationKey
{
    public string GameId { get; init; } = string.Empty;
    public string SubjectIdentifier { get; init; } = string.Empty; // Pl. "RandomAgent", "Heuristic_Gen50", vagy "Máté"
    public int PlayerCount { get; init; }

    public string ToStorageId()
    {
        return $"{GameId}:{SubjectIdentifier}:{PlayerCount}P";
    }
}