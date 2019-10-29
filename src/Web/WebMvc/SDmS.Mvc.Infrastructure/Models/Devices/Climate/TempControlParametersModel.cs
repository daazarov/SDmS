namespace SDmS.Infrastructure.Models.Devices.Climate
{
    public class TempControlParametersModel
    {
        public double temp_c { get; set; }
        public bool is_control_enable { get; set; }
        public int desired_temp { get; set; }
    }
}
