using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Broker.System.Contracts.V1;
using Broker.System.Controllers.V1.Requests;
using Broker.System.Controllers.V1.Responses;
using Broker.System.Domain;
using Broker.System.Extensions;
using Broker.System.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Broker.System.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Broker")]
    [ApiController]
    public class LimitController : Controller
    {
        private readonly ILimitService _limitService;
        private readonly IMapper _mapper;

        public LimitController(ILimitService limitService, IMapper mapper)
        {
            _limitService = limitService;
            _mapper = mapper;
        }


        [HttpGet(ApiRoutes.Limit.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var user = HttpContext.User;
            // var user = User.Identities.ToList();
            // var limits = await _limitService.GetLimitsAsync(HttpContext.GetUserId());
            //
            // return Ok(_mapper.Map<List<Limit>>(limits));
            //var token = HttpContext.GetTokenAsync("access_token");
            return Ok(new {response = "asd"});
        }

        [HttpPost(ApiRoutes.Limit.Create)]
        public async Task<IActionResult> Create([FromBody] CreateLimitRequest limitRequest)
        {
            Limit limit = new Limit()
            {
                BrokerId = HttpContext.GetUserId(),
                Value = limitRequest.Value,
                CoverType = limitRequest.CoverType
            };

            var createdLimit = await _limitService.CreateAsync(limit);

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
            var userOwnsPost = await _limitService.UserOwnsLimit(limitId, HttpContext.GetUserId());

            if (userOwnsPost)
            {
                var limitFromDb = await _limitService.GetByIdAsync(limitId);
                limitFromDb.Value = updateLimitRequest.Value;
                limitFromDb.CoverType = updateLimitRequest.CoverType;

                var updated = await _limitService.UpdateAsync(limitFromDb);
                if (updated) return Ok(limitFromDb);
            }

            return BadRequest(new {error = "You do not own this post !"});
        }

        [HttpDelete(ApiRoutes.Limit.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int limitId)
        {
            var userOwnsPost = await _limitService.UserOwnsLimit(limitId, HttpContext.GetUserId());

            if (userOwnsPost)
            {
                var deleted = await _limitService.DeleteAsync(limitId);
                if (deleted) return NoContent();
            }

            return NotFound();
        }
    }
}