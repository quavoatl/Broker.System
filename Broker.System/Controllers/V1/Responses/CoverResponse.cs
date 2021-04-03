namespace Broker.System.Controllers.V1.Responses
{
    public class CoverResponse
    {
        public int CoverId { get; set; }
        public string BrokerId { get; set; }
        public string Type { get; set; }
        public double LimitMultiplier { get; set; }
    }
}