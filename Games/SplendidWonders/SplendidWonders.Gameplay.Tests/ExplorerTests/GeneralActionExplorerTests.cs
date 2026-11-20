using NSubstitute;
using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Tests.TestDataModel;
using SplendidWonders.Gameplay;
using SplendidWonders.Gameplay.Explorers;
using SplendidWonders.Gameplay.Tests.ExplorerTests;

namespace SplendidWonders.Common.Tests.ExplorerTests
{
    public class GeneralActionExplorerTests : ActionExplorerTestsBase
    {
        private ActionExplorer _sut;
        private IActionExplorer<DrawDoubleTokensAction> _drawDoubleTokensExplorer;
        private IActionExplorer<DrawThreeTokensAction> _drawThreeTokensExplorer;
        private IActionExplorer<ReturnTokensAction> _returnTokensExplorer;
        private IActionExplorer<ReserveFromBoardAction> _reserveFromBoardExplorer;
        private IActionExplorer<ReserveFromDeckAction> _reserveFromDeckExplorer;
        private IActionExplorer<PurchaseAction> _purchaseExplorer;
        private IActionExplorer<SelectLeaderAction> _selectLeaderExplorer;

        [SetUp]
        public void Setup()
        {
            SetupDefaultGameState();

            _drawDoubleTokensExplorer = Substitute.For<IActionExplorer<DrawDoubleTokensAction>>();
            _drawThreeTokensExplorer = Substitute.For<IActionExplorer<DrawThreeTokensAction>>();
            _returnTokensExplorer = Substitute.For<IActionExplorer<ReturnTokensAction>>();
            _reserveFromBoardExplorer = Substitute.For<IActionExplorer<ReserveFromBoardAction>>();
            _reserveFromDeckExplorer = Substitute.For<IActionExplorer<ReserveFromDeckAction>>();
            _purchaseExplorer = Substitute.For<IActionExplorer<PurchaseAction>>();
            _selectLeaderExplorer = Substitute.For<IActionExplorer<SelectLeaderAction>>();

            _sut = new ActionExplorer(_drawThreeTokensExplorer,
                _drawDoubleTokensExplorer,
                _returnTokensExplorer,
                _reserveFromBoardExplorer,
                _reserveFromDeckExplorer,
                _purchaseExplorer,
                _selectLeaderExplorer);
        }

        [Test]
        public void Explore_AlwaysCallSelectLeaderAndReturnTokensExplorers()
        {
            // ARRANGE
            var gameState = _defaultBuilder.Build();

            // ACT
            _sut.Explore(gameState);

            // ASSERT
            _returnTokensExplorer.Received(1).Explore(gameState);
            _selectLeaderExplorer.Received(1).Explore(gameState);
        }

        [TestCase(4)]
        [TestCase(6)]
        public void Explore_ReturnTokenActionsExplored_ReturnReturnTokenActions(int actionsToGenerate)
        {
            // ARRANGE
            var gameState = _defaultBuilder.Build();
            var dummyActions = Enumerable.Repeat(new ReturnTokensAction(), actionsToGenerate).ToArray();
            _returnTokensExplorer.Explore(gameState).Returns(dummyActions);

            // ACT
            var result = _sut.Explore(gameState);

            // ASSERT
            Assert.That(result, Is.All.InstanceOf<ReturnTokensAction>());
            Assert.That(result, Has.Count.EqualTo(actionsToGenerate));
        }

        [TestCase(3)]
        [TestCase(5)]
        public void Explore_ReturnTokenActionsExplored_NeverCallNonRequiredExplorers(int actionsToGenerate)
        {
            // ARRANGE
            var gameState = _defaultBuilder.Build();
            var dummyActions = Enumerable.Repeat(new ReturnTokensAction(), actionsToGenerate).ToArray();
            _returnTokensExplorer.Explore(gameState).Returns(dummyActions);

            // ACT
            var result = _sut.Explore(gameState);

            // ASSERT
            _drawThreeTokensExplorer.DidNotReceive().Explore(gameState);
            _drawDoubleTokensExplorer.DidNotReceive().Explore(gameState);
            _purchaseExplorer.DidNotReceive().Explore(gameState);
            _reserveFromBoardExplorer.DidNotReceive().Explore(gameState);
            _reserveFromDeckExplorer.DidNotReceive().Explore(gameState);
        }

        [TestCase(2)]
        [TestCase(3)]
        public void Explore_SelectLeaderActionsExplored_ReturnOnlySelectLeaderActions(int actionsToGenerate)
        {
            // ARRANGE
            var gameState = _defaultBuilder.Build();
            var dummyActions = Enumerable.Repeat(new SelectLeaderAction(), actionsToGenerate).ToArray();
            _selectLeaderExplorer.Explore(gameState).Returns(dummyActions);

            // ACT
            var result = _sut.Explore(gameState);

            // ASSERT
            Assert.That(result, Is.All.InstanceOf<SelectLeaderAction>());
            Assert.That(result, Has.Count.EqualTo(actionsToGenerate));
        }

        [TestCase(2)]
        [TestCase(3)]
        public void Explore_SelectLeaderActionsExplored_NeverCallNonRequiredExplorers(int actionsToGenerate)
        {
            // ARRANGE
            var gameState = _defaultBuilder.Build();
            var dummyActions = Enumerable.Repeat(new SelectLeaderAction(), actionsToGenerate).ToArray();
            _selectLeaderExplorer.Explore(gameState).Returns(dummyActions);

            // ACT
            var result = _sut.Explore(gameState);

            // ASSERT
            _drawThreeTokensExplorer.DidNotReceive().Explore(gameState);
            _drawDoubleTokensExplorer.DidNotReceive().Explore(gameState);
            _purchaseExplorer.DidNotReceive().Explore(gameState);
            _reserveFromBoardExplorer.DidNotReceive().Explore(gameState);
            _reserveFromDeckExplorer.DidNotReceive().Explore(gameState);
        }

        [Test]
        public void Explore_ZeroActionsExplored_ReturnPassAction()
        {
            // ARRANGE
            var gameState = _defaultBuilder.Build();
            _returnTokensExplorer.Explore(gameState).Returns([]);
            _selectLeaderExplorer.Explore(gameState).Returns([]);
            _drawThreeTokensExplorer.Explore(gameState).Returns([]);
            _drawDoubleTokensExplorer.Explore(gameState).Returns([]);
            _reserveFromBoardExplorer.Explore(gameState).Returns([]);
            _reserveFromDeckExplorer.Explore(gameState).Returns([]);
            _purchaseExplorer.Explore(gameState).Returns([]);

            // ACT
            var result = _sut.Explore(gameState);

            // ASSERT
            Assert.That(result, Is.All.InstanceOf<PassAction>());
            Assert.That(result, Has.Count.EqualTo(1));
        }

        [TestCase(6, 2, 12, 3, 2, 1)]
        [TestCase(7, 1, 0, 0, 0, 2)]
        [TestCase(3, 0, 12, 3, 5, 0)]
        [TestCase(10, 5, 12, 3, 0, 0)]
        public void Explore_HasActionExplored_ReturnSumOfActions(int drawThreeCount, int drawDoubleCount, int reserveFromBoardCount, int reserveFromDeckCount, int purchaseFromBoardCount, int purchaseFromReservedCount)
        {
            // ARRANGE
            var gameState = _defaultBuilder.Build();
            var dummyActions1 = Enumerable.Repeat(new DrawDoubleTokensAction(), drawDoubleCount).ToArray();
            _drawDoubleTokensExplorer.Explore(gameState).Returns(dummyActions1);
            var dummyActions2 = Enumerable.Repeat(new DrawThreeTokensAction(), drawThreeCount).ToArray();
            _drawThreeTokensExplorer.Explore(gameState).Returns(dummyActions2);
            var dummyActions3 = Enumerable.Repeat(new ReserveFromBoardAction(), reserveFromBoardCount).ToArray();
            _reserveFromBoardExplorer.Explore(gameState).Returns(dummyActions3);
            var dummyActions4 = Enumerable.Repeat(new ReserveFromDeckAction(), reserveFromDeckCount).ToArray();
            _reserveFromDeckExplorer.Explore(gameState).Returns(dummyActions4);
            var dummyActions5 = Enumerable.Repeat(new PurchaseFromBoardAction(), purchaseFromBoardCount).Cast<PurchaseAction>();
            var dummyActions6 = dummyActions5.Concat(Enumerable.Repeat(new PurchaseFromReservedAction(), purchaseFromReservedCount).Cast<PurchaseAction>()).ToArray();
            _purchaseExplorer.Explore(gameState).Returns(dummyActions6);

            // ACT
            var result = _sut.Explore(gameState);

            // ASSERT
            Assert.That(result, Has.None.InstanceOf<PassAction>());
            Assert.That(result, Has.Exactly(drawDoubleCount).InstanceOf<DrawDoubleTokensAction>());
            Assert.That(result, Has.Exactly(drawThreeCount).InstanceOf<DrawThreeTokensAction>());
            Assert.That(result, Has.Exactly(reserveFromBoardCount).InstanceOf<ReserveFromBoardAction>());
            Assert.That(result, Has.Exactly(reserveFromDeckCount).InstanceOf<ReserveFromDeckAction>());
            Assert.That(result, Has.Exactly(purchaseFromBoardCount + purchaseFromReservedCount).InstanceOf<PurchaseAction>());
        }
    }
}
