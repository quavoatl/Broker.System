using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Broker.System.Data;
using Broker.System.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NotImplementedException = System.NotImplementedException;

namespace Broker.System.Services
{
    public class LimitService : ILimitService
    {
        private readonly BrokerDbContext _brokerDbContext;

        public LimitService(BrokerDbContext brokerDbContext)
        {
            _brokerDbContext = brokerDbContext;
        }

        public async Task<List<Limit>> GetLimitsAsync(int brokerId)
        {
            return await EntityFrameworkQueryableExtensions.ToListAsync(
                _brokerDbContext.Limits.Where(l => l.BrokerId.Equals(brokerId)));
        }

        public async Task<Limit> GetByIdAsync(int limitId)
        {
            return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(
                _brokerDbContext.Limits.Where(l => l.LimitId.Equals(limitId)));
        }

        public async Task<bool> UpdateLimitAsync(Limit limitToUpdate)
        {
            _brokerDbContext.Limits.Update(limitToUpdate);
            var update = await _brokerDbContext.SaveChangesAsync();

            return update > 0;
        }

        public async Task<bool> DeleteLimitAsync(int limitId)
        {
            var limitFromList = await GetByIdAsync(limitId);

            _brokerDbContext.Limits.Remove(limitFromList);
            var deleted = await _brokerDbContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<EntityEntry<Limit>> CreateLimitAsync(Limit limit)
        {
            var res = await _brokerDbContext.Limits.AddAsync(limit);
            var created = await _brokerDbContext.SaveChangesAsync();
            return res;
        }
    }
}