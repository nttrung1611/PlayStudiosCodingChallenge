using Microsoft.AspNetCore.Mvc;
using Moq;
using PlayStudiosCodingChallenge.Data.Models;
using PlayStudiosCodingChallenge.Server.Controllers;
using PlayStudiosCodingChallenge.Services;
using PlayStudiosCodingChallenge.Services.Models;
using PlayStudiosCodingChallenge.Services.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosCodingChallenge.UnitTests
{
    public class QuestControllerTests
    {
        private readonly Mock<IPlayerQuestService> _service = new Mock<IPlayerQuestService>();

        [Fact]
        public async Task State_ShouldReturnOk_WhenPlayerQuestStateExists()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var questState = new PlayerQuestState
            {
                PlayerId = playerId,
                TotalQuestPointsEarned = 0,
                LastMilestoneIndexCompleted = 0
            };

            var response = new QuestStateResponse 
            { 
                TotalQuestPercentCompleted = 0,
                LastMilestoneIndexCompleted = 0 
            };

            _service.Setup(x => x.GetQuestState(playerId)).ReturnsAsync(response);

            var controller = new QuestController(_service.Object);

            // Act
            var result = await controller.State(playerId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task State_ShouldReturnNotFound_WhenPlayerQuestStateDoesNotExist()
        {
            // Arrange
            _service.Setup(x => x.GetQuestState(It.IsAny<Guid>())).ReturnsAsync(() => null);

            var controller = new QuestController(_service.Object);

            // Act
            var result = await controller.State(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Progress_ShouldReturnOk_WhenQuestProgressionRequestChipAmountBetGreaterThan0()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var request = new QuestProgressionRequest
            {
                PlayerId = playerId,
                ChipAmountBet = 100,
                PlayerLevel = 1
            };

            var response = new QuestProgressionResponse
            {
                TotalQuestPercentCompleted = 10.1,
                QuestPointsEarned = 101,
                MilestonesCompleted = new List<MilestoneCompleted>
                {
                    new MilestoneCompleted
                    {
                        MilestoneIndex = 1,
                        ChipsAwarded = 50
                    }
                }
            };

            _service.Setup(x => x.UpdateQuestProgress(request)).ReturnsAsync(response);

            var controller = new QuestController(_service.Object);

            // Act
            var result = await controller.Progress(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Progress_ShouldReturnBadRequest_WhenQuestProgressionRequestChipAmountBetEqualOrLessThan0()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var request = new QuestProgressionRequest
            {
                PlayerId = playerId,
                ChipAmountBet = 0,
                PlayerLevel = 1
            };

            var controller = new QuestController(_service.Object);

            // Act
            var result = await controller.Progress(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
