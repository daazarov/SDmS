namespace SDmS.Mvc.Areas.Dashboard.Models.Climate
{
    public class TemperatureViewModel
    {
        public double TempC { get; set; }
        public double TempF { get { return (TempC * (9 / 5)) + 32; } }
    }
}