namespace Broker.System.Controllers.V1.Requests
{
    public class PasswordGrantTokenRequest
    {
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}