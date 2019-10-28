using SDmS.Mvc.Areas.Dashboard.Models.Climate;
using System.Collections.Generic;

namespace SDmS.Mvc.Areas.Dashboard.Models.Pages
{
    public class ClimatePageViewModel
    {
        public IEnumerable<TempSensorViewModel> TempSensors { get; set; }
        public IEnumerable<TempControlViewModel> TempControlSensors { get; set; }
        public AddDeviceViewModel DeviceAdd { get; set; }
    }
}