namespace SDmS.Domain.Core.Models.Led
{
    public class LedDomainModel
    {
        public string device_id { get; set; }
        public string name { get; set; }
        public string serial_number { get; set; }
        public bool is_enabled { get; set; }
        public bool is_online { get; set; }
        public int power { get; set; }
        public int intensity { get; set; }
        public string user_id { get; set; }
        public int min_voltage { get; set; }
        public int max_voltage { get; set; }
    }
}
