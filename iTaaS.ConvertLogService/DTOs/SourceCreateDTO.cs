using System.ComponentModel.DataAnnotations;

namespace iTaaS.ConvertLogService.DTOs
{
    public class SourceCreateDTO
    {
        public string Log { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
