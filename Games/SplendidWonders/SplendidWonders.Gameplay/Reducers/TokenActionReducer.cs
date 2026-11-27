using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;

namespace SplendidWonders.Gameplay.Reducers
{
    public class TokenActionReducer : ReducerBase, IStateReducer<DrawDoubleTokensAction>, IStateReducer<DrawThreeTokensAction>, IStateReducer<ReturnTokensAction>
    {
        public GameState Reduce(GameState sourceState, DrawThreeTokensAction action)
        {
            return sourceState
                .Transform(DrawTokens, action.Tokens.ToArray())
                .Transform(AdvanceTurn, action);
        }

        public GameState Reduce(GameState sourceState, DrawDoubleTokensAction action)
        {
            return sourceState
                .Transform(DrawTokens, action.Tokens.ToArray())
                .Transform(AdvanceTurn, action);
        }

        public GameState Reduce(GameState sourceState, ReturnTokensAction action)
        {
            return sourceState
                .Transform(ReturnTokens, action.Tokens.ToArray())
                .Transform(AdvanceTurn, action);

        }
    }
}