using SDmS.Domain.Core.Interfases.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDmS.Domain.Core.Models;
using SDmS.Domain.Core.Models.Climate;
using SDmS.Domain.Core.Models.Enums;
using SDmS.Domain.Common.Identity;
using System.Configuration;
using System.Web;
using SDmS.Mvc.Infrastructure;
using SDmS.Infrastructure.Models.Enums;
using SDmS.Mvc.Infrastructure.Services;
using SDmS.Infrastructure.Models;
using SDmS.Infrastructure.Models.Devices;
using SDmS.Infrastructure.Models.Devices.Climate;
using SDmS.Domain.Mappers.Climate;
using SDmS.Mvc.Domain.Core.Constants;

namespace SDmS.Domain.Services
{
    public class ClimateDeviceService : IClimateDeviceService
    {
        private readonly string _baseResourceApiAddress;
        private readonly IIdentityParser<ApplicationUser> _identityParser;
        private readonly ApplicationUser _user;

        public ClimateDeviceService(IIdentityParser<ApplicationUser> identityParser)
        {
            this._baseResourceApiAddress = ConfigurationManager.AppSettings["as:ResourceApiUrl"];
            this._identityParser = identityParser;
            this._user = this._identityParser.Parse(HttpContext.Current.User);
        }

        public async Task<Response<bool>> AssignToUserAsync(DeviceAddToUserDomainModel model)
        {
            return await AssignToUserAsync(model.serial_number, model.user_id, model.name, model.type);
        }

        public async Task<Response<bool>> AssignToUserAsync(string serialNumber, string userId, string deviceName, string type = null)
        {
            string uri = API.Devices.AssignToUser(_baseResourceApiAddress, ApiVersion.v1, userId);

            var result = await CommandFactory.Instance.GetCommand("BASE_POST_COMMAND")
                .RunAsync<ResponseInfrastructureModel<bool>>(uri, new DeviceInfrastructureAddModel
                {
                    name = deviceName,
                    serial_number = serialNumber,
                    user_id = userId,
                    type = type ?? "temperature"
                });

            if (result != null)
            {
                if (!result.HasValue && result.Response.IsSuccessStatusCode)
                {
                    return new Response<bool> { Value = true };
                }

                var response = new Response<bool>();

                response.Value = result.HasValue ? result.Value.Value : false;
                response.Error = result.HasValue ? result.Value.Error : result.Error;
                response.ErrorCode = result.HasValue ? result.Value.ErrorCode : (int)result.ResponseCode;

                return response;
            }

            return new Response<bool> { Error = "Unidentified error", ErrorCode = -999, Value = false };
        }

        public async Task ChangeDesiredTemperatureAsync(string serialNumber, int value)
        {
            string uri = API.Devices.ExecuteCommand(_baseResourceApiAddress, ApiVersion.v1, serialNumber, _user.id);

            DeviceInfracstructureActionModel actionModel = new DeviceInfracstructureActionModel(DeviceCommands.SetDesiredTemperature, "control");

            actionModel.AddParameter("temperature", value);

            await CommandFactory.Instance.GetCommand("BASE_POST_COMMAND").RunAsync(uri, actionModel);
        }

        public async Task<Response<bool>> DeleteDeviceAsync(string serialNumber)
        {
            string uri = API.Devices.DeleteDevice(_baseResourceApiAddress, ApiVersion.v1, _user.id, serialNumber);

            var result = await CommandFactory.Instance.GetCommand("BASE_DELETE_COMMAND").RunAsync<ResponseInfrastructureModel<bool>>(uri);

            if (result != null)
            {
                if (result.ResponseCode == System.Net.HttpStatusCode.NoContent)
                {
                    return new Response<bool> { Value = true };
                }

                var response = new Response<bool>();

                response.Value = result.HasValue ? result.Value.Value : false;
                response.Error = result.HasValue ? result.Value.Error : result.Error;
                response.ErrorCode = result.HasValue ? result.Value.ErrorCode : (int)result.ResponseCode;

                return response;
            }

            return new Response<bool> { Error = "Unidentified error", ErrorCode = -999, Value = false };
        }

        public async Task<Response<TempSensorDomainModel>> GetTempSensorDeviceBySerialNumberAsync(string serialNumber)
        {
            string uri = API.Devices.FindBySerialNumber(_baseResourceApiAddress, ApiVersion.v1, serialNumber, _user.id);

            var result = await CommandFactory.Instance.GetCommand("BASE_GET_COMMAND").RunAsync<ResponseInfrastructureModel<TempSensorInfrastructureModel>>(uri);

            if (result != null)
            {
                Response<TempSensorDomainModel> response = new Response<TempSensorDomainModel>();

                response.Value = result.HasValue ? result.Value.Value.InfrastructureToDomain() : null;
                response.Error = result.HasValue ? result.Value.Error : result.Error;
                response.ErrorCode = result.HasValue ? result.Value.ErrorCode : (int)result.ResponseCode;

                return response;
            }

            return new Response<TempSensorDomainModel> { ErrorCode = -999, Error = "Unidentified error" };
        }

        public async Task<ResponseCollection<TempControlDomainModel>> GetTempControlDevicesAsync(DeviceRequestDomainModel request)
        {
            string uri = API.Devices.GetDevices(_baseResourceApiAddress, ApiVersion.v1, request.limit, request.offset, _user.id, request.type ?? "control");

            var result = await CommandFactory.Instance.GetCommand("BASE_GET_COMMAND").RunAsync<ResponseInfrastructureCollectionModel<TempControlInfrastructureModel>>(uri);

            if (result != null)
            {
                ResponseCollection<TempControlDomainModel> response = new ResponseCollection<TempControlDomainModel>();

                if (result.HasValue)
                {
                    if (result.Value.TotalCount == 0)
                    {
                        response.Collection = new List<TempControlDomainModel>();
                    }
                    else
                    {
                        response.Collection = result.Value.Collection.Select(x => x.InfrastructureToDomain());
                    }

                    response.Error = result.Value.Error;
                    response.ErrorCode = result.Value.ErrorCode;
                    response.Next = result.Value.Next;
                    response.Previous = result.Value.Previous;
                    response.TotalCount = result.Value.TotalCount;
                }
                else
                {
                    response.Error = result.Error;
                    response.ErrorCode = (int)result.ResponseCode;
                    response.Collection = new List<TempControlDomainModel>();
                }

                return response;
            }

            return new ResponseCollection<TempControlDomainModel> { ErrorCode = -999, Error = "Unidentified error" };
        }

        public async Task<Response<TempControlDomainModel>> GetTempControlDeviceBySerialNumberAsync(string serialNumber)
        {
            string uri = API.Devices.FindBySerialNumber(_baseResourceApiAddress, ApiVersion.v1, serialNumber, _user.id);

            var result = await CommandFactory.Instance.GetCommand("BASE_GET_COMMAND").RunAsync<ResponseInfrastructureModel<TempControlInfrastructureModel>>(uri);

            if (result != null)
            {
                Response<TempControlDomainModel> response = new Response<TempControlDomainModel>();

                response.Value = result.HasValue ? result.Value.Value.InfrastructureToDomain() : null;
                response.Error = result.HasValue ? result.Value.Error : result.Error;
                response.ErrorCode = result.HasValue ? result.Value.ErrorCode : (int)result.ResponseCode;

                return response;
            }

            return new Response<TempControlDomainModel> { ErrorCode = -999, Error = "Unidentified error" };
        }

        public async Task<ResponseCollection<TempSensorDomainModel>> GetTempSensorDevicesAsync(DeviceRequestDomainModel request)
        {
            string uri = API.Devices.GetDevices(_baseResourceApiAddress, ApiVersion.v1, request.limit, request.offset, _user.id, request.type ?? "temperature");

            var result = await CommandFactory.Instance.GetCommand("BASE_GET_COMMAND").RunAsync<ResponseInfrastructureCollectionModel<TempSensorInfrastructureModel>>(uri);

            if (result != null)
            {
                ResponseCollection<TempSensorDomainModel> response = new ResponseCollection<TempSensorDomainModel>();

                if (result.HasValue)
                {
                    if (result.Value.TotalCount == 0)
                    {
                        response.Collection = new List<TempSensorDomainModel>();
                    }
                    else
                    {
                        response.Collection = result.Value.Collection.Select(x => x.InfrastructureToDomain());
                    }

                    response.Error = result.Value.Error;
                    response.ErrorCode = result.Value.ErrorCode;
                    response.Next = result.Value.Next;
                    response.Previous = result.Value.Previous;
                    response.TotalCount = result.Value.TotalCount;
                }
                else
                {
                    response.Error = result.Error;
                    response.ErrorCode = (int)result.ResponseCode;
                    response.Collection = new List<TempSensorDomainModel>();
                }

                return response;
            }

            return new ResponseCollection<TempSensorDomainModel> { ErrorCode = -999, Error = "Unidentified error" };
        }

        public async Task<Response<bool>> SwitchTempControlAsync(string serialNumber, TempControlState state)
        {
            string uri = API.Devices.ExecuteCommand(_baseResourceApiAddress, ApiVersion.v1, serialNumber, _user.id);

            DeviceInfracstructureActionModel actionModel = new DeviceInfracstructureActionModel(DeviceCommands.SwitchClimateControle, "control");

            actionModel.AddParameter("state", (int)state);

            var result = await CommandFactory.Instance.GetCommand("BASE_POST_COMMAND")
                .RunAsync<ResponseInfrastructureModel<bool>>(uri, actionModel);

            if (result != null)
            {
                if (result.ResponseCode == System.Net.HttpStatusCode.OK)
                {
                    return new Response<bool> { Value = true};
                }

                var response = new Response<bool>();

                response.Value = result.HasValue ? result.Value.Value : false;
                response.Error = result.HasValue ? result.Value.Error : result.Error;
                response.ErrorCode = result.HasValue ? result.Value.ErrorCode : (int)result.ResponseCode;

                return response;
            }

            return new Response<bool> { Error = "Unidentified error", ErrorCode = -999, Value = false };
        }
    }
}
