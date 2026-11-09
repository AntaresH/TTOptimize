using SplendidWonders.Common.Tests.TestDataModel;
using SplendidWonders.Gameplay;
using SplendidWonders.Gameplay.Explorers;

namespace SplendidWonders.Common.Tests.ExplorerTests
{
    public partial class ActionExplorerTests
    {
        private GameStateBuilder _defaultBuilder;
        private ActionExplorer _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            var drawThreeTokensExplorer = new DrawThreeTokensExplorer();
            var drawDoubleTokensExplorer = new DrawDoubleTokensExplorer();
            var returnTokensExplorer = new ReturnTokensExplorer();
            var reserveFromBoardExplorer = new ReserveFromBoardExplorer();
            var reserveFromDeckExplorer = new ReserveFromDeckExplorer();
            var purchaseExplorer = new PurchaseExplorer();
            var selectLeaderExplorer = new SelectLeaderExplorer();

            _sut = new ActionExplorer(drawThreeTokensExplorer,
                drawDoubleTokensExplorer,
                returnTokensExplorer,
                reserveFromBoardExplorer,
                reserveFromDeckExplorer,
                purchaseExplorer,
                selectLeaderExplorer);
        }
    }
}
