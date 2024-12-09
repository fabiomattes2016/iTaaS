using System;

namespace iTaaS.ConvertLogService.DTOs
{
    public class DestinationReadDTO
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string FilePath { get; set; }
        public string ConvertedLog { get; set; }
        public string OriginalLog { get; set; }
        public DateTime Created { get; set; }
    }
}
