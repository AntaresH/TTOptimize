namespace TTOptimize.Engine;

public sealed record class BatchExecutionResult
{
    public Guid BatchRunId { get; }
    public int MatchCount { get; }
    public TimeSpan TotalDuration { get; }

    public double AverageMillisecondsPerMatch
    {
        get { return MatchCount > 0 ? TotalDuration.TotalMilliseconds / MatchCount : 0.0; }
    }

    public BatchExecutionResult(Guid batchRunId, int matchCount, TimeSpan totalDuration)
    {
        BatchRunId = batchRunId;
        MatchCount = matchCount;
        TotalDuration = totalDuration;
    }
}
