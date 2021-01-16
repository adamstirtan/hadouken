using System;
using System.ComponentModel.DataAnnotations;

namespace Hadouken.ObjectModel
{
    public class Message : BaseEntity
    {
        [Required]
        [StringLength(128)]
        public string UserName { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}