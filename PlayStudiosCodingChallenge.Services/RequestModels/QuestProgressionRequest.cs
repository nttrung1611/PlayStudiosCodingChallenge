using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosCodingChallenge.Services.Models
{
    public class QuestProgressionRequest
    {
        public Guid PlayerId { get; set; }
        public int PlayerLevel { get; set; }
        public uint ChipAmountBet { get; set; }

    }
}
