namespace SDmS.Mvc.Areas.Dashboard.Models.Climate
{
    public class TempControlSensorViewModel : DeviceViewModel
    {
        public double TempC { get; set; }
        public double TempF { get { return (TempC * (9 / 5)) + 32; } }
        public bool IsControlEnable { get; set; }
        public int DesiredTemp { get; set; }
    }
}