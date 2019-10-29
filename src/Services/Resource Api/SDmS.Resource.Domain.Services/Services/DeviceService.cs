using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NServiceBus;
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
            throw new NotImplementedException();
        }

        public async Task<bool> AddDeviceAsync(DeviceAddDomainModel model)
        {
            return await AddDeviceAsync(GetCurrentUserId(), model);
        }

        public Task<bool> AddDeviceAsync(string user_id, DeviceAddDomainModel model)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDevice(string serialNumber)
        {
            return DeleteDevice(GetCurrentUserId(), serialNumber);
        }

        public bool DeleteDevice(string userId, string serialNumber)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteDeviceAsync(string serialNumber)
        {
            return await DeleteDeviceAsync(GetCurrentUserId(), serialNumber);
        }

        public Task<bool> DeleteDeviceAsync(string userId, string serialNumber)
        {
            throw new NotImplementedException();
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
