namespace TTOptimize.Core.Statistics;

public interface IStatisticsRepository
{
    StatisticsGroup? GetByKey(AggregationKey key);
    void Upsert(StatisticsGroup group);
    IReadOnlyList<StatisticsGroup> GetByGame(string gameId);
}
