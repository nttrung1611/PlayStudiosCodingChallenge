using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosCodingChallenge.Services.ServiceModels
{
    public class QuestConfigurationOptions
    {
        public const string QuestConfiguration = "QuestConfiguration";

        public double RateFromBet { get; set; }
        public double LevelBonusRate { get; set; }
        public double TotalQuestPointsForCompletion { get; set; }
        public int QuestMilestones { get; set; }
        public int ChipsAwardedForMilestoneCompletion { get; set; }
    }
}
