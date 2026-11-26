namespace TTOptimize.Core.GameAbstractions;

public interface IParticipant
{
    int Seat { get; } // 1..N
    string Name { get; }
}
