using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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
    public class CoverController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;

        public CoverController(IMapper mapper, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Cover.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var coversClient = _clientFactory.CreateClient();

            coversClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await HttpContext.GetTokenAsync("access_token"));
            
            var coversResponse = await coversClient.GetAsync("https://localhost:5045/api/v1/covers");
            var coversContent = await coversResponse.Content.ReadFromJsonAsync<List<CoverResponse>>();

            if (coversContent.Count == 0) return NoContent();

            return Ok(coversContent);
        }

    }
}