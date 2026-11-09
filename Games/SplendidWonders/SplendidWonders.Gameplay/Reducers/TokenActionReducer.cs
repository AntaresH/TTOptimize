using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay.Actions;

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