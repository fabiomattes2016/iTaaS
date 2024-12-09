using System;
using System.ComponentModel.DataAnnotations;

namespace iTaaS.ConvertLogService.Models
{
    public class BaseModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
