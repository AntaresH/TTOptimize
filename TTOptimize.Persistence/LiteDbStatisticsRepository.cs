using LiteDB;
using TTOptimize.Core.Statistics;

namespace TTOptimize.Persistence;

public class LiteDbStatisticsRepository : IStatisticsRepository
{
    private const string STATS_COLLECTION = "statistics_groups";
    private readonly ILiteCollection<StatisticsGroup> _groups;

    public LiteDbStatisticsRepository(LiteDatabase database)
    {
        _groups = database.GetCollection<StatisticsGroup>(STATS_COLLECTION);
        _groups.EnsureIndex(x => x.Key.GameId);
    }

    public StatisticsGroup? GetByKey(AggregationKey key)
    {
        return _groups.FindById(key.ToStatisticsGroupId());
    }

    public void Upsert(StatisticsGroup group)
    {
        _groups.Upsert(group);
    }

    public IReadOnlyList<StatisticsGroup> GetByGame(string gameId)
    {
        return _groups.Find(x => x.Key.GameId == gameId).ToList();
    }
}
