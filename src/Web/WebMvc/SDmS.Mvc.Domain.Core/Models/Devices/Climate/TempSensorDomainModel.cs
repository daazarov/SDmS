namespace SDmS.Domain.Core.Models.Climate
{
    public class TempSensorDomainModel
    {
        public string serial_number { get; set; }
        public string name { get; set; }
        public bool is_online { get; set; }
        public double temp_c { get; set; }
    }
}
