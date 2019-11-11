using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SDmS.Resource.Api.Attributes.Filters;
using SDmS.Resource.Api.Models.Devices;
using SDmS.Resource.Api.Models.Mappers;
using SDmS.Resource.Domain.Interfaces.Services;

namespace SDmS.Resource.Api.Controllers.v1
{
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(ExceptionFilterAttribute))]
    public class DevicesController : BaseApiController
    {
        private readonly IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService, IErrorInformatorService errorInformatorService) : base(errorInformatorService)
        {
            this._deviceService = deviceService;
        }

        #region [Device CRUD operations]
        [HttpGet]
        [Route("api/v1/users/{user_id:guid}/devices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDevices(string user_id, [FromQuery]DeviceRequestModel model)
        {
            var response = await GetCollectionResultAsync<DeviceResponseModel>(
                getCount: () => _deviceService.DeviceAllCount(user_id),
                getItems: async () =>
                {
                    var devices = await _deviceService.GetDevicesAsync(user_id, model.RequestToDomain());
                    return devices.Select(x => x.DomainToResponse());
                },
                modelState: ModelState
                );

            return Result(response);
        }

        [HttpGet]
        [Route("api/v1/users/{user_id:guid}/devices/{serial_number}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDeviceBySerialNumber([FromQuery]string serial_number, [FromQuery]string user_id)
        {
            var response = await GetResultAsync<DeviceResponseModel>(
                getItem: async () =>
                {
                    var device = await _deviceService.GetDeviceAsync(user_id, serial_number);
                    return device.DomainToResponse();
                },
                modelState: ModelState
                );

            return Result(response);
        }

        [HttpPost]
        [Route("api/v1/users/{user_id:guid}/devices")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddDevice(string user_id, [FromBody]DeviceAddModel model)
        {
            var response = await GetResultAsync<bool>(
                getItem: async () => await _deviceService.AddDeviceAsync(model.RequestToDomain()),
                modelState: ModelState
                );

            return Result(response);
        }

        [HttpDelete]
        [Route("api/v1/users/{user_id:guid}/devices/{serial_number}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteDevice(string user_id, string serial_number)
        {
            var response = await GetResultAsync<bool>(
                getItem: async () => await _deviceService.DeleteDeviceAsync(user_id, serial_number),
                modelState: ModelState,
                httpStatusCode: 204
                );

            return Result(response);
        }
        #endregion

        [HttpPost]
        [Route("api/v1/users/{user_id:guid}/devices/{serial_number}/commands")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExecuteCommand([FromQuery]string user_id, [FromQuery]string serial_number, [FromBody]DeviceCommandModel model)
        {
            var response = await GetResultAsync(
                action: async () => await this._deviceService.ExecuteActionAsync(user_id, serial_number, model.action_name, model.type, model.parameters),
                modelState: ModelState
                );

            return Result(response);
        }
    }
}