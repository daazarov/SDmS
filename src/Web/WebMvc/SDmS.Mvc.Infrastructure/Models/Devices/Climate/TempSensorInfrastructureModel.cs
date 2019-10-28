namespace SDmS.Infrastructure.Models.Devices.Climate
{
    public class TempSensorInfrastructureModel
    {
        public string serial_number { get; set; }
        public string name { get; set; }
        public bool is_online { get; set; }
        public double temp_c { get; set; }
    }
}
