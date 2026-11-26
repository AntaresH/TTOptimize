using LiteDB;
using TTOptimize.Core.Statistics;

namespace TTOptimize.Persistence;

public class LiteDbMatchRepository : IMatchRepository, IDisposable
{
    private const string MATCHES_COLLECTION = "matches";
    private readonly LiteDatabase _database;
    private readonly ILiteCollection<MatchRecord> _matches;

    public LiteDbMatchRepository(string databaseFilePath)
    {
        _database = new LiteDatabase(databaseFilePath);
        _matches = _database.GetCollection<MatchRecord>(MATCHES_COLLECTION);

        _matches.EnsureIndex(x => x.GameId);
        _matches.EnsureIndex(x => x.BatchRunId);
    }

    public void SaveMatch(MatchRecord record)
    {
        _matches.Insert(record);
    }

    public void SaveMatchesBulk(IEnumerable<MatchRecord> records)
    {
        _matches.InsertBulk(records);
    }

    public IReadOnlyList<MatchRecord> GetMatchesByGame(string gameId)
    {
        return _matches.Find(x => x.GameId == gameId).ToList();
    }

    public void Dispose()
    {
        _database.Dispose();
    }
}
