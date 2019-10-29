using SDmS.Mvc.Areas.Dashboard.Models.Climate;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SDmS.Mvc.Areas.Dashboard.Models.Pages
{
    public class ClimatePageViewModel
    {
        public IEnumerable<TempSensorViewModel> TempSensors { get; set; }
        public IEnumerable<TempControlViewModel> TempControlSensors { get; set; }
        public AddDeviceViewModel DeviceAdd { get; set; }
        public SelectList DeviceTypes { get; set; }
    }
}