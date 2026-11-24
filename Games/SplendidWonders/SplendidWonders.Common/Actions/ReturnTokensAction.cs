namespace SplendidWonders.Common.Actions;

public record class ReturnTokensAction : TokensAction
{
    public override string Description
    {
        get
        {
            var tokenKeyList = GetTokenKeys();

            return string.Format(Strings.ActionDesc_ReturnTokensFormat, tokenKeyList.Count, string.Join(", ", tokenKeyList));
        }
    }
}