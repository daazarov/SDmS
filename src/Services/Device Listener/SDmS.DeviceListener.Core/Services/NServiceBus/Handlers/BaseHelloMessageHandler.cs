using NServiceBus;
using SDmS.DeviceListener.Core.Interfaces.Services;
using SDmS.Messages.Common.Events;
using System;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services.NServiceBus.Handlers
{
    public class BaseHelloMessageHandler : IHandleMessages<DeviceHelloMessage>
    {
        private readonly INewDeviceService _newDeviceService;

        public BaseHelloMessageHandler(INewDeviceService newDeviceService)
        {
            this._newDeviceService = newDeviceService ?? throw new ArgumentNullException(nameof(newDeviceService));
        }

        public async Task Handle(DeviceHelloMessage message, IMessageHandlerContext context)
        {
            await _newDeviceService.RegisterDeviceAsync<DeviceHelloMessage>(message);
        }
    }
}
