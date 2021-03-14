using System.Collections.Generic;

namespace Broker.System.Controllers.V1.Responses
{
    public class LoginResponse
    {
        public IEnumerable<string> Messages { get; set; }
        public bool Success { get; set; }
    }
}