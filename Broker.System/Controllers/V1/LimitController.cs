using System.Collections.Generic;
using System.Linq;
using Broker.System.Contracts.V1;
using Broker.System.Controllers.V1.Requests;
using Broker.System.Controllers.V1.Responses;
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

        [HttpPost(ApiRoutes.Limit.Create)]
        public IActionResult Create([FromBody] CreateLimitRequest limitRequest)
        {
            Limit limit = null;
            if (limitRequest.BrokerId != null)
            {
                limit = new Limit()
                    {BrokerId = limitRequest.BrokerId, Value = limitRequest.Value, CoverType = limitRequest.CoverType};
                
                _limits.Add(limit);
            }

            var baseUrl = $"{HttpContext.Request.Scheme}://" + $"{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Limit.Get.Replace("{id}", limitRequest.BrokerId.ToString());

            var limitResponse = new LimitResponse()
                {BrokerId = limit.BrokerId, Value = limit.Value, CoverType = limit.CoverType};
            return Created(locationUri, limitResponse);
        }
    }
}