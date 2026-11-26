using TTOptimize.Core.Statistics;

namespace SplendidWonders.Common.Statistics;

public sealed record class SplendidWondersGameMetrics : BaseGameMetrics
{
    // 1. Melyik kártyák fogytak el, és melyek maradtak a piacon?
    // (Ha egy kártya 10.000 meccsből 9.500-szor a piacon ragad, túl drága/gyenge;
    // ha azonnal megveszik amint feljön, túl erős/olcsó.)
    public List<int> PurchasedCardIds { get; init; } = new();
    public List<int> ReservedCardIds { get; init; } = new();
    public List<int> UnboughtMarketCardIds { get; init; } = new();

    // 2. Melyik kártyák voltak a GYŐZTES játékos tablóján?
    // (Ez mutatja meg az igazi "overpowered" kártyákat: magas win-rate korreláció.)
    public List<int> WinnerTableauCardIds { get; init; } = new();

    // 3. Vezérek (Leaders) statisztikái
    public List<int> AvailableLeaderIdsAtStart { get; init; } = new();
    public List<int> ClaimedLeaderIds { get; init; } = new();
    public List<int> WinnerLeaderIds { get; init; } = new();

    // 4. Hányadik körben érték el a győzelmi pontlimitet?
    public int FinalTurnNumber { get; init; }
}