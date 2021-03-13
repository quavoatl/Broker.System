using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Broker.System.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Broker.System.Services
{
    public interface ILimitService
    {
        Task<List<Limit>> GetLimitsAsync(string brokerId);
        Task<Limit> GetByIdAsync(int limitId);
        Task<bool> UpdateAsync(Limit limitToUpdate);
        Task<bool> DeleteAsync(int limitId);
        Task<EntityEntry<Limit>> CreateAsync(Limit limit);
        Task<bool> UserOwnsLimit(int limitId, string userId);
    }
}