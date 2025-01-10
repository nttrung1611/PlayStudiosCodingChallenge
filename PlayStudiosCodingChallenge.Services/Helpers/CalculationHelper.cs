using Microsoft.Extensions.Options;
using PlayStudiosCodingChallenge.Services.ServiceModels;

namespace PlayStudiosCodingChallenge.Services.Helpers
{
    public class CalculationHelper
    {
        private readonly QuestConfigurationOptions _questConfigurationOptions;

        public CalculationHelper(IOptions<QuestConfigurationOptions> questConfigurationOptions)
        {
            _questConfigurationOptions = questConfigurationOptions.Value;
        }

        public double QuestPointsCalculator(int chipAmountBet, int playerLevel, double totalQuestPointsEarned)
        {
            var questPointsEarned = (chipAmountBet * _questConfigurationOptions.RateFromBet) + (playerLevel * _questConfigurationOptions.LevelBonusRate);

            var surplusPoints = totalQuestPointsEarned + questPointsEarned - _questConfigurationOptions.TotalQuestPointsForCompletion;

            if (surplusPoints > 0)
            {
                questPointsEarned -= surplusPoints;
            }

            return questPointsEarned;
        }

        public static double TotalQuestPercentCompletedCalculator(int totalQuestPointsForCompletion, double totalQuestPointsEarned)
        {
            var percentage = Math.Round(totalQuestPointsEarned / totalQuestPointsForCompletion, 2) * 100;
            return percentage;
        }
    }
}
