using SDmS.Domain.Core.Models;
using SDmS.Domain.Core.Models.Enums;
using SDmS.Domain.Core.Models.Led;
using System.Threading.Tasks;

namespace SDmS.Domain.Core.Interfases.Services
{
    public interface ILedDeviceService
    {
        Task<ResponseCollection<LedDomainModel>> GetLedDevicesAsync(LedRequestDomainModel request);
        Task<Response<LedDomainModel>> GetLedDeviceBySerialNumberAsync(string serialNumber);
        Task<Response<bool>> AssignToUserAsync(LedAddToUserDomainModel model);
        Task<Response<bool>> AssignToUserAsync(string serialNumber, string userId, string deviceName);
        Task ChangeIntensityAsync(int value, string serialNumber);
        Task<Response<bool>> ChangeStateAsync(LedState state, string serialNumber);
        Task<Response<bool>> DeleteDeviceAsync(string serialNumber);
    }
}
