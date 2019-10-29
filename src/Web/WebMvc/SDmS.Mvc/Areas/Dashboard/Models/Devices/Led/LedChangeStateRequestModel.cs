namespace SDmS.Mvc.Areas.Dashboard.Models.Led
{
    public class LedChangeStateRequestModel
    {
        public int? Intensity { get; set; }
        public bool? IsEnable { get; set; }
        public int Type { get; set; }
    }
}