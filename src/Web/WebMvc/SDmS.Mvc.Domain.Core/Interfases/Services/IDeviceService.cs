using SDmS.Domain.Core.Models;
using System.Threading.Tasks;

namespace SDmS.Domain.Core.Interfases.Services
{
    public interface IDeviceService
    {
        Task<Response<bool>> AssignToUserAsync(DeviceAddToUserDomainModel model);
        Task<Response<bool>> AssignToUserAsync(string serialNumber, string userId, string deviceName, int deviceType);
        Task<Response<bool>> DeleteDeviceAsync(string serialNumber);
    }
}
