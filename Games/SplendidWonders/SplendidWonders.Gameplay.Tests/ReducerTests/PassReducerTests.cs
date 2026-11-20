using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay.Reducers;
using SplendidWonders.Gameplay.Tests.TestDataModel;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Gameplay.Tests.ReducerTests
{
    public class PassReducerTests
    {
        private GameStateBuilder _defaultBuilder;
        private IStateReducer<PassAction> _sut;

        [SetUp]
        public void Setup()
        {
            _defaultBuilder = GameStateBuilder.Default;

            _sut = new PassActionReducer();
        }

        [TestCase(1, 2)]
        [TestCase(2, 1)]
        public void Reduce_TwoPlayersFirstPass_TurnAdvanced(int currentPlayerId, int nextPlayerId)
        {
            // Arrange
            var baseState = _defaultBuilder
                .WithCurrentPlayer(currentPlayerId)
                .Build();
            var passAction = new PassAction();

            // Act
            var newState = _sut.Reduce(baseState, passAction);

            // Assert
            Assert.That(newState.ActivePlayer.Id, Is.EqualTo(nextPlayerId));
            Assert.That(newState.ConsecutivePasses, Is.EqualTo(1));
            Assert.That(newState.GameProgression, Is.EqualTo(GameProgression.InProgress));
        }

        [TestCase(1, 2)]
        [TestCase(2, 1)]
        public void Reduce_TwoPlayersSecondPass_StalemateReached(int currentPlayerId, int nextPlayerId)
        {
            // Arrange
            var baseState = _defaultBuilder
                .WithCurrentPlayer(currentPlayerId)
                .WithConsecutivePasses(1)
                .Build();
            var passAction = new PassAction();

            // Act
            var newState = _sut.Reduce(baseState, passAction);

            // Assert
            Assert.That(newState.GameProgression, Is.EqualTo(GameProgression.GameOver));
            Assert.That(newState.GameOverReason, Is.EqualTo(GameOverReason.Stalemate));
        }

        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 1)]
        public void Reduce_ThreePlayersSecondPass_TurnAdvanced(int currentPlayerId, int nextPlayerId)
        {
            // Arrange
            var threePlayers = new List<Player>() {
                new Player() { Id = 1 },
                new Player() { Id = 2 },
                new Player() { Id = 3 }
            };

            var baseState = _defaultBuilder
                .WithPlayers(threePlayers)
                .WithConsecutivePasses(1)
                .WithCurrentPlayer(currentPlayerId)
                .Build();

            var passAction = new PassAction();

            // Act
            var newState = _sut.Reduce(baseState, passAction);

            // Assert
            Assert.That(newState.ActivePlayer.Id, Is.EqualTo(nextPlayerId));
            Assert.That(newState.ConsecutivePasses, Is.EqualTo(2));
            Assert.That(newState.GameProgression, Is.EqualTo(GameProgression.InProgress));
        }

        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 1)]
        public void Reduce_ThreePlayersThirdPass_StalemateReached(int currentPlayerId, int nextPlayerId)
        {
            // Arrange
            var threePlayers = new List<Player>() {
                new Player() { Id = 1 },
                new Player() { Id = 2 },
                new Player() { Id = 3 }
            };

            var baseState = _defaultBuilder
                .WithPlayers(threePlayers)
                .WithCurrentPlayer(currentPlayerId)
                .WithConsecutivePasses(2)
                .Build();
            var passAction = new PassAction();

            // Act
            var newState = _sut.Reduce(baseState, passAction);

            // Assert
            Assert.That(newState.GameProgression, Is.EqualTo(GameProgression.GameOver));
            Assert.That(newState.GameOverReason, Is.EqualTo(GameOverReason.Stalemate));
        }

        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        [TestCase(4, 1)]
        public void Reduce_FourPlayersThirdPass_TurnAdvanced(int currentPlayerId, int nextPlayerId)
        {
            // Arrange
            var fourPlayers = new List<Player>() { 
                new Player() { Id = 1 }, 
                new Player() { Id = 2 }, 
                new Player() { Id = 3 }, 
                new Player() { Id = 4 } 
            };

            var baseState = _defaultBuilder
                .WithPlayers(fourPlayers)
                .WithCurrentPlayer(currentPlayerId)
                .WithConsecutivePasses(2)
                .Build();
            var passAction = new PassAction();

            // Act
            var newState = _sut.Reduce(baseState, passAction);

            // Assert
            Assert.That(newState.ActivePlayer.Id, Is.EqualTo(nextPlayerId));
            Assert.That(newState.ConsecutivePasses, Is.EqualTo(3));
            Assert.That(newState.GameProgression, Is.EqualTo(GameProgression.InProgress));
        }

        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        [TestCase(4, 1)]
        public void Reduce_FourPlayersFourthPass_StalemateReached(int currentPlayerId, int nextPlayerId)
        {
            // Arrange
            var fourPlayers = new List<Player>() { 
                new Player() { Id = 1 }, 
                new Player() { Id = 2 }, 
                new Player() { Id = 3 }, 
                new Player() { Id = 4 }
            };

        var baseState = _defaultBuilder
                .WithPlayers(fourPlayers)
                .WithCurrentPlayer(currentPlayerId)
                .WithConsecutivePasses(3)
                .Build();
            var passAction = new PassAction();

            // Act
            var newState = _sut.Reduce(baseState, passAction);

            // Assert
            Assert.That(newState.GameProgression, Is.EqualTo(GameProgression.GameOver));
            Assert.That(newState.GameOverReason, Is.EqualTo(GameOverReason.Stalemate));
        }
    }
}
