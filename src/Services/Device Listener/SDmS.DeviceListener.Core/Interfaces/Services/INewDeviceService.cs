using SDmS.Messages.Common.Messages;
using SDmS.Messages.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Interfaces.Services
{
    public interface INewDeviceService
    {
        Task RegisterDeviceAsync<T>(T device) where T : DeviceEvent;
        Task<bool> AssignToUserAsync<T>(T device, string collectionName) where T : DeviceCommand;
        Task<DeviceExistenceResponseMessage> CheckDeviceExistenceInNewAsync(string serialNumber);
    }
}
