using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SDmS.Mvc.Attributes.Filters;
using SDmS.Mvc.Areas.Dashboard.Models.Led;
using SDmS.Domain.Core.Interfases.Services;
using SDmS.Domain.Common.Identity;
using SDmS.Mvc.Areas.Dashboard.Models.Pages;
using System.Threading.Tasks;
using SDmS.Mvc.Areas.Dashboard.Mappers.Led;
using SDmS.Mvc.Models;
using SDmS.Mvc.Models.Enums;
using SDmS.Mvc.Areas.Dashboard.Models;

namespace SDmS.Mvc.Areas.Dashboard.Controllers
{
    [DashboardAuthorization]
	public class LightsController : BaseDashboardController
    {
        private readonly IIdentityParser<ApplicationUser> _identityParser;
        private readonly ILedDeviceService _ledDeviceService;

        private static List<LedViewModel> fakeModels = new List<LedViewModel>
        {
            new LedViewModel { Name = "Kitchen", Intensity = 68, IsEnable = true, IsOnline = true, Power = 24, SerialNumber = "JHB4345KHJ35K", VoltageRange = "110-130V" },
            new LedViewModel { Name = "Garage", Intensity = 100, IsEnable = false, IsOnline = false, Power = 24, SerialNumber = "JHB75KTJ35K", VoltageRange = "110-130V" },
            new LedViewModel { Name = "Pool", Intensity = 20, IsEnable = false, IsOnline = true, Power = 60, SerialNumber = "TGB6835KHJ12K", VoltageRange = "220V" }
        };

        public LightsController(
            IIdentityParser<ApplicationUser> identityParser, 
            ILedDeviceService ledDeviceService,
            ILoggingService loggingService) : base(loggingService)
        {
            this._identityParser = identityParser;
            this._ledDeviceService = ledDeviceService;
        }

        public async Task<ActionResult> Index()
        {
            //var response = await this._ledDeviceService.GetLedDevicesAsync(new LedRequestDomainModel { limit = 0, offset = 0 });

            LightPageViewModel model = new LightPageViewModel
            {
                Leds = fakeModels,
                LedAdd = new AddDeviceViewModel()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddDevice(AddDeviceViewModel model)
        {
            if (String.IsNullOrEmpty(model.UserId))
            {
                var user = _identityParser.Parse(User);
                model.UserId = user.id;
            }

            var response = await this._ledDeviceService.AssignToUserAsync(model.ViewToDomain());

            if (!response.Value)
            {
                ShowMessage(new GenericMessageViewModel { Type = MessageTypes.warning, Message = response.Error });
            }

            return RedirectToAction(nameof(Index));
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

        protected override void OnException(ExceptionContext filterContext)
        {
            ShowMessage(new GenericMessageViewModel { Type = MessageTypes.warning, Message = " 500 Internal Server Error. Please try again later" });

            filterContext.Result = this.RedirectToAction("Index", "General");
            filterContext.ExceptionHandled = true;

            try
            {
                this._loggingService.Error(filterContext.Exception);
            }
            catch { }
        }
    }
}