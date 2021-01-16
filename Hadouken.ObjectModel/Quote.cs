using System.ComponentModel.DataAnnotations;

namespace Hadouken.ObjectModel
{
    public class Quote : BaseEntity
    {
        [Required]
        [StringLength(128)]
        public string UserName { get; set; }

        [Required]
        public string Content { get; set; }
    }
}