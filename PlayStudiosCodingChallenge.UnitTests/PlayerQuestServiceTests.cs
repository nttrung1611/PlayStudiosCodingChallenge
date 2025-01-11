using Microsoft.Extensions.Options;
using Moq;
using PlayStudiosCodingChallenge.Data.Models;
using PlayStudiosCodingChallenge.Data.Repositories;
using PlayStudiosCodingChallenge.Services;
using PlayStudiosCodingChallenge.Services.Models;
using PlayStudiosCodingChallenge.Services.ServiceModels;

namespace PlayStudiosCodingChallenge.UnitTests
{
    public class PlayerQuestServiceTests
    {
        private readonly Mock<IPlayerQuestStateRepository> _repository = new Mock<IPlayerQuestStateRepository>();
        private readonly Mock<IOptions<QuestConfigurationOptions>> _options = new Mock<IOptions<QuestConfigurationOptions>>();
        private readonly QuestConfigurationOptions questConfig = new QuestConfigurationOptions
        {
            TotalQuestPointsForCompletion = 1000,
            QuestMilestones = 10,
            LevelBonusRate = 1,
            RateFromBet = 1,
            ChipsAwardedForMilestoneCompletion = 50
        };

        #region GetQuestState
        [Fact]
        public async Task GetQuestState_ShouldReturnQuestStateResponse_WhenPlayerQuestStateExists()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var questState = new PlayerQuestState 
            { 
                PlayerId = playerId,
                TotalQuestPointsEarned = 0,
                LastMilestoneIndexCompleted = 0
            };

            _repository.Setup(x => x.GetQuestStateByPlayerId(playerId)).ReturnsAsync(questState);
            _options.Setup(x => x.Value).Returns(questConfig);

            var service = new PlayerQuestService(_repository.Object, _options.Object);

            // Act
            var questStateResponse = await service.GetQuestState(playerId);

            // Assert
            Assert.NotNull(questStateResponse);
            Assert.Equal(0D, questStateResponse.TotalQuestPercentCompleted);
            Assert.Equal(0, questStateResponse.LastMilestoneIndexCompleted);
        }

        [Fact]
        public async Task GetQuestState_ShouldCalculateTotalQuestPercentCompleted_WhenTotalQuestPointsEarned_HasValue()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var questState = new PlayerQuestState
            {
                PlayerId = playerId,
                TotalQuestPointsEarned = 666,
                LastMilestoneIndexCompleted = 6
            };

            _repository.Setup(x => x.GetQuestStateByPlayerId(playerId)).ReturnsAsync(questState);
            _options.Setup(x => x.Value).Returns(questConfig);

            var service = new PlayerQuestService(_repository.Object, _options.Object);

            // Act
            var questStateResponse = await service.GetQuestState(playerId);

            // Assert
            Assert.NotNull(questStateResponse);
            Assert.Equal(66.6D, questStateResponse.TotalQuestPercentCompleted);
            Assert.Equal(6, questStateResponse.LastMilestoneIndexCompleted);
        }

        [Fact]
        public async Task GetQuestState_ShouldReturn100_ForTotalQuestPercentCompleted_WhenTotalQuestPointsEarned_HasValueEqualsTotalQuestPointsForCompletion()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var questState = new PlayerQuestState
            {
                PlayerId = playerId,
                TotalQuestPointsEarned = 1000,
                LastMilestoneIndexCompleted = 10
            };

            _repository.Setup(x => x.GetQuestStateByPlayerId(playerId)).ReturnsAsync(questState);
            _options.Setup(x => x.Value).Returns(questConfig);

            var service = new PlayerQuestService(_repository.Object, _options.Object);

            // Act
            var questStateResponse = await service.GetQuestState(playerId);

            // Assert
            Assert.NotNull(questStateResponse);
            Assert.Equal(100D, questStateResponse.TotalQuestPercentCompleted);
            Assert.Equal(10, questStateResponse.LastMilestoneIndexCompleted);
        }

        [Fact]
        public async Task GetQuestState_ShouldReturnNull_WhenPlayerQuestStateDoesNotExist()
        {
            // Arrange
            _repository.Setup(x => x.GetQuestStateByPlayerId(It.IsAny<Guid>())).ReturnsAsync(() => null);
            var service = new PlayerQuestService(_repository.Object, _options.Object);

            // Act
            var questStateResponse = await service.GetQuestState(Guid.NewGuid());

            // Assert
            Assert.Null(questStateResponse);
        }
        #endregion

        #region UpdateQuestProgress
        [Fact]
        public async Task UpdateQuestProgress_ShouldCreatePlayerQuestState_WhenPlayerQuestStateDoesNotExist()
        {
            // Arrange
            var playerId = Guid.NewGuid();

            var request = new QuestProgressionRequest 
            { 
                PlayerId = playerId,
                ChipAmountBet = 50,
                PlayerLevel = 1
            };

            _repository.Setup(x => x.GetQuestStateByPlayerId(It.IsAny<Guid>())).ReturnsAsync(() => null);
            _options.Setup(x => x.Value).Returns(questConfig);

            var service = new PlayerQuestService(_repository.Object, _options.Object);

            // Act
            var questProgressionResponse = await service.UpdateQuestProgress(request);

            // Assert
            _repository.Verify(x => x.CreateQuestState(It.IsAny<PlayerQuestState>()), Times.Once());
        }
     
        [Fact]
        public async Task UpdateQuestProgress_ShouldUpdatePlayerQuestState_WhenPlayerQuestStateExists()
        {
            // Arrange
            var playerId = Guid.NewGuid();

            var request = new QuestProgressionRequest
            {
                PlayerId = playerId,
                ChipAmountBet = 100,
                PlayerLevel = 1
            };

            var questState = new PlayerQuestState
            {
                PlayerId = playerId,
                TotalQuestPointsEarned = 150,
                LastMilestoneIndexCompleted = 1
            };

            _repository.Setup(x => x.GetQuestStateByPlayerId(playerId)).ReturnsAsync(questState);
            _options.Setup(x => x.Value).Returns(questConfig);

            var service = new PlayerQuestService(_repository.Object, _options.Object);

            // Act
            var questProgressionResponse = await service.UpdateQuestProgress(request);

            // Assert
            _repository.Verify(x => x.UpdateQuestState(It.IsAny<PlayerQuestState>()), Times.Once());
        }

        [Fact]
        public async Task UpdateQuestProgress_ShouldReturnQuestProgressionResponse()
        {
            // Arrange
            var playerId = Guid.NewGuid();

            var request = new QuestProgressionRequest
            {
                PlayerId = playerId,
                ChipAmountBet = 50,
                PlayerLevel = 1
            };

            _repository.Setup(x => x.GetQuestStateByPlayerId(It.IsAny<Guid>())).ReturnsAsync(() => null);
            _options.Setup(x => x.Value).Returns(questConfig);

            var service = new PlayerQuestService(_repository.Object, _options.Object);

            // Act
            var questProgressionResponse = await service.UpdateQuestProgress(request);

            // Assert
            Assert.Equal(51, questProgressionResponse.QuestPointsEarned);
            Assert.Equal(5.1, questProgressionResponse.TotalQuestPercentCompleted);
            Assert.Empty(questProgressionResponse.MilestonesCompleted);
        }

        [Fact]
        public async Task UpdateQuestProgress_ShouldReturnMilestonesCompletedList_WhenMilestoneCompleted()
        {
            // Arrange
            var playerId = Guid.NewGuid();

            var request = new QuestProgressionRequest
            {
                PlayerId = playerId,
                ChipAmountBet = 250,
                PlayerLevel = 1
            };

            var questState = new PlayerQuestState
            {
                PlayerId = playerId,
                TotalQuestPointsEarned = 150,
                LastMilestoneIndexCompleted = 1
            };

            _repository.Setup(x => x.GetQuestStateByPlayerId(playerId)).ReturnsAsync(questState);
            _options.Setup(x => x.Value).Returns(questConfig);

            var service = new PlayerQuestService(_repository.Object, _options.Object);

            // Act
            var questProgressionResponse = await service.UpdateQuestProgress(request);

            // Assert
            Assert.Equal(3, questProgressionResponse.MilestonesCompleted.Count());
            Assert.Equal(2, questProgressionResponse.MilestonesCompleted.First().MilestoneIndex);
            Assert.Equal(4, questProgressionResponse.MilestonesCompleted.Last().MilestoneIndex);
        }
        #endregion
    }
}