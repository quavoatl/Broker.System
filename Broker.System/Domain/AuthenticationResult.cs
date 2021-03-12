using System.Collections.Generic;

namespace Broker.System.Domain
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public bool Success { get; set; }
    }
}