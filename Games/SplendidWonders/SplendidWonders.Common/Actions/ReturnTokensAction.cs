namespace SplendidWonders.Common.Actions;

public class ReturnTokensAction : TokensAction
{
    public override string Description
    {
        get
        {
            var tokenKeyList = GetTokenKeys();

            return string.Format(Strings.ActionDesc_ReturnTokensFormat, tokenKeyList.Count, string.Join(", ", tokenKeyList));
        }
    }

    override public bool Equals(object? obj)
    {
        if (obj is ReturnTokensAction other)
        {
            return Tokens == other.Tokens;
        }

        return base.Equals(obj);
    }
}