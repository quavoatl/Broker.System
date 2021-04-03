using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Broker.System.Domain
{
    public class Limit
    {
        [Key] public int LimitId { get; set; }
        public int Value { get; set; }
        public string CoverType { get; set; }
        public string BrokerId { get; set; }

        [ForeignKey(nameof(BrokerId))] 
        public IdentityUser User { get; set; }
    }
}