using SDmS.Resource.Domain.Models.Devices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SDmS.Resource.Domain.Interfaces.Services
{
    public interface IDeviceService
    {
        /// <summary>
        /// View the number of all user devices
        /// </summary>
        int Count { get; }

        IEnumerable<DeviceDomainModel> GetDevices(DeviceRequestDomainModel request);
        IEnumerable<DeviceDomainModel> GetDevices(string userId, DeviceRequestDomainModel request);
        Task<IEnumerable<DeviceDomainModel>> GetDevicesAsync(DeviceRequestDomainModel request);
        Task<IEnumerable<DeviceDomainModel>> GetDevicesAsync(string userId, DeviceRequestDomainModel request);
        DeviceDomainModel GetDevice(string serialNumber);
        DeviceDomainModel GetDevice(string userId, string serialNumber);
        Task<DeviceDomainModel> GetDeviceAsync(string serialNumber);
        Task<DeviceDomainModel> GetDeviceAsync(string userId, string serialNumber);
        bool AddDevice(DeviceAddDomainModel model);
        bool AddDevice(string userId, DeviceAddDomainModel model);
        Task<bool> AddDeviceAsync(DeviceAddDomainModel model);
        Task<bool> AddDeviceAsync(string user_id, DeviceAddDomainModel model);
        bool DeleteDevice(string serialNumber);
        bool DeleteDevice(string userId, string serialNumber);
        Task<bool> DeleteDeviceAsync(string serialNumber);
        Task<bool> DeleteDeviceAsync(string userId, string serialNumber);
        void ExecuteAction(string userId, string serialNumber, string actionName, int deviceType, IDictionary<string, dynamic> parameters);
        Task ExecuteActionAsync(string userId, string serialNumber, string actionName, int deviceType, IDictionary<string, dynamic> parameters);
        ExecutionResult ExecuteActionWithResult(string userId, string serialNumber, string actionName, int deviceType, IDictionary<string, dynamic> parameters);
        Task<ExecutionResult> ExecuteActionWithResultAsync(string userId, string serialNumber, string actionName, int deviceType, IDictionary<string, dynamic> parameters);

        int DeviceCount(string userId, int deviceType);
        int DeviceAllCount(string userId);
    }
}
