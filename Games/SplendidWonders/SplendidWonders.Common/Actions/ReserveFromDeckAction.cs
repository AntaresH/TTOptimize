using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Common.Actions;

public record class ReserveFromDeckAction : GameAction
{
    public int Level { get; set; }

    public override string Description 
    {
        get 
        {
            string levelRomanCount = string.Concat(Enumerable.Repeat("I", Level));
            string levelName = $"{GetNameForLevel()} ({levelRomanCount})";

            return string.Format(Strings.ActionDesc_ReserveFromDeckFormat, levelName);
        }
    }

    private string GetNameForLevel()
    {
        return Level switch
        {
            1 => Strings.Level1DeckName,
            2 => Strings.Level2DeckName,
            3 => Strings.Level3DeckName,
            _ => throw new InvalidOperationException()
        };
    }
}