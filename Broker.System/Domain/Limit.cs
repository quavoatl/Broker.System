namespace Broker.System.Domain
{
    public class Limit
    {
        public int BrokerId { get; set; }
        public int LimitId { get; set; }
        public int Value { get; set; }
        public string CoverType { get; set; }
    }
}