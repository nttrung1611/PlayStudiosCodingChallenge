using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlayStudiosCodingChallenge.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosCodingChallenge.Data
{
    public class QuestStateDbContext : DbContext
    {
        public DbSet<PlayerQuestState> PlayerQuestStates { get; set; }

        public QuestStateDbContext(DbContextOptions<QuestStateDbContext> options) : base(options)
        {

        }
    }
}
