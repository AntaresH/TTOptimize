namespace TTOptimize.Engine;

public record class SessionResult
{
    public MatchOutcome Outcome { get; }
    public MatchExecutionInfo ExecutionInfo { get; }

    public SessionResult(MatchOutcome outcome, MatchExecutionInfo executionInfo)
    {
        Outcome = outcome;
        ExecutionInfo = executionInfo;
    }
}
