using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PlayStudiosCodingChallenge.Data.Models;
using PlayStudiosCodingChallenge.Data.Repositories;
using PlayStudiosCodingChallenge.Services.Helpers;
using PlayStudiosCodingChallenge.Services.Models;
using PlayStudiosCodingChallenge.Services.ResponseModels;
using PlayStudiosCodingChallenge.Services.ServiceModels;

namespace PlayStudiosCodingChallenge.Services
{
    public interface IPlayerQuestService 
    {
        Task<QuestProgressionResponse> UpdateQuestProgress(QuestProgressionRequest request);
        Task<QuestStateResponse?> GetQuestState(Guid playerId);
        Guid GetPlayerId();
    }

    public class PlayerQuestService : IPlayerQuestService
    {
        private readonly IPlayerQuestStateRepository _playerQuestStateRepository;
        private readonly QuestConfigurationOptions _questConfiguration;

        public PlayerQuestService(IPlayerQuestStateRepository playerQuestStateRepository, IOptions<QuestConfigurationOptions> questConfiguration)
        {
            _playerQuestStateRepository = playerQuestStateRepository;
            _questConfiguration = questConfiguration.Value;
        }

        public async Task<QuestProgressionResponse> UpdateQuestProgress(QuestProgressionRequest request)
        {
            try
            {
                var questState = await _playerQuestStateRepository.GetQuestStateByPlayerId(request.PlayerId);

                double questPointsEarned = 0D;
                int milestonesCompleted = 0;
                int? lastMilestoneIndexCompleted = questState?.LastMilestoneIndexCompleted;

                if (questState == null)
                {
                    // If no quest state then create new one
                    questPointsEarned = CalculateQuestPoints(request.ChipAmountBet, request.PlayerLevel);
                    milestonesCompleted = CalculateMilestoneCompleted(questPointsEarned, 0);

                    var newQuestState = new PlayerQuestState
                    {
                        PlayerId = request.PlayerId,
                        TotalQuestPointsEarned = questPointsEarned,
                        LastMilestoneIndexCompleted = milestonesCompleted
                    };

                    await _playerQuestStateRepository.CreateQuestState(newQuestState);         
                }
                else
                {
                    // Else update current quest state
                    questPointsEarned = CalculateQuestPoints(request.ChipAmountBet, request.PlayerLevel);
                    milestonesCompleted = CalculateMilestoneCompleted(questPointsEarned, questState.LastMilestoneIndexCompleted);

                    questState.TotalQuestPointsEarned += questPointsEarned;
                    questState.LastMilestoneIndexCompleted += milestonesCompleted;

                    await _playerQuestStateRepository.UpdateQuestState(questState);
                }

                double totalQuestPointsEarned = questState != null ? questState.TotalQuestPointsEarned : questPointsEarned;

                var milestonesCompletedList = GetListMilestonesCompleted(lastMilestoneIndexCompleted, milestonesCompleted);

                return new QuestProgressionResponse
                {
                    TotalQuestPercentCompleted = CalculateTotalQuestPercentCompleted(totalQuestPointsEarned),
                    QuestPointsEarned = questPointsEarned,
                    MilestonesCompleted = milestonesCompletedList
                };

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public async Task<QuestStateResponse?> GetQuestState(Guid playerId)
        {
            try
            {
                var questState = await _playerQuestStateRepository.GetQuestStateByPlayerId(playerId);

                if (questState == null) return null;

                return new QuestStateResponse
                {
                    TotalQuestPercentCompleted = CalculateTotalQuestPercentCompleted(questState.TotalQuestPointsEarned),
                    LastMilestoneIndexCompleted = questState.LastMilestoneIndexCompleted,
                };

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public Guid GetPlayerId()
        {
            return Guid.NewGuid();
        }

        #region Private methods
        private double CalculateQuestPoints(uint chipAmountBet, int playerLevel)
        {
            var questPointsEarned = (chipAmountBet * _questConfiguration.RateFromBet) + (playerLevel * _questConfiguration.LevelBonusRate);

            return Math.Round(questPointsEarned, 2);
        }

        private int CalculateMilestoneCompleted(double questPointsEarned, int lastMilestoneIndexCompleted)
        {
            var milestoneCompletionPoint = (double)(_questConfiguration.TotalQuestPointsForCompletion / _questConfiguration.QuestMilestones);

            var milestonesCompleted = (int)Math.Floor(questPointsEarned / milestoneCompletionPoint);

            // If exceeds total quest milestones then remove surplus milestones
            var surplusMilestones = lastMilestoneIndexCompleted + milestonesCompleted - _questConfiguration.QuestMilestones;
            if (surplusMilestones > 0)
                milestonesCompleted -= surplusMilestones;

            return milestonesCompleted;
        }

        private double CalculateTotalQuestPercentCompleted(double totalQuestPointsEarned)
        {
            if (totalQuestPointsEarned >= _questConfiguration.TotalQuestPointsForCompletion)
            {
                return 100;
            }

            var percentage = Math.Round(totalQuestPointsEarned / _questConfiguration.TotalQuestPointsForCompletion, 4) * 100;
            return percentage;
        }

        private List<MilestoneCompleted> GetListMilestonesCompleted(int? lastMilestoneIndexCompleted, int milestonesCompleted)
        {
            var milestonesCompletedList = new List<MilestoneCompleted>();

            if (milestonesCompleted > 0)
            {
                for (int i = 1; i <= milestonesCompleted; i++)
                {
                    milestonesCompletedList.Add(new MilestoneCompleted
                    {
                        MilestoneIndex = (lastMilestoneIndexCompleted ?? 0) + i,
                        ChipsAwarded = _questConfiguration.ChipsAwardedForMilestoneCompletion
                    });
                }
            }

            return milestonesCompletedList;
        }
        #endregion
    }
}
