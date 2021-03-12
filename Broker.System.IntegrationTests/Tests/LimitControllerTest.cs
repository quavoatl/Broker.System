using System.Threading.Tasks;
using Xunit;

namespace Broker.System.IntegrationTests.Tests
{
    public class LimitControllerTest : IntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutAnyLimit_ReturnsEmpty()
        {
            // Arrange
            await AuthenticateAsync();


            // Act

            // Assert
        }
    }
}