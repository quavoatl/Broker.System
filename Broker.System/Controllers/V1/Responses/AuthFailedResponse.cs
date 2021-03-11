using System.Collections.Generic;

namespace Broker.System.Controllers.V1.Responses
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}