using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SDmS.Domain.Core.Interfases.Services;
using SDmS.Mvc.Attributes.Filters;
using SDmS.Mvc.Areas.Dashboard.Models.Climate;
using SDmS.Mvc.Areas.Dashboard.Models.Pages;

namespace SDmS.Mvc.Areas.Dashboard.Controllers
{
    [DashboardAuthorization]
	public class ClimateController : BaseDashboardController
    {
        private static List<TempSensorViewModel> fakeModels1 = new List<TempSensorViewModel>
        {
            new TempSensorViewModel { IsOnline = true, Name = "Kitchen", SerialNumber = "GDT67MDNDYT423PO", TempC = 23},
            new TempSensorViewModel { IsOnline = false, Name = "Garage", SerialNumber = "NDB79HDGRDF075QW"},
            new TempSensorViewModel { IsOnline = true, Name = "Hall", SerialNumber = "EYG13LPTBDX321BT", TempC = 21.7}
        };

        private static List<TempControlSensorViewModel> fakeModels2 = new List<TempControlSensorViewModel>
        {
            new TempControlSensorViewModel { Name = "Bedroom", DesiredTemp = 24, IsControlEnable = false, IsOnline = true, SerialNumber = "JRB65GFBC87BD5423", TempC = 24.8 },
            new TempControlSensorViewModel { Name = "Living Room", DesiredTemp = 24, IsOnline = false, SerialNumber = "POI12PDVB34BF5981" },
            new TempControlSensorViewModel { Name = "Children's room", DesiredTemp = 25, IsControlEnable = true, IsOnline = true, SerialNumber = "HTY85PLTE15MC3675", TempC = 25.3 },
        };

        public ClimateController(ILoggingService loggingService) : base(loggingService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            ClimatePageViewModel model = new ClimatePageViewModel
            {
                DeviceAdd = new Models.AddDeviceViewModel(),
                TempSensors = fakeModels1,
                TempControlSensors = fakeModels2
            };

            return View(model);
        }

        [HttpDelete]
        public ActionResult DeleteDevice(string serial_number)
        {
            var model1 = fakeModels1.FirstOrDefault(x => x.SerialNumber == serial_number);

            if (model1 != null)
            {
                fakeModels1.Remove(model1);
            }

            var model2 = fakeModels2.FirstOrDefault(x => x.SerialNumber == serial_number);

            if (model2 != null)
            {
                fakeModels2.Remove(model2);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}