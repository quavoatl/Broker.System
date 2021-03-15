using System;

namespace Broker.System.Controllers.V1.Responses
{
    public class LimitResponse
    {
        public int LimitId { get; set; }
        public int Value { get; set; }
        public string CoverType { get; set; }
        public string BrokerId { get; set; }
    }
}