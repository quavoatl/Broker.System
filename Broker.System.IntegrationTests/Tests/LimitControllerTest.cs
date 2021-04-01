using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Broker.System.Contracts.V1;
using Broker.System.Controllers.V1.Requests;
using Broker.System.Controllers.V1.Responses;
using Broker.System.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Broker.System.IntegrationTests.Tests
{
    public class LimitControllerTest : IntegrationTest
    {
        [Fact]
        public async Task GetAll_InMemoryDatabase_ShouldReturn204NoContent()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var getAllResponse = await TestClient.GetAsync(ApiRoutes.Limit.GetAll);

            // Assert
            getAllResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task Create_GetAll_PostTwoLimits_ShouldReturnTwoLimits()
        {
            // Arrange
            await AuthenticateAsync();
            await TestClient.PostAsJsonAsync(ApiRoutes.Limit.Create, new CreateLimitRequest()
            {
                Value = 100,
                CoverType = "theft"
            });
            await TestClient.PostAsJsonAsync(ApiRoutes.Limit.Create, new CreateLimitRequest()
            {
                Value = 200,
                CoverType = "naturalhazards"
            });

            // Act
            var getAllResponse = await TestClient.GetAsync(ApiRoutes.Limit.GetAll);
            var x = await getAllResponse.Content.ReadFromJsonAsync<IEnumerable<LimitResponse>>();

            // Assert
            getAllResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

            x.ToList().Should().HaveCount(2);
            x.ToList()[0].Value.Should().Be(100);
            x.ToList()[0].CoverType.Should().Be("theft");

            x.ToList()[1].Value.Should().Be(200);
            x.ToList()[1].CoverType.Should().Be("naturalhazards");
        }
    }
}