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
        public async Task<IActionResult> GetDevices([FromQuery]DeviceRequestModel model)
        {
            return Ok(new string[] { "value1", "value2" });
        }

        [HttpGet]
        [Route("api/v1/users/{user_id:guid}/devices{serial_number:string}")]
        public async Task<IActionResult> GetDeviceBySerialNumber([FromQuery]string serial_number, [FromQuery]string user_id)
        {
            return Ok("value");
        }

        [HttpPost]
        [Route("api/v1/users/{user_id:guid}/devices")]
        public async Task<IActionResult> AddDevice([FromQuery]string user_id, [FromBody]DeviceAddModel model)
        {
            return new StatusCodeResult((int)HttpStatusCode.Created);
        }

        [HttpDelete]
        [Route("api/v1/users/{user_id:guid}/devices{serial_number:string}")]
        public async Task<IActionResult> DeleteDevice(string user_id, string serial_number)
        {
            return NoContent();
        }
        #endregion

        [HttpPost]
        [Route("api/v1/users/{user_id:guid}/devices/{serial_number:string}/commands")]
        public async Task<IActionResult> ExecuteCommand([FromQuery]string user_id, [FromQuery]string serial_number, [FromBody]DeviceCommandModel model)
        {
            return Ok();
        }
    }
}