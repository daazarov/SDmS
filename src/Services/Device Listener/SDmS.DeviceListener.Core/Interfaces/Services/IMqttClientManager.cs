using SDmS.DeviceListener.Infrastructure.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Interfaces.Services
{
    public interface IMqttClientManager
    {
        Task ChangeDevicesStatusAsync(bool status, string mqtt_client_id);
        Task<IEnumerable<Device>> GetDevicesByMqttClientAsync(string mqtt_client_id);
    }
}
