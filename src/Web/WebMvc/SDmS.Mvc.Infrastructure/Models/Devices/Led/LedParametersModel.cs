namespace SDmS.Infrastructure.Models.Devices.Led
{
    public class LedParametersModel
    {
        public bool is_enabled { get; set; }
        public int power { get; set; }
        public int intensity { get; set; }
        public int min_voltage { get; set; }
        public int max_voltage { get; set; }
    }
}
