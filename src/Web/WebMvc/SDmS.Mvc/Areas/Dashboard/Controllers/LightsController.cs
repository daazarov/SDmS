using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SDmS.Mvc.Attributes.Filters;
using SDmS.Mvc.Areas.Dashboard.Models.Led;

namespace SDmS.Mvc.Areas.Dashboard.Controllers
{
    [DashboardAuthorization]
	public class LightsController : BaseDashboardController
    {
        private static List<LedViewModel> fakeModels = new List<LedViewModel>
        {
            new LedViewModel { Name = "Kitchen", Intensity = 68, IsEnable = true, IsOnline = true, Power = 24, SerialNumber = "JHB4345KHJ35K", VoltageRange = "110-130V" },
            new LedViewModel { Name = "Garage", Intensity = 100, IsEnable = false, IsOnline = false, Power = 24, SerialNumber = "JHB75KTJ35K", VoltageRange = "110-130V" },
            new LedViewModel { Name = "Pool", Intensity = 20, IsEnable = false, IsOnline = true, Power = 60, SerialNumber = "TGB6835KHJ12K", VoltageRange = "220V" }
        };

        public LightsController()
        {

        }

        // GET: Dashboard/Lights
        public ActionResult Index()
        {
            return View(fakeModels);
        }

        [HttpPost]
        public ActionResult AddLed(LedCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return View();
        }

        [HttpDelete]
        public JsonResult DeleteLed(string serial_number)
        {
            fakeModels.Remove(fakeModels.First(_ => _.SerialNumber == serial_number));
            return Json("1");
        }

        [HttpGet]
        public ActionResult Configure(string serial_number)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public JsonResult ChangeProperty(string serial_number, LedChangeStateRequestModel model)
        {
            return Json("1");
        }
    }
}