using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Gameplay.Reducers
{
    public class PassActionReducer : ReducerBase, IStateReducer<PassAction>
    {
        public GameState Reduce(GameState state, PassAction action)
        {
            return state.Transform(AdvanceTurn, action);
        }
    }
}
