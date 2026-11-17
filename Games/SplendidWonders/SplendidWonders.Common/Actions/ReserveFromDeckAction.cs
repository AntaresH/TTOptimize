using SplendidWonders.Common;
using System.Runtime.CompilerServices;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Common.Actions;

public class ReserveFromDeckAction : GameAction
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

    public override bool Equals(object? obj)
    {
        if (obj is ReserveFromDeckAction other)
        {
            return Level == other.Level;
        }

        return base.Equals(obj);
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