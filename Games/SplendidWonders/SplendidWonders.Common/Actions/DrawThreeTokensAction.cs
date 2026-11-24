namespace SplendidWonders.Common.Actions;

public record class DrawThreeTokensAction : TokensAction
{
    public override string Description
    {
        get
        {
            var tokenKeyList = GetTokenKeys();

            return string.Format(Strings.ActionDesc_ThreeTokensFormat, tokenKeyList.Count, string.Join(", ", tokenKeyList));
        }
    }
}