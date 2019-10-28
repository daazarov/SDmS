using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SDmS.Domain.Core.Interfases.Services;
using SDmS.Mvc.Attributes.Filters;
using SDmS.Mvc.Areas.Dashboard.Models.Climate;
using SDmS.Mvc.Areas.Dashboard.Models.Pages;
using System.Threading.Tasks;
using SDmS.Mvc.Areas.Dashboard.Models;
using SDmS.Domain.Common.Identity;
using SDmS.Mvc.Models;
using SDmS.Mvc.Models.Enums;
using SDmS.Domain.Core.Models;
using SDmS.Mvc.Areas.Dashboard.Mappers.Climate;

namespace SDmS.Mvc.Areas.Dashboard.Controllers
{
    [DashboardAuthorization]
	public class ClimateController : BaseDashboardController
    {
        private readonly IIdentityParser<ApplicationUser> _identityParser;
        private readonly IClimateDeviceService _climateDeviceService;

        private bool _useFakeData = true;

        private static List<TempSensorViewModel> fakeModels1 = new List<TempSensorViewModel>
        {
            new TempSensorViewModel { IsOnline = true, Name = "Kitchen", SerialNumber = "GDT67MDNDYT423PO", TempC = 23},
            new TempSensorViewModel { IsOnline = false, Name = "Garage", SerialNumber = "NDB79HDGRDF075QW"},
            new TempSensorViewModel { IsOnline = true, Name = "Hall", SerialNumber = "EYG13LPTBDX321BT", TempC = 21.7}
        };

        private static List<TempControlViewModel> fakeModels2 = new List<TempControlViewModel>
        {
            new TempControlViewModel { Name = "Bedroom", DesiredTemp = 24, IsControlEnable = false, IsOnline = true, SerialNumber = "JRB65GFBC87BD5423", TempC = 24.8 },
            new TempControlViewModel { Name = "Living Room", DesiredTemp = 24, IsOnline = false, SerialNumber = "POI12PDVB34BF5981" },
            new TempControlViewModel { Name = "Children's room", DesiredTemp = 25, IsControlEnable = true, IsOnline = true, SerialNumber = "HTY85PLTE15MC3675", TempC = 25.3 },
        };

        public ClimateController(
            IIdentityParser<ApplicationUser> identityParser,
            IClimateDeviceService climateDeviceService,
            ILoggingService loggingService
            ) : base(loggingService)
        {
            this._identityParser = identityParser;
            this._climateDeviceService = climateDeviceService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (_useFakeData)
            {
                ClimatePageViewModel fakeModel = new ClimatePageViewModel
                {
                    DeviceAdd = new AddDeviceViewModel(),
                    TempSensors = fakeModels1,
                    TempControlSensors = fakeModels2
                };

                return View(fakeModel);
            }

            var controlResponse = await _climateDeviceService.GetTempControlDevicesAsync(new DeviceRequestDomainModel { limit = 0, offset = 0, type = "control" });

            if (controlResponse.ErrorCode != null)
            {
                ShowMessage(new GenericMessageViewModel { Type = MessageTypes.warning, Message = controlResponse.Error });
            }

            var tempSensorResponse = await _climateDeviceService.GetTempSensorDevicesAsync(new DeviceRequestDomainModel { limit = 0, offset = 0, type = "temperature" });

            if (tempSensorResponse.ErrorCode != null)
            {
                ShowMessage(new GenericMessageViewModel { Type = MessageTypes.warning, Message = tempSensorResponse.Error });
            }

            ClimatePageViewModel model = new ClimatePageViewModel
            {
                DeviceAdd = new AddDeviceViewModel(),
                TempSensors = tempSensorResponse.Collection.Select(x => x.DomainToView()),
                TempControlSensors = controlResponse.Collection.Select(x => x.DomainToView())
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

            /*var response = await this._ledDeviceService.AssignToUserAsync(model.ViewToDomain());

            if (!response.Value)
            {
                ShowMessage(new GenericMessageViewModel { Type = MessageTypes.warning, Message = response.Error });
            }*/

            return RedirectToAction(nameof(Index));
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