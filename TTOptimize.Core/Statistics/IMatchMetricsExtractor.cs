using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.Statistics;

public interface IMatchMetricsExtractor
{
    // Játékosonkénti extra számok (pl. TableauSize, AcquiredLeaders)
    IPlayerMetrics ExtractPlayerMetrics(IPlayerMetrics baseMetrics, IGameState finalState);

    // Ágens-független komponens adatok (későbbi optimalizáláshoz: mely kártyák/vezérek fogytak el)
    IGameMetrics ExtractGameMetrics(IGameMetrics baseMetrics, IGameState finalState, IReadOnlyList<ActionLogEntry> history);
}
