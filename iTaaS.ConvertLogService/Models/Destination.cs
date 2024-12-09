namespace iTaaS.ConvertLogService.Models
{
    public class Destination : BaseModel
    {

        public string Url { get; set; }
        public string FilePath { get; set; }
        public string ConvertedLog { get; set; }
        public string OriginalLog { get; set; }
        public string Name { get; set; }
    }
}
