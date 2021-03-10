using System.Collections.Generic;
using System.Linq;
using Broker.System.Contracts.V1;
using Broker.System.Controllers.V1.Requests;
using Broker.System.Controllers.V1.Responses;
using Broker.System.Domain;
using Broker.System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Broker.System.Controllers.V1
{
    [ApiController]
    public class LimitController : Controller
    {
        private readonly ILimitService _limitService;

        public LimitController(ILimitService limitService)
        {
            _limitService = limitService;
        }

        [HttpGet(ApiRoutes.Limit.GetAll)]
        public IActionResult GetAll(int brokerId)
        {
            return Ok(_limitService.GetLimits(brokerId));
        }

        [HttpPost(ApiRoutes.Limit.Create)]
        public IActionResult Create([FromBody] CreateLimitRequest limitRequest)
        {
            Limit limit = null;
            if (limitRequest.BrokerId != 0)
            {
                limit = new Limit()
                {
                    BrokerId = limitRequest.BrokerId, LimitId = limitRequest.LimitId, Value = limitRequest.Value,
                    CoverType = limitRequest.CoverType
                };

                _limitService.AddLimit(limit);
            }

            var baseUrl = $"{HttpContext.Request.Scheme}://" + $"{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Limit.Get.Replace("{limitId}", limitRequest.LimitId.ToString());

            var limitResponse = new LimitResponse()
                {BrokerId = limit.BrokerId, LimitId = limit.LimitId, Value = limit.Value, CoverType = limit.CoverType};
            return Created(locationUri, limitResponse);
        }

        [HttpGet(ApiRoutes.Limit.Get)]
        public IActionResult Get([FromRoute] int limitId)
        {
            var limit = _limitService.GetById(limitId);

            if (limit == null) return NotFound(new {name = "Resource not found!"});

            return Ok(limit);
        }
        
        [HttpPut(ApiRoutes.Limit.Update)]
        public IActionResult Update([FromRoute] int limitId, [FromBody] UpdateLimitRequest updateLimitRequest)
        {
            var newLimit = new Limit()
            {
                LimitId = limitId,
                BrokerId = updateLimitRequest.BrokerId,
                Value = updateLimitRequest.Value,
                CoverType = updateLimitRequest.CoverType
            };

            var updated = _limitService.UpdateLimit(newLimit);
            if(updated) return Ok(newLimit);
            return NotFound();
        }
    }
}