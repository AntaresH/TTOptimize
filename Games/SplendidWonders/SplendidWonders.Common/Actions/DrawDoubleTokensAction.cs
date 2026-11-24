namespace SplendidWonders.Common.Actions;

public record class DrawDoubleTokensAction : TokensAction
{
    public override string Description 
    {
        get
        {
            var tokenKeyList = GetTokenKeys();

            return string.Format(Strings.ActionDesc_DoubleTokensFormat, string.Join(", ", tokenKeyList));
        }
    }
}