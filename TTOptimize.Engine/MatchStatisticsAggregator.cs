using System.Collections.Concurrent;
using System.Reflection;
using TTOptimize.Core.Statistics;

namespace TTOptimize.Engine;

public class MatchStatisticsAggregator
{
    private readonly IStatisticsRepository _statisticsRepository;

    // Típusonként egyszer olvassuk ki a [TrackMetric] property-ket (nulla reflexiós overhead batch közben!)
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<(string MetricName, PropertyInfo Property)>> _trackedPropertiesCache = new();

    public MatchStatisticsAggregator(IStatisticsRepository statisticsRepository)
    {
        _statisticsRepository = statisticsRepository;
    }

    public void ProcessMatches(IEnumerable<MatchRecord> records)
    {
        var cache = new Dictionary<string, StatisticsGroup>();

        foreach (MatchRecord record in records)
        {
            int playerCount = record.PlayerMetrics.Count;

            foreach (IPlayerMetrics player in record.PlayerMetrics)
            {
                var allKey = new AggregationKey(
                    record.GameId,
                    playerCount,
                    player.SubjectId,
                    winnersOnly: false);

                AccumulatePlayer(cache, allKey, player);

                if (player.IsWinner)
                {
                    var winnersKey = new AggregationKey(
                        record.GameId,
                        playerCount,
                        player.SubjectId,
                        winnersOnly: true);

                    AccumulatePlayer(cache, winnersKey, player);
                }
            }
        }

        foreach (StatisticsGroup group in cache.Values)
        {
            _statisticsRepository.Upsert(group);
        }
    }

    private void AccumulatePlayer(Dictionary<string, StatisticsGroup> cache, AggregationKey key, IPlayerMetrics player)
    {
        string docId = key.ToStatisticsGroupId();

        if (!cache.TryGetValue(docId, out StatisticsGroup? group))
        {
            group = _statisticsRepository.GetByKey(key) ?? new StatisticsGroup { Key = key };
            cache[docId] = group;
        }

        UpdateGroup(group, player);
    }

    private static void UpdateGroup(StatisticsGroup group, IPlayerMetrics metrics)
    {
        group.SampleCount++;
        group.VictoryPoints.Track(metrics.VictoryPoints);
        group.TurnCount.Track(metrics.TurnCount);

        foreach (var (actionName, ratio) in metrics.ActionRatios)
        {
            if (!group.ActionRatios.TryGetValue(actionName, out RunningMetric? runningRatio))
            {
                runningRatio = new RunningMetric();
                group.ActionRatios[actionName] = runningRatio;
            }
            runningRatio.Track(ratio);
        }

        var trackedProps = _trackedPropertiesCache.GetOrAdd(metrics.GetType(), DiscoverTrackedProperties);

        foreach (var (metricName, property) in trackedProps)
        {
            object? rawValue = property.GetValue(metrics);
            if (rawValue != null)
            {
                double numericValue = Convert.ToDouble(rawValue);

                if (!group.TrackedGameMetrics.TryGetValue(metricName, out RunningMetric? customRunning))
                {
                    customRunning = new RunningMetric();
                    group.TrackedGameMetrics[metricName] = customRunning;
                }
                customRunning.Track(numericValue);
            }
        }
    }

    private static IReadOnlyList<(string MetricName, PropertyInfo Property)> DiscoverTrackedProperties(Type type)
    {
        return type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(prop => (Property: prop, Attr: prop.GetCustomAttribute<TrackMetricAttribute>()))
            .Where(x => x.Attr != null)
            .Select(x => (x.Attr!.DisplayName ?? x.Property.Name, x.Property))
            .ToList();
    }
}
