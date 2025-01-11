using Microsoft.EntityFrameworkCore;
using PlayStudiosCodingChallenge.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosCodingChallenge.Data.Repositories
{
    public interface IPlayerQuestStateRepository
    {
        Task<PlayerQuestState?> GetQuestStateByPlayerId(Guid playerId);
        Task CreateQuestState(PlayerQuestState playerQuestState);
        Task UpdateQuestState(PlayerQuestState playerQuestState);
    }

    public class PlayerQuestStateRepository : IPlayerQuestStateRepository
    {
        private readonly QuestStateDbContext _dbContext;

        public PlayerQuestStateRepository(QuestStateDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Insert a PlayerQuestState into database
        /// </summary>
        /// <param name="playerQuestState"></param>
        /// <returns></returns>
        public async Task CreateQuestState(PlayerQuestState playerQuestState)
        {
            try
            {
                await _dbContext.PlayerQuestStates.AddAsync(playerQuestState);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update a PlayerQuestState to database
        /// </summary>
        /// <param name="playerQuestState"></param>
        /// <returns></returns>
        public async Task UpdateQuestState(PlayerQuestState playerQuestState)
        {
            try
            {
                _dbContext.Entry(playerQuestState).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get PlayerQuestState using playerId
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task<PlayerQuestState?> GetQuestStateByPlayerId(Guid playerId)
        {
            var questState = await _dbContext.PlayerQuestStates.FindAsync(playerId);

            return questState;
        }
    }
}
