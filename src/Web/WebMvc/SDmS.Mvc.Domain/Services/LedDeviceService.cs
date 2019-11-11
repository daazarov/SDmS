using SDmS.Domain.Core.Interfases.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SDmS.Domain.Core.Models;
using SDmS.Domain.Core.Models.Enums;
using SDmS.Domain.Core.Models.Led;
using System.Configuration;
using SDmS.Mvc.Infrastructure;
using SDmS.Infrastructure.Models.Enums;
using SDmS.Domain.Common.Identity;
using System.Web;
using SDmS.Mvc.Infrastructure.Services;
using SDmS.Infrastructure.Models.Devices.Led;
using SDmS.Infrastructure.Models;
using SDmS.Domain.Mappers.Led;
using SDmS.Infrastructure.Models.Devices;
using SDmS.Mvc.Domain.Core.Constants;

namespace SDmS.Domain.Services
{
    public class LedDeviceService : ILedDeviceService
    {
        private readonly string _baseResourceApiAddress;
        private readonly IIdentityParser<ApplicationUser> _identityParser;
        private readonly ApplicationUser _user;

        public LedDeviceService(IIdentityParser<ApplicationUser> identityParser)
        {
            this._baseResourceApiAddress = ConfigurationManager.AppSettings["as:ResourceApiUri"];
            this._identityParser = identityParser;
            this._user = this._identityParser.Parse(HttpContext.Current.User);
        }

        public async Task<Response<bool>> AssignToUserAsync(DeviceAddToUserDomainModel model)
        {
            return await AssignToUserAsync(model.serial_number, model.user_id, model.name, model.type);
        }

        public async Task<Response<bool>> AssignToUserAsync(string serialNumber, string userId, string deviceName, int type)
        {
            string uri = API.Devices.AssignToUser(_baseResourceApiAddress, ApiVersion.v1, userId);

            var result = await CommandFactory.Instance.GetCommand("BASE_POST_COMMAND")
                .RunAsync<ResponseInfrastructureModel<bool>>(uri, new DeviceInfrastructureAddModel
                {
                    name = deviceName,
                    serial_number = serialNumber,
                    user_id = userId,
                    type = type});

            if (result != null)
            {
                if (!result.HasValue && result.Response.IsSuccessStatusCode)
                {
                    return new Response<bool> { Value = true, HttpResponseCode = (int)result.ResponseCode };
                }

                var response = new Response<bool>();

                response.Value = result.HasValue ? result.Value.Value : false;
                response.Error = result.HasValue ? result.Value.Error : result.Error;
                response.ErrorCode = result.HasValue ? result.Value.ErrorCode : null;
                response.HttpResponseCode = (int)result.ResponseCode;

                return response;
            }

            return new Response<bool> { Error = "Unidentified error", ErrorCode = -999, Value = false, HttpResponseCode = 500 };
        }

        public async Task ChangeIntensityAsync(int value, int deviceType, string serialNumber)
        {
            string uri = API.Devices.ExecuteCommand(_baseResourceApiAddress, ApiVersion.v1, serialNumber, _user.id);

            DeviceInfracstructureActionModel actionModel = new DeviceInfracstructureActionModel(DeviceCommands.SetLedIntensity, deviceType);

            actionModel.AddParameter("intensity", value);

            await CommandFactory.Instance.GetCommand("BASE_POST_COMMAND").RunAsync(uri, actionModel);
        }

        public async Task<Response<bool>> ChangeStateAsync(LedState state, int deviceType, string serialNumber)
        {
            string uri = API.Devices.ExecuteCommand(_baseResourceApiAddress, ApiVersion.v1, serialNumber, _user.id);

            DeviceInfracstructureActionModel actionModel = new DeviceInfracstructureActionModel(DeviceCommands.SwitchLed, deviceType);

            actionModel.AddParameter("state", (int)state);

            var result = await CommandFactory.Instance.GetCommand("BASE_POST_COMMAND")
                .RunAsync<ResponseInfrastructureModel<bool>>(uri, actionModel);

            if (result != null)
            {
                var response = new Response<bool>();

                response.Value = result.HasValue ? result.Value.Value : false;
                response.Error = result.HasValue ? result.Value.Error : result.Error;
                response.ErrorCode = result.HasValue ? result.Value.ErrorCode : null;
                response.HttpResponseCode = (int)result.ResponseCode;

                return response;
            }

            return new Response<bool> { Error = "Unidentified error", ErrorCode = -999, Value = false, HttpResponseCode = 500 };
        }

        public async Task<Response<LedDomainModel>> GetLedDeviceBySerialNumberAsync(string serialNumber)
        {
            string uri = API.Devices.FindBySerialNumber(_baseResourceApiAddress, ApiVersion.v1, serialNumber, _user.id);

            var result = await CommandFactory.Instance.GetCommand("BASE_GET_COMMAND").RunAsync<ResponseInfrastructureModel<LedInfrastructureModel>>(uri);

            if (result != null)
            {
                Response<LedDomainModel> response = new Response<LedDomainModel>();

                response.Value = result.HasValue ? result.Value.Value.InfrastructureToDomain() : null;
                response.Error = result.HasValue ? result.Value.Error : result.Error;
                response.ErrorCode = result.HasValue ? result.Value.ErrorCode : null;
                response.HttpResponseCode = (int)result.ResponseCode;

                return response;
            }

            return new Response<LedDomainModel> { ErrorCode = -999, Error = "Unidentified error", HttpResponseCode = 500 };
        }

        public async Task<ResponseCollection<LedDomainModel>> GetLedDevicesAsync(DeviceRequestDomainModel request)
        {
            string uri = API.Devices.GetDevices(_baseResourceApiAddress, ApiVersion.v1, request.limit, request.offset, _user.id, request.type);

            var result = await CommandFactory.Instance.GetCommand("BASE_GET_COMMAND").RunAsync<ResponseInfrastructureCollectionModel<LedInfrastructureModel>>(uri);

            if (result != null)
            {
                ResponseCollection<LedDomainModel> response = new ResponseCollection<LedDomainModel>();

                if (result.HasValue)
                {
                    if (result.Value.TotalCount == 0)
                    {
                        response.Collection = new List<LedDomainModel>();
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
                    response.HttpResponseCode = (int)result.ResponseCode;
                }
                else
                {
                    response.Error = result.Error;
                    response.ErrorCode = null;
                    response.HttpResponseCode = (int)result.ResponseCode;
                    response.Collection = new List<LedDomainModel>();
                }

                return response;
            }

            return new ResponseCollection<LedDomainModel> { ErrorCode = -999, Error = "Unidentified error", HttpResponseCode = 500 };
        }

        public async Task<Response<bool>> DeleteDeviceAsync(string serialNumber)
        {
            string uri = API.Devices.DeleteDevice(_baseResourceApiAddress, ApiVersion.v1, _user.id, serialNumber);

            var result = await CommandFactory.Instance.GetCommand("BASE_DELETE_COMMAND").RunAsync<ResponseInfrastructureModel<bool>>(uri);

            if (result != null)
            {
                if (result.ResponseCode == System.Net.HttpStatusCode.NoContent)
                {
                    return new Response<bool> { Value = true, HttpResponseCode = (int)result.ResponseCode };
                }

                var response = new Response<bool>();

                response.Value = result.HasValue ? result.Value.Value : false;
                response.Error = result.HasValue ? result.Value.Error : result.Error;
                response.ErrorCode = result.HasValue ? result.Value.ErrorCode : null;
                response.HttpResponseCode = (int)result.ResponseCode;

                return response;
            }

            return new Response<bool> { Error = "Unidentified error", ErrorCode = -999, Value = false, HttpResponseCode = 500 };
        }
    }
}
