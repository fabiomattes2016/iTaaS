using System.ComponentModel.DataAnnotations;

namespace iTaaS.ConvertLogService.Models
{
    public class Source : BaseModel
    {
        public string Log { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
