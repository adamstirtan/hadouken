using System;
using System.ComponentModel.DataAnnotations;

namespace Hadouken.ObjectModel.Commands
{
    public class CommandRequest : BaseEntity
    {
        [Required]
        public DateTime Created { get; set; }

        public DateTime? Executed { get; set; }
    }
}