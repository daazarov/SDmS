using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SDmS.Messages.Common.Commands;
using SDmS.Messages.Common.Messages;
using SDmS.Resource.Common;
using SDmS.Resource.Common.Entities.Devices;
using SDmS.Resource.Common.Exceptions;
using SDmS.Resource.Domain.Interfaces.Services;
using SDmS.Resource.Domain.Models.Devices;
using SDmS.Resource.Infrastructure.Interfaces;
using SDmS.Resource.Infrastructure.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDmS.Resource.Domain.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IIdentityParser<ApplicationUser> _identityParser;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceBusSender _serviceBusSender;

        public DeviceService(
            IUnitOfWorkManager unitOfWorkManager, 
            ILogger<DeviceService> logger, 
            IMapper mapper, 
            IIdentityParser<ApplicationUser> identityParser,
            IHttpContextAccessor httpContextAccessor,
            IServiceBusSender serviceBusSender)
        {
            this._logger = logger;
            this._unitOfWorkManager = unitOfWorkManager;
            this._mapper = mapper;
            this._identityParser = identityParser;
            this._httpContextAccessor = httpContextAccessor;
            this._serviceBusSender = serviceBusSender;
        }

        public int Count => DeviceAllCount(this._identityParser.Parse(this._httpContextAccessor.HttpContext.User).id);

        public bool AddDevice(DeviceAddDomainModel model)
        {
            return AddDevice(GetCurrentUserId(), model);
        }

        public bool AddDevice(string userId, DeviceAddDomainModel model)
        {
            var cancellationTokenSource = new CancellationTokenSource();
			cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));

            DeviceExistenceResponseMessage deviceIsExist;

            try
			{
                var message = new DeviceVerificationMessage { serial_number = model.serial_number, CheckType = "EXISTENCE_IN_NEW" };

                deviceIsExist = this._serviceBusSender.SendCallbackMessage<DeviceExistenceResponseMessage, DeviceVerificationMessage>(message, cancellationTokenSource.Token);
			}
			catch (OperationCanceledException)
			{
			    _logger.LogError($"Failed to add a new device. DETAILS: Not received response about the existence of a registered device\n\t" +
                    $"UserId: {userId}, deviceType: {model.type}, serial number: {model.serial_number}, device name: {model.name}");
                throw new ResourceException(-203, 404);
			}

            if (!deviceIsExist.is_exist)
            {
                _logger.LogError($"Failed to add a new device. DETAILS: Not Found in mongo db\n\t" +
                    $"UserId: {userId}, deviceType: {model.type}, serial number: {model.serial_number}, device name: {model.name}");
                throw new ResourceException(-201, 404);
            }

            var unitOfWork = _unitOfWorkManager.GetUnitOfWork();

            var deviceRepository = unitOfWork.Repository<IGenericRepository<Device>, Device>();
            var deviceParameterBindingsRepository = unitOfWork.Repository<IGenericRepository<DeviceParameterBinding>, DeviceParameterBinding>();
            var deviceParametersRepository = unitOfWork.Repository<IGenericRepository<DeviceParameter>, DeviceParameter>();

            var requiredParameter = deviceParametersRepository.GetbyFilter(x => x.description.ToLower() == deviceIsExist.device_info.type_text.ToLower(), 0, 0).FirstOrDefault();

            if (requiredParameter == null)
            {
                _logger.LogError($"Failed to add a new device. DETAILS: Unknown device type\n\t" +
                    $"UserId: {userId}, deviceType: {model.type}, serial number: {model.serial_number}, device name: {model.name}");
                throw new ResourceException(-204, 400);
            }
            if (requiredParameter.parameter_id != model.type)
            {
                throw new ResourceException(-205, 400); // the type of device being added does not match the real device
            }

            Device device = new Device
            {
                serial_number = model.serial_number,
                name = model.name,
                device_type_id = model.type,
                user_id = userId,
                is_online = deviceIsExist.device_info.is_online,
                is_enable = false,
                mqtt_client_id = deviceIsExist.device_info.mqtt_client_id
            };

            deviceRepository.Insert(device);

            var parameters = deviceParameterBindingsRepository.GetbyFilter(x => x.device_type_id == model.type, 0, 0).Select(x => x.Parameter);

            List<DeviceParameterValue> parameterValues = new List<DeviceParameterValue>();

            foreach (var parameter in parameters)
            {
                parameterValues.Add(new DeviceParameterValue
                {
                    date_on = DateTime.UtcNow,
                    device_id = device.device_id,
                    parameter_id = parameter.parameter_id,
                    value = deviceIsExist.parameters.TryGetValue(parameter.description, out dynamic val) ? val : parameter.default_value
                });
            }

            var deviceParameterValuesRepository = unitOfWork.Repository<IGenericRepository<DeviceParameterValue>, DeviceParameterValue>();

            deviceParameterValuesRepository.InsertRange(parameterValues);

            var command = new DeviceAssignCommand
            {
                device_id = device.device_id,
                serial_number = device.serial_number,
                mqtt_client_id = device.mqtt_client_id,
                type_text = device.Type.description,
                parameters = device.DeviceParameters
                    .Where(param => !String.IsNullOrEmpty(param.Parameter.processing_flag))
                    .ToDictionary(x => x.Parameter.description, x => (dynamic)x.value)
            };

            this._serviceBusSender.SendCommand(command);

            return true;
        }

        public async Task<bool> AddDeviceAsync(DeviceAddDomainModel model)
        {
            return await AddDeviceAsync(GetCurrentUserId(), model);
        }

        public async Task<bool> AddDeviceAsync(string userId, DeviceAddDomainModel model)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));

            DeviceExistenceResponseMessage deviceIsExist;

            try
            {
                var message = new DeviceVerificationMessage { serial_number = model.serial_number, CheckType = "EXISTENCE_IN_NEW" };

                deviceIsExist = await this._serviceBusSender.SendCallbackMessageAsync<DeviceExistenceResponseMessage, DeviceVerificationMessage>(message, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                _logger.LogError($"Failed to add a new device. DETAILS: Not received response about the existence of a registered device\n\t" +
                    $"UserId: {userId}, deviceType: {model.type}, serial number: {model.serial_number}, device name: {model.name}");
                throw new ResourceException(-203, 404);
            }

            if (!deviceIsExist.is_exist)
            {
                _logger.LogError($"Failed to add a new device. DETAILS: Not Found in mongo db\n\t" +
                    $"UserId: {userId}, deviceType: {model.type}, serial number: {model.serial_number}, device name: {model.name}");
                throw new ResourceException(-201, 404);
            }

            var unitOfWork = _unitOfWorkManager.GetUnitOfWork();

            var deviceRepository = unitOfWork.Repository<IGenericRepository<Device>, Device>();
            var deviceParameterBindingsRepository = unitOfWork.Repository<IGenericRepository<DeviceParameterBinding>, DeviceParameterBinding>();
            var deviceParametersRepository = unitOfWork.Repository<IGenericRepository<DeviceParameter>, DeviceParameter>();

            var requiredParameters = await deviceParametersRepository.GetbyFilterAsync(x => x.description.ToLower() == deviceIsExist.device_info.type_text.ToLower(), 0, 0);
            var requiredParameter = requiredParameters.FirstOrDefault();

            if (requiredParameter == null)
            {
                _logger.LogError($"Failed to add a new device. DETAILS: Unknown device type\n\t" +
                    $"UserId: {userId}, deviceType: {model.type}, serial number: {model.serial_number}, device name: {model.name}");
                throw new ResourceException(-204, 400);
            }
            if (requiredParameter.parameter_id != model.type)
            {
                throw new ResourceException(-205, 400); // the type of device being added does not match the real device
            }

            Device device = new Device
            {
                serial_number = model.serial_number,
                name = model.name,
                device_type_id = model.type,
                user_id = userId,
                is_online = deviceIsExist.device_info.is_online,
                is_enable = false,
                mqtt_client_id = deviceIsExist.device_info.mqtt_client_id
            };

            await deviceRepository.InsertAsync(device);

            var bindings = await deviceParameterBindingsRepository.GetbyFilterAsync(x => x.device_type_id == model.type, 0, 0);
            var parameters = bindings.Select(x => x.Parameter);

            List<DeviceParameterValue> parameterValues = new List<DeviceParameterValue>();

            foreach (var parameter in parameters)
            {
                parameterValues.Add(new DeviceParameterValue
                {
                    date_on = DateTime.UtcNow,
                    device_id = device.device_id,
                    parameter_id = parameter.parameter_id,
                    value = deviceIsExist.parameters.TryGetValue(parameter.description, out dynamic val) ? val : parameter.default_value
                });
            }

            var deviceParameterValuesRepository = unitOfWork.Repository<IGenericRepository<DeviceParameterValue>, DeviceParameterValue>();

            await deviceParameterValuesRepository.InsertRangeAsync(parameterValues);

            var command = new DeviceAssignCommand
            {
                device_id = device.device_id,
                serial_number = device.serial_number,
                mqtt_client_id = device.mqtt_client_id,
                type_text = device.Type.description,
                parameters = device.DeviceParameters
                    .Where(param => !String.IsNullOrEmpty(param.Parameter.processing_flag))
                    .ToDictionary(x => x.Parameter.description, x => (dynamic)x.value)
            };

            await this._serviceBusSender.SendCommandAsync(command);

            return true;
        }

        public bool DeleteDevice(string serialNumber)
        {
            return DeleteDevice(GetCurrentUserId(), serialNumber);
        }

        public bool DeleteDevice(string userId, string serialNumber)
        {
            var unitOfWork = _unitOfWorkManager.GetUnitOfWork();

            var deviceRepository = unitOfWork.Repository<IGenericRepository<Device>, Device>();

            var device = deviceRepository.GetbyFilter(x => x.serial_number == serialNumber && x.user_id == userId, 0, 0).FirstOrDefault();

            if (device != null)
            {
                var deviceParametersRepository = unitOfWork.Repository<IGenericRepository<DeviceParameterValue>, DeviceParameterValue>();
                var parameters = deviceParametersRepository.GetbyFilter(x => x.device_id == device.device_id, 0, 0);
                deviceParametersRepository.DeleteRange(parameters);

                deviceRepository.Delete(device);

                var command = new DeviceDeleteCommand
                {
                    serial_number = serialNumber,
                    mqtt_client_id = device.mqtt_client_id,
                    type_text = device.Type.description
                };

                this._serviceBusSender.SendCommand(command);
            }

            return true;
        }

        public async Task<bool> DeleteDeviceAsync(string serialNumber)
        {
            return await DeleteDeviceAsync(GetCurrentUserId(), serialNumber);
        }

        public async Task<bool> DeleteDeviceAsync(string userId, string serialNumber)
        {
            var unitOfWork = _unitOfWorkManager.GetUnitOfWork();

            var deviceRepository = unitOfWork.Repository<IGenericRepository<Device>, Device>();

            var device = await deviceRepository.GetbyFilterAsync(x => x.serial_number == serialNumber && x.user_id == userId, 0, 0);

            if (device.FirstOrDefault() != null)
            {
                var deviceParametersRepository = unitOfWork.Repository<IGenericRepository<DeviceParameterValue>, DeviceParameterValue>();
                var parameters = await deviceParametersRepository.GetbyFilterAsync(x => x.device_id == device.FirstOrDefault().device_id, 0, 0);
                deviceParametersRepository.DeleteRange(parameters);

                var command = new DeviceDeleteCommand
                {
                    serial_number = serialNumber,
                    mqtt_client_id = device.FirstOrDefault().mqtt_client_id,
                    type_text = device.FirstOrDefault().Type.description
                };

                deviceRepository.Delete(device.FirstOrDefault());

                await this._serviceBusSender.SendCommandAsync(command);
            }

            return true;
        }

        public int DeviceAllCount(string userId)
        {
            var unitOfWork = _unitOfWorkManager.GetUnitOfWork();

            var deviceRepository = unitOfWork.Repository<IGenericRepository<Device>, Device>();

            return deviceRepository.Table.Where(x => x.user_id == userId).Count();
        }

        public int DeviceCount(string userId, int deviceType)
        {
            var unitOfWork = _unitOfWorkManager.GetUnitOfWork();

            var deviceRepository = unitOfWork.Repository<IGenericRepository<Device>, Device>();

            return deviceRepository.Table.Where(x => x.user_id == userId && x.device_type_id == deviceType).Count();
        }

        public void ExecuteAction(string userId, string serialNumber, string actionName, int deviceType, IDictionary<string, dynamic> parameters)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteActionAsync(string userId, string serialNumber, string actionName, int deviceType, IDictionary<string, dynamic> parameters)
        {
            throw new NotImplementedException();
        }

        public ExecutionResult ExecuteActionWithResult(string userId, string serialNumber, string actionName, int deviceType, IDictionary<string, dynamic> parameters)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> ExecuteActionWithResultAsync(string userId, string serialNumber, string actionName, int deviceType, IDictionary<string, dynamic> parameters)
        {
            throw new NotImplementedException();
        }

        public DeviceDomainModel GetDevice(string serialNumber)
        {
            return GetDevice(GetCurrentUserId(), serialNumber);
        }

        public DeviceDomainModel GetDevice(string userId, string serialNumber)
        {
            var unitOfWork = _unitOfWorkManager.GetUnitOfWork();

            var deviceRepository = unitOfWork.Repository<IGenericRepository<Device>, Device>();

            var entity = deviceRepository.GetbyFilter(x => x.user_id == userId && x.serial_number == serialNumber, 0, 0).FirstOrDefault();

            if (entity == null)
            {
                throw new ResourceException(-201, 404);
            }

            return _mapper.Map<DeviceDomainModel>(entity);
        }

        public async Task<DeviceDomainModel> GetDeviceAsync(string serialNumber)
        {
            return await GetDeviceAsync(GetCurrentUserId(), serialNumber);
        }

        public async Task<DeviceDomainModel> GetDeviceAsync(string userId, string serialNumber)
        {
            var unitOfWork = _unitOfWorkManager.GetUnitOfWork();

            var deviceRepository = unitOfWork.Repository<IGenericRepository<Device>, Device>();

            var entity = await deviceRepository.GetbyFilterAsync(x => x.user_id == userId && x.serial_number == serialNumber, 0, 0);

            if (entity.FirstOrDefault() == null)
            {
                throw new ResourceException(-201, 404);
            }

            return _mapper.Map<DeviceDomainModel>(entity);
        }

        public IEnumerable<DeviceDomainModel> GetDevices(DeviceRequestDomainModel request)
        {
            return GetDevices(GetCurrentUserId(), request);
        }

        public IEnumerable<DeviceDomainModel> GetDevices(string userId, DeviceRequestDomainModel request)
        {
            var unitOfWork = _unitOfWorkManager.GetUnitOfWork();

            var deviceRepository = unitOfWork.Repository<IGenericRepository<Device>, Device>();

            var entities = deviceRepository.GetbyFilter(x => x.user_id == userId && x.device_type_id == request.type, request.limit, request.offset);

            if (entities.Count() == 0)
            {
                return new List<DeviceDomainModel>();
            }

            return _mapper.Map<IEnumerable<DeviceDomainModel>>(entities);
        }

        public async Task<IEnumerable<DeviceDomainModel>> GetDevicesAsync(DeviceRequestDomainModel request)
        {
            return await GetDevicesAsync(GetCurrentUserId(), request);
        }

        public async Task<IEnumerable<DeviceDomainModel>> GetDevicesAsync(string userId, DeviceRequestDomainModel request)
        {
            var unitOfWork = _unitOfWorkManager.GetUnitOfWork();

            var deviceRepository = unitOfWork.Repository<IGenericRepository<Device>, Device>();

            var entities = await deviceRepository.GetbyFilterAsync(x => x.user_id == userId && x.device_type_id == request.type, request.limit, request.offset);

            if (entities.Count() == 0)
            {
                return new List<DeviceDomainModel>();
            }

            return _mapper.Map<IEnumerable<DeviceDomainModel>>(entities);
        }

        private string GetCurrentUserId()
        {
            var user = this._identityParser.Parse(this._httpContextAccessor.HttpContext.User);

            if (string.IsNullOrEmpty(user.id))
            {
                throw new ResourceException(-10, 401);
            }

            return user.id;
        }
    }
}
