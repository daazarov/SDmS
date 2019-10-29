namespace SDmS.Domain.Core.Models.Climate
{
    public class TempSensorDomainModel
    {
        public string device_id { get; set; }
        public string serial_number { get; set; }
        public string user_id { get; set; }
        public string name { get; set; }
        public bool is_online { get; set; }
        public double temp_c { get; set; }
    }
}
