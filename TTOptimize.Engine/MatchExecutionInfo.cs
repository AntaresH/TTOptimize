namespace TTOptimize.Engine;

public record class MatchExecutionInfo
{
    public DateTime StartedAtUtc { get; }
    public DateTime FinishedAtUtc { get; }
    public TimeSpan Elapsed { get; }

    public MatchExecutionInfo(DateTime startedAtUtc, DateTime executedAtUtc, TimeSpan elapsed)
    {
        StartedAtUtc = startedAtUtc;
        FinishedAtUtc = executedAtUtc;
        Elapsed = elapsed;
    }
}
