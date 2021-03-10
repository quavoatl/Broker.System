using System;
using System.ComponentModel.DataAnnotations;

namespace Broker.System.Domain
{
    public class Question
    {
        public Guid BrokerId { get; set; }
        [Key] 
        public int QuestionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}