using System;
using System.ComponentModel.DataAnnotations;

namespace Broker.System.Domain
{
    public class Limit
    {
        public Guid BrokerId { get; set; }
        [Key]
        public int LimitId { get; set; }
        public int Value { get; set; }
        public string CoverType { get; set; }

    }
}