using System;

namespace Broker.System.Controllers.V1.Requests
{
    public class UpdateLimitRequest
    {
        public int Value { get; set; }
        public string CoverType { get; set; }
    }
}