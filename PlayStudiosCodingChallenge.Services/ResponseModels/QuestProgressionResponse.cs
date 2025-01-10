using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosCodingChallenge.Services.ResponseModels
{
    public class QuestProgressionResponse
    {
        public double QuestPointsEarned { get; set; }
        public double TotalQuestPercentCompleted { get; set; }
        public List<MilestoneCompleted> MilestonesCompleted { get; set; } = new List<MilestoneCompleted>();
    }

    public class MilestoneCompleted
    {
        public int MilestoneIndex { get; set; }
        public int ChipsAwarded { get; set; }
    }
}
