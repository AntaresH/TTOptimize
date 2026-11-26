namespace TTOptimize.Core.Statistics;

public interface IMatchRepository
{
    void SaveMatch(MatchRecord record);
    void SaveMatchesBulk(IEnumerable<MatchRecord> records);
    IReadOnlyList<MatchRecord> GetMatchesByGame(string gameId);
}