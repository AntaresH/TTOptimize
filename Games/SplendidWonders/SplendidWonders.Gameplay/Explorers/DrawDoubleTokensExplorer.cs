using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay.Actions;

namespace SplendidWonders.Gameplay.Explorers
{
    public class DrawDoubleTokensExplorer : IActionExplorer<DrawDoubleTokensAction>
    {
        public IEnumerable<DrawDoubleTokensAction> Explore(GameState gameState)
        {
            List<DrawDoubleTokensAction> gameActions = new List<DrawDoubleTokensAction>();

            if (gameState.TokenSupply.Yellow >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 2, 0, 0, 0, 0, 0 }) });
            if (gameState.TokenSupply.Blue >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 2, 0, 0, 0, 0 }) });
            if (gameState.TokenSupply.Green >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 0, 2, 0, 0, 0 }) });
            if (gameState.TokenSupply.Brown >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 0, 0, 2, 0, 0 }) });
            if (gameState.TokenSupply.Red >= 4)
                gameActions.Add(new DrawDoubleTokensAction() { Tokens = TokenGroup.FromArray(new int[] { 0, 0, 0, 0, 2, 0 }) });

            return gameActions;
        }
    }
}
