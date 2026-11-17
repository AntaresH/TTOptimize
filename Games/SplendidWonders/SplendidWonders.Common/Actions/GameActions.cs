
using SplendidWonders.Common;
using SplendidWonders.Common.Model;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Common.Actions;

public abstract class TokensAction : GameAction
{
    public TokenGroup Tokens { get; set; }

    protected List<string> GetTokenKeys()
    {
        List<string> tokenKeys = new List<string>();

        foreach (var item in Tokens.AsEnumerable().Where(t => t.Value > 0))
        {
            tokenKeys.Add(GetKeyForDevelopmentColor(item.Key));
        }

        return tokenKeys;
    }

    protected string GetKeyForDevelopmentColor(DevelopmentColor devColor)
    {
        return devColor switch
        {
            DevelopmentColor.Yellow => Strings.TokenKey_Yellow,
            DevelopmentColor.Blue => Strings.TokenKey_Blue,
            DevelopmentColor.Green => Strings.TokenKey_Green,
            DevelopmentColor.Brown => Strings.TokenKey_Brown,
            DevelopmentColor.Red => Strings.TokenKey_Red,
            _ => string.Empty

        };
    }
}

public abstract class CardAction : GameAction
{
    public DevelopmentCard Card { get; set; }
}