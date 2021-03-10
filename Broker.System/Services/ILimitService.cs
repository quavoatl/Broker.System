using System.Collections.Generic;
using Broker.System.Domain;

namespace Broker.System.Services
{
    public interface ILimitService
    {
        List<Limit> GetLimits(int brokerId);
        Limit GetById(int limitId);
        void AddLimit(Limit limit);
        bool UpdateLimit(Limit limitToUpdate);
    }
}