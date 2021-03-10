using System.Collections.Generic;
using System.Linq;
using Broker.System.Domain;
using NotImplementedException = System.NotImplementedException;

namespace Broker.System.Services
{
    public class LimitService : ILimitService
    {
        private readonly List<Limit> _limits;

        public LimitService()
        {
            _limits = new List<Limit>();
            _limits.Add(new Limit() {BrokerId = 0, Value = 10000});
            _limits.Add(new Limit() {BrokerId = 0, Value = 20000});
            _limits.Add(new Limit() {BrokerId = 0, Value = 30000});
            _limits.Add(new Limit() {BrokerId = 0, Value = 40000});
            _limits.Add(new Limit() {BrokerId = 0, Value = 50000});
        }

        public List<Limit> GetLimits(int brokerId)
        {
            return _limits.Where(l => l.BrokerId.Equals(brokerId)).ToList();
        }

        public Limit GetById(int limitId)
        {
            return _limits.Where(l => l.BrokerId.Equals(limitId)).FirstOrDefault();
        }

        public void AddLimit(Limit limit)
        {
            _limits.Add(limit);
        }

        public bool UpdateLimit(Limit limitToUpdate)
        {
            var exists = GetById(limitToUpdate.LimitId) != null;

            if (!exists) return false;
            var index = _limits.FindIndex(l => l.LimitId.Equals(limitToUpdate.LimitId));
            _limits[index] = limitToUpdate;

            return true;
        }
    }
}