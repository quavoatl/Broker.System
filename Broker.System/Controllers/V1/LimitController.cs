using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Broker.System.Contracts.V1;
using Broker.System.Controllers.V1.Requests;
using Broker.System.Controllers.V1.Responses;
using Broker.System.Domain;
using Broker.System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Broker.System.Controllers.V1
{
    [Authorize]
    [ApiController]
    public class LimitController : Controller
    {
        private readonly ILimitService _limitService;

        public LimitController(ILimitService limitService)
        {
            _limitService = limitService;
        }

        [HttpGet(ApiRoutes.Limit.GetAllByBroker)]
        public async Task<IActionResult> GetAll(Guid brokerId)
        {
            var response = await _limitService.GetLimitsAsync(brokerId);
            if (response != null) return Ok(response);
            return NotFound();
        }

        [HttpGet(ApiRoutes.Limit.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _limitService.GetLimitsAsync();
            if (response != null) return Ok(response);
            return NotFound();
        }

        [HttpPost(ApiRoutes.Limit.Create)]
        public async Task<IActionResult> Create([FromBody] CreateLimitRequest limitRequest)
        {
            Limit limit = new Limit()
            {
                BrokerId = limitRequest.BrokerId,
                Value = limitRequest.Value,
                CoverType = limitRequest.CoverType
            };

            var createdLimit = await _limitService.CreateLimitAsync(limit);

            var baseUrl = $"{HttpContext.Request.Scheme}://" + $"{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" +
                              ApiRoutes.Limit.Get.Replace("{limitId}", createdLimit.Entity.LimitId.ToString());

            var limitResponse = new LimitResponse()
                {BrokerId = limit.BrokerId, LimitId = limit.LimitId, Value = limit.Value, CoverType = limit.CoverType};
            return Created(locationUri, limitResponse);
        }

        [HttpGet(ApiRoutes.Limit.Get)]
        public async Task<IActionResult> Get([FromRoute] int limitId)
        {
            var limit = await _limitService.GetByIdAsync(limitId);

            if (limit == null) return NotFound(new {name = "Resource not found!"});

            return Ok(limit);
        }

        [HttpPut(ApiRoutes.Limit.Update)]
        public async Task<IActionResult> Update([FromRoute] int limitId,
            [FromBody] UpdateLimitRequest updateLimitRequest)
        {
            var newLimit = new Limit()
            {
                LimitId = limitId,
                BrokerId = updateLimitRequest.BrokerId,
                Value = updateLimitRequest.Value,
                CoverType = updateLimitRequest.CoverType
            };

            var updated = await _limitService.UpdateLimitAsync(newLimit);
            if (updated) return Ok(newLimit);
            return NotFound();
        }

        [HttpDelete(ApiRoutes.Limit.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int limitId)
        {
            var deleted = await _limitService.DeleteLimitAsync(limitId);
            if (deleted) return NoContent();
            return NotFound();
        }
    }
}