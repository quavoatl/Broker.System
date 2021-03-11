using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Broker.System.Data;
using Broker.System.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Broker.System.Services
{
    public class LimitService : ILimitService
    {
        private readonly BrokerDbContext _brokerDbContext;

        public LimitService(BrokerDbContext brokerDbContext)
        {
            _brokerDbContext = brokerDbContext;
        }

        public async Task<List<Limit>> GetLimitsAsync(string brokerId)
        {
            return await EntityFrameworkQueryableExtensions.ToListAsync(
                _brokerDbContext.Limits.Where(l => l.BrokerId.Equals(brokerId)));
        }

        public async Task<List<Limit>> GetLimitsAsync()
        {
            return await EntityFrameworkQueryableExtensions.ToListAsync(_brokerDbContext.Limits);
        }

        public async Task<Limit> GetByIdAsync(int limitId)
        {
            return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(
                _brokerDbContext.Limits.Where(l => l.LimitId.Equals(limitId)));
        }

        public async Task<bool> UpdateAsync(Limit limitToUpdate)
        {
            _brokerDbContext.Limits.Update(limitToUpdate);
            var update = await _brokerDbContext.SaveChangesAsync();

            return update > 0;
        }

        public async Task<bool> DeleteAsync(int limitId)
        {
            var limitFromList = await GetByIdAsync(limitId);

            if (limitFromList != null)
            {
                _brokerDbContext.Limits.Remove(limitFromList);
                var deleted = await _brokerDbContext.SaveChangesAsync();
                return deleted > 0;
            }

            return false;
        }

        public async Task<EntityEntry<Limit>> CreateAsync(Limit limit)
        {
            var res = await _brokerDbContext.Limits.AddAsync(limit);
            var created = await _brokerDbContext.SaveChangesAsync();
            return res;
        }

        public async Task<bool> UserOwnsLimit(int limitId, string userId)
        {
            var limit = await _brokerDbContext.Limits.AsNoTracking().SingleOrDefaultAsync(x => x.LimitId == limitId);

            if (limit == null) return false;

            if (limit.BrokerId == userId) return true;

            return false;
        }
    }
}