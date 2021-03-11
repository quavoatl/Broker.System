using System;

namespace Broker.System.Controllers.V1.Responses
{
    public class LimitResponse
    {
        public string BrokerId { get; set; }
        public int Value { get; set; }
        public int LimitId { get; set; }
        public string CoverType { get; set; }
    }
}