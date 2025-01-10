using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosCodingChallenge.Data.Models
{
    public class PlayerQuestState
    {
        [Key]
        public Guid PlayerId { get; set; }
        public double TotalQuestPointsEarned { get; set; }
        public int LastMilestoneIndexCompleted { get; set; }
    }
}
