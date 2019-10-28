using SDmS.Domain.Core.Models;
using SDmS.Domain.Core.Models.Enums;
using SDmS.Domain.Core.Models.Led;
using System.Threading.Tasks;

namespace SDmS.Domain.Core.Interfases.Services
{
    public interface ILedDeviceService : IDeviceService
    {
        Task<ResponseCollection<LedDomainModel>> GetLedDevicesAsync(DeviceRequestDomainModel request);
        Task<Response<LedDomainModel>> GetLedDeviceBySerialNumberAsync(string serialNumber);
        Task ChangeIntensityAsync(int value, string serialNumber);
        Task<Response<bool>> ChangeStateAsync(LedState state, string serialNumber);
    }
}
