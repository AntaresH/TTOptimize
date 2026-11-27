namespace TTOptimize.Core.GameAbstractions;

public interface IPlayer
{
    int Seat { get; } // 1..N
    string Name { get; }
}
