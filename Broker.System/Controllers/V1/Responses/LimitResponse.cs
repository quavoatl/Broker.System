namespace Broker.System.Controllers.V1.Responses
{
    public class LimitResponse
    {
        public int BrokerId { get; set; }
        public int Value { get; set; }
        public string CoverType { get; set; }
    }
}