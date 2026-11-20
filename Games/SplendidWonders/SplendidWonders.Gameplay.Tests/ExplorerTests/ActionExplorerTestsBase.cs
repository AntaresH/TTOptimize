using SplendidWonders.Gameplay.Tests.TestDataModel;

namespace SplendidWonders.Gameplay.Tests.ExplorerTests;

public class ActionExplorerTestsBase
{
    protected GameStateBuilder _defaultBuilder;

    protected void SetupDefaultGameState()
    {
        _defaultBuilder = GameStateBuilder.Default;
    }
}
