using System;

namespace iTaaS.ConvertLogService.DTOs
{
    public class SourceReadDTO
    {
        public Guid Id { get; set; }
        public string Log { get; set; }
        public string Url { get; set; }
        public DateTime Created { get; set; }
    }
}
