namespace SplendidWonders.Common.Actions;

public class DrawDoubleTokensAction : TokensAction
{
    public override string Description 
    {
        get
        {
            var tokenKeyList = GetTokenKeys();

            return string.Format(Strings.ActionDesc_DoubleTokensFormat, string.Join(", ", tokenKeyList));
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is DrawDoubleTokensAction other)
        {
            return Tokens.Equals(other.Tokens);
        }

        return base.Equals(obj);
    }
}