using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.Statistics;

public interface IMatchMetricsExtractor
{
    IReadOnlyList<string> KnownActionTypes { get; }

    IPlayerMetrics ExtractPlayerMetrics(IPlayerMetrics baseMetrics, IGameState finalState);

    IGameMetrics ExtractGameMetrics(IGameMetrics baseMetrics, IGameState finalState, IReadOnlyList<ActionLogEntry> history);
}
