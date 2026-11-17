using SplendidWonders.Common;
using SplendidWonders.Common.Model;
using System.Runtime.Versioning;

namespace SplendidWonders.Common.Actions;

public class DrawThreeTokensAction : TokensAction
{
    public override string Description
    {
        get
        {
            var tokenKeyList = GetTokenKeys();

            return string.Format(Strings.ActionDesc_ThreeTokensFormat, tokenKeyList.Count, string.Join(", ", tokenKeyList));
        }
    }

    override public bool Equals(object? obj)
    {
        if (obj is DrawThreeTokensAction other)
        {
            return Tokens == other.Tokens;
        }

        return base.Equals(obj);
    }
}