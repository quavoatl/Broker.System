using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Broker.System.IntegrationTests.Configuration
{
    public abstract class TestBase : IClassFixture<TestApplicationFactory<Startup>>
    {
        protected TestApplicationFactory<FakeStartup> Factory { get; }

        public TestBase(TestApplicationFactory<Startup> factory)
        {
            // Factory = factory;            
        }

        // Add you other helper methods here
    }
}