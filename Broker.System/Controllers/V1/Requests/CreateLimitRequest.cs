namespace Broker.System.Controllers.V1.Requests
{
    public class CreateLimitRequest
    {
        public int BrokerId { get; set; }
        public int Value { get; set; }
        public int LimitId { get; set; }
        public string CoverType { get; set; }
    }
}