using SDmS.Domain.Core.Models;
using SDmS.Domain.Core.Models.Climate;
using SDmS.Domain.Core.Models.Enums;
using System.Threading.Tasks;

namespace SDmS.Domain.Core.Interfases.Services
{
    public interface IClimateDeviceService : IDeviceService
    {
        Task<ResponseCollection<TempSensorDomainModel>> GetTempSensorDevicesAsync(DeviceRequestDomainModel request);
        Task<ResponseCollection<TempControlDomainModel>> GetTempControlDevicesAsync(DeviceRequestDomainModel request);
        Task<Response<TempSensorDomainModel>> GetTempSensorDeviceBySerialNumberAsync(string serialNumber);
        Task<Response<TempControlDomainModel>> GetTempControlDeviceBySerialNumberAsync(string serialNumber);
        Task<Response<bool>> SwitchTempControlAsync(string serialNumber, TempControlState state);
        Task ChangeDesiredTemperatureAsync(string serialNumber, int value);
    }
}
