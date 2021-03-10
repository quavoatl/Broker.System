using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Broker.System.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Broker.System.Services
{
    public interface ILimitService
    {
        Task<List<Limit>> GetLimitsAsync(Guid brokerId);
        Task<List<Limit>> GetLimitsAsync();
        Task<Limit> GetByIdAsync(int limitId);
        Task<bool> UpdateLimitAsync(Limit limitToUpdate);
        Task<bool> DeleteLimitAsync(int limitId);
        Task<EntityEntry<Limit>> CreateLimitAsync(Limit limit);
    }
}