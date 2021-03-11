using System;

namespace Broker.System.Controllers.V1.Requests
{
    public class CreateLimitRequest
    {
        public int Value { get; set; }
        public string CoverType { get; set; }
    }
}