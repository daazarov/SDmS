namespace SDmS.Infrastructure.Models.Devices.Climate
{
    public class TempControlParametersModel
    {
        public double temperature_data { get; set; }
        public bool desired_enabled { get; set; }
        public int desired_temperature { get; set; }
    }
}
