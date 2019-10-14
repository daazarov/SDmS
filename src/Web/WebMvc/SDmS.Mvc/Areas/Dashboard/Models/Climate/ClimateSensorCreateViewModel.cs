using System.Web.Mvc;

namespace SDmS.Mvc.Areas.Dashboard.Models.Climate
{
    public class ClimateSensorCreateViewModel
    {
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
    }
}