namespace SDmS.Infrastructure.Models.Devices.Climate
{
    public class TempControlInfrastructureModel
    {
        public string serial_number { get; set; }
        public string name { get; set; }
        public bool is_online { get; set; }
        public double temp_c { get; set; }
        public bool is_control_enable { get; set; }
        public int desired_temp { get; set; }
    }
}
