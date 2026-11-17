using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;

namespace SplendidWonders.Gameplay.Reducers
{
    public class TokenActionReducer : ReducerBase, IStateReducer<DrawDoubleTokensAction>, IStateReducer<DrawThreeTokensAction>, IStateReducer<ReturnTokensAction>
    {
        public GameState Reduce(GameState baseState, DrawThreeTokensAction action)
        {
            return baseState
                .Transform(DrawTokens, action.Tokens.ToArray())
                .Transform(AdvanceTurn);
        }

        public GameState Reduce(GameState baseState, DrawDoubleTokensAction action)
        {
            return baseState
                .Transform(DrawTokens, action.Tokens.ToArray())
                .Transform(AdvanceTurn);
        }

        public GameState Reduce(GameState baseState, ReturnTokensAction action)
        {
            return baseState
                .Transform(ReturnTokens, action.Tokens.ToArray())
                .Transform(AdvanceTurn);

        }
    }
}