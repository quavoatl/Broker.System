using System.Collections.Generic;
using System.Linq;
using Broker.System.Contracts.V1;
using Broker.System.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Broker.System.Controllers.V1
{
    [ApiController]
    public class LimitController : Controller
    {
        private readonly List<Limit> _limits;

        public LimitController()
        {
            _limits = new List<Limit>();
            _limits.Add(new Limit() {BrokerId = 1, Value = 10000});
            _limits.Add(new Limit() {BrokerId = 1, Value = 20000});
            _limits.Add(new Limit() {BrokerId = 1, Value = 30000});
            _limits.Add(new Limit() {BrokerId = 1, Value = 40000});
            _limits.Add(new Limit() {BrokerId = 1, Value = 50000});
        }

        [HttpGet(ApiRoutes.Limit.GetAll)]
        public IActionResult GetAll(int id)
        {
            return Ok(_limits.Where(l => l.BrokerId.Equals(id)));
        }
    }
}