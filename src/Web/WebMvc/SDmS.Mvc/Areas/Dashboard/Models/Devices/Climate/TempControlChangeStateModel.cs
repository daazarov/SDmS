namespace SDmS.Mvc.Areas.Dashboard.Models.Devices.Climate
{
    public class TempControlChangeStateModel
    {
        public bool? IsControlEnabled { get; set; }
        public int? DesiredTemperature { get; set; }
        public int Type { get; set; }
    }
}