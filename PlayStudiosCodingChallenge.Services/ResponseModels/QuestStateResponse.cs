using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosCodingChallenge.Services.ResponseModels
{
    public class QuestStateResponse
    {
        public double TotalQuestPercentCompleted { get; set; }
        public int LastMilestoneIndexCompleted { get; set; }
    }
}
