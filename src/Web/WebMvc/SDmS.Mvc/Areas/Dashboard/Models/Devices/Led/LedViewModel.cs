namespace SDmS.Mvc.Areas.Dashboard.Models.Led
{
    public class LedViewModel : DeviceViewModel
    {
        public bool IsEnable { get; set; }
        public int Intensity { get; set; }
        public string Power { get; set; }
        public string VoltageRange { get; set; }
    }
}