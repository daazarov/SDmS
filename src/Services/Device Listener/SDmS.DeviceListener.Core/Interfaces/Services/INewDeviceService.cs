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
        Task AssignToUserAsync();
    }
}
